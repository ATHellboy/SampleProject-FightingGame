using UnityEngine;
using Zenject;
using AlirezaTarahomi.FightingGame.Service;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Tool;
using AlirezaTarahomi.FightingGame.General.Variable;
using Infrastructure.ObjectPooling;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterHurtBoxHandler : MonoBehaviour
    {
        private IOwnershipService _ownershipService;
        private PoolingSystem _poolSystem;
        private CharacterMainStateMachineContext _mainStateMachineContext;
        private CharacterLocomotionHandler _locomotionHandler;

        [Inject]
        public void Construct(IOwnershipService ownershipService, PoolingSystem poolSystem,
            CharacterMainStateMachineContext mainStateMachineContext, CharacterLocomotionHandler locomotionHandler)
        {
            _poolSystem = poolSystem;
            _ownershipService = ownershipService;
            _mainStateMachineContext = mainStateMachineContext;
            _locomotionHandler = locomotionHandler;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.CharactersTrigger))
            {
                _locomotionHandler.Stop();
            }

            if (collision.CompareTag(Tags.Hitbox))
            {
                if (GetInjured(collision))
                {
                    return;
                }
            }

            PickUpObject(collision);
        }

        private bool GetInjured(Collider2D collision)
        {
            ITool tool = collision.gameObject.GetComponent<ITool>();
            if (!_ownershipService.Contains(collision.gameObject) && tool.IsDeadly)
            {
                _mainStateMachineContext.isInjured = true;
                return true;
            }
            return false;
        }

        public void PickUpObject(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Hitbox) && _ownershipService.Contains(collision.gameObject))
            {
                IThrowableObject throwableObject = collision.GetComponent<IThrowableObject>();
                IPooledObject pooledObject = collision.GetComponent<IPooledObject>();
                if (throwableObject.CanPick)
                {
                    throwableObject.ObjectCounter.value++;
                    _poolSystem.Despawn(pooledObject.PooledObjectStats, collision.transform);
                    _ownershipService.Remove(collision.gameObject);
                }
            }
        }
    }
}