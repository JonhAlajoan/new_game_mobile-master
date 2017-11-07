using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------Script para controle do main character----------------------

public class Player : MonoBehaviour {

    /* -----------ATRIBUTOS----------
     * health: Quantidade de hp que o player terá (normal = 3)
     * actualColor: Flag que controla se a cor atual é verde ou vermelha (0 = vermelho, 1 = verde).
     * shieldMuzzle: Local onde os novos shields serão spawnados.
     * shield: Shield sendo utilizado no momento
     * modifiableColor: Array de sprites que é percorrido num foreach para modificar a cor do player
     * shieldColor: pega o particleSystem do shield atual
     * mainShield: Módulo principal do mainShield, necessário para modificar a cor por não conseguir fazer isso diretamente.
     * colorStart = Cor inicial do player
     * colorStartShield = Cor inicial do shield
     * event onDeath = Evento de morte do player que ativa a função die() para indicar a morte do player
     * dead: Bool para informar melhor a situação de vida e morte do player
     */
    int health;
    int actualColor;

    public Transform shieldMuzzle;
    public GameObject shield;    

    public SpriteRenderer[] modifiableColor;

    ParticleSystem shieldColor;
    ParticleSystem.MainModule mainShield;

    Color32 colorStart = new Color32(60, 255, 142, 255);
    Color colorStartShield = new Color(0.23529f, 1.00000f, 0.55686f);

    public event System.Action OnDeath;

    bool dead;

    void Start () {

       
        //actual color: 1 = vermelho, 0 = verde;
        actualColor = 0;
        health = 3;
        //setando a cor inicial pra verde   
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
