using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parallaxEffect;

    GameObject cam;
    float lenght;
    float initialPos;
    void Start()
    {
        cam = Camera.main.gameObject;
        initialPos = transform.position.y;
        lenght = GetComponent<SpriteRenderer>().bounds.size.y;
    }
    void Update()
    {
        float temp = cam.transform.position.y * (1 - parallaxEffect);
        float distance = cam.transform.position.y * parallaxEffect;
        transform.position = new Vector3(transform.position.x, initialPos -  distance, transform.position.z);
        if (temp > initialPos + lenght)
            initialPos += lenght;
        else if (temp < initialPos - lenght)
            initialPos -= lenght;
    }
}
