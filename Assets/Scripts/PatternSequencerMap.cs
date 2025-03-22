using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatternSequencerMap
{
    public int startMeasure;
    public List<int> sequencer;

    public PatternSequencerMap(int startMeasureIn, List<int> sequencerIn) 
    {
        startMeasure = startMeasureIn;
        sequencer = sequencerIn;
    }
    
}
