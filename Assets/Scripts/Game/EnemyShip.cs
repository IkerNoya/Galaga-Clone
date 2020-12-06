using System;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] Bullet bullet;
    [SerializeField] ParticleSystem muzzle;
    [SerializeField] float speed;
    [SerializeField] float parabolicMagnitude;
    public static event Action<EnemyShip> onDestroy;
    public static event Action<EnemyShip> killedByPlayer;
    public enum Type
    {
       Common, Agile, Hive
    }
    public Type type;

    GameObject player;
    float timer = 0;
    float timeLimit = 1.5f;
    bool canRespawn;
    int lastSpawnPoint;
    Vector3 direction;

    // Update is called once per frame
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        switch (type)
        {
            case Type.Common:
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
                if (transform.position.y < player.transform.position.y) canRespawn = true;
                else canRespawn = false;
                break;
            case Type.Agile:
                //parabola 
                direction = Vector3.down;
                direction = new Vector3(Mathf.Sin(Time.timeSinceLevelLoad * parabolicMagnitude), direction.y, 0);
                transform.position += direction * speed * Time.deltaTime;
                if (transform.position.y < player.transform.position.y) canRespawn = true;
                else canRespawn = false;
                break;
            case Type.Hive:
                direction = Vector3.down;
                direction = new Vector3(Mathf.Sin(Time.timeSinceLevelLoad * parabolicMagnitude), direction.y, 0);
                transform.position += direction * speed * Time.deltaTime;
                if (transform.position.y < player.transform.position.y) canRespawn = true;
                else canRespawn = false;
                break;
            default:
                break;
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().GetUser() == Bullet.User.player)
        {
            killedByPlayer?.Invoke(this);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("KillZone") || collision.gameObject.CompareTag("Player"))
        {
            onDestroy?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
