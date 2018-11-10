using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBerserk : Player {


	public override void Start ()
	{
		base.Start();
		base.SetStartingAttributes(200, 3, 0.5f, 0.05f);
	}

	public override void Attack(string _typeAttack, int _numProjectiles)
	{
		base.Attack("Projectile_Player",4);
	}


}
