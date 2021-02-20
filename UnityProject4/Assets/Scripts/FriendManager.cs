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
        //showFriend(1, new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Start functions
    public void showAllFriends()
    {
        Debug.Log(this.ToString() + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        initializeFriendLocations();
        //Get friend ID & Put them in the world
        string[] friendID = localData.GetComponent<Data>().friendID;
        Point[] friendLocations = localData.GetComponent<Data>().friendLocation;
        for (int i = 0; i < friendID.Length; i++)
        {
            string fid = friendID[i];
            //Debug.Log("Friend ID: "+fid);
            float x = friendLocations[i].x;
            float z = friendLocations[i].z;
            int level = ServerService.getLevel(fid);
            //Debug.Log("LEVEL: " + level);
            showFriend(level, new Vector3(x, 0, z));
        }
    }
    public void initializeFriendLocations()
    {
        /*Order of placement
        

                                (3.46,42)
                                            (20.76,30)

                                (1.73,21)             
                                                          (38.06,18)
                   (-17.3,12)               (19.03,9)
                                  (0,0)                   (36.33,-3)
                  (-19.03,-9)               (17.3,-12)
                                                          (34.6,-24)
                              (-1.73,-21.0)



        */
        string[] friendID = localData.GetComponent<Data>().friendID;
        localData.GetComponent<Data>().friendLocation = new Point[friendID.Length];
        Point[] friendLocation = localData.GetComponent<Data>().friendLocation;

        float[] addX = new float[] {17.3f, -1.73f, -19.03f, -17.3f, 1.73f, 19.03f };
        float[] addZ = new float[] {-12f, -21f, -9f, 12f, 21f, 9f };

        int counter = 0;
        int loop = 1;
        float currentX = 0f;
        float currentZ = 0f;

        bool exit = false;
        while (exit == false)
        {
            for (int j = -1; j < 6 * loop; j++)
            {
                if (counter == friendID.Length)
                {
                    exit = true;
                    break;
                }
                if (j == -1)
                {
                    currentX = currentX + 1.73f;
                    currentZ = currentZ + 21f;
                }
                else
                {
                    currentX = currentX + addX[j / loop];
                    currentZ = currentZ + addZ[j / loop];
                }
                if (j != 6 * loop - 1)
                {
                    friendLocation[counter] = new Point(currentX, currentZ);
                    //Debug.Log(currentX + " " + currentZ);
                    counter++;
                }
            }
            loop++;
        }
        for (int i = 0; i < friendID.Length; i++)
        {
            //Debug.Log(friendLocation[i].x + " " + friendLocation[i].z);
        }
    }
    public void showFriend(int level, Vector3 x)
    {
        //Debug.Log(level);
        Instantiate(hex[level], x, Quaternion.identity);
    }
    public void addFriendLocations()
    {
        Point[] friendLocations = localData.GetComponent<Data>().friendLocation;
    }

    //Functions
    public void sendRequest(Text username)
    {
        Debug.Log(this.ToString()+" "+System.Reflection.MethodBase.GetCurrentMethod().Name);
        string id = localData.GetComponent<Data>().id;
        string friendID = ServerService.getID(username.text);
        Transform Panel = GameObject.Find("Canvas").transform.Find("Account - Yes or No");
        Text t = Panel.transform.Find("Display Info").transform.Find("Text").gameObject.GetComponent<Text>();
        if (friendID==null)
        {
            //Display: can't find user
            Debug.Log("Can't find user");
            t.text = "Can't find user";
        }
        else
        {
            bool success = ServerService.sendFriendRequest(id, friendID);
            if (success)
            {
                //Display: sent friend request
                Debug.Log("Success! Sent friend request");
                t.text = "Success! Sent friend request";
            }
            else
            {
                //Display: fail to send
                Debug.Log("Already Sent or Fail to send");
                t.text = "Already Sent or Fail to send";
            }
        }
        Panel.gameObject.SetActive(true);
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

            //Update Total XP
            localData.GetComponent<Data>().totalXP += 50;
            bool updatedTotalXP = ServerService.updateTotalXP() && ServerService.updateTotalXP(friendID,50);
            GameObject.Find("Canvas").transform.Find("Main Page").transform.Find("Radial Progress bar").GetComponent<ProgressBar>().setFill();
            if (updatedTotalXP)
                Debug.Log("UpdatedTotalXP");
            else
                Debug.Log("Could not update totalXP");

            //Update Coins
            localData.GetComponent<Data>().coins += 50;
            bool updatedCoins = ServerService.updateCoins() && ServerService.updateCoins(friendID,50);
            GameObject.Find("Canvas").transform.Find("Main Page").transform.Find("Horizontal Panel").transform.Find("Text (TMP)").GetComponent<CoinPanel>().updateCoinCount();
            if (updatedCoins)
                Debug.Log("UpdatedCoins");
            else
                Debug.Log("Could not update coins");

            //Remove friend Request
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
