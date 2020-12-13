using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendRequestContentManager : MonoBehaviour
{
    public GameObject friendRequest;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showFriendRequests()
    {
        Debug.Log(this.ToString() + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        string[] friendsID = GameObject.Find("Local Data").GetComponent<Data>().friendRequestID;
        for (int i = 0; i < friendsID.Length; i++)
        {
            GameObject g = Instantiate(friendRequest, transform);
            g.GetComponentInChildren<Text>().text = ServerService.getUsername(friendsID[i]);
        }
    }
}
