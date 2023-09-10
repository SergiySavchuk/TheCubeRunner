using UnityEngine;

//Class for text with world space looking at the camera
public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;

    private void Awake()
    {
        transform.forward = mainCamera.forward;
    }
}
