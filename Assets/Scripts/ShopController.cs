using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BayatGames.SaveGameFree;
using System.Net;
using System;

public class ShopController : MonoBehaviour {

#region attributes
    Transform mainCam;
    public Transform[] shipPositions;
    Transform initialTarget;
    Transform nextTarget;

    float timeToCompleteTravel;
    
    public int index;

    public Text nameOfShipStart;
    public Text nameOfShipDetails;

    public GameObject canvasShop;
    public GameObject canvasDetails;
    public GameObject canvasPopUPBuy;
    public GameObject canvasPopUPBuyConfirmation;
    public GameObject canvasPopUpWatchAd;
    public GameObject canvasShipAlreadyOwned;

    List<Player> shipsOwnedByPlayer = new List<Player>();

    public Text buffDetails;
    public Text debuffDetails;

    public Text initialPoints;
    public Text initialPointDetails;

    public Text priceOfItens;

    public int scoreShop;

    bool startOnLoad = true;

    float valueToTilt;

    public int numProjectilesToBeUpgradedOnShop;
    public int sapaceShipToBeUsedOnStart;
    public int delayBetweenAttacksToBeUpgraded;
    public List<int> isSpaceshipOwned = new List<int>();
    public int priceOfShip;
    #endregion /attributes

    private void Start()
    {
        index = 0;
        nameOfShipStart.text = "Verstek";
        timeToCompleteTravel = 5f;

        if(startOnLoad)
        {
           Load();
        }
        

    }

    #region selectionOfShips
    

    public void selectShipPositively()
    {
        StartCoroutine("ControlCameraBetweenShipsPositive");
    }

    public void selectShipNegatively()
    {
        StartCoroutine("ControlCameraBetweenShipsNegative");
    }
    
    public void setNameOfShip()
    {
        Text nameOfShip = nameOfShipStart.GetComponent<Text>();
        Text nameOfShipDetail = nameOfShipDetails.GetComponent<Text>();
        switch (index)
        {           
            case 0:                
                nameOfShip.text = "Verstek";
                nameOfShipDetail.text = "Verstek";
                break;
            case 1:
                nameOfShip.text = "Kaze";
                nameOfShipDetail.text = "Kaze";
                break;
            case 2:
                nameOfShip.text = "Berserk";
                nameOfShipDetail.text = "Berserk";
                break;
            case 3:
                nameOfShip.text = "Vereist";
                nameOfShipDetail.text = "Vereist";
                break;
            case 4:
                nameOfShip.text = "Hira";
                nameOfShipDetail.text = "Hira";
                break;
            case 5:
                nameOfShip.text = "Rykdom";
                nameOfShipDetail.text = "Rykdom";
                break;
            case 6:
                nameOfShip.text = "Bombardier";
                nameOfShipDetail.text = "Bombardier";
                break;
        }
    }

