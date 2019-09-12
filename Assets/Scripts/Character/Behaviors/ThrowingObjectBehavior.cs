using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using Assets.Infrastructure.Scripts.CQRS;
using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Service;
using AlirezaTarahomi.FightingGame.Tool;
using Infrastructure.ObjectPool;
using AlirezaTarahomi.FightingGame.Tool.Event;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    [CreateAssetMenu(menuName = "Attacks/ThrowingObjectBehavior")]
    public class ThrowingObjectBehavior : ScriptableObject
    {
        public int maxObjectNumber = 9;
        public float throwingForce = 1.5f;

        public int MaxObjectNumber { get; private set; }

        [HideInInspector] public int counter;

        [SerializeField] private PooledObjectStats _throwableObjectPoolStats = default;

        private IMessageBus _messageBus;
        private PoolSystem _poolSystem;
        private CharacterBehaviorContext _context;

        [Inject]
        public void Construct(IMessageBus messageBus, PoolSystem poolSystem)
        {
            _messageBus = messageBus;
            _poolSystem = poolSystem;
            counter = maxObjectNumber;
        }

        public void Inject(CharacterBehaviorContext context)
        {
            _context = context;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                if (_context.OwnershipService.Contains(collision.gameObject))
                {
                    ShurikenController throwableObjectController = collision.GetComponent<ShurikenController>();
                    if (throwableObjectController.IsStuck)
                    {
                        PickObject(collision.gameObject);
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
                ShurikenController throwableObjectController = obj.GetComponent<ShurikenController>();
                if (_context.isPowerupActive)
                {
                    throwableObjectController.isIllusion = true;
                }
                throwableObjectController.Throw(throwingForce, direction.normalized);
            }
            _messageBus.RaiseEvent(new OnAttackToggled(_context.PlayerId, _context.Stats, false));
        }

        public void PickObject(GameObject obj)
        {
            counter++;
            _context.OwnershipService.Remove(obj);
            _messageBus.RaiseEvent(new OnThrowableObjectPicked(obj));
        }
    }
}