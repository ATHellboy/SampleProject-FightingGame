using AlirezaTarahomi.FightingGame.Player;
using Infrastructure.Extension;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterLocomotionHandler
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly CharacterPivot _pivot;
        private readonly CharacterContext _characterContext;
        private readonly SurfaceCheck _surfaceCheck;

        private float _onAirGravityScale;
        private float _throwingAngle;
        private float _moveSpeed;

        public CharacterLocomotionHandler(Rigidbody2D rigidbody, Transform transform, CharacterPivot pivot, CharacterContext characterContext,
            SurfaceCheck surfaceCheck)
        {
            _rigidbody = rigidbody;
            _transform = transform;
            _pivot = pivot;
            _characterContext = characterContext;
            _surfaceCheck = surfaceCheck;

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
            _rigidbody.velocity = new Vector2(
                _surfaceCheck.onGround ? -moveAxes.x * _moveSpeed * _surfaceCheck.SlopeNormalPerp.x : moveAxes.x * _moveSpeed,
                _surfaceCheck.onGround ? -moveAxes.x * _moveSpeed * _surfaceCheck.SlopeNormalPerp.y : _rigidbody.velocity.y);
        }

        public void StopSliding()
        {
            if (_surfaceCheck.onGround)
                _rigidbody.SetXVelocity(0);
        }

        /// Variable jump by <see cref=Jump/> and <see cref=StopJump/> functions
        public void Jump(float height, float speed)
        {
            _rigidbody.gravityScale = (Mathf.Pow(speed, 2) / (2 * height)) / 9.81f;
            _rigidbody.SetYVelocity(speed);
        }

        public void StopJump()
        {
            _rigidbody.SetYVelocity(0);
        }

        public void PushForward(float velocity)
        {
            _rigidbody.velocity = new Vector2(_pivot.transform.right.x * velocity, 0);
        }

        private void ProjectMotion(Side side, Vector3 targetPos)
        {
            float d = Mathf.Abs(targetPos.x - _transform.position.x);
            float h = Mathf.Abs(_transform.position.y - targetPos.y);
            float v = (d * Mathf.Sqrt(-Physics2D.gravity.y * _rigidbody.gravityScale)) /
                Mathf.Sqrt(2 * Mathf.Cos(_throwingAngle * Mathf.Deg2Rad) *
                (d * Mathf.Sin(_throwingAngle * Mathf.Deg2Rad) + h * Mathf.Cos(_throwingAngle * Mathf.Deg2Rad)));
            _rigidbody.velocity = new Vector2(-(int)side * Mathf.Cos(_throwingAngle * Mathf.Deg2Rad), Mathf.Sin(_throwingAngle * Mathf.Deg2Rad)) * v;
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
                _pivot.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _pivot.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            _surfaceCheck.face = (int)side;
        }

        public void Flip(float moveXAxis)
        {
            float sign = Mathf.Sign(moveXAxis);

            if (moveXAxis == 0)
                return;

            if (sign == 1)
            {
                _pivot.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _pivot.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            _surfaceCheck.face = (int)sign;
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