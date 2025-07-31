using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(target.position.x, target.position.y, transform.position.z),
                followSpeed * Time.deltaTime);
        }
    }
}
