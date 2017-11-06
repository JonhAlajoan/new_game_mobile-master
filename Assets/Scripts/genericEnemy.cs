using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genericEnemy : MonoBehaviour
{

    public Transform Muzzles;
    public static int quantityMuzzlesUsed;
    public float msBetweenShots = 100;
    public float nextShotTime;
    public float timeBetweenAttacks = 0.1f;
    public float nextAttackTime;

    public float health;
    protected bool dead;
    public float startingHealth;
    public event System.Action OnDeath;
    float countTimeChange;

    public void Start()
    {
        countTimeChange = 1 * Time.deltaTime;
        health = startingHealth;
    }

    
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {

            //spawnControl = GameObject.FindWithTag ("Spawner");	
            //spawnControl.GetComponent<Spawner>().OnEnemyDeath();
            //scoreUpdt.GetComponent<Score>().updateScoreEnemyDeath();
            //gameObject.GetComponent<LivingEntity> ().randomDropBomb ();
            //gameObject.GetComponent<LivingEntity> ().randomDropWPower();
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
        health = startingHealth;
    }

    void Attack(string colorAttack)
    {
        if (Time.time > nextShotTime)
        {
            if(colorAttack == "red")
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                GameObject projectileRed = TrashMan.spawn("Bullet_Projectile_Red", Muzzles.transform.position, Muzzles.transform.rotation);
            }

            if (colorAttack == "green")
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                GameObject projectile = TrashMan.spawn("Bullet_Projectile", Muzzles.transform.position, Muzzles.transform.rotation);
            }            
        }
    }

    private void Update()
    {
        if (Time.time > nextAttackTime)
        {
            countTimeChange += 1;
            nextAttackTime = Time.time + timeBetweenAttacks;
            if (countTimeChange > 3)
            {
                Attack("red");
                countTimeChange = 0;
            }
            //auxSfxController.PlaySFXSounds("Lotus_Projectile");
            Attack("green");
        }
    }
}