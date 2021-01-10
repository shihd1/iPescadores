using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class ServerService : MonoBehaviour
{
    private static string appserver = "http://18.218.236.171:3000";

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
    public static bool signIn(string username, string password)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        var cryptoMD5 = System.Security.Cryptography.MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = cryptoMD5.ComputeHash(bytes);
        string encryptedPassword = BitConverter.ToString(hash).Replace("-", string.Empty);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appserver+"/signin/" + username + "/" + encryptedPassword);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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
            GameObject.Find("Manager").GetComponent<FriendManager>().showAllFriends();
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
            
            return true;
        }
        else
        {
            return false;
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

    public string[] friendRequest;
    public string[] friendList;
    public bool[] achievementStatus;
    public int[] numLife;
}