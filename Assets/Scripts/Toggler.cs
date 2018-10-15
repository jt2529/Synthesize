using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Toggler : MonoBehaviour
{
    public abstract void toggle();
    public abstract bool isToggled();
}
