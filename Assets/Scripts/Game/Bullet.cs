using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 direction;
    void Start()
    {
        direction = new Vector3(0, 1, 0);
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
