﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public GameObject[] objects;
    public int[] objectCost;
    public int[] objectLevel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void buy(int index)
    {
        Debug.Log(this.ToString() + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        int costI = objectCost[index];
        Debug.Log("Buy item " + index + " costing "+costI);
        //GameObject.Find("Local Data").GetComponent<Data>().addCoins(-costI);

        GameObject.Find("Manager").GetComponent<ModelManager>().showModel(index);
    }
    public void removeLevelRestrictions()
    {
        Debug.Log(this.ToString() + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        int playerLevel = GameObject.Find("Local Data").GetComponent<Data>().level;
        for(int i = 0; i < objectLevel.Length; i++)
        {
            if(playerLevel >= objectLevel[i])
            {
                objects[i].transform.Find("Panel").gameObject.SetActive(false);
            }
        }
    }
}
