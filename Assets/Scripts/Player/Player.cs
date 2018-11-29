using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;
//--------------------------------------Script para controle do main character----------------------

public abstract class Player : MonoBehaviour, IPlayerBehaviour	
{

	#region Attributes (variables)

	public event System.Action OnDeath;

	public ManagerScene managerGetVariables;

	LivingEntity target;

	public Transform[] muzzleShoot;
    public Transform shieldMuzzle;
    public GameObject shield;
    GameObject sceneManager;
	GameObject enemy;
	GameObject managerObject;

	public SpriteRenderer[] modifiableColor;

    ParticleSystem shieldColor;
    ParticleSystem.MainModule mainShield;

    Color32 colorStart = new Color32(60, 255, 142, 255);
    Color colorStartShield = new Color(0.23529f, 1.00000f, 0.55686f);
   
    public int numberProjectiles;
    public int spaceshipUsed;
	public int health;
	int actualColor;

	public float count;
    public float timeBetweenAttacks;
	public float delayBetweenAttack;
	private float doubleClickTime = 0.5f;
	private float lastClickTime = -10f;
	public float speed;
	float timeOfInvulnerability;
	float deathCount;

	bool dead;
	bool move;
	bool canChangeColor = false;
	bool isInvulnerable = false;

	public Animator playerAnimator;
	

	Vector3 targetMove;

	CapsuleCollider2D colliderPlayer;

	#endregion

	public void SetStartingAttributes(float _speed, int _health, float _timeOfInvul, float _delayBetweenAttacks)
	{
		speed = _speed;
		health = _health;
		timeOfInvulnerability = _timeOfInvul;
		delayBetweenAttack = _delayBetweenAttacks;
	}

	public virtual void Start()
    {
        timeBetweenAttacks = 1 * Time.deltaTime;
        count = 1 * Time.deltaTime;       

        actualColor = 0;
        health = 3;

        
        foreach (SpriteRenderer sprite in modifiableColor)
        {
            sprite.color = colorStart;
        }

		SpawnNewShield("Shield_UP");

        managerObject = GameObject.FindGameObjectWithTag("SceneManager");
        managerGetVariables = managerObject.GetComponent<ManagerScene>();

        spaceshipUsed = managerGetVariables.GetComponent<ManagerScene>().typeOfSpaceshipBeingUsed;
        numberProjectiles = managerGetVariables.GetComponent<ManagerScene>().numProjectiles;
        delayBetweenAttack = managerGetVariables.GetComponent<ManagerScene>().delayBetweenAttacks;

		colliderPlayer = GetComponent<CapsuleCollider2D>();

		playerAnimator = GetComponentInChildren<Animator>();
    }


    #region Color Control Functions
    public void changeColorPlayer(int color)
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

		if(color == 2)
		{
			foreach (SpriteRenderer sprite in modifiableColor)
			{
				//lerp para cor Roxa
				sprite.color = Color32.Lerp(new Color32(255, 255, 0, 255), new Color32(172, 0, 255, 255), Mathf.Sin(Time.time * 40));
				gameObject.tag = "green";
			}
		}
    }
    public void changeColorShield(int color)
    {
        shieldColor = shield.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainShield = shieldColor.main;

		if (color == 0)
		{
			mainShield.startColor = new Color(1.00000f, 0.23529f, 0.23529f);
			actualColor = 1;
		}

		if (color == 1)
        {
            mainShield.startColor = new Color(0.23529f, 1.00000f, 0.55686f);
            actualColor = 0;
        }

		if(color == 2)
		{
			mainShield.startColor = Color.Lerp(new Color(0.67059f, 0f, 1f), new Color(1f, 1f, 0f), Mathf.Sin(Time.time * 400));
			actualColor = 0;
		}
    }
    #endregion

    #region Damage and Death Control

	IEnumerator Invulnerability(float _timeOfInvulnerability)
	{
		colliderPlayer.enabled = false;

		isInvulnerable = true;

		yield return new WaitForSeconds(_timeOfInvulnerability);

		changeColorPlayer(1);
		changeColorShield(1);
		isInvulnerable = false;

		yield return new WaitForSeconds(1f);

		colliderPlayer.enabled = true;
	}

    public void takeDamage(int damage)
    {
        health -= damage;

        if (health == 2)
        {
			SpawnNewShield("Shield_Breaking");
			StartCoroutine(Invulnerability(timeOfInvulnerability));
			
        }

        if (health == 1)
        {
			SpawnNewShield("Shield_Fim");
			StartCoroutine(Invulnerability(timeOfInvulnerability));
			
        }

		if (health <= 0 && !dead)
        {
			dead = true;
        }

    }

	public virtual void SpawnNewShield(string _typeOfShield)
	{
		if(shield == null)
		{
			GameObject newShield;
			newShield = TrashMan.spawn(_typeOfShield, shieldMuzzle.position, shieldMuzzle.rotation);
			shield = newShield;
		}
		else
		{
			TrashMan.despawn(shield);
			GameObject newShield;
			newShield = TrashMan.spawn(_typeOfShield, shieldMuzzle.position, shieldMuzzle.rotation);
			shield = newShield;
		}

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

		shield.transform.parent = gameObject.transform;
	}


    public virtual void Die()
    {
        dead = true;

        if (OnDeath != null)
        {
            OnDeath();
        }


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


    public virtual void Attack(string typeAttack, int numProjectiles)
    {
		 playerAnimator.SetBool("isAttacking", true);
         for (int i = 0; i < numProjectiles; i++)
         {
			TrashMan.spawn(typeAttack, muzzleShoot[i].transform.position, muzzleShoot[i].transform.rotation);
         }
    }

    public virtual void Update()
    {
        enemy = GameObject.FindWithTag("Enemy");
        managerObject = GameObject.FindGameObjectWithTag("SceneManager");
        managerGetVariables = managerObject.GetComponent<ManagerScene>();
		move = false;

		
		try
        {
            target = enemy.GetComponent<LivingEntity>();
        }
        catch(NullReferenceException)
        {
            Debug.Log("Inimigo Não encontrado!");
        }

        timeBetweenAttacks += 1 * Time.deltaTime;



#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

		if(Input.GetMouseButtonDown(0))
		{
			/*float timeDelta = Time.time - lastClickTime;

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
			}*/
			
			if(canChangeColor == true)
			{
				changeColorPlayer(actualColor);
				changeColorShield(actualColor);
				canChangeColor = false;
			}

		}

		if(Input.GetMouseButtonUp(0))
		{
			canChangeColor = true;
			playerAnimator.SetBool("isAttacking", false);
		}

		if (Input.GetMouseButton(0))
        {
			
			if (timeBetweenAttacks >  delayBetweenAttack)
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
        }
		if (move == true && !dead)
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
				if(canChangeColor == true)
				{
					changeColorPlayer(actualColor);
					changeColorShield(actualColor);
					canChangeColor = false;
				}
			}
			
			if(myTouch.phase == TouchPhase.Ended)
			{
				canChangeColor = true;
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
		if(isInvulnerable == true)
		{
			changeColorPlayer(2);
			changeColorShield(2);
		}

		if(dead == true)
		{
			deathCount += 1 * Time.deltaTime;
			TrashMan.spawn("VFX_DEATH_PLAYER", transform.position, transform.rotation);
			if (deathCount > 1.5f)
				Die();
		}
	}
}

