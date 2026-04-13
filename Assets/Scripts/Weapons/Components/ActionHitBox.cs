using System;
using UnityEngine;
using Synthesize.CoreSystem;

namespace Synthesize.Weapons.Components
{
    public class ActionHitBox : WeaponComponent<ActionHitBoxData, AttackActionHitBox>
    {
        public event Action<Collider2D[]> OnDetectedCollider2D;

        private Vector2 offset;

        private CoreComp<CoreSystem.MovementPhysics> movement;

        private Collider2D[] detected;

        private void HandleAttackAction()
        {
            offset.Set(
                transform.position.x + (currentAttackData.HitBox.center.x * movement.Comp.facingDirection),
                transform.position.y + currentAttackData.HitBox.center.y
            );

            detected = Physics2D.OverlapBoxAll(offset, currentAttackData.HitBox.size, 0f, data.DetectableLayers);

            if (detected.Length == 0)
                return;

            OnDetectedCollider2D?.Invoke(detected);
        }

        protected override void Start()
        {
            base.Start();

            movement = new CoreComp<CoreSystem.MovementPhysics>(Core);
            AnimationEventHandler.OnAttackAction += HandleAttackAction;
            PlayerAbilitiesInputHandler playerAbilitiesInputHandler = this.GetComponentInParent<PlayerAbilitiesInputHandler>();
            playerAbilitiesInputHandler.OnAttackAction += HandleAttackAction;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AnimationEventHandler.OnAttackAction -= HandleAttackAction;
            PlayerAbilitiesInputHandler playerAbilitiesInputHandler = this.GetComponentInParent<PlayerAbilitiesInputHandler>();
            playerAbilitiesInputHandler.OnAttackAction -= HandleAttackAction;
        }

        private void OnDrawGizmos()
        {
            if (data == null)
                return;

            foreach (var item in data.GetAllAttackData())
            {
                if (!item.Debug)
                    continue;
                
                Gizmos.DrawWireCube(transform.position + (Vector3)item.HitBox.center, item.HitBox.size);
            }
        }
    }
}