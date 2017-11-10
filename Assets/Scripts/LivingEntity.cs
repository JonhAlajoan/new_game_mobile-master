using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour {

    public float startingHealth;
    protected float health;
    protected bool dead;
   
    public event System.Action OnDeath;
    public float auxHealth;

    protected virtual void Start()
    {
        health = startingHealth;
        auxHealth = health;
        Debug.Log(startingHealth);
        //spawnControlAux = spawnControl.GetComponent<Spawner> ().enemiesRemainingAlive;
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
        GameObject.Destroy(gameObject);
    }
}

