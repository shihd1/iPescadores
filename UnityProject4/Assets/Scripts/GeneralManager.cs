using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject particleSystem = FindInActiveObjectByName("Particle System");
        if (particleSystem.activeSelf == true && !particleSystem.GetComponent<ParticleSystem>().isPlaying)
        {
            particleSystem.SetActive(false);
        }
    }
    
    public void changeCoin(int amount)
    {
        //Update LocalData coins
        GameObject.Find("Local Data").GetComponent<Data>().coins += amount;
        
        //Update UI display
        GameObject.Find("Canvas")
                .transform.Find("Main Page")
                .transform.Find("Horizontal Panel")
                .transform.Find("Text (TMP)")
                .GetComponent<CoinPanel>().updateCoinCount();

        ////Gain xp
        //changeXP(amount);

        //Update server data - coins, numObjects
        ServerService.updateCoins();
    }
    public void changeXP(int amount)
    {
        //Update LocalData xp
        GameObject.Find("Local Data").GetComponent<Data>().totalXP += amount;

        int currentLevel = GameObject.Find("Local Data").GetComponent<Data>().level;

        //Convert xp into level and Update UI display
        GameObject.Find("Canvas")
            .transform.Find("Main Page")
            .transform.Find("Radial Progress bar")
            .GetComponent<ProgressBar>().setFill();

        //Show animation
        int newLevel = GameObject.Find("Local Data").GetComponent<Data>().level;
        if (currentLevel != newLevel)
        {
            FindInActiveObjectByName("Particle System").SetActive(true);
        }

        //Update level restrictions
        GameObject.Find("Canvas")
            .transform.Find("Shop")
            .GetComponent<ShopScript>().removeLevelRestrictions();

        //Update server data - totalXP, level
        //ServerService.updateLevel();
        //ServerService.updateTotalXP();

    }
    public static GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
