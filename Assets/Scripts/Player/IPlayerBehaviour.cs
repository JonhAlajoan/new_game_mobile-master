using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBehaviour
{
	void takeDamage(int _damage);
	void SpawnNewShield(string _typeOfShield);
	void changeColorPlayer(int color);
	void changeColorShield(int color);
	void Attack(string _typeAttack, int _numProjectiles);
	void Die();
}
