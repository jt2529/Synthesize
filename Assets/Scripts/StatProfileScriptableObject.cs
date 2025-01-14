using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Stats/StatProfile", order = 2)]
public class StatProfileScriptableObject : ScriptableObject
{

    public CoreStatScriptableObject mentalStat;
    public CoreStatScriptableObject socialStat;
    public CoreStatScriptableObject physicalStat;
    public CoreStatScriptableObject luckStat;


    public string profileName = "Defult Profile Name";

    public int mentalBonus;
    public int socialBonus;
    public int physicalBonus;
    public int luckBonus;

    public int physicalValue()
    {
        return physicalStat.value + physicalBonus;
    }

    public int socialValue()
    {
        return socialStat.value + socialBonus;
    }

    public int mentalValue()
    {
        return mentalStat.value + mentalBonus;
    }

    public int luckValue()
    {
        return luckStat.value + luckBonus;
    }

}