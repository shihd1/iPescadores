using UnityEngine;
using UnityEngine.UI;

public class FriendManager : MonoBehaviour
{
    //Accessing other object scripts:
    public GameObject localData;
    public GameObject server;

    //Accessing other objects:
    public GameObject[] hex;

    // Start is called before the first frame update
    void Start()
    {
        //Get friend ID & Put them in the world
        string[] friendID = localData.GetComponent<Data>().friendID;
        point[] friendLocations = localData.GetComponent<Data>().friendLocation;
        for (int i = 0; i < friendID.Length; i++)
        {
            string fid = friendID[i];
            float x = friendLocations[i].x;
            float z = friendLocations[i].z;
            int level = ServerService.getLevel(fid);
            showFriend(level, new Vector3(x, 0, z));
        }
        showFriend(1, new Vector3(-19.03f, 0f, 9f));
        showFriend(1, new Vector3(-19.03f, 0f, -9f));
        showFriend(1, new Vector3(-1.732f, 0f, -20.997f));
        showFriend(1, new Vector3(-1.732f, 0f, 20.997f));
        showFriend(1, new Vector3(19.03f, 0f, 9f));
        showFriend(1, new Vector3(19.03f, 0f, -9f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Start functions
    public void showFriend(int level, Vector3 x)
    {
        //Debug.Log(level);
        Instantiate(hex[level], x, Quaternion.identity);
    }

    //Functions
    public void sendRequest(Text username)
    {
        string id = localData.GetComponent<Data>().id;
        string friendID = ServerService.getID(username.text);
        if(friendID==null)
        {
            //Display: can't find user
            Debug.Log("Can't find user");
        }
        else
        {
            bool success = ServerService.sendFriendRequest(id, friendID);
            if (success)
            {
                //Display: sent friend request
                Debug.Log("Success! Sent friend request");
            }
            else
            {
                //Display: fail to send
                Debug.Log("Fail to send");
            }
        }
    }
    
    public void confirmFriendRequest(Text username)
    {
        string id = localData.GetComponent<Data>().id;
        string friendID = ServerService.getID(username.text);
        bool successidadd = ServerService.addFriend(id,friendID);
        bool successfriendadd = ServerService.addFriend(friendID, id);
        if (successidadd & successfriendadd)
        {
            //Display: added friend
            Debug.Log("Success! Added friend (from both ways)");
            bool removeSuccess = ServerService.removeFriendRequest(id, friendID);
            if (removeSuccess)
            {
                Debug.Log("Success! Removed Friend Request");
            }
            else
            {
                Debug.Log("Your friend's request cannot be removed at this time");
            }
        }
        else if (successidadd & !successfriendadd)
        {
            Debug.Log("Your friend could not add you (but you could add your friend).");
        }
        else if (!successidadd & successfriendadd)
        {
            Debug.Log("You could not add your friend (but your friend could add you).");
        }
        else
        {
            //Display: failed to add friend
            Debug.Log("Your friend cannot be added at this time.");
        }
    }
    public void rejectFriendRequest(Text username)
    {
        string id = localData.GetComponent<Data>().id;
        string friendID = ServerService.getID(username.text);
        bool removeSuccess = ServerService.removeFriendRequest(id, friendID);
        if (removeSuccess)
        {
            Debug.Log("Success! Removed Friend Request");
        }
        else
        {
            Debug.Log("Your friend's request cannot be removed at this time");
        }
    }
}
