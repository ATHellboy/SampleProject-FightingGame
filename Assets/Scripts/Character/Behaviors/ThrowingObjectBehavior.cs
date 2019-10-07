using UnityEngine;
using Zenject;
using Assets.Infrastructure.Scripts.CQRS;
using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Tool;
using Infrastructure.ObjectPooling;
using AlirezaTarahomi.FightingGame.Tool.Event;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    [CreateAssetMenu(menuName = "Attacks/ThrowingObjectBehavior")]
    public class ThrowingObjectBehavior : ScriptableObject
    {
        [SerializeField] private int _maxObjectNumber = 9;
        [SerializeField] private float _throwingForce = 1.5f;
        [SerializeField] private PooledObjectStats _throwableObjectPoolStats = default;

        [HideInInspector] public int counter;

        private IMessageBus _messageBus;
        private PoolingSystem _poolSystem;
        private CharacterBehaviorContext _context;

        [Inject]
        public void Construct(IMessageBus messageBus, PoolingSystem poolSystem)
        {
            _messageBus = messageBus;
            _poolSystem = poolSystem;

            counter = _maxObjectNumber;
        }

        public void Inject(CharacterBehaviorContext context)
        {
            _context = context;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Hitbox))
            {
                if (_context.OwnershipService.Contains(collision.gameObject))
                {
                    IThrowableObject throwableObject = collision.GetComponent<IThrowableObject>();
                    if (throwableObject.CanPick)
                    {
                        PickUpObject(collision.gameObject);
                    }
                }
            }
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
            _messageBus.RaiseEvent(new OnAttackToggled(_context.CharacterId, _context.PlayerId, false));
        }

        public void PickUpObject(GameObject obj)
        {
            counter++;
            _messageBus.RaiseEvent(new OnThrowableObjectPickedUp(obj));
            _context.OwnershipService.Remove(obj);
        }
    }
}