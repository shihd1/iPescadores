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
            int x = friendLocations[i].x;
            int z = friendLocations[i].z;
            int level = ServerService.getLevel(fid);
            showFriend(level, new Vector3(x, 0, z));
        }
        //showFriend(serverService.getLevel(1604026294571+""), new Vector3(-19.03f, 0, 9));

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

    }
    public void rejectFriendRequest(Text username)
    {

    }
}
