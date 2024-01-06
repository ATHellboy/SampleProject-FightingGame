using UnityEngine;
using VContainer;
using AlirezaTarahomi.FightingGame.Tool;
using Infrastructure.ObjectPooling;
using AlirezaTarahomi.FightingGame.General.Variable;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    public abstract class ThrowingObjectBehavior : ScriptableObject
    {
        [SerializeField] private IntVariable _objectCounter;
        [SerializeField] private float _throwingForce = 1.5f;
        [SerializeField] private PooledObjectStats _throwableObjectPoolStats;
        
        private PoolingSystem _poolSystem;
        protected CharacterBehaviorContext _context;

        [Inject]
        public void Construct(PoolingSystem poolSystem, CharacterBehaviorContext context)
        {
            _poolSystem = poolSystem;
            _context = context;
        }

        public void UseObject(Vector3 direction)
        {
            if (_objectCounter.value > 0)
            {
                _objectCounter.value--;
                Transform obj = _poolSystem.Spawn(_throwableObjectPoolStats, _context.Transform.position +
                    _context.Pivot.transform.right, Quaternion.identity);
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
    }
}