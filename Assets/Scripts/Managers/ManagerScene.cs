using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using System.Net;
using System.IO;
public class ManagerScene : MonoBehaviour {

    public int numProjectiles;
    public int typeOfSpaceshipBeingUsed;
    public int delayBetweenAttacks;
    public int score;
    public bool startOnLoad = true;
    public bool isPlayerAlive;

    public Transform muzzlePlayer;

    public GameObject canvasGameOver;
    public GameObject canvasShowAds;
 
    public Text scoreText;

    private BannerView bannerView;

    Player player;

    string appId = "pub - 8813499873915106";
    private string gameId = "1605643";

    bool isConnectedOnInternet;

    private void Awake()
    {
        if (startOnLoad)
        {
            Load();
            CheckIfConnectedToInternet();
        }
    }
    void Start () {

        Advertisement.Initialize(gameId);
        

        switch (typeOfSpaceshipBeingUsed)
        {
            case 0:
                TrashMan.spawn("Main_Player_Default", muzzlePlayer.transform.position, muzzlePlayer.transform.rotation);
                break;
            case 1:
                TrashMan.spawn("Main_Player_SecondWind", muzzlePlayer.transform.position, muzzlePlayer.transform.rotation);
                break;
            case 2:
                TrashMan.spawn("Main_Player_Berserk", muzzlePlayer.transform.position, muzzlePlayer.transform.rotation);
                break;
            case 3:
                TrashMan.spawn("Main_Player_Frozen", muzzlePlayer.transform.position, muzzlePlayer.transform.rotation);
                break;
            case 4:
                TrashMan.spawn("Main_Player_Regenerator", muzzlePlayer.transform.position, muzzlePlayer.transform.rotation);
                break;
            case 5:
                TrashMan.spawn("Main_Player_DoubleGold", muzzlePlayer.transform.position, muzzlePlayer.transform.rotation);
                break;
            case 6:
                TrashMan.spawn("Main_Player_Bombardier", muzzlePlayer.transform.position, muzzlePlayer.transform.rotation);
                break;
        }

    #if UNITY_ANDROID
        
    #endif
        MobileAds.Initialize(appId);

        isPlayerAlive = true;        
        bannerView = new BannerView(appId, AdSize.SmartBanner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();
//
        // Load the banner with the request.
        bannerView.LoadAd(request);
        bannerView.Show();
        

    }
	
	// Update is called once per frame
	void Update ()
    {        
        if(!isPlayerAlive)
        {
            SaveScoreState();
        }
        scoreText.text = score.ToString();

        if(GameObject.FindWithTag("green"))
        {
            player = gameObject.GetComponent<Player>();
        }

        if(GameObject.FindGameObjectWithTag("red"))
        {
            player = gameObject.GetComponent<Player>();
        }
	}

    public void watchAd()
    {
        while (!Advertisement.IsReady())
        {
            Debug.Log("Waiting");
        }

        if (Advertisement.IsReady("video"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("video", options);
        }

    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                SaveScoreState();
                SceneManager.LoadScene("_ShopScene");
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                SaveScoreState();
                SceneManager.LoadScene("_ShopScene");

                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                SaveScoreState();
                SceneManager.LoadScene("_ShopScene");
                break;
        }
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

    public void playAgain()
    {
        SaveScoreState();
        SceneManager.LoadScene("main_scene");
    }

    public void goToShop()
    {
        if(isConnectedOnInternet == true)
        {
            watchAd();
        }
        else
        {
            SceneManager.LoadScene("_ShopScene");
        }
               
    }

    public string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

    private void CheckIfConnectedToInternet()
    {
        string HtmlText = GetHtmlFromUri("http://google.com");
        if (HtmlText == "")
        {
            isConnectedOnInternet = false;
        }
        else if (!HtmlText.Contains("schema.org/WebPage"))
        {
            isConnectedOnInternet = false;
        }
        else
        {
            isConnectedOnInternet = true;
        }
    }

}
