namespace Synthesize.Weapons.Components
{
    public class ChargeToProjectileSpawnerData : ComponentData<AttackChargeToProjectileSpawner>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(ChargeToProjectileSpawner);
        }
    }
}