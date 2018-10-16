using System.Collections;
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

    bool dead;

    public ManagerScene managerGetVariables;
    GameObject enemy;
    LivingEntity target;
    GameObject managerObject;

	public float speed;
	bool move;
	Vector3 targetMove;

	private float doubleClickTime = 0.5f;
	private float lastClickTime = -10f;


	#endregion

	void Start()
    {
		speed = 100;
        timeBetweenAttacks = 1 * Time.deltaTime;
        count = 1 * Time.deltaTime;       

        //actual color: 1 = vermelho, 0 = verde;
        actualColor = 0;
        health = 3;

        //setando a cor inicial pra verde   
        foreach (SpriteRenderer sprite in modifiableColor)
        {
            sprite.color = colorStart;
        }

        GameObject newShield;
        newShield = TrashMan.spawn("Shield_UP", shieldMuzzle.position, shieldMuzzle.rotation);
        shield = newShield;
		shield.transform.parent = gameObject.transform;

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

		shield.transform.parent = gameObject.transform;

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


            for (int i = 0; i < 2; i++)
            {
                    TrashMan.spawn(typeAttack, muzzleShoot[i].transform.position, muzzleShoot[i].transform.rotation);
            }
     

    }

    void Update()
    {
        enemy = GameObject.FindWithTag("Enemy");
        managerObject = GameObject.FindGameObjectWithTag("SceneManager");
        managerGetVariables = managerObject.GetComponent<ManagerScene>();
		move = false;

		//switch who'll choose which type of attack will be used based on the number of the spaceship
		try
        {
            target = enemy.GetComponent<LivingEntity>();
        }
        catch(NullReferenceException)
        {
            Debug.Log("erro de viado");
        }

        timeBetweenAttacks += 1 * Time.deltaTime;

        AttackOrBuffsBasedOnTypeOfSpaceship();


#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

		if(Input.GetMouseButtonDown(0))
		{
			float timeDelta = Time.time - lastClickTime;

			if (timeDelta < doubleClickTime)
			{
				Debug.Log("double click" + timeDelta);

				changeColorPlayer(actualColor);
				changeColorShield(actualColor);

				lastClickTime = 0;
			}
			else
			{
				lastClickTime = Time.time;
			}

		}

		if (Input.GetMouseButton(0))
        {
			
			if (timeBetweenAttacks > 0.09f)
			{
				
				Attack("Projectile_Player", numberProjectiles);
				count = 0;
				timeBetweenAttacks = 0;
			}

			targetMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
			Input.mousePosition.y, Camera.main.nearClipPlane));
			targetMove.z = 0;

			if (move == false)
				move = true;
			
           
			//transform.position = playerPos;
			//Debug.Log(actualColor);

			
			
        }
		if (move == true)
			transform.position = Vector3.MoveTowards(transform.position, targetMove, speed * Time.deltaTime);

#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		if (timeBetweenAttacks > 0.09f)
				{TrashMan.spawn("Instantiated_Bullet", shieldMuzzle.transform.position, shieldMuzzle.transform.rotation);
					Attack("Projectile_Player", numberProjectiles);
					count = 0;
					timeBetweenAttacks = 0;
				}

		if (Input.touchCount > 0)
        {
            //Store the first touch detected.
            Touch myTouch = Input.touches[0];
			if (myTouch.phase == TouchPhase.Began)
			{
				float timeDelta = Time.time - lastClickTime;

				if (timeDelta < doubleClickTime)
				{
					Debug.Log("double click" + timeDelta);

					changeColorPlayer(actualColor);
					changeColorShield(actualColor);

					lastClickTime = 0;
				}
				else
				{
					lastClickTime = Time.time;
				}
			}
            //Check if the phase of that touch equals Began
            if (myTouch.phase == TouchPhase.Began || myTouch.phase == TouchPhase.Moved)
            {
				
				targetMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.nearClipPlane));
				targetMove.z = 0;

				if (move == false)
					move = true;
            }
			if (move == true)
				transform.position = targetMove;
					//transform.position = Vector3.MoveTowards(transform.position, targetMove, speed* Time.deltaTime);
        }
#endif

	}
}

