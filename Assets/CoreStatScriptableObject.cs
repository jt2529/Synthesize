using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Stats/Core", order = 1)]
public class CoreStatScriptableObject : ScriptableObject
{
    public string statName = "Default Core Stat Name";
    public Color color_prime = Color.white;

    public int value { 
        get { return totalBuff() + value; }
        set { this.value = value; } 
    }

    private List<int> buffList = new List<int>();

    public void addBuff(int buffValue)
    {
        buffList.Add(buffValue);
    }
    
    public void clearAllBuffs()
    {
        buffList.Clear();
    }
    
    public int totalBuff()
    {
        int buffTotal = 0; ;
        foreach(int buff in buffList)
        {
            buffTotal += buff;
        }

        return buffTotal;
    }

}