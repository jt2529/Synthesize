using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{

    public PlayerStats abilitySource;

    public GameEventScriptableObject beginPrimaryEvent;
    public GameEventScriptableObject endPrimaryEvent;

    public GameEventScriptableObject beginSecondaryEvent;
    public GameEventScriptableObject endSecondaryEvent;

    public abstract void beginPrimary();
    public abstract void endPrimary();
    public abstract void beginSecondary();
    public abstract void endSecondary();
    public abstract void reload();
}