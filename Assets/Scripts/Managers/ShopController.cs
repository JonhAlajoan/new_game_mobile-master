using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BayatGames.SaveGameFree;
using System.Net;
using System;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

/* ------------------------------------------------Class that controls the entirety of the shop--------------------------
 * 
 */
public class ShopController : MonoBehaviour {

    #region attributes

    /*Attributes:
     * Maincam = Main camera, used to lerp between ships and tilt to left while seeing the details
     * shipPositions = Array that stores each Transform of the ships on the Shop(0 - default, 1 - kaze, 2 - Berserk, 3 - Frozen, 4 - hira, 5 - doublegold, 6 - bombardier)
     * initialTarget = The initial target of the shop (The default ship)
     * NextTarget = Variable that stores the incoming ship that the camera must move to show
     * TimeToCompleteTravel = Time that the camera will waste the travel until the end is reached
     * Index = Value to indicate and iterate where the ship is positioned in the array shipPositions
     * nameOfShipStart = Name of the ship that is show at first in the shop
     * nameOfShipDetails = Name that is shown at the details from shop
     * CanvasPopUPBuy = canvas that is shown when you click to buy a ship
     * CanvasPopUpBuyConfirmation = Canvas that show the price and confirms if you want to buy the ship
     * isSpaceshipOwned = List of ships that are owned by the player. The ships are identified using the index.
     * buff and debuffDeatails = Buffs or debuffs that are shown  in the details page
     * scoreShop = variable that represents the amount of gold that the player does have right now and the variable that is saved in persistent datapath
     * startOnLoad = Defines if the data should be loaded when the script is initialized
     * numProjectilesToBeUpgradedOnShop = variable that stores the amount of projectiles that a player does have after the upgrade and its valued is save on persistent data;
     * sapaceShipToBeUsedOnStart = variable that stores spaceship that'll be used when start game is pressed;
     * delayBetweenAttacksToBeUpgraded = Variable that stores the delay between attacks that a player upgraded and its value is saved on persistent datapath;
     * 
     */
    Transform mainCam;
    public Transform[] shipPositions;
    Transform initialTarget;
    Transform nextTarget;    

    public Text nameOfShipStart;
    public Text nameOfShipDetails;

    public GameObject canvasShop;
    public GameObject canvasDetails;
    public GameObject canvasPopUPBuy;
    public GameObject canvasPopUPBuyConfirmation;
    public GameObject canvasPopUpWatchAd;
    public GameObject canvasShipAlreadyOwned;
    public GameObject canvasUpgrade;
    public GameObject canvasUpgradeBuyPopUpProjectile;
    public GameObject canvasUpgradeWatchAd;
    public GameObject canvasWatchADWarn;
    public GameObject canvasWatchADFailed;
    public GameObject canvasUpgradeBuyPopUpDelay;
    public GameObject canvasProjectileSuccessfullyBought;
    public GameObject canvasDelaySuccessfullyBought;
    public GameObject canvasWatchADWarnShipShop;
    public GameObject canvasNotConnectedToInternet;

    public Text buffDetails;
    public Text debuffDetails;

    public Text initialPoints;
    public Text initialPointDetails;
    public Text initialPointsUpgrade;

    public Text priceOfItens;
    public Text textPriceOfUpgradeProjectile;
    public Text textPriceOfUpgradeDelay;

    public Text numOfProjectilesText;
    public Text numOfDelayBetweenAttacksText;

    public Button buttonStartGame;

    public int scoreShop;

    bool startOnLoad = true;
    bool firstTimePlaying;
    bool isConnectedOnInternet;
    

    float valueToTilt;

    public int numProjectilesToBeUpgradedOnShop;
    public int spaceShipToBeUsedOnStart;
    public int delayBetweenAttacksToBeUpgraded;
    public List<int> isSpaceshipOwned = new List<int>();
    public int priceOfShip;
    public int priceOfUpgradeDelay;
    public int priceOfUpgradeNumProjectiles;
    public int index;
    float timeToCompleteTravel;

    private string gameId = "1605643";



    #endregion /attributes

    private void Start()
    {
        firstTimePlaying = true;

        bool firstTimePlayingSave = SaveGame.Load("firstTimePlaying", firstTimePlaying);
        index = 0;
        nameOfShipStart.text = "Verstek";
        timeToCompleteTravel = 5f;
        isSpaceshipOwned.Add(index);
        priceOfUpgradeNumProjectiles = 500;
        priceOfUpgradeDelay = 200;
        if (startOnLoad)
        {
           Load();
        }
        
        if(firstTimePlayingSave == true)
        {
            numProjectilesToBeUpgradedOnShop = 4;
            firstTimePlayingSave = false;
            Debug.Log("falseFirstTime");
            SaveGame.Save("firstTimePlaying", firstTimePlayingSave);

        }
    }

    #region selectionOfShips

    ///The two first functions, selectShipPositively and selectShipNegatively is utilized to call the coroutines that moves the camera to left or right

    public void selectShipPositively()
    {
        StartCoroutine("ControlCameraBetweenShipsPositive");
    }

