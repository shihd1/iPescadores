﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ServerService : MonoBehaviour
{
    //public static string appserver = "http://114.35.46.192:3000";
    public static string appserver = "http://localhost:3000";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getRequest()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
    }
    public static string usernameExist(string username)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/userexist/"+username);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        if (info.status.Equals("success"))
        {
            return ""+info.exist;
        }
        else
        {
            Debug.Log("ERROR --> cannot determine if username exist");
            return "ERROR";
        }
    }
    public static string createUser(string username, string firstname, string lastname, string password)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        var cryptoMD5 = System.Security.Cryptography.MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = cryptoMD5.ComputeHash(bytes);
        string encryptedPassword = BitConverter.ToString(hash).Replace("-",string.Empty);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/useradd/" + username + "/" + firstname + "/" + lastname + "/" + encryptedPassword);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        if (info.status.Equals("success"))
        {
            Debug.Log(info.id);
            GameObject.Find("Local Data").GetComponent<Data>().id = info.id;
            return info.id;
        }
        else
        {
            Debug.Log("ERROR --> cannot create user");
            return "ERROR";
        }
    }
    public static int signIn(string username, string password)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        var cryptoMD5 = System.Security.Cryptography.MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = cryptoMD5.ComputeHash(bytes);
        string encryptedPassword = BitConverter.ToString(hash).Replace("-", string.Empty);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/signin/" + username + "/" + encryptedPassword);
        HttpWebResponse response = null;
        try
        {
            response = (HttpWebResponse)request.GetResponse();
        }
        catch(Exception)
        {
            return 2;
        }
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output o = JsonUtility.FromJson<Output>(jsonResponse);
        if (o.status.Equals("success"))
        {
            GameObject.Find("Local Data").GetComponent<Data>().id = o.id;
            GameObject.Find("Local Data").GetComponent<Data>().level = o.level;
            GameObject.Find("Local Data").GetComponent<Data>().coins = o.coins;
            GameObject.Find("Local Data").GetComponent<Data>().totalXP = o.totalXP;
            GameObject.Find("Local Data").GetComponent<Data>().friendID = o.friendList;
            GameObject.Find("Local Data").GetComponent<Data>().friendRequestID = o.friendRequest;
            GameObject.Find("Local Data").GetComponent<Data>().achievementStatus = o.achievementStatus;
            GameObject.Find("Local Data").GetComponent<Data>().numLife = o.numLife;
            if(GameObject.Find("Local Data").GetComponent<Data>().numLife.Length == 0)
            {
                GameObject.Find("Local Data").GetComponent<Data>().numLife = new int[20];
            }
            GameObject.Find("Manager").GetComponent<ModelManager>().showMyModel();
            GameObject.Find("Manager").GetComponent<ModelManager>().showAllMyModels();
            GameObject.Find("Manager").GetComponent<FriendManager>().showAllFriends();
            GameObject.Find("Manager").GetComponent<GeneralManager>().startChecking();
            GameObject.Find("Canvas")
                .transform.Find("Friend Requests")
                .transform.Find("Scroll View")
                .transform.Find("Viewport")
                .transform.Find("Content")
                .GetComponent<FriendRequestContentManager>().showFriendRequests();
            GameObject.Find("Local Data").GetComponent<Data>().analyzeData();
            GameObject.Find("Canvas")
                .transform.Find("Main Page")
                .transform.Find("Horizontal Panel")
                .transform.Find("Text (TMP)")
                .GetComponent<CoinPanel>().updateCoinCount();
            GameObject.Find("Canvas")
                .transform.Find("Shop")
                .GetComponent<ShopScript>().removeLevelRestrictions();

            return 0;
        }
        else
        {
            return 1;
        }
    }
    public static int getLevel(string id)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/getLevel/" + id);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        if (info.status.Equals("success"))
        {
            return info.level;
        }
        else
        {
            Debug.Log("ERROR --> cannot determine if username exist");
            return -1;
        }
    }
    public static bool sendFriendRequest(string id, string friendID)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/sendfriendrequest/" + id + "/" + friendID);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static string getID(string username)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/getID/" + username);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.id;
    }
    public static string getUsername(string id)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/getusername/" + id);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.username;
    }
    public static int getXP(string id)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver + "/getXP/" + id);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.totalXP;
    }
    public static int getCoins(string id)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver + "/getCoins/" + id);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.coins;
    }
    public static int getPoints(string id)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver + "/getPoints/" + id);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.points;
    }
    public static bool addFriend(string id, string friendID)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/addFriend/" + id + "/" + friendID);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static bool removeFriendRequest(string id, string friendID)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/removefriendrequest/" + id + "/" + friendID);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static bool updateTotalXP()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        string id = GameObject.Find("Local Data").GetComponent<Data>().id;
        int newXP = GameObject.Find("Local Data").GetComponent<Data>().totalXP;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/updateTotalXP/" + id + "/" + newXP);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static bool updateTotalXP(string friendID, int xp)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        int newXP = ServerService.getXP(friendID)+xp;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver + "/updateTotalXP/" + friendID + "/" + newXP);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static bool updateCoins()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        string id = GameObject.Find("Local Data").GetComponent<Data>().id;
        int newCoins = GameObject.Find("Local Data").GetComponent<Data>().coins;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver + "/updateCoins/" + id + "/" + newCoins);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static bool updateCoins(string friendID,int coins)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        int newCoins = ServerService.getCoins(friendID)+coins;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver + "/updateCoins/" + friendID + "/" + newCoins);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static bool updateAchievementStatus(int index)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        string id = GameObject.Find("Local Data").GetComponent<Data>().id;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/updateAchievementStatus/" + id + "/" + index);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static bool updateNumLife(int index, int newLife)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        string id = GameObject.Find("Local Data").GetComponent<Data>().id;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/updateNumLife/" + id + "/" + index + "/" + newLife);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static bool updateLevel()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        string id = GameObject.Find("Local Data").GetComponent<Data>().id;
        int level = GameObject.Find("Local Data").GetComponent<Data>().level;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver + "/updateLevel/" + id + "/" + level);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static bool erasePoints()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        string id = GameObject.Find("Local Data").GetComponent<Data>().id;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver + "/updatePoints/" + id + "/" + 0);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
}

[System.Serializable]
public class Output
{
    public string status;

    public string id;
    public string username;
    public int totalXP;
    public int coins;
    public bool exist;
    public int level;
    public int points;

    public string[] friendRequest;
    public string[] friendList;
    public bool[] achievementStatus;
    public int[] numLife;
}