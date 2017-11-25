using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.IO;
using BayatGames.SaveGameFree;
public class checkSDKLevel : MonoBehaviour {

    //Checks SDK level and connection to Internet

    bool isConnectedToInternet;

    public int GetSDKLevel()
    {
        var clazz = AndroidJNI.FindClass("android.os.Build$VERSION");
        var fieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
        var sdkLevel = AndroidJNI.GetStaticIntField(clazz, fieldID);
        return sdkLevel;
    }

    private void Awake()
    {
        int sdkLevelUsed = GetSDKLevel();
        if(sdkLevelUsed <19)
        {
            Application.targetFrameRate = 30;
        }
        if(sdkLevelUsed>=19)
        {
            Application.targetFrameRate = 60;
        }
        CheckIfConnectedToInternet();
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
            isConnectedToInternet = false;
        }
        else if (!HtmlText.Contains("schema.org/WebPage"))
        {
            isConnectedToInternet = false;
        }
        else
        {
            isConnectedToInternet = true;
        }

        SaveGame.Save("statusOfConnection", isConnectedToInternet);
    }

}
