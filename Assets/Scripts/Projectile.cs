using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


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

        if (GameObject.FindGameObjectWithTag("green"))
        {
            target = GameObject.FindGameObjectWithTag("green");
        }

        if (GameObject.FindGameObjectWithTag("red"))
        {
            target = GameObject.FindGameObjectWithTag("red");
        }

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
       
        if (damageableObject != null && damageableObject.tag=="red")
        {
            TrashMan.spawn("Hit", gameObject.transform.position, gameObject.transform.rotation);
            damageableObject.takeDamage(damage);            
        }

        TrashMan.despawn(gameObject); 
    }

}
