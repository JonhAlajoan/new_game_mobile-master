﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;
//--------------------------------------Script para controle do main character----------------------

public class Player : MonoBehaviour
{

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

    #region Attributes (variables)
    public int health;
    int actualColor;

    public Transform[] muzzleShoot;
    public Transform shieldMuzzle;
    public GameObject shield;
    GameObject sceneManager;

    public SpriteRenderer[] modifiableColor;

    ParticleSystem shieldColor;
    ParticleSystem.MainModule mainShield;

    Color32 colorStart = new Color32(60, 255, 142, 255);
    Color colorStartShield = new Color(0.23529f, 1.00000f, 0.55686f);

    public event System.Action OnDeath;
    public int numberProjectiles;
    public int spaceshipUsed;
    public int delayBetweenAttack;

    public float count;
    public float timeBetweenAttacks;
    public float countRegeneration;
    public float countRemoveMoreMSBetweenShots;

    bool dead;
    bool canSpawnChargeAttack;
    public ManagerScene managerGetVariables;
    GameObject enemy;
    LivingEntity target;
    GameObject managerObject;

    public bool canRemoveMore = true;
    public bool canReviveMore;
    public bool canRegenerate;
    public bool canReviveAndShowAds;

	public float speed;
	bool move;
	Vector3 targetMove;



    #endregion

    void Start()
    {
		speed = 30;
        timeBetweenAttacks = 1 * Time.deltaTime;
        count = 1 * Time.deltaTime;
        countRegeneration = 10;
        countRemoveMoreMSBetweenShots = 10;
       

        //actual color: 1 = vermelho, 0 = verde;
        actualColor = 0;
        health = 3;
        canSpawnChargeAttack = true;
        canReviveMore = true;
        canRegenerate = true;
        canReviveAndShowAds = true;

        //setando a cor inicial pra verde   
        foreach (SpriteRenderer sprite in modifiableColor)
        {
            sprite.color = colorStart;
        }

        GameObject newShield;
        newShield = TrashMan.spawn("Shield_UP", shieldMuzzle.position, shieldMuzzle.rotation);
        shield = newShield;

        managerObject = GameObject.FindGameObjectWithTag("SceneManager");
        managerGetVariables = managerObject.GetComponent<ManagerScene>();

        spaceshipUsed = managerGetVariables.GetComponent<ManagerScene>().typeOfSpaceshipBeingUsed;
        numberProjectiles = managerGetVariables.GetComponent<ManagerScene>().numProjectiles;
        delayBetweenAttack = managerGetVariables.GetComponent<ManagerScene>().delayBetweenAttacks;


        if (spaceshipUsed == 7)
        {
            health -= 1;
        }
    }

