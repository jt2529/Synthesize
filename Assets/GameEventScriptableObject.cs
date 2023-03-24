using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A generic scriptable object that is used to notify listeners that an event has occurred.
// Create a new GameEventScriptableObject in the Create Assets menu and name it something relevant.
// In a script, create a GameEventScriptableObject variable and assign the appropriate asset
// In the script, use Raise() on that object to notify listeners that the even happened.

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Game Event", order = 1)]
public class GameEventScriptableObject : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised();
    }

    public void RegisterListener(GameEventListener listener)
    { listeners.Add(listener); }

    public void UnregisterListener(GameEventListener listener)
    { listeners.Remove(listener); }

    // this is a big fart

}