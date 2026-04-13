using UnityEngine;

namespace Synthesize.CoreSystem
{
    public class Death : CoreComponent
    {
        [SerializeField] private GameObject[] deathParticles;
        public EnemyStats EnemyStats => enemyStats ? enemyStats : core.GetComponentInParent<EnemyStats>();
        public EnemyStats enemyStats;
        private ParticleManager ParticleManager =>
            particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    
        private ParticleManager particleManager;

        private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
        private Stats stats;
    
        public void Die()
        {
            //foreach (var particle in deathParticles)
            //{
            //    ParticleManager.StartParticles(particle);
            //}

            EnemyStats.DropRewards();
            core.transform.parent.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Stats.Health.OnCurrentValueZero += Die;
        }

        private void OnDisable()
        {
            Stats.Health.OnCurrentValueZero -= Die;
        }
    }
}