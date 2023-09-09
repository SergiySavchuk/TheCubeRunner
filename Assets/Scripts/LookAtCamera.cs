using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;

    private void Start()
    {
        transform.forward = mainCamera.forward;
    }
}
