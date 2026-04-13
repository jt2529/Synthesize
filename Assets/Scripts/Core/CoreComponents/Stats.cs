using System;
using Synthesize.CoreSystem.StatsSystem;
using UnityEngine;

namespace Synthesize.CoreSystem
{
    public class Stats : CoreComponent
    {
       [field: SerializeField] public Stat Health { get; set; }
       [field: SerializeField] public Stat Poise { get; set; }


        [SerializeField] private float poiseRecoveryRate;
        
        protected override void Awake()
        {
            base.Awake();
            
            Health.Init();
            Poise.Init();
        }

        private void Update()
        {
            if (Poise.CurrentValue.Equals(Poise.MaxValue))
                return;
            
            Poise.Increase(poiseRecoveryRate * Time.deltaTime);
        }
    }
}
