using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    //Player info
    public string id;
    public int level;
    public int coins;
    public int totalXP;
    public bool[] achievementStatus;
    public int[] numLife;

    //Friend info
    public string[] friendID;
    public Point[] friendLocation;

    //Friend request info
    public string[] friendRequestID;

    void Start()
    {
        //SaveData.current.profile = new PlayerProfile();
        //SerializationManager.Save("playerProgress", SaveData.current.profile);
        //SaveData.current.profile = (PlayerProfile)SerializationManager.Load(Application.persistentDataPath + "/saves/" + "playerProgress" + ".save");
        //Debug.Log(Application.persistentDataPath+"/saves/");
    }
    public void analyzeData()
    {
        GameObject.Find("Canvas").transform.Find("Main Page").transform.Find("Radial Progress bar").GetComponent<ProgressBar>().setFill();

        for(int i = 0; i < achievementStatus.Length; i++)
        {
            if (achievementStatus[i])
            {
                GameObject.Find("Canvas")
                    .transform.Find("Achievements")
                    .transform.Find("Scroll View")
                    .transform.Find("Viewport")
                    .transform.Find("Content")
                    .GetChild(i)
                    .transform.Find("Image_after")
                    .gameObject.SetActive(true);
            }
        }
        //sServerService.updateAchievementStatus(1);
        //Add some way to instantiate the animals and plants.
    }
}
public class Point
{
    public float x;
    public float z;
    public Point(float x, float z)
    {
        this.x = x;
        this.z = z;
    }
}
