using System;
using System.Collections;
using System.Threading;
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
       Common, Agile, MiniBoss, MidBoss, LateBoss
    }
    public Type type;
    GameObject player;
    BoxCollider2D collider;

    float timer = 0;
    float timerParabolicBullet = 0;
    float timeLimit = 1.5f;
    float bossTimeLimit = 0.5f;
    float bossParaboleTimeLimit = 1.5f;
    float colliderHalfWidth;
    float rightScreenPos;
    float leftightScreenPos;
    float shootCooldownMid = 5;
    float shootCooldownLate = 3;
    float shootTimer = 0;
    float cooldownTimer = 0;
    float shootLateLimit = 8;
    float shootMidLimit = 6;


    int damage = 50;

    bool isDead = false;
    bool canShoot = true;

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
        if (type == Type.MidBoss || type == Type.LateBoss)
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
                // testear si es necesario agregar o no minijefes
                break;
            case Type.MidBoss:
                if (transform.position.x + colliderHalfWidth> rightScreenPos)
                {
                    direction = new Vector3(-1, 1, 0);
                }
                else if (transform.position.x - colliderHalfWidth < leftightScreenPos)
                {
                    direction = new Vector3(1, 1, 0);
                }
                transform.position += direction * speed * Time.deltaTime;
                if (timer >= bossTimeLimit && canShoot)
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
                if (shootTimer >= shootMidLimit)
                {
                    canShoot = false;
                    StartCoroutine(ShootCooldown(shootCooldownMid));
                }
                shootTimer += Time.deltaTime;
                break;

            case Type.LateBoss:
                if (transform.position.x + colliderHalfWidth > rightScreenPos)
                {
                    direction = new Vector3(-1, 1, 0);
                }
                else if (transform.position.x - colliderHalfWidth < leftightScreenPos)
                {
                    direction = new Vector3(1, 1, 0);
                }
                transform.position += direction * speed * Time.deltaTime;
                if (timer >= bossParaboleTimeLimit && canShoot)
                {
                    timer = 0;
                    bulletParabole.GetComponent<Bullet>().SetUser(Bullet.User.boss);
                    bullet.GetComponent<Bullet>().SetUser(Bullet.User.enemy);
                    muzzle.Play();
                    bossParaboleTimeLimit = UnityEngine.Random.Range(0, 1.5f);
                    Instantiate(bulletParabole, gunMid.transform.position, Quaternion.identity);
                    Instantiate(bulletParabole, gunLeft.transform.position, Quaternion.identity);
                    Instantiate(bulletParabole, gunRight.transform.position, Quaternion.identity);
                    
                }
                if (shootTimer >= shootLateLimit)
                {
                    canShoot = false;
                    StartCoroutine(ShootCooldown(shootCooldownLate));
                }
                shootTimer += Time.deltaTime;
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
            if (type == Type.MidBoss || type == Type.LateBoss)
                StartCoroutine(FlashColorsBoss(1));
            else
                StartCoroutine(FlashColors());

            if (hp <= 0)
            {
                if(type == Type.MidBoss || type == Type.LateBoss)
                {
                    isDead = true;
                    if(anim!=null)
                        anim.SetBool("expl", true);
                    bossDeath?.Invoke(this);
                    StartCoroutine(WaitForExplotion(2));
                }
                else
                {
                    Destroy(gameObject);
                    killedByPlayer?.Invoke(this);
                }
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            killedByPlayer?.Invoke(this);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("KillZone"))
        {
            onDestroy?.Invoke(this);
            gameObject.SetActive(false);
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
    IEnumerator ShootCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true;
        shootTimer = 0;
        yield return null;
    }
}
