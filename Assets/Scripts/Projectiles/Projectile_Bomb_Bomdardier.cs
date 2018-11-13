using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bomb_Bomdardier : MonoBehaviour {

    public  float speed;
    float lifetime;
    GameObject target;
    CameraShake cam;
    int randomDir;

    private void Start()
    {
        speed = 4f;
        lifetime = 1 * Time.deltaTime;
        randomDir = Random.Range(0, 2);
    }
    void Update () {

        

        GameObject target = GameObject.FindGameObjectWithTag("Enemy");
        float moveDistance = speed * Time.deltaTime;
        transform.Translate(Vector2.up * moveDistance);

        GameObject camSearch = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camSearch.GetComponent<CameraShake>();

        if (randomDir == 0)
            transform.Translate(Vector2.left * moveDistance);
        if (randomDir == 1)
            transform.Translate(Vector2.right * moveDistance);

        lifetime += 1 * Time.deltaTime;
        if (lifetime >= 1 && lifetime<3)
        {
            cam.Shake(0.08f, 1f, 1f);
            gameObject.transform.up = target.transform.position - gameObject.transform.position;
            speed -= 1 * Time.deltaTime;                       
        }

      

        if (lifetime >= 3.6f)
        {
            cam.Shake(0.5f, 0.7f, 1.9f);
            TrashMan.spawn("Explosion_Bombardier", gameObject.transform.position, gameObject.transform.rotation);
            
            lifetime = 0;
			//target = null;
            speed = 4f;
            TrashMan.despawn(gameObject);
        }
	}
}
