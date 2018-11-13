using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBombardier : Player {

	public override void Start()
	{
		base.Start();
		base.SetStartingAttributes(50, 3, 2, 3f);
	}

	public override void Attack(string typeAttack, int numProjectiles)
	{

		base.Attack("Bomb_Bombardier", 1);
	}
}
