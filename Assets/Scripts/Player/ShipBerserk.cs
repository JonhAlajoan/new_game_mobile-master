using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBerserk : Player {


	public override void Start ()
	{
		base.Start();
		base.SetStartingAttributes(200, 3, 0.5f);
	}

	public override void Attack(string _typeAttack, int _numProjectiles)
	{
		for (int i = 0; i < 4; i++)
		{
			TrashMan.spawn(_typeAttack, muzzleShoot[i].transform.position, muzzleShoot[i].transform.rotation);
		}
	}


}
