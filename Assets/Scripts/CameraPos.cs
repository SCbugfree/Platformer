using UnityEngine;

public class CameraPos : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;//stores the position of the target obejct

    public float smoothSpeed = 0.5f;
    public Vector3 offset;

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;//target camera position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);//calculate a smoothedPosition for the camera
        transform.position = smoothedPosition;//assign the camera position to calculated smoothedPosition


    }

}
