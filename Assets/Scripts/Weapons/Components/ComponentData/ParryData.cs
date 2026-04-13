using UnityEngine;

namespace Synthesize.Weapons.Components
{
    public class ParryData : ComponentData<AttackParry>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Parry);
        }
    }
}