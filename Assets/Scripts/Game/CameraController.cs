using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 direction;
    
    void Update()
    {
        direction = Vector3.up;
        transform.position += direction * speed * Time.deltaTime;
    }
}
