using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyCoinScript : MonoBehaviour
{
    public bool collected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string currentDate = PlayerPrefs.GetString("CurrentDate");
        if (currentDate == "" || !currentDate.Equals(System.DateTime.Now.ToString("yyyy-MM-dd")))
        {
            //Show panel to collect coin
            GeneralManager.FindInActiveObjectByName("Coin Canvas").SetActive(true);
        }
    }
    public void setDate()
    {
        PlayerPrefs.SetString("CurrentDate", System.DateTime.Now.ToString("yyyy-MM-dd"));
    }
}
