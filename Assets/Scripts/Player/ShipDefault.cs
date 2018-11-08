using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDefault : Player {

	
	public override void Start ()
	{
		base.Start();
		base.SetStartingAttributes(100, 3, 2f);
	}


}
