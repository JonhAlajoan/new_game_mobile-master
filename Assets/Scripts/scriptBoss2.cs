using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptBoss2 : LivingEntity {

    public float nextShotTime;
    public float timeBetweenAttacks = 0.1f;
    public float nextAttackTime;
    float countTimeChange;
    float counterChangeMsAttack;

    public GameObject boss;
    public Animator animatorBoss2;
    int randomBullet;

    public Transform muzzle;

    protected override void Start()
    {
        randomBullet = 0;
        base.Start();
        

        countTimeChange = 1 * Time.deltaTime;
        counterChangeMsAttack = 1 * Time.deltaTime;
        canShoot = true;


    }
    private void Awake()
    {
        health = startingHealth;
        msBetweenShots = 6000f;
    }



    void Attack(int colorAttack)
    {

        if (Time.time > nextShotTime)
        {
            
            if (colorAttack == 1)
            {
                animatorBoss2.SetTrigger("attack1");
                StartCoroutine("startFlameRed");
                GameObject chargeAttack = TrashMan.spawn("Charge_Attack1_Boss2", muzzle.transform.position, muzzle.transform.rotation);
                chargeAttack.transform.parent = muzzle;
                nextShotTime = Time.time + msBetweenShots / 1000;
                

            }

            if (colorAttack == 0)
            {
                animatorBoss2.SetTrigger("attack1");
                StartCoroutine("startFlameGreen");
                GameObject chargeAttack = TrashMan.spawn("Charge_Attack1_Boss2_Green", muzzle.transform.position, muzzle.transform.rotation);
                chargeAttack.transform.parent = muzzle;
                nextShotTime = Time.time + msBetweenShots / 1000;
                
            }
        }
    }

    IEnumerator startFlameRed()
    {
        yield return new WaitForSeconds(1.8f);
        Vector3 positionToSpawn = muzzle.transform.position + new Vector3(0, 4f, 0);
        GameObject flame = TrashMan.spawn("Flame_Red_Attack1", positionToSpawn, muzzle.transform.rotation);
        flame.transform.parent = muzzle;
        yield return new WaitForSeconds(1f);
    }

    IEnumerator startFlameGreen()
    {
        yield return new WaitForSeconds(1.8f);
        Vector3 positionToSpawn = muzzle.transform.position + new Vector3(0, 4f, 0);
        GameObject flame = TrashMan.spawn("Flame_Green_Attack1", positionToSpawn, muzzle.transform.rotation);
        flame.transform.parent = muzzle;
        yield return new WaitForSeconds(1f);

    }

    /*
    IEnumerator Attack(int colorAttack)
    {
        yield return new WaitForSeconds(2f);
        if (colorAttack == 1)
        {
            animatorBoss2.SetTrigger("attack1");
            GameObject chargeAttack = TrashMan.spawn("Charge_Attack1_Boss2_Red", muzzle.transform.position, muzzle.transform.rotation);
            chargeAttack.transform.parent = muzzle;
            nextShotTime = Time.time + msBetweenShots / 1000;

        }

        if (colorAttack == 0)
        {
            animatorBoss2.SetTrigger("attack1");
            GameObject chargeAttack = TrashMan.spawn("Charge_Attack1_Boss2_Green", muzzle.transform.position, muzzle.transform.rotation);
            chargeAttack.transform.parent = muzzle;
            nextShotTime = Time.time + msBetweenShots / 1000;
        }
        yield return new WaitForSeconds(5f);
        canShoot = true;
    }*/

    private void Update()
    {
        boss = GameObject.FindGameObjectWithTag("Enemy");
        animatorBoss2 = boss.GetComponentInChildren<Animator>();
        counterChangeMsAttack += 1 * Time.deltaTime;
        countTimeChange += 1 * Time.deltaTime;

        if (counterChangeMsAttack >= 10 && msBetweenShots >= 2000)
        {
            msBetweenShots -= 100;
            counterChangeMsAttack = 0;
        }

        /*if (countTimeChange > 4)
        {
           
            if (canShoot == true)
            {
              
                    
                    int randomBullet = Random.Range(0, 2);

                    if (randomBullet == 1)
                    {

                        StartCoroutine(Attack(1));
                        countTimeChange = 0;
                    canShoot = false;
                    }
                    if (randomBullet == 0)
                    {
                        StartCoroutine(Attack(0));
                        countTimeChange = 0;
                    canShoot = false;
                    }

                
            }

        }*/
        if (counterChangeMsAttack >= 10 && msBetweenShots >= 2000)
        {
            msBetweenShots -= 500;
            counterChangeMsAttack = 0;
        }

        if (countTimeChange > 4)
        {
            if (canShoot == true)
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
