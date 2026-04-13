using Synthesize.Weapons.Components;
using UnityEngine;

namespace Synthesize.Weapons.Modifiers
{
    public delegate bool ConditionalDelegate(Transform source, out DirectionalInformation directionalInformation);
}