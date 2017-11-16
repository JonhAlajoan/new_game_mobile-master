using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;
using UnityEngine.UI;
public class ManagerScene : MonoBehaviour {

    public int numProjectiles;
    public int typeOfSpaceshipBeingUsed;
    public int delayBetweenAttacks;
    public int score;
    public bool startOnLoad = true;
    public bool isPlayerAlive;
 
    public Text scoreText;


	void Start () {

        isPlayerAlive = true;
        if (startOnLoad)
        {
            Load();
        }

	}
	
	// Update is called once per frame
	void Update ()
    {        
        if(!isPlayerAlive)
        {
            SaveScoreState();
        }
        scoreText.text = score.ToString();
	}

    public void SaveScoreState()
    {        
        SaveGame.Save("score", score);
    }

    public void Load()
    {
        score = SaveGame.Load("score", score);
        numProjectiles = SaveGame.Load("numProjectiles", numProjectiles);
        typeOfSpaceshipBeingUsed = SaveGame.Load("typeOfSpaceShipBeingUsed", typeOfSpaceshipBeingUsed);
        delayBetweenAttacks = SaveGame.Load("delayBetweenAttacks", delayBetweenAttacks);        
    }
}
