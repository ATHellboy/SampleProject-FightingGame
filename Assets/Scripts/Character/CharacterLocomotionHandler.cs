﻿using AlirezaTarahomi.FightingGame.Player;
using Infrastructure.Extension;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterLocomotionHandler
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly CharacterContext _characterContext;

        private float _onAirGravityScale;
        private float _throwingAngle;
        private float _moveSpeed;

        public CharacterLocomotionHandler(Rigidbody2D rigidbody, Transform transform, CharacterContext characterContext)
        {
            _rigidbody = rigidbody;
            _transform = transform;
            _characterContext = characterContext;

            _onAirGravityScale = (Mathf.Pow(
                _characterContext.stats.airMovementValues.jumpSpeed, 2) / (2 * _characterContext.stats.airMovementValues.jumpHeight)) / 9.81f;
        }

        public void ChangeDetectionCollisionMode(CollisionDetectionMode2D mode)
        {
            _rigidbody.collisionDetectionMode = mode;
        }

        public void ChangeMoveSpeed(float speed)
        {
            _moveSpeed = speed;
        }

        public void Move(Vector2 moveAxes)
        {
            Flip(moveAxes.x);
            _rigidbody.SetXVelocity(moveAxes.x * _moveSpeed);
        }

        public void Jump(float height, float speed)
        {
            _rigidbody.gravityScale = (Mathf.Pow(speed, 2) / (2 * height)) / 9.81f;
            _rigidbody.SetYVelocity(speed);
        }

        public void PushForward(float velocity)
        {
            _rigidbody.velocity = new Vector2(_transform.right.x * velocity, 0);
        }

        private void ProjectMotion(Side side, Vector3 targetPos)
        {
            float d = Mathf.Abs(targetPos.x - _transform.position.x);
            float h = Mathf.Abs(_transform.position.y - targetPos.y);
            float v = (d * Mathf.Sqrt(-Physics2D.gravity.y * _rigidbody.gravityScale)) /
                Mathf.Sqrt(2 * Mathf.Cos(_throwingAngle * Mathf.Deg2Rad) *
                (d * Mathf.Sin(_throwingAngle * Mathf.Deg2Rad) + h * Mathf.Cos(_throwingAngle * Mathf.Deg2Rad)));

            float Vx = v * Mathf.Cos(_throwingAngle * Mathf.Deg2Rad);
            float Vy = v * Mathf.Sin(_throwingAngle * Mathf.Deg2Rad);
            Vector2 velocity = new(Vx * -(int)side, Vy);

            _rigidbody.velocity = velocity;
        }

        public void ThrowInside(Side side, Vector3 targetPos)
        {
            Stop();
            SetEnterGravityScale();
            Flip(side);
            ProjectMotion(side, targetPos);
        }

        public void Teleport(Vector2 position)
        {
            _transform.position = position;
        }

        public void GoOutside(float velocity, Side side)
        {
            SetNoGravityScale();
            _rigidbody.velocity = new Vector2((int)side, 0) * velocity;
        }

        public Vector2 GetVelocity()
        {
            return _rigidbody.velocity;
        }

        public void Flip(Side side)
        {
            if ((int)side == -1)
            {
                _transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        public void Flip(float moveXAxis)
        {
            float sign = Mathf.Sign(moveXAxis);

            if (moveXAxis == 0)
                return;

            if (sign == 1)
            {
                _transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        public void Stop()
        {
            _rigidbody.velocity = Vector2.zero;
        }

        public void SetNoGravityScale()
        {
            _rigidbody.gravityScale = 0;
        }

        public void SetGroundGravityScale()
        {
            _rigidbody.gravityScale = 1;
        }

        public void SetAirGravityScale()
        {
            _rigidbody.gravityScale = _onAirGravityScale;
        }

        public void SetEnterGravityScale()
        {
            _rigidbody.gravityScale = 25;
        }
    }
}