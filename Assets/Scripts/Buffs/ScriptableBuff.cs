using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffScriptableObject : ScriptableObject
{

    /**
     * Time duration of the buff in seconds.
     */
    public float Duration;

    /**
     * Duration is increased each time the buff is applied.
     */
    public bool IsDurationStacked;

    /**
     * Effect value is increased each time the buff is applied.
     */
    public bool IsEffectStacked;

    public abstract TimedBuff InitializeBuff(GameObject obj);

}
