using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] Bullet bullet;
    [SerializeField] ParticleSystem muzzle;
    [SerializeField] float speed;
    [SerializeField] GameObject[] spawners;
    SpriteRenderer sr;
    float timer = 0;
    float timeLimit = 1.5f;
    Vector3 direction;

    // Update is called once per frame
    void Start()
    {
        spawners = GameObject.FindGameObjectsWithTag("Spawners");
        Respawn();
    }
    void Update()
    {
        //parabola 
        direction = Vector3.down;

        transform.position += direction * speed * Time.deltaTime; 
        timer += Time.deltaTime;
        if (timer >= timeLimit)
        {
            timer = 0;
            bullet.GetComponent<Bullet>().SetUser(Bullet.User.enemy);
            muzzle.Play();
            Instantiate(bullet, gun.transform.position, Quaternion.identity);
        }
    }
    void Respawn()
    {
        if(spawners!=null)
            transform.position = spawners[Random.Range(0, 3)].transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().GetUser() == Bullet.User.player)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        Respawn();
    }
}
