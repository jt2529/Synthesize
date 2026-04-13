using System;
using Synthesize.Weapons.Components;
using UnityEngine;
using Synthesize.CoreSystem;

namespace Synthesize.Weapons.Components
{
    public class Movement : WeaponComponent<MovementData, AttackMovement>
    {
        private MovementPhysics movementPhysics;

        private float velocity;
        private Vector2 direction;

        private void HandleStartMovement()
        {
            velocity = currentAttackData.Velocity;
            direction = currentAttackData.Direction;
            SetVelocity();
        }

        private void HandleStopMovement()
        {
            velocity = 0f;
            direction = Vector2.zero;

            SetVelocity();
        }

        protected override void HandleEnter()
        {
            base.HandleEnter();
            
            velocity = 0f;
            direction = Vector2.zero;
        }

        private void FixedUpdate()
        {
            if(!isAttackActive)
                return;
            
            SetVelocityX();
        }

        private void SetVelocity()
        {
            movementPhysics.SetVelocity(direction * velocity * movementPhysics.facingDirection * Time.deltaTime);
        }

        private void SetVelocityX()
        {
            movementPhysics.SetVelocityX((direction * velocity * movementPhysics.facingDirection).x * Time.deltaTime);
        }

        protected override void Start()
        {
            base.Start();

            movementPhysics = Core.GetCoreComponent<MovementPhysics>();
            
            AnimationEventHandler.OnStartMovement += HandleStartMovement;
            AnimationEventHandler.OnStopMovement += HandleStopMovement;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            AnimationEventHandler.OnStartMovement -= HandleStartMovement;
            AnimationEventHandler.OnStopMovement -= HandleStopMovement;
        }
    }
}