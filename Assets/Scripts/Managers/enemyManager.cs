using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

public class enemyManager : MonoBehaviour {
    public int numEnemy;
    public Transform muzzleEnemies;
    public bool needToSpawnEnemy;
    bool isInfiniteMode;

    public void Start()
    {
        needToSpawnEnemy = true;
        SaveGame.Load("isInfiniteMode", isInfiniteMode);
    }
    
    public void updateEnemy()
    {
        numEnemy++;
    }

    public void spawnEnemy(int enemyInOrder)
    {
        switch (enemyInOrder)
        {
            case 0:
                TrashMan.spawn("Enemy1", muzzleEnemies.transform.position, muzzleEnemies.transform.rotation);
                break;
            case 1:
                TrashMan.spawn("Boss1", muzzleEnemies.transform.position, muzzleEnemies.transform.rotation);
                break;
                
            case 2:
                TrashMan.spawn("Boss2", muzzleEnemies.transform.position, muzzleEnemies.transform.rotation);
                break;
                /*
                case 1:
                    break;
                case 1:
                    break;
                case 1:
                    break;
                case 1:
                    break;
                    */
        }
    }

    public void Update()
    {
        if(needToSpawnEnemy == true)
        {
            spawnEnemy(numEnemy);
            needToSpawnEnemy = false;
        }

    }
}
