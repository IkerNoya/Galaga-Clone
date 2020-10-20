using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [Space]
    [SerializeField] Vector3 InitialPos;
    [Space]
    [SerializeField] GameObject bullet;
    [SerializeField] KeyCode FireButton;
    [Space]
    [SerializeField] GameObject gunLeft;
    [SerializeField] GameObject gunRight;
    Vector3 movement;

    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += movement * speed * Time.deltaTime;
        Inputs();
    }
    void Inputs()
    {
        if (Input.GetKeyDown(FireButton))
        {
            if (bullet != null)
            {
                if(gunLeft!=null)
                    Instantiate(bullet, gunLeft.transform.position, Quaternion.identity);
                if(gunRight!=null)
                    Instantiate(bullet, gunRight.transform.position, Quaternion.identity);
            }
        }
    }
}
