using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LivingEntity : MonoBehaviour {

    public float startingHealth;
    protected float health;
    protected bool dead;
    public  bool canShoot;
   
    public event System.Action OnDeath;
    public float auxHealth;
    public float msBetweenShots;
    GameObject player;
    Player target;

    GameObject enemyObject;
    public Animator animatorEnemy;

    GameObject sceneManagerObject;
    enemyManager enemyManager;

    bool resetTheMSBetweenAttacks;
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("green");

        try
        {
            target = player.GetComponent<Player>();
        }
        catch(NullReferenceException)
        {
            Debug.Log("Player");
        }

        try
        {
            sceneManagerObject = GameObject.FindGameObjectWithTag("SceneManager");
            enemyManager = sceneManagerObject.GetComponent<enemyManager>();

            enemyObject = GameObject.FindGameObjectWithTag("Enemy");
            animatorEnemy = enemyObject.GetComponentInChildren<Animator>();
        }
        catch(NullReferenceException)
        {
            Debug.Log("erros");
        }
        

        health = startingHealth;
        auxHealth = health;   
        //spawnControlAux = spawnControl.GetComponent<Spawner> ().enemiesRemainingAlive;
    }

    public void SetMsBetweenAttacks(float msBetweenAttack)
    {
        msBetweenShots = msBetweenAttack;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        auxHealth = health;

        if (health <= 0 && !dead)
        {

            /*spawnControl.GetComponent<Spawner>().OnEnemyDeath();
            scoreUpdt.GetComponent<Score>().updateScoreEnemyDeath();
            gameObject.GetComponent<LivingEntity>().randomDropBomb();
            gameObject.GetComponent<LivingEntity>().randomDropWPower();
            */
            ManagerScene updateScore = sceneManagerObject.GetComponent<ManagerScene>();

            updateScore.score += 100;

            animatorEnemy.SetTrigger("death");
			

            if (animatorEnemy.isInitialized)
            {
                StartCoroutine("destruction");
				TrashMan.spawn("VFX_DEATH_BOSS", transform.position, transform.rotation);
			}

            Die();
        }
    }

    IEnumerator destruction()
    {
        canShoot = false;
        yield return new WaitForSeconds(10f);
        enemyManager.updateEnemy();
        enemyManager.needToSpawnEnemy = true;

        dead = false;
        health = startingHealth;
        TrashMan.despawn(gameObject);

    }

    [ContextMenu("Self Destruct")]
    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        
    }
}

