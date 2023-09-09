using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }
}
