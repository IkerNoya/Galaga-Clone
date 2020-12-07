using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    float extraSpeed = 1;
    float paraboleMagnitude;
    Vector3 direction;
    public enum User
    {
        enemy, player, boss
    }
    public User user;
    void Start()
    {
        switch (user)
        {
            case User.player:
                speed += extraSpeed;
                direction = Vector3.up;
                break;
            case User.enemy:
                direction = Vector3.down;
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case User.boss:
                direction = Vector3.down;
                paraboleMagnitude = Random.Range(3, 5.5f);
                break;
        }
        
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        if(user == User.boss) direction = new Vector3(Mathf.Sin(Time.timeSinceLevelLoad * paraboleMagnitude), direction.y, 0);
        transform.position += direction * speed * Time.deltaTime;
    }
    public void SetUser(User _user)
    {
        user = _user;
    }
    public User GetUser()
    {
        return user;
    }
}