    #region Color Control Functions
    void changeColorPlayer(int color)
    {
        if (color == 0)
        {
            foreach (SpriteRenderer sprite in modifiableColor)
            {
                //cor vermelha
                sprite.color = Color32.Lerp(new Color32(60, 255, 142, 255), new Color32(255, 60, 60, 255), 1f);
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

        if (color == 0)
        {
            mainShield.startColor = new Color(1.00000f, 0.23529f, 0.23529f);
            actualColor = 1;
        }
    }
    #endregion

    #region Damage and Death Control
    public void takeDamage(int damage)
    {
        if (spaceshipUsed == 2)
        {
            damage += 1;
        }

        health -= damage;

        if (health == 2)
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

        if (health <= 1 && spaceshipUsed == 1 && canReviveMore == true)
        {
            TrashMan.spawn("Clear_Screen", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
            health = 1;
            canReviveMore = false;
        }

        if (health == 1)
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


    public void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        //TrashMan.spawn ("FireDestruct", gameObject.transform.position, gameObject.transform.rotation * flipSpawn);
        GameObject canvasGameOver = managerGetVariables.canvasGameOver;
        canvasGameOver.SetActive(true);
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
        ManagerScene manageSceneVariableChanger = sceneManager.GetComponent<ManagerScene>();
        manageSceneVariableChanger.isPlayerAlive = false;

        TrashMan.despawn(gameObject);
        dead = false;
        health = 3;

    }
    #endregion

    void AttackOrBuffsBasedOnTypeOfSpaceship()
    {
        switch (spaceshipUsed)
        {
            //case 1: Default spaceship the one without any kind of buffs
            case 0:

                if (timeBetweenAttacks > 1f)
                {
					TrashMan.spawn("Instantiated_Bullet", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
					Attack("Projectile_Player", numberProjectiles);
					count = 0;
					timeBetweenAttacks = 0;
					canSpawnChargeAttack = true;
					// count += 1 * Time.deltaTime;

					/*if (canSpawnChargeAttack == true)
                    {
                        TrashMan.spawn("Charge_Attack", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        canSpawnChargeAttack = false;
                    }*/

					//ParticleSystem particleChargeAttack = chargeAttack.GetComponent<ParticleSystem>();
					/*if (count > 2)
                    {
                        
                    }*/
                }
                break;

            //case 2: The spaceship who gets doubleDamage and causes double damage:
            case 2:
                if (timeBetweenAttacks > (10 - delayBetweenAttack))
                {
                    count += 1 * Time.deltaTime;
                    /*GameObject chargeAttack =*/
                    if (canSpawnChargeAttack == true)
                    {
                        TrashMan.spawn("Charge_Attack", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        canSpawnChargeAttack = false;
                    }

                    //ParticleSystem particleChargeAttack = chargeAttack.GetComponent<ParticleSystem>();
                    if (count > 2)
                    {
                        int numProjectiless = numberProjectiles * 2;
                        TrashMan.spawn("Instantiated_Bullet", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        Attack("Projectile_Player", numProjectiless);
                        count = 0;
                        timeBetweenAttacks = 0;
                        canSpawnChargeAttack = true;
                    }
                }
                break;

            //case 3: The bombardier spaceship: It does spawn a set of bombs that triples the damage instead of normal projectiles but will only spawn 1/4 of the projectiles
            case 6:
                int newDelay =  (10 - delayBetweenAttack) + 3;
                if (timeBetweenAttacks > newDelay)
                {
                    count += 1 * Time.deltaTime;
                    /*GameObject chargeAttack =*/
                    if (canSpawnChargeAttack == true)
                    {
                        TrashMan.spawn("Charge_Attack", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        canSpawnChargeAttack = false;
                    }

                    //ParticleSystem particleChargeAttack = chargeAttack.GetComponent<ParticleSystem>();
                    if (count > 2)
                    {
                        int numBombs = numberProjectiles / 2;

                        TrashMan.spawn("Instantiated_Bullet", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        if (numberProjectiles <= 4)
                        {
                            Attack("Bomb_Bombardier", 1);
                        }

                        if (numberProjectiles > 4)
                        {
                            Attack("Bomb_Bombardier", numBombs);
                        }

                        count = 0;
                        timeBetweenAttacks = 0;
                        canSpawnChargeAttack = true;
                    }
                }
                break;

            //case 4: Frozen is the spaceship that decreases the enemy attack speed
            case 3:
                //that part of the code gets the miliseconds between each shot of the enemy and then subtracts 100 from it

                float msBetweenAttacksEnemy = target.GetComponent<LivingEntity>().msBetweenShots;
                float timeDoubledBetweenAttacks = delayBetweenAttack * 2;

                if (msBetweenAttacksEnemy >= 2000)
                {
                    canRemoveMore = false;
                    countRemoveMoreMSBetweenShots = 0;
                }

                if (canRemoveMore == true)
                {
                    msBetweenAttacksEnemy = msBetweenAttacksEnemy + 100;
                    canRemoveMore = false;
                    countRemoveMoreMSBetweenShots = 5;
                }

                target.SetMsBetweenAttacks(msBetweenAttacksEnemy);

                if (timeBetweenAttacks > (20 - timeDoubledBetweenAttacks))
                {
                    count += 1 * Time.deltaTime;
                    /*GameObject chargeAttack =*/
                    if (canSpawnChargeAttack == true)
                    {
                        TrashMan.spawn("Charge_Attack", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        canSpawnChargeAttack = false;
                    }

                    //ParticleSystem particleChargeAttack = chargeAttack.GetComponent<ParticleSystem>();
                    if (count > 2)
                    {
                        TrashMan.spawn("Instantiated_Bullet", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        Attack("Projectile_Player", numberProjectiles);
                        count = 0;
                        timeBetweenAttacks = 0;
                        canSpawnChargeAttack = true;
                    }
                }
                break;
            //case 5: Regenerator - Is the one who can regenerate his own shield every 5 seconds but can spawn half of the projectiles
            case 4:
                if (timeBetweenAttacks > (10 -delayBetweenAttack))
                {
                    if (health < 3 && canRegenerate == true)
                    {
                        health = 3;

                        TrashMan.despawn(shield);
                        GameObject newShield;
                        newShield = TrashMan.spawn("Shield_UP", shieldMuzzle.position, shieldMuzzle.rotation);
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
                        canRegenerate = false;
                        countRegeneration = 10;
                    }

                    count += 1 * Time.deltaTime;
                    /*GameObject chargeAttack =*/
                    if (canSpawnChargeAttack == true)
                    {
                        TrashMan.spawn("Charge_Attack", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        canSpawnChargeAttack = false;
                    }

                    //ParticleSystem particleChargeAttack = chargeAttack.GetComponent<ParticleSystem>();
                    if (count > 2)
                    {
                        int nProjectiles = numberProjectiles / 2;
                        TrashMan.spawn("Instantiated_Bullet", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        Attack("Projectile_Player", nProjectiles);
                        count = 0;
                        timeBetweenAttacks = 0;
                        canSpawnChargeAttack = true;
                    }
                }
                break;

            //case 6: DoubleGold - receives double of the gold but damage time between attacks is doubled
            case 5:
                
                float doubleTimeAttack = delayBetweenAttack * 2;
                int numProj = numberProjectiles / 2;
                if (timeBetweenAttacks > doubleTimeAttack)
                {
                    count += 1 * Time.deltaTime;
                    /*GameObject chargeAttack =*/
                    if (canSpawnChargeAttack == true)
                    {
                        TrashMan.spawn("Charge_Attack", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        canSpawnChargeAttack = false;
                    }

                    //ParticleSystem particleChargeAttack = chargeAttack.GetComponent<ParticleSystem>();
                    if (count > 2)
                    {
                        TrashMan.spawn("Instantiated_Bullet", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        Attack("Projectile_Player", numProj);
                        count = 0;
                        timeBetweenAttacks = 0;
                        canSpawnChargeAttack = true;
                    }
                }
                break;
            //case 7: Second wind - The killer projectile will be deflected instead and clean all the screem but he begins with 1 less health
            case 1:
                if (timeBetweenAttacks > (20 -delayBetweenAttack))
                {
                    count += 1 * Time.deltaTime;
                    /*GameObject chargeAttack =*/
                    if (canSpawnChargeAttack == true)
                    {
                        TrashMan.spawn("Charge_Attack", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        canSpawnChargeAttack = false;
                    }

                    //ParticleSystem particleChargeAttack = chargeAttack.GetComponent<ParticleSystem>();
                    if (count > 2)
                    {
                        TrashMan.spawn("Instantiated_Bullet", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
                        Attack("Projectile_Player", numberProjectiles);
                        count = 0;
                        timeBetweenAttacks = 0;
                        canSpawnChargeAttack = true;
                    }
                }
                break;

            default:

                break;
        }
    }

    /*----------------------------------------Attack function---------------------------------------------
     * typeAttack will be explained if any especial projectiles are being used in future
     * 1 - Normal projectile
     */

    void Attack(string typeAttack, int numProjectiles)
    {

        if(numProjectiles > 2 && numProjectiles % 2 == 0)
        {
            for (int i = 0; i < numProjectiles / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    TrashMan.spawn(typeAttack, muzzleShoot[j].transform.position, muzzleShoot[j].transform.rotation);
                }
            }
        }

        else
        {

            for (int i = 0; i < numProjectiles; i++)
            {
                TrashMan.spawn(typeAttack, muzzleShoot[i].transform.position, muzzleShoot[i].transform.rotation);
            }

        }

    }

    void Update()
    {
        enemy = GameObject.FindWithTag("Enemy");
        managerObject = GameObject.FindGameObjectWithTag("SceneManager");
        managerGetVariables = managerObject.GetComponent<ManagerScene>();

        //switch who'll choose which type of attack will be used based on the number of the spaceship
        try
        {
            target = enemy.GetComponent<LivingEntity>();
        }
        catch(NullReferenceException)
        {
            Debug.Log("erro de viado");
        }
        
        if (spaceshipUsed == 5 && canRegenerate == false)
        {
            countRegeneration -= 1 * Time.deltaTime;
            if (countRegeneration <= 0)
            {
                canRegenerate = true;
            }
        }

        if (spaceshipUsed == 4 && canRemoveMore == false)
        {
            countRemoveMoreMSBetweenShots -= 1 * Time.deltaTime;
            if (countRemoveMoreMSBetweenShots <= 0)
            {
                canRemoveMore = true;
            }
        }
        timeBetweenAttacks += 1 * Time.deltaTime;

        AttackOrBuffsBasedOnTypeOfSpaceship();

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IOS

        if (Input.touchCount > 0)
        {
            //Store the first touch detected.
            Touch myTouch = Input.touches[0];

            //Check if the phase of that touch equals Began
            if (myTouch.phase == TouchPhase.Ended)
            {
                changeColorPlayer(actualColor);
                changeColorShield(actualColor);
            }
        }
#endif
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

		
        if (Input.GetMouseButtonDown(0))
        {
		
			targetMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
			Input.mousePosition.y, Camera.main.nearClipPlane));
			targetMove.z = 0;



			if (move == false)
				move = true;
			
            changeColorPlayer(actualColor);
            changeColorShield(actualColor);
			//transform.position = playerPos;
            //Debug.Log(actualColor);
			
        }
		if (move == true)
			transform.position = Vector3.MoveTowards(transform.position, targetMove, speed * Time.deltaTime);

#endif

	}
}