using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 direction;
    public enum User
    {
        enemy, player
    }
    public User user;
    void Start()
    {
        if (user == User.player)
            direction = Vector3.up;
        if (user == User.enemy)
            direction = Vector3.down;
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
