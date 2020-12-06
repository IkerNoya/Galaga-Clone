using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

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

    BoxCollider2D collider;
    Vector3 movement;
    Camera cam;
    float colliderHalfWidth;
    float colliderHalfHeight;
    float rightScreenPos;
    float leftightScreenPos;
    float topScreenPos;
    float bottomScreenPos;

    void Start()
    {
        cam = Camera.main;
        collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            colliderHalfWidth = collider.size.x / 2;
            colliderHalfHeight = collider.size.y / 2;
        }
        rightScreenPos = cam.ViewportToWorldPoint(Vector3.right).x;
        leftightScreenPos = cam.ViewportToWorldPoint(Vector3.zero).x;

    }
    void Update()
    {
        topScreenPos = cam.ViewportToWorldPoint(Vector3.up).y;
        bottomScreenPos = cam.ViewportToWorldPoint(Vector3.zero).y;

        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += movement * speed * Time.deltaTime;

        Inputs();

        //limit movement in x
        if(transform.position.x - colliderHalfWidth > rightScreenPos)
            transform.position = new Vector3(leftightScreenPos - colliderHalfWidth, transform.position.y, transform.position.z);
        else if(transform.position.x + colliderHalfWidth < leftightScreenPos)
            transform.position = new Vector3(rightScreenPos + colliderHalfWidth, transform.position.y, transform.position.z);
        //limit movement in y
        if (transform.position.y + colliderHalfHeight >= topScreenPos)
            transform.position = new Vector3(transform.position.x, topScreenPos - colliderHalfHeight, transform.position.z);
        else if (transform.position.y - colliderHalfHeight <= bottomScreenPos)
            transform.position = new Vector3(transform.position.x, bottomScreenPos + colliderHalfHeight, transform.position.z);
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
        if((collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().GetUser() == Bullet.User.enemy) || collision.gameObject.CompareTag("Enemy_Common") || collision.gameObject.CompareTag("Enemy_Erratic"))
        {
            lives--;
            Destroy(collision.gameObject);
        }
    }
}
