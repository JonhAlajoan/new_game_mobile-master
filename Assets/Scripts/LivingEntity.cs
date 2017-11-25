using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LivingEntity : MonoBehaviour {

    public float startingHealth;
    protected float health;
    protected bool dead;
   
    public event System.Action OnDeath;
    public float auxHealth;
    public float msBetweenShots;
    GameObject player;
    Player target;
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
            Debug.Log("não encontrado");
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
                Die();
        }
    }

    [ContextMenu("Self Destruct")]
    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        TrashMan.despawn(gameObject);
        dead = false;
        health = startingHealth;
    }
}

