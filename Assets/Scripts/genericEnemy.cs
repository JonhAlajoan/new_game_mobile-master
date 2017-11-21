using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genericEnemy : LivingEntity
{

    public Transform Muzzles;
    public static int quantityMuzzlesUsed;
    public float nextShotTime;
    public float timeBetweenAttacks = 0.1f;
    public float nextAttackTime;
    float countTimeChange;
    float counterChangeMsAttack;


    protected override void Start()
    {
        base.Start();
        
        countTimeChange = 1 * Time.deltaTime;
        counterChangeMsAttack = 1 * Time.deltaTime;
        
        
    }
    private void Awake()
    {
        health = startingHealth;
        msBetweenShots = 1000f;
    }
    void Attack(int colorAttack)
    {
        
        if (Time.time > nextShotTime)
        {
            if(colorAttack == 1)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                GameObject projectileRed = TrashMan.spawn("Bullet_Projectile_Red", Muzzles.transform.position, Muzzles.transform.rotation);
            }

            if (colorAttack == 0)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                GameObject projectile = TrashMan.spawn("Bullet_Projectile", Muzzles.transform.position, Muzzles.transform.rotation);
            }            
        }
    }

    private void Update()
    {
    
        counterChangeMsAttack += 1 * Time.deltaTime;
        countTimeChange += 1 * Time.deltaTime;

        if (counterChangeMsAttack >= 10 && msBetweenShots >= 500)
        {
            msBetweenShots -= 100;
            counterChangeMsAttack = 0;
        }

        if (countTimeChange > 1)
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