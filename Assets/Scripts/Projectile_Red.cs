using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Red : MonoBehaviour {

    float speed = 5f;
    int damage = 1;
    public Transform Hit_vfx;
    float lifetime;
    float skinWidth = .1f;
    GameObject target;
    Player damageableObject;
    float count;
    void Start()
    {
        count = 1;
    }


    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {

        target = GameObject.FindGameObjectWithTag("Player");
        damageableObject = target.GetComponent<Player>();
        lifetime += 1 * Time.deltaTime;

        if (lifetime > 3)
        {
            TrashMan.despawn(gameObject);
            lifetime = 0;
        }
        float moveDistance = speed * Time.deltaTime;
        transform.Translate(Vector2.left * moveDistance);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log("tocou");
        if (damageableObject != null)
        {
            TrashMan.spawn("Hit", gameObject.transform.position, gameObject.transform.rotation);
            damageableObject.takeDamage(damage);
        }
        TrashMan.despawn(gameObject);
    }
}
