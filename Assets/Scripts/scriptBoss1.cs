using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptBoss1 : LivingEntity {

    public float nextShotTime;
    public float timeBetweenAttacks = 0.1f;
    public float nextAttackTime;
    float countTimeChange;
    float counterChangeMsAttack;

    Color32 colorStart = new Color32(60, 255, 142, 255);
    Color colorStartShield = new Color(0.23529f, 1.00000f, 0.55686f);

    ParticleSystem weaponColor;
    ParticleSystem.MainModule mainWeapon;
    public GameObject[] weapons;

    public GameObject boss;
    Animator animatorBoss1;
    int randomBullet;

    protected override void Start()
    {
        randomBullet = 0;
        base.Start();
        boss = GameObject.FindGameObjectWithTag("Enemy");
        animatorBoss1 = boss.GetComponentInChildren<Animator>();

        countTimeChange = 1 * Time.deltaTime;
        counterChangeMsAttack = 1 * Time.deltaTime;
        canShoot = true;


    }
    private void Awake()
    {
        health = startingHealth;
        msBetweenShots = 4000f;
    }

    

    void Attack(int colorAttack)
    {

        if (Time.time > nextShotTime)
        {
            
            if (colorAttack == 1)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                changeColor(1);
                animatorBoss1.SetTrigger("attackLeft");
                
            }

            if (colorAttack == 0)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                changeColor(0);
                animatorBoss1.SetTrigger("attackRight");
            }
        }
    }

   public void changeColor(int color)
    {
        
        foreach(GameObject weapon in weapons)
        {
            weaponColor = weapon.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainWeapon = weaponColor.main;

            if (color == 1)
            {
                mainWeapon.startColor = new Color(1.00000f,0f, 0f);

            }

            if (color == 0)
            {
                mainWeapon.startColor = new Color(0.00000f,  0.59608f,  0.25098f);
            }
        }            
           
    }

    private void Update()
    {
        counterChangeMsAttack += 1 * Time.deltaTime;
        countTimeChange += 1 * Time.deltaTime;

        if (counterChangeMsAttack >= 10 && msBetweenShots >= 2000)
        {
            msBetweenShots -= 500;
            counterChangeMsAttack = 0;
        }

        if (countTimeChange > 1)
        {
            if(canShoot == true)
            {
                if (Time.time > nextAttackTime)
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    int randomBullet = Random.Range(0, 2);
                    if (randomBullet == 1)
                    {

                        Attack(1);

                    }
                    if (randomBullet == 0)
                    {
                        Attack(0);
                    }

                }
            }
            
        }
    }
}
