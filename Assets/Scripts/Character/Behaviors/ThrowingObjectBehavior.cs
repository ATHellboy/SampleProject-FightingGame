using UnityEngine;
using Zenject;
using AlirezaTarahomi.FightingGame.Tool;
using Infrastructure.ObjectPooling;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    [CreateAssetMenu(menuName = "Attacks/ThrowingObjectBehavior")]
    public class ThrowingObjectBehavior : ScriptableObject
    {
        [SerializeField] private int _maxObjectNumber = 9;
        [SerializeField] private float _throwingForce = 1.5f;
        [SerializeField] private PooledObjectStats _throwableObjectPoolStats = default;

        [HideInInspector] public int counter;

        private PoolingSystem _poolSystem;
        private CharacterBehaviorContext _context;

        [Inject]
        public void Construct(PoolingSystem poolSystem)
        {
            _poolSystem = poolSystem;

            counter = _maxObjectNumber;
        }

        public void Inject(CharacterBehaviorContext context)
        {
            _context = context;
        }

        public void UseObject(Vector3 direction)
        {
            if (counter > 0)
            {
                counter--;
                Transform obj = _poolSystem.Spawn(_throwableObjectPoolStats, _context.Transform.position +
                    _context.Transform.right, Quaternion.identity);
                _context.OwnershipService.Add(obj.gameObject);
                IThrowableObject throwableObject = obj.GetComponent<IThrowableObject>();
                if (_context.isPowerupActive)
                {
                    throwableObject.PrepareForPowerup();
                }
                throwableObject.Throw(_throwingForce, direction.normalized);
            }
            _context.OnAttackEnded?.Invoke();
        }

        public void PickUpObject(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Hitbox))
            {
                if (_context.OwnershipService.Contains(collision.gameObject))
                {
                    IThrowableObject throwableObject = collision.GetComponent<IThrowableObject>();
                    if (throwableObject.CanPick)
                    {
                        counter++;
                        _poolSystem.Despawn(_throwableObjectPoolStats, collision.transform);
                        _context.OwnershipService.Remove(collision.gameObject);
                    }
                }
            }
        }
    }
}