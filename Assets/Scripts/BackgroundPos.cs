using UnityEngine;

public class BackgroundPos : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;//stores the position of the target obejct

    public Vector3 offset;

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;//target background position
        transform.position = targetPosition;

    }

}

