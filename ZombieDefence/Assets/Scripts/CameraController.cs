using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, 
            player.position + new Vector3(0, 0, -10), 0.1f);
    }
}
