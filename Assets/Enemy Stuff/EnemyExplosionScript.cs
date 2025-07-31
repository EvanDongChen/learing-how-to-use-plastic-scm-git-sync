using UnityEngine;

public class EnemyExplosionScript : MonoBehaviour
{
    public float growDuration = 1f;
    public float growSpeed = 3f;
    private float timer = 0f;

    void Update()
    {
        if (timer < growDuration)
        {
            float scaleInc = growSpeed * Time.deltaTime;
            transform.localScale += new Vector3(scaleInc, scaleInc, scaleInc);
            timer += Time.deltaTime;
            if (timer >= growDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}
