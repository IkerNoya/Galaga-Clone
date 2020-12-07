using System;
using System.Collections;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] GameObject gunMid;
    [SerializeField] GameObject gunLeft;
    [SerializeField] GameObject gunRight;
    [SerializeField] Bullet bullet;
    [SerializeField] Bullet bulletParabole;
    [SerializeField] ParticleSystem muzzle;
    [SerializeField] float speed;
    [SerializeField] float parabolicMagnitude;
    [SerializeField] int hp;
    public static event Action<EnemyShip> onDestroy;
    public static event Action<EnemyShip> killedByPlayer;
    public static event Action<EnemyShip> bossDeath;
    Animator anim;
    public enum Type
    {
       Common, Agile, MiniBoss, Boss
    }
    public Type type;

    GameObject player;
    BoxCollider2D collider;

    float timer = 0;
    float timerParabolicBullet = 0;
    float timeLimit = 1.5f;
    float bossTimeLimit = 1;
    float bossParaboleTimeLimit = 1.5f;
    float colliderHalfWidth;
    float rightScreenPos;
    float leftightScreenPos;

    int damage = 50;

    bool isDead = false;

    Vector3 direction;
    Camera cam;


    void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
        collider = GetComponent<BoxCollider2D>();
        if(collider!=null)
            colliderHalfWidth = collider.bounds.size.x / 2;
        rightScreenPos = cam.ViewportToWorldPoint(Vector3.right).x;
        leftightScreenPos = cam.ViewportToWorldPoint(Vector3.zero).x;
        player = GameObject.FindGameObjectWithTag("Player");
        if (type == Type.Boss)
            direction = new Vector3(1, 1, 0);
    }
    void Update()
    {
        if (isDead)
            return;
        switch (type)
        {
            case Type.Common:
                direction = Vector3.down;
                transform.position += direction * speed * Time.deltaTime;
                if (timer >= timeLimit)
                {
                    timer = 0;
                    bullet.GetComponent<Bullet>().SetUser(Bullet.User.enemy);
                    muzzle.Play();
                    Instantiate(bullet, gunMid.transform.position, Quaternion.identity);
                }
                break;
            case Type.Agile:
                //parabola 
                direction = Vector3.down;
                direction = new Vector3(Mathf.Sin(Time.timeSinceLevelLoad * parabolicMagnitude), direction.y, 0);
                transform.position += direction * speed * Time.deltaTime;
                break;
            case Type.MiniBoss:
                break;
            case Type.Boss:
                if (transform.position.x + colliderHalfWidth> rightScreenPos)
                {
                    direction = new Vector3(-1, 1, 0);
                }
                else if (transform.position.x - colliderHalfWidth < leftightScreenPos)
                {
                    direction = new Vector3(1, 1, 0);
                }
                transform.position += direction * speed * Time.deltaTime;
                if (timer >= bossTimeLimit)
                {
                    timer = 0;
                    bulletParabole.GetComponent<Bullet>().SetUser(Bullet.User.boss);
                    bullet.GetComponent<Bullet>().SetUser(Bullet.User.enemy);
                    muzzle.Play();
                    Instantiate(bullet, gunMid.transform.position, Quaternion.identity);
                    if(timerParabolicBullet >= bossParaboleTimeLimit)
                    {
                        timerParabolicBullet = 0;
                        bossParaboleTimeLimit = UnityEngine.Random.Range(0, 1.5f);
                        Instantiate(bulletParabole, gunLeft.transform.position, Quaternion.identity);
                        Instantiate(bulletParabole, gunRight.transform.position, Quaternion.identity);
                    }
                }
                break;
            default:
                break;
        }
        timerParabolicBullet += Time.deltaTime;
        timer += Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().GetUser() == Bullet.User.player)
        {
            hp -= damage;
            if (type != Type.Boss)
                StartCoroutine(FlashColors());
            else
                StartCoroutine(FlashColorsBoss(1));
            if (hp <= 0)
            {
                if (type != Type.Boss)
                {
                    Destroy(gameObject);
                    killedByPlayer?.Invoke(this);
                }
                else
                {
                    isDead = true;
                    if(anim!=null)
                        anim.SetBool("expl", true);
                    bossDeath?.Invoke(this);
                    StartCoroutine(WaitForExplotion(2));
                }
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("KillZone") || collision.gameObject.CompareTag("Player"))
        {
            onDestroy?.Invoke(this);
            Destroy(gameObject);
        }
    }
    IEnumerator FlashColors()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        var alpha = sr.color;
        alpha.a = 0;
        sr.color = alpha;
        yield return new WaitForSeconds(0.1f);
        alpha.a = 1;
        sr.color = alpha;
        yield return null;
    }
    IEnumerator FlashColorsBoss(int times)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        for (int i = 0; i < times; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        sr.color = Color.white;
        yield return null;
    }
    IEnumerator WaitForExplotion(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
