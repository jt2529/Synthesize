using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffableEntity : MonoBehaviour
{

    [SerializeField] private PlayerStats playerStats;

    private readonly List<TimedBuff> _buffs = new List<TimedBuff>();
    private readonly Dictionary<int, TimedBuff> _tBuffs = new Dictionary<int, TimedBuff>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //OPTIONAL, return before updating each buff if game is paused
        //if (Game.isPaused)
        //    return;

        foreach (var buff in _buffs)
        {
            buff.Tick(Time.deltaTime);
            if (buff.IsFinished)
            {
                _buffs.Remove(buff);
            }
        }
    }

}