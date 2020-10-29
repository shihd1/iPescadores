using UnityEngine;

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
        string[] friendID = localData.GetComponent<Data>().friendID;
        point[] friendLocations = localData.GetComponent<Data>().friendLocation;
        for (int i = 0; i < friendID.Length; i++)
        {
            string fid = friendID[i];
            int x = friendLocations[i].x;
            int z = friendLocations[i].z;
            int level = serverService.getLevel(fid);
            showFriend(level, new Vector3(x, 0, z));
        }
        showFriend(1, new Vector3(-19.03f, 0, 9));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showFriend(int level, Vector3 x)
    {
        //hex[level].SetActive(true);
        Instantiate(hex[level], x, Quaternion.identity);
    }
}
