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
    public Transform muzzleLeft;
    public Transform muzzleRight;

    public Transform[] MuzzlesBulletPhase2;

    bool isPhase2Active;

    protected override void Start()
    {
        
        base.Start();

        isPhase2Active = false;
        randomBullet = 0;

        countTimeChange = 1 * Time.deltaTime;
        counterChangeMsAttack = 1 * Time.deltaTime;
        canShoot = true;

		boss = GameObject.FindGameObjectWithTag("Enemy");
		animatorBoss2 = boss.GetComponentInChildren<Animator>();


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

    void AttackSecondPhase(int colorAttack)
    {

        if (Time.time > nextShotTime)
        {

            if (colorAttack == 1)
            {
                animatorBoss2.SetTrigger("attack1Phase2");
                StartCoroutine(startProjectile(1));
                nextShotTime = Time.time + msBetweenShots / 1000;
            }

            if (colorAttack == 0)
            {
                animatorBoss2.SetTrigger("attack2Phase2");
                StartCoroutine(startProjectile(0));
                nextShotTime = Time.time + msBetweenShots / 1000;

            }
        }
    }

    IEnumerator startProjectile(int color)
    {
        // 1 é vermelho 0 é verde
        yield return new WaitForSeconds(1f);
        if(color == 0)
        {
            TrashMan.spawn("spawnBulletFx", muzzleRight.transform.position);
            yield return new WaitForSeconds(1f);

            foreach (Transform muzzless in MuzzlesBulletPhase2)
            {
                int random = Random.Range(0, 2);
                if (random == 1)
                {
                    TrashMan.spawn("BombSecondPhaseBoss2_Red", muzzless.transform.position, muzzless.transform.rotation);
                }
                if (random == 0)
                {
                    TrashMan.spawn("BombSecondPhaseBoss2", muzzless.transform.position, muzzless.transform.rotation);
                }
            }
            yield return new WaitForSeconds(4);
        }
        if (color == 1)
        {
            TrashMan.spawn("spawnBulletFx", muzzleLeft.transform.position);
            yield return new WaitForSeconds(1f);
            
            foreach(Transform muzzless in MuzzlesBulletPhase2)
            {
                int random = Random.Range(0, 2);
                if(random == 1)
                {
                    TrashMan.spawn("BombSecondPhaseBoss2_Red", muzzless.transform.position, muzzless.transform.rotation);
                }
                if (random == 0)
                {
                    TrashMan.spawn("BombSecondPhaseBoss2", muzzless.transform.position, muzzless.transform.rotation);
                }
            }
            yield return new WaitForSeconds(4);

        }
        


    }

    IEnumerator startFlameRed()
    {
        yield return new WaitForSeconds(1.8f);
        Vector3 positionToSpawns = muzzle.transform.position + new Vector3(0, 4f, 0);
        GameObject flames = TrashMan.spawn("Flame_Red_Attack1", positionToSpawns, muzzle.transform.rotation);
        flames.transform.parent = muzzle;
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


    private void Update()
    {
        boss = GameObject.FindGameObjectWithTag("Enemy");
        animatorBoss2 = boss.GetComponentInChildren<Animator>();
        counterChangeMsAttack += 1 * Time.deltaTime;
        countTimeChange += 1 * Time.deltaTime;

        if(health <= (startingHealth / 2) || auxHealth <= (startingHealth / 2))
        {
            isPhase2Active = true;
            animatorBoss2.SetTrigger("secondPhase");
        }
        

        if (counterChangeMsAttack >= 10 && msBetweenShots >= 2000)
        {
            msBetweenShots -= 100;
            counterChangeMsAttack = 0;
        }

        
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
                    if (isPhase2Active == true)
                    {
                        nextAttackTime = Time.time + timeBetweenAttacks;
                        int randomType = Random.Range(0, 2);
                        if (randomType == 1)
                        {

                            AttackSecondPhase(1);

                        }
                        if (randomType == 0)
                        {
                            AttackSecondPhase(0);
                        }
                    } 

                    else
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
}
