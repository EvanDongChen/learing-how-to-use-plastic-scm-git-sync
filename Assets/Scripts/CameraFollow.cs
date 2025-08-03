using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public GameObject playerObject; // ✅ Player GameObject reference
    private Transform target;       // ✅ Internal reference to player's transform
    public float followSpeed = 5f;

    void Start()
    {
        StartCoroutine(FindPlayerWhenReady());
    }

    private IEnumerator FindPlayerWhenReady()
    {
        while (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player"); // ✅ Find by tag
            if (playerObject != null)
            {
                target = playerObject.transform; // ✅ Get the transform from GameObject
                yield break;
            }

            yield return null;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }
}
