using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class MovementColliderActivator : MonoBehaviour
    {
        [SerializeField] private GameObject _movementCollider = default;

        private string _characterId;
        private IMessageBus _messageBus;
        private Collider2D _colliderActivator;
        private Collider2D[] _movementColliders;

        [Inject]
        public void Construct(IMessageBus messageBus, [Inject(Id = "id")] string characterId)
        {
            _messageBus = messageBus;
            _characterId = characterId;
        }

        void Awake()
        {
            _colliderActivator = GetComponent<Collider2D>();
            _movementColliders = _movementCollider.GetComponents<Collider2D>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            ToggleColliderActivator(false);
            ToggleMovementColliders(true);
        }

        public void ToggleMovementColliders(bool active)
        {
            for (int i = 0; i < _movementColliders.Length; i++)
            {
                _movementColliders[i].enabled = active;
            }
        }

        public void ToggleColliderActivator(bool active)
        {
            _colliderActivator.enabled = active;
        }
    }
}