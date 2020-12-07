using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] TrailRenderer trailLeft;
    [SerializeField] TrailRenderer trailRight;

    BoxCollider2D collider;
    Vector3 movement;
    Camera cam;

    float colliderHalfWidth;
    float colliderHalfHeight;
    float rightScreenPos;
    float leftightScreenPos;
    float topScreenPos;
    float bottomScreenPos;

    int maxLives;

    bool canTakeDamage = true;

    public static event Action<PlayerController> gameOver;
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
        maxLives = lives;
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
        {
            transform.position = new Vector3(leftightScreenPos - colliderHalfWidth, transform.position.y, transform.position.z);
            trailLeft.Clear();
            trailRight.Clear();
        }
        else if(transform.position.x + colliderHalfWidth < leftightScreenPos)
        {
            transform.position = new Vector3(rightScreenPos + colliderHalfWidth, transform.position.y, transform.position.z);
            trailLeft.Clear();
            trailRight.Clear();
        }
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
        if(((collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().GetUser() != Bullet.User.player) || collision.gameObject.CompareTag("Enemy_Common") || collision.gameObject.CompareTag("Enemy_Erratic")) && canTakeDamage || collision.gameObject.CompareTag("Enemy_Boss"))
        {
            canTakeDamage = false;
            lives--;
            if (lives <= 0)
                gameOver?.Invoke(this);
            StartCoroutine(FlashColors(5));
            Destroy(collision.gameObject);
        }
    }
    public float currentHealthPercentage(int value)
    {
        int maxHealth = 100;
        int maxPercentage = 100;
        return ((value * maxPercentage) / maxHealth);
    }
    public int GetLives()
    {
        return lives;
    }
    public int GetMaxLives()
    {
        return maxLives;
    }
    IEnumerator FlashColors(int times)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        var alpha = sr.color;
        for (int i = 0; i < times; i++)
        {
            alpha.a = 1;
            GetComponent<SpriteRenderer>().color = alpha;
            yield return new WaitForSeconds(0.1f);
            alpha.a = 0;
            GetComponent<SpriteRenderer>().color = alpha;
            yield return new WaitForSeconds(0.1f);
        }
        alpha.a = 1;
        GetComponent<SpriteRenderer>().color = alpha;
        canTakeDamage = true;
        yield return null;
    }
 
}
