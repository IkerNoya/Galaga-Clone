using System;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] Bullet bullet;
    [SerializeField] ParticleSystem muzzle;
    [SerializeField] float speed;
    [SerializeField] float parabolicMagnitude;
    [SerializeField] GameObject[] spawners;
    public static event Action<EnemyShip> onDestroy;
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
        spawners = GameObject.FindGameObjectsWithTag("Spawners");
        player = GameObject.FindGameObjectWithTag("Player");
        Respawn();
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
    void Respawn()
    {
        if (spawners != null)
        {
            int spawn = UnityEngine.Random.Range(0, 3);
            if (lastSpawnPoint != spawn)
            {
                transform.position = spawners[spawn].transform.position;
                lastSpawnPoint = spawn;
            }
            else
            {
                if (spawn >= 3)
                    spawn--;
                else if (spawn <= 0)
                    spawn++;
                transform.position = spawners[spawn].transform.position;
                lastSpawnPoint = spawn;
            }
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
