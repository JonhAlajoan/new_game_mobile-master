using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    int health;
    public Transform shieldMuzzle;
    public GameObject shield;
    int actualColor;
    public SpriteRenderer[] modifiableColor;
    float count;
    int randomColor;
    float duration;
    ParticleSystem shieldColor;
    ParticleSystem.MainModule mainShield;
    Color32 colorStart = new Color32(60, 255, 142, 255);
    Color colorStartShield = new Color(0.23529f, 1.00000f, 0.55686f);
    public event System.Action OnDeath;
    bool dead;
    void Start () {
        
        duration = 1 * Time.deltaTime;
        //no actual color: 1 = vermelho, 0 = verde;
        actualColor = 0;
        health = 3;
        foreach (SpriteRenderer sprite in modifiableColor)
        {
            sprite.color = colorStart;
        }
    }

    void changeColorPlayer(int color)
    {
        if (color == 0)
        {
            foreach (SpriteRenderer sprite in modifiableColor)
            {
                //cor vermelha
                sprite.color = Color32.Lerp(new Color32(60, 255, 142, 255),new Color32(255, 60, 60, 255), 1f);
                gameObject.tag = "red";
            }
        }
        

        if (color == 1)
        {
            foreach (SpriteRenderer sprite in modifiableColor)
            {
                //lerp para cor verde
                 sprite.color = Color32.Lerp(new Color32(255, 60, 60, 255), new Color32(60, 255, 142, 255), 1f);
                gameObject.tag = "green";
            }

        }
    }
    void changeColorShield(int color)
    {
        shieldColor = shield.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainShield = shieldColor.main;
        if (color == 1)
        {
            mainShield.startColor = new Color(0.23529f, 1.00000f, 0.55686f);
            actualColor = 0;
        }

        if(color == 0)
        {
            mainShield.startColor = new Color(1.00000f, 0.23529f, 0.23529f);
            actualColor = 1;
        }        
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health < 3)
        {
            TrashMan.despawn(shield);
            GameObject newShield;
            newShield = TrashMan.spawn("Shield_Breaking", shieldMuzzle.position, shieldMuzzle.rotation);
            shield = newShield;

            if (GameObject.FindGameObjectWithTag("red"))
            {
                shieldColor = shield.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule mainShield = shieldColor.main;
                mainShield.startColor = new Color(1.00000f, 0.23529f, 0.23529f);
            }

            if (GameObject.FindGameObjectWithTag("green"))
            {
                shieldColor = shield.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule mainShield = shieldColor.main;
                mainShield.startColor = new Color(0.23529f, 1.00000f, 0.55686f);
            }
        }

        if (health < 2)
        {
            TrashMan.despawn(shield);
            GameObject newShield;
            newShield = TrashMan.spawn("Shield_Fim", shieldMuzzle.position, shieldMuzzle.rotation);
            shield = newShield;

            if (GameObject.FindGameObjectWithTag("red"))
            {
                shieldColor = shield.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule mainShield = shieldColor.main;
                mainShield.startColor = new Color(1.00000f, 0.23529f, 0.23529f);
            }

            if (GameObject.FindGameObjectWithTag("green"))
            {
                shieldColor = shield.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule mainShield = shieldColor.main;
                mainShield.startColor = new Color(0.23529f, 1.00000f, 0.55686f);
            }
        }

        if (health <= 0 && !dead)
        {
            Die();
        }
        
    }

    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        //TrashMan.spawn ("FireDestruct", gameObject.transform.position, gameObject.transform.rotation * flipSpawn);
        TrashMan.despawn(gameObject);
        dead = false;
        health = 3;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            changeColorPlayer(actualColor);
            changeColorShield(actualColor);
            Debug.Log(actualColor);
        }
    }

}