    IEnumerator ControlCameraBetweenShipsPositive ()
    {
        switch (index)
        {
            case 0:
                nextTarget = shipPositions[1];
                index = 1;
                break;
            case 1:
                nextTarget = shipPositions[2];
                index = 2;
                break;
            case 2:
                nextTarget = shipPositions[3];
                index = 3;
                break;
            case 3:
                nextTarget = shipPositions[4];
                index = 4;
                break;
            case 4:
                nextTarget = shipPositions[5];
                index = 5;
                break;
            case 5:
                nextTarget = shipPositions[6];
                index = 6;
                break;
            case 6:
                nextTarget = shipPositions[0];
                index = 0;
                break;
        }

        while (Vector2.Distance(mainCam.position, nextTarget.position) > 0.001f)
        {

            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, nextTarget.transform.position, timeToCompleteTravel * Time.deltaTime);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, nextTarget.transform.position, timeToCompleteTravel * Time.deltaTime);
            setNameOfShip();
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        yield break;
    }

    IEnumerator ControlCameraBetweenShipsNegative()
    {
        switch (index)
        {
            case 0:
                nextTarget = shipPositions[6];
                index = 6;
                break;
            case 1:
                nextTarget = shipPositions[0];
                index = 0;
                break;
            case 2:
                nextTarget = shipPositions[1];
                index = 1;
                break;
            case 3:
                nextTarget = shipPositions[2];
                index = 2;
                break;
            case 4:
                nextTarget = shipPositions[3];
                index = 3;
                break;
            case 5:
                nextTarget = shipPositions[4];
                index = 4;
                break;
            case 6:
                nextTarget = shipPositions[5];
                index = 5;
                break;
        }


        while (Vector3.Distance(mainCam.transform.position, nextTarget.position) > 0.001f)
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, nextTarget.transform.position, timeToCompleteTravel * Time.deltaTime);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, nextTarget.transform.position, timeToCompleteTravel * Time.deltaTime);
            setNameOfShip();
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        yield break;

    }
    #endregion /selecionOfShips

    #region Upgrades
    public void buyDelayBetweenAttacksUpgrade()
    { }

    public void buyNumberOfProjectilesUpgrade()
    { }

    #endregion /Upgrades

    #region detailsAndBuyPhase

    public void setBuffOfShip()
    {
        switch (index)
        {
            case 0:
                buffDetails.text = "no buffs, there isn't any debuffs too.";

                break;
            case 1:
                buffDetails.text = "Buff - The last hit from enemies that would normally be fatal will be parried";
                priceOfShip = 100;
                break;
            case 2:

                buffDetails.text = "Buff - it doubles the quantity of projectiles bought by upgrades.";
                priceOfShip = 500;
                break;
            case 3:
                valueToTilt = 5f;
                buffDetails.text = "Buff - Reduce enemy attack speed every 5s. (maximum of 2s of delay between shots)";
                priceOfShip = 1000;
                break;
            case 4:
                buffDetails.text = "Buff - Every 10 seconds, will regenerate the shield from any damage";
                priceOfShip = 1500;
                break;
            case 5:
                buffDetails.text = "Buff - Will get double Energy Concentratum";
                priceOfShip = 2000;
                break;
            case 6:
                buffDetails.text = "Buff - 10x more damage and more damage radius";
                priceOfShip = 3000;
                break;
        }
    }
    
    public void setDebuffOfShip()
    {
        switch (index)
        {
            case 0:
               debuffDetails.text = "Debuff - None";
                break;
            case 1:
                debuffDetails.text = "Debuff - No number of projectiles or delay between attacks will be upgraded in this ship";
                break;
            case 2:
                debuffDetails.text = "Debuff - Takes double damage from enemy projectiles";
                break;
            case 3:
                debuffDetails.text = "Debuff - Delay between attacks is doubled.";
                break;
            case 4:
                debuffDetails.text = "Debuff - Delay between attacks is doubled and number of projectiles is halved.";
                break;
            case 5:
                debuffDetails.text = "Debuff - Number of projectiles is halved";
                break;
            case 6:
                debuffDetails.text = "Debuff - The number of projectiles is reduced by 75%";
                break;
        }
    }

    public bool checkIfBought(int indexOfShip)
    {
        if(isSpaceshipOwned.Contains(indexOfShip))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void buyNewShip()
    {

    }

    public void showDetails()
    {        
        canvasShop.SetActive(false);
        canvasDetails.SetActive(true);

        setBuffOfShip();
        setDebuffOfShip();
        setNameOfShip();

        StartCoroutine("tiltCameraToLeft");
    }

    public void returnToSelect()
    {
        canvasDetails.SetActive(false);
        canvasShop.SetActive(true);

        StartCoroutine("getCameraBackToNormal");

    }

    public void buyPopUpActivation()
    {
        canvasPopUPBuy.SetActive(true);
        priceOfItens.text = priceOfShip.ToString();
    }

    public void buy()
    {
        if(scoreShop >= priceOfShip)
        {
            bool isShipOw = checkIfBought(index);

            if(isShipOw)
            {
                canvasShipAlreadyOwned.SetActive(true);
                canvasPopUPBuy.SetActive(false);
            }
            else
            {
                isSpaceshipOwned.Add(index);
                canvasPopUPBuyConfirmation.SetActive(true);
                canvasPopUPBuy.SetActive(false);
            }            
            scoreShop = scoreShop - priceOfShip;
            Debug.Log(isShipOw);

        }

        if (scoreShop < priceOfShip)
        {
            canvasPopUpWatchAd.SetActive(true);
            canvasPopUPBuy.SetActive(false);
        }
    }

    public void watchAd()
    {
        canvasPopUpWatchAd.SetActive(false);
    }

    public void deactivateCanvasAfterPurchase()
    {
        canvasPopUPBuyConfirmation.SetActive(false);
    }

    public void backToDetailsAfterRefusingToBuy()
    {
        canvasPopUPBuy.SetActive(false);
    }

    public void backToDetailsAfterRefusingWatchingAd()
    {
        canvasPopUPBuy.SetActive(false);
        canvasPopUpWatchAd.SetActive(false);

    }

    public void backToDetailsAfterShipIsOwned()
    {
        canvasShipAlreadyOwned.SetActive(false);
        canvasDetails.SetActive(true);
    }
  
    IEnumerator tiltCameraToLeft()
    {
        Vector3 correctedLocal = new Vector3(2f,0,0);
        Vector3 newCamLocal = mainCam.transform.position + correctedLocal;
        
        while (Vector3.Distance(mainCam.transform.position, newCamLocal) > 0.001f)
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, newCamLocal, timeToCompleteTravel * Time.deltaTime);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, newCamLocal, timeToCompleteTravel * Time.deltaTime);

            setNameOfShip();
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        yield break;
    }

    IEnumerator getCameraBackToNormal()
    {
        Vector3 correctedLocal = new Vector3(-2f, 0, 0);
        Vector3 newCamLocal = mainCam.transform.position + correctedLocal;

        while (Vector3.Distance(mainCam.transform.position, newCamLocal) > 0.001f)
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, newCamLocal, timeToCompleteTravel * Time.deltaTime);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, newCamLocal, timeToCompleteTravel * Time.deltaTime);

            setNameOfShip();
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        yield break;
    }

    #endregion /detailsAndBuyPhase

    #region saveAndLoad
    public void Load()
    {
        scoreShop = SaveGame.Load("score", scoreShop);
        initialPoints.text = scoreShop.ToString();
        initialPointDetails.text = scoreShop.ToString();

        numProjectilesToBeUpgradedOnShop = SaveGame.Load("numProjectiles", numProjectilesToBeUpgradedOnShop);
        delayBetweenAttacksToBeUpgraded = SaveGame.Load("delayBetweenAttacks", delayBetweenAttacksToBeUpgraded);
    }

    public void Save()
    {
        //save new quantity of points
        
    }

#endregion /saveAndLoad

    public void Update()
    {
        initialPoints.text = scoreShop.ToString();
        initialPointDetails.text = scoreShop.ToString();

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;       
    }
}

