using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class ServerService : MonoBehaviour
{
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
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
    }
    public static string usernameExist(string username)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/userexist/"+username);
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
        var cryptoMD5 = System.Security.Cryptography.MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = cryptoMD5.ComputeHash(bytes);
        string encryptedPassword = BitConverter.ToString(hash).Replace("-",string.Empty);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/useradd/" + username + "/" + firstname + "/" + lastname + "/" + encryptedPassword);
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
        var cryptoMD5 = System.Security.Cryptography.MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = cryptoMD5.ComputeHash(bytes);
        string encryptedPassword = BitConverter.ToString(hash).Replace("-", string.Empty);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/signin/" + username + "/" + encryptedPassword);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output o = JsonUtility.FromJson<Output>(jsonResponse);
        if (o.status.Equals("success"))
        {
            GameObject.Find("Local Data").GetComponent<Data>().id = o.id;
            GameObject.Find("Local Data").GetComponent<Data>().level = o.level;
            GameObject.Find("Local Data").GetComponent<Data>().friendID = o.friendList;
            GameObject.Find("Local Data").GetComponent<Data>().friendRequestID = o.friendRequest;
            GameObject.Find("Manager").GetComponent<FriendManager>().showAllFriends();
            return true;
        }
        else
        {
            return false;
        }
    }
    public static int getLevel(string id)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/getLevel/" + id);
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
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/sendfriendrequest/" + id + "/" + friendID);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static string getID(string username)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/getID/" + username);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.id;
    }
    public static string getUsername(string id)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/getusername/" + id);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.username;
    }
    public static bool addFriend(string id, string friendID)
    {
        Debug.Log(id + "   " + friendID);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/addFriend/" + id + "/" + friendID);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Output info = JsonUtility.FromJson<Output>(jsonResponse);
        return info.status.Equals("success");
    }
    public static bool removeFriendRequest(string id, string friendID)
    {
        Debug.Log(id + "   " + friendID);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/removefriendrequest/" + id + "/" + friendID);
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
    public bool exist;
    public int level;

    public string[] friendRequest;
    public string[] friendList;
}