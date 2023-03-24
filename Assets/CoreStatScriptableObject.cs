using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Stats/Core", order = 1)]
public class CoreStatScriptableObject : ScriptableObject
{
    public string statName = "Default Core Stat Name";
    public Color color_prime = Color.white;

    public int value;
    
}