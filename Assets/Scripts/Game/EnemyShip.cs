using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] Bullet bullet;
    [SerializeField] ParticleSystem muzzle;
    float timer = 0;
    float timeLimit = 1.5f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeLimit)
        {
            timer = 0;
            bullet.GetComponent<Bullet>().SetUser(Bullet.User.enemy);
            muzzle.Play();
            Instantiate(bullet, gun.transform.position, Quaternion.identity);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().GetUser() == Bullet.User.player)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
