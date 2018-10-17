using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBarManager : MonoBehaviour {

	public float initialBossHealth;
	public float currentBossHealth;
	public GameObject currentBossObject;

	Image healthBar;
	public float fillAmount;

	private void Start()
	{
		healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();

		try
		{
			currentBossObject = GameObject.FindGameObjectWithTag("Enemy");
			initialBossHealth = currentBossObject.GetComponent<LivingEntity>().startingHealth;
		}
		catch(NullReferenceException)
		{
			Debug.Log("Boss não encontrado!");
			currentBossObject = GameObject.FindGameObjectWithTag("Enemy");
		}

		healthBar.fillAmount = 0.5f;
		
	}

	private void Update()
	{


		if (!currentBossObject)
		{
			Debug.Log("Boss não encontrado!");
			currentBossObject = GameObject.FindGameObjectWithTag("Enemy");
			initialBossHealth = currentBossObject.GetComponent<LivingEntity>().startingHealth;
		}

		if(currentBossHealth < 0)
		{
			currentBossObject = GameObject.FindGameObjectWithTag("Enemy");
			initialBossHealth = currentBossObject.GetComponent<LivingEntity>().startingHealth;
		}
		
			currentBossHealth = currentBossObject.GetComponent<LivingEntity>().auxHealth;
			healthBar.fillAmount = HealthBarModifier(currentBossHealth, initialBossHealth);
		
		
		
	}

	private float HealthBarModifier(float currentHealth, float maxHealth)
	{
		return (currentHealth  * 1) / maxHealth;
	}
}
