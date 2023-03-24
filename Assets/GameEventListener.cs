using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// Add this as a component to a Game Object. Assign the GameEventScriptableObject for the event you want to listen for.
// Add whatever methods you want to call when the Game Event occurs.
public class GameEventListener : MonoBehaviour
{

    public GameEventScriptableObject Event;
    public UnityEvent Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised()
    { Response.Invoke(); }
}