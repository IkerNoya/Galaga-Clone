using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int lives;
    [Space]
    [SerializeField] Vector3 InitialPos;
    [Space]
    [SerializeField] GameObject bullet;
    [SerializeField] KeyCode FireButton;
    [Space]
    [SerializeField] GameObject gunLeft;
    [SerializeField] GameObject gunRight;
    [SerializeField] ParticleSystem muzzleLeft;
    [SerializeField] ParticleSystem muzzleRight;
    Vector3 movement;

    void Start()
    {
    }
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
                bullet.GetComponent<Bullet>().SetUser(Bullet.User.player);
                if (gunLeft != null && muzzleLeft != null)
                {
                    Instantiate(bullet, gunLeft.transform.position, Quaternion.identity);
                    muzzleLeft.Play();
                }
                if (gunRight != null && muzzleRight != null)
                {
                    Instantiate(bullet, gunRight.transform.position, Quaternion.identity);
                    muzzleRight.Play();
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().GetUser() == Bullet.User.enemy)
        {
            Debug.Log("HOLA");
            lives--;
            Destroy(collision.gameObject);
        }
    }
}
