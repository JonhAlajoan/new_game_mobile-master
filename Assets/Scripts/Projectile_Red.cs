using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Red : MonoBehaviour {

    float speed = 6f;
    int damage = 1;
    public Transform Hit_vfx;
    float lifetime;
    float skinWidth = .1f;
    GameObject target;
    Player damageableObject;
    float count;
    CameraShake cam;

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
        GameObject camSearch = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camSearch.GetComponent<CameraShake>();

        float moveDistance = speed * Time.deltaTime;
        transform.Translate(Vector2.down * moveDistance);

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

    }

    void OnTriggerEnter2D(Collider2D c)
    {

        if (damageableObject != null && damageableObject.tag == "green")
        {
            TrashMan.spawn("Hit_Red", gameObject.transform.position, gameObject.transform.rotation);
            cam.Shake(0.5f,0.3f);
            damageableObject.takeDamage(damage);
        }
        else
        {
            TrashMan.spawn("Hit_Absorbed_Red", target.transform.position, target.transform.rotation);
        }
        TrashMan.despawn(gameObject);
    }
}
