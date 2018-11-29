using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSecondWind : Player
{
	public override void Start()
	{
		base.Start();
		base.SetStartingAttributes(100, 3, 2f, 0.3f);
	}

	public override void Attack(string typeAttack, int numProjectiles)
	{
		base.Attack(typeAttack, 2);
	}

}
