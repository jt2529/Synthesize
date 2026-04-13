using Synthesize.Combat.KnockBack;
using Synthesize.ModifierSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Synthesize.CoreSystem
{
    public class KnockBackReceiver : CoreComponent, IKnockBackable
    {
        public Modifiers<Modifier<KnockBackData>, KnockBackData> Modifiers { get; } = new();

        [SerializeField] private float maxKnockBackTime  = 0.2f ;

        private bool isKnockBackActive;
        private float knockBackStartTime;
        private MovementPhysics movementPhysics;

        public override void LogicUpdate()
        {
            CheckKnockBack();
        }

        public void KnockBack(KnockBackData data)
        {
            data = Modifiers.ApplyAllModifiers(data);

            movementPhysics.ForceX += data.Strength * data.Angle.x * data.Direction;
            movementPhysics.ForceY += data.Strength * data.Angle.y;
            movementPhysics.canSetVelocity = false;
            isKnockBackActive = true;
            knockBackStartTime = Time.time;
        }

        private void CheckKnockBack()
        {
            if (isKnockBackActive
                && ((movementPhysics.collisions.below)
                    || Time.time >= knockBackStartTime + maxKnockBackTime)
               )
            {
                isKnockBackActive = false;
                movementPhysics.canSetVelocity = true;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            movementPhysics = core.GetCoreComponent<MovementPhysics>();
        }
    }
}