using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class scriptSpawnFlameGreen : MonoBehaviour {
    ParticleSystem particleSys;
    public float count;
    GameObject boss;
    Transform muzzle;

	void Start () {
        count = 1 * Time.deltaTime;
        particleSys = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        try
        {
            boss = GameObject.FindGameObjectWithTag("Enemy");
            muzzle = boss.GetComponent<scriptBoss2>().muzzle;

        }
        catch(NullReferenceException)
        {
            Debug.Log("não encontrad");
        }
        
        count += 1 * Time.deltaTime;
        if(count > 3f)
        {
            Vector3 positionToSpawn = muzzle.transform.position + new Vector3(0, 2f, 0);
            GameObject parentToFlame = TrashMan.spawn("Flame_Green_Attack1", positionToSpawn,muzzle.transform.rotation);
            parentToFlame.transform.parent = gameObject.transform.parent;
            positionToSpawn = new Vector3(0, 0, 0);
            count = 0;
            TrashMan.despawn(gameObject);
        }
        /*
        if(!particleSys.IsAlive())
        {
            Vector3 positionToSpawn = gameObject.transform.position + new Vector3(0, 2f, 0);
            GameObject parentToFlame = TrashMan.spawn("Flame_Green_Attack1",positionToSpawn);
            parentToFlame.transform.parent = gameObject.transform.parent;
            positionToSpawn = new Vector3(0, 0, 0);
            TrashMan.despawn(gameObject);
            
        }*/
	}
}
