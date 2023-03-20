using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Player Data
    public int health;

    public float baseMaxJumpHeight;
    public float jumpHeightMultipler;
    public float minJumpHeight;
    public float maxJumpHeight;

    public int numberOfJumps;
    public float timeToJumpApex;

    public float wallJumpPower;
    public float wallJumpHeight;
    public float wallSlideSpeedDampener;

    public int numberOfDashes;
    public float fullDashTime;
    public float dashSpeedMultiplier;
    public float dashChargeCooldownTime;

    public float baseMoveSpeed;
    public float moveSpeedMultipler;
    public float moveSpeed;

    public float damageMultipler;

    public float knockbackMultiplier;

    public int baseMaxHealth;
    public float maxHealthMultiplier;
    public int maxHealth;

    public float meleeDamageMultiplier;
    public float rangedDamageMultiplier;

    //Current Game Data
    public int numberOfEnemies;
    public int numberOfKeys;
    public float lootOdds;
    public string activeSceneName;
}