    public void selectShipNegatively()
    {
        StartCoroutine("ControlCameraBetweenShipsNegative");
    }
    
    // This function utilizes the index to find the ship that the camera is pointing and modify the text from start and details to show the ship correct name 
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

    // This coroutine utilizes the coroutine to find the ship and modify the nextTarget to the transform stored onto the shipPositionsArray at given index + 1
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

        //While so the lerp and moveTowards can be used correctly
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

    // This coroutine utilizes the coroutine to find the ship and modify the nextTarget to the transform stored onto the shipPositionsArray at given index - 1
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


    #region detailsAndBuyPhase

    //Function that modifies the values for the text shown at the ship buff scren and the price of ships
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

    //Function that modifies the values for the text shown at the ship debuff
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

    //This functions returns a bool that return true if the list isSpaceshipOwned contais the same number as the index that is passed as reference
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

    //Functions that deactivate the default canvas, activate the details canvas, call the function to set the name, buffs and debuffs of the ship and start the coroutine to tilt camera to left 
    public void showDetails()
    {        
        canvasShop.SetActive(false);
        canvasDetails.SetActive(true);

        setBuffOfShip();
        setDebuffOfShip();
        setNameOfShip();

        StartCoroutine("tiltCameraToLeft");
    }

    //function that closes the canvas of details and activates the default canvas + startCoroutine that put camera on normal position
    public void returnToSelect()
    {
        canvasDetails.SetActive(false);
        canvasShop.SetActive(true);

        StartCoroutine("getCameraBackToNormal");
    }

    public void buyPopUpActivation()
    {
        bool isShipOwned = checkIfBought(index);
        if(isShipOwned)
        {
            canvasShipAlreadyOwned.SetActive(true);
            canvasPopUPBuy.SetActive(false);
        }
        else
        {
            canvasPopUPBuy.SetActive(true);
            priceOfItens.text = priceOfShip.ToString() + " EC";
        }
       
    }

    public void buy()
    {
        bool isShipOw = checkIfBought(index);

        if (isShipOw)
        {
            canvasShipAlreadyOwned.SetActive(true);
            canvasPopUPBuy.SetActive(false);
        }

        if (scoreShop >= priceOfShip && !isShipOw)
        {  
            isSpaceshipOwned.Add(index);
            canvasPopUPBuyConfirmation.SetActive(true);
            canvasPopUPBuy.SetActive(false);
            
            scoreShop = scoreShop - priceOfShip;
        }

        else  
        {
            if(isConnectedOnInternet == true)
            {
                canvasPopUpWatchAd.SetActive(true);
                canvasPopUPBuy.SetActive(false);
            }
            else
            {
                canvasNotConnectedToInternet.SetActive(true);

                canvasPopUPBuy.SetActive(false);
            }            
            
        }
    }

   /* public void clearSave()
    {
        SaveGame.Delete("numProjectiles");
        SaveGame.Delete("delayBetweenAttacks");
        SaveGame.Delete("listOfSpaceshipsOwned");
        numProjectilesToBeUpgradedOnShop = 4;
        delayBetweenAttacksToBeUpgraded = 2;
    }*/


#region Ads
    public void watchAd()
    {
        while(!Advertisement.IsReady())
        {
            Debug.Log("Waiting");
        }

        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }        
        canvasPopUpWatchAd.SetActive(false);
        canvasUpgradeWatchAd.SetActive(false);
        canvasDetails.SetActive(false);       
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                canvasWatchADWarn.SetActive(true);
                scoreShop += 100;
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                canvasWatchADFailed.SetActive(true);

                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                canvasWatchADFailed.SetActive(true);
                break;
        }
    }
    #endregion /Ads

#region canvasTreatment
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

    public void returnToNormalCanvasAfterAdWatched()
    {
        StartCoroutine("getCameraBackToNormal");
        canvasUpgrade.SetActive(false);
        canvasWatchADWarn.SetActive(false);
        canvasDetails.SetActive(false);
        canvasShop.SetActive(true);
    }
    public void returnToNormalCanvasAfterAdWatchedFail()
    {
        StartCoroutine("getCameraBackToNormal");
        canvasUpgrade.SetActive(false);
        canvasWatchADWarn.SetActive(false);
        canvasWatchADFailed.SetActive(false);
        canvasDetails.SetActive(false);
        canvasShop.SetActive(true);
    }
    public void upgradeCanvas()
    {

        canvasUpgrade.SetActive(true);
        canvasShop.SetActive(false);
    }

    public void canvasPopUpConfirmationNumProjectiles()
    {
                
        canvasUpgradeBuyPopUpProjectile.SetActive(true);
        textPriceOfUpgradeProjectile.text = priceOfUpgradeNumProjectiles.ToString() + " EC";
    }

    public void buyUpgradeNumProjectiles()
    {
        if (scoreShop > priceOfUpgradeNumProjectiles)
        {
            scoreShop -= priceOfUpgradeNumProjectiles;
            numProjectilesToBeUpgradedOnShop += 2;
            canvasProjectileSuccessfullyBought.SetActive(true);
            canvasUpgradeBuyPopUpProjectile.SetActive(false);
        }
        else
        {
            canvasUpgradeBuyPopUpProjectile.SetActive(false);
            canvasUpgradeWatchAd.SetActive(true);
        }

    }

    public void returnFromPopUpConfirmationNumProjectiles()
    {
        canvasUpgradeBuyPopUpProjectile.SetActive(false);
        canvasUpgrade.SetActive(true);
    }

    public void returnFromSuccesfullyBoughtProjectile()
    {
        canvasProjectileSuccessfullyBought.SetActive(false);
        canvasUpgrade.SetActive(true);
    }
    //-----------------------------------------------------------------------
    public void canvasPopUpConfirmationDelay()
    {
        if (delayBetweenAttacksToBeUpgraded > 1 && delayBetweenAttacksToBeUpgraded < 3)
        {
            priceOfUpgradeDelay += 200;
        }

        if (delayBetweenAttacksToBeUpgraded >= 3 && delayBetweenAttacksToBeUpgraded <= 5)
        {
            priceOfUpgradeDelay += 500;
        }

        if (delayBetweenAttacksToBeUpgraded > 5)
        {
            priceOfUpgradeNumProjectiles += 1000;
        }

        canvasUpgradeBuyPopUpDelay.SetActive(true);
        textPriceOfUpgradeDelay.text = priceOfUpgradeDelay.ToString() + " EC";
    }

    public void buyUpgradDelay()
    {
        if (scoreShop > priceOfUpgradeDelay)
        {
            scoreShop -= priceOfUpgradeDelay;
            priceOfUpgradeDelay += 1;
            canvasDelaySuccessfullyBought.SetActive(true);
            canvasUpgradeBuyPopUpProjectile.SetActive(false);
        }
        else
        {
            canvasUpgradeBuyPopUpDelay.SetActive(false);
            canvasUpgradeWatchAd.SetActive(true);
        }

    }

    public void returnFromPopUpConfirmationDelay()
    {
        canvasUpgradeBuyPopUpDelay.SetActive(false);
        canvasUpgrade.SetActive(true);
    }

    public void returnFromSuccesfullyBoughtDelay()
    {
        canvasDelaySuccessfullyBought.SetActive(false);
        canvasUpgrade.SetActive(true);
    }
    //-------------------------------------------------------------------------------------------



    public void returnToNormalCanvasFromUpgrade()
    {
        canvasUpgrade.SetActive(false);
        canvasShop.SetActive(true);
    }

    public void returnToUpgradeCanvasFromWatchAdUpgrade()
    {
        canvasUpgradeWatchAd.SetActive(false);
        canvasUpgradeBuyPopUpProjectile.SetActive(false);
        canvasUpgrade.SetActive(true);
    }

    public void returnToNormalAfterNoInternet()
    {
        canvasPopUpWatchAd.SetActive(false);
        canvasUpgradeWatchAd.SetActive(false);
        canvasDetails.SetActive(false);
        canvasShop.SetActive(true);
        canvasNotConnectedToInternet.SetActive(false);
        StartCoroutine("getCameraBackToNormal");
    }

#endregion /canvasTreatment

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
        isSpaceshipOwned = SaveGame.Load("listOfSpaceshipsOwned", isSpaceshipOwned);
        isConnectedOnInternet = SaveGame.Load("StatusOfConnection", isConnectedOnInternet);
    }

    public void Save()
    {
        SaveGame.Save("score", scoreShop);
        SaveGame.Save("numProjectiles", numProjectilesToBeUpgradedOnShop);
        SaveGame.Save("delayBetweenAttacks", delayBetweenAttacksToBeUpgraded);
        SaveGame.Save("typeOfSpaceShipBeingUsed", index);
        SaveGame.Save("listOfSpaceshipsOwned", isSpaceshipOwned);
    }

    #endregion /saveAndLoad

    #region startGame

    public void StartGame()
    {
        Save();        
        bool isShipOwnedBeforeStartingTheGame = checkIfBought(index);

        if(isShipOwnedBeforeStartingTheGame)
        {
            SceneManager.LoadScene("main_scene");
        }        
    }

    #endregion /startGame


public void Update()
    {
        initialPoints.text = scoreShop.ToString() + " EC";
        initialPointDetails.text = scoreShop.ToString() + " EC";
        initialPointsUpgrade.text = scoreShop.ToString() + " EC";

        numOfDelayBetweenAttacksText.text = delayBetweenAttacksToBeUpgraded.ToString();
        numOfProjectilesText.text = numProjectilesToBeUpgradedOnShop.ToString();

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        bool isShipOwned = checkIfBought(index);


        if (isShipOwned)
        {
            Text textOfStartGameButton = buttonStartGame.GetComponent<Text>();
            textOfStartGameButton.text = "Start Game";
            buttonStartGame.interactable = true;            
        }

        else
        {
            Text textOfStartGameButton = buttonStartGame.GetComponent<Text>();
            textOfStartGameButton.text = "Ship not owned";
            buttonStartGame.interactable = false;
        }

    }
}

