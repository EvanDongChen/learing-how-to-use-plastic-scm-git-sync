using UnityEngine;

public class ProjectileManagerScript : MonoBehaviour
{
    public GameObject noteProjectilePrefab;
    public Transform spawnPoint;
    private GameObject lyreObject;
    private LyreScript lyreScript;

    private NoteData fireNoteData;
    private NoteData waterNoteData;
    private NoteData lightningNoteData;

    void Start()
    {
        // Find Lyre GameObject and LyreScript
        lyreObject = GameObject.Find("Lyre");
        if (lyreObject != null)
        {
            lyreScript = lyreObject.GetComponent<LyreScript>();
            if (lyreScript == null)
            {
                Debug.LogWarning("Lyre GameObject found, but LyreScript component is missing.");
            }
        }
        else
        {
            Debug.LogWarning("Lyre GameObject not found in the scene.");
        }

        fireNoteData = ScriptableObject.CreateInstance<NoteData>();
        fireNoteData.noteName = "Fire Note";
        fireNoteData.element = NoteData.Elements.Fire;
        fireNoteData.noteDuration = 2f;
        fireNoteData.rarity = 1;
        fireNoteData.level = 1;
        fireNoteData.attributes = new System.Collections.Generic.List<NoteData.AttributeType> { NoteData.AttributeType.Forte };

        waterNoteData = ScriptableObject.CreateInstance<NoteData>();
        waterNoteData.noteName = "Water Note";
        waterNoteData.element = NoteData.Elements.Water;
        waterNoteData.noteDuration = 2f;
        waterNoteData.rarity = 1;
        waterNoteData.level = 1;
        waterNoteData.attributes = new System.Collections.Generic.List<NoteData.AttributeType> {  };

        lightningNoteData = ScriptableObject.CreateInstance<NoteData>();
        lightningNoteData.noteName = "MultiShot Note";
        lightningNoteData.element = NoteData.Elements.Lightining;
        lightningNoteData.noteDuration = 2f;
        lightningNoteData.rarity = 1;
        lightningNoteData.level = 1;
        lightningNoteData.attributes = new System.Collections.Generic.List<NoteData.AttributeType> {
          
        };
        int expectedCount = 1;
        expectedCount += 1; // Staccato
        expectedCount += 2; // Trio
        expectedCount += 3; // Quartet
        expectedCount += 4; // Quintet
        Debug.Log($"[TEST] MultiShot Note should spawn {expectedCount} projectiles.");
    }

    void Update()
    {
        // Press 1 for fire, 2 for water, 3 for lightning (Input System)
        if (UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            ShootNoteAtCursor(fireNoteData);
        }
        if (UnityEngine.InputSystem.Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ShootNoteAtCursor(waterNoteData);
        }
        if (UnityEngine.InputSystem.Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            ShootNoteAtCursor(lightningNoteData);
        }
    }

    // Shoots a note projectile toward the cursor
    public void ShootNoteAtCursor(NoteData data)
    {
        if (data == null || noteProjectilePrefab == null || spawnPoint == null)
            return;

        // Call LyreScript TriggerAttack if available
        if (lyreScript != null)
        {
            lyreScript.TriggerAttack();
        }

        // Get mouse position in world space (2D)
        Vector2 mouseScreenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));
        mouseWorldPos.z = 0; // Ensure 2D
        Vector3 direction = (mouseWorldPos - spawnPoint.position).normalized;
        direction.z = 0;

        // Trio: shoot 3 notes in a spread; Staccato: shoot 2 notes
        int noteCount = 1;
        if (data.attributes != null)
        {
            noteCount += data.attributes.Contains(NoteData.AttributeType.Staccato) ? 1 : 0;
            noteCount += data.attributes.Contains(NoteData.AttributeType.Trio) ? 2 : 0;
            noteCount += data.attributes.Contains(NoteData.AttributeType.Quartet) ? 3 : 0;
            noteCount += data.attributes.Contains(NoteData.AttributeType.Quintet) ? 4 : 0;
        }

        // Calculate spread angles
        float spread = 15f; // total spread in degrees
        float[] angles = new float[noteCount];
        if (noteCount == 1)
        {
            angles[0] = 0f;
        }
        else
        {
            float step = noteCount > 1 ? spread / (noteCount - 1) : 0f;
            float start = -spread / 2f;
            for (int i = 0; i < noteCount; i++)
            {
                angles[i] = start + step * i;
            }
        }

        for (int i = 0; i < noteCount; i++)
        {
            Vector3 spawnPos = spawnPoint.position;
            Vector3 dir = Quaternion.Euler(0, 0, angles[i]) * direction;
            GameObject proj = Instantiate(noteProjectilePrefab, spawnPos, Quaternion.identity);
            ProjectileScript np = proj.GetComponent<ProjectileScript>();
            if (np != null)
            {
                np.Initialize(data);
                np.direction = dir;
            }
        }
    }
}
