using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    float extraSpeed = 1;
    Vector3 direction;
    public enum User
    {
        enemy, player
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
        }
        
        Destroy(gameObject, 1f);
    }

    void Update()
    {
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
