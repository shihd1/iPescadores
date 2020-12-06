using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    private int coins;
    private int totalXP;
    //private string[,] achievementDescription;
    //private int[] achievementXP;
    private bool[] achievementStatus;
    private int[] numLife;

    public PlayerProfile()
    {
        coins = 0;
        totalXP = 0;
        achievementStatus = new bool[12] {false,false,false,false,false,false,false,false,false,false,false,false,};
        numLife = new int[6] {0,0,0,0,0,0,};
    }

    public int getCoins()
    {
        return coins;
    }
    public int getTotalXP()
    {
        return totalXP;
    }
    public bool[] getAchievementStatus()
    {
        return achievementStatus;
    }
    public int[] getNumLife()
    {
        return numLife;
    }

    public void addCoins(int amount)
    {
        coins += amount;
    }
    public void addXP(int amount)
    {
        totalXP += amount;
    }
    public void completeAchievement(int index)
    {
        achievementStatus[index] = true;
    }
    public void addOneLife(int index)
    {
        numLife[index]++;
    }
}
