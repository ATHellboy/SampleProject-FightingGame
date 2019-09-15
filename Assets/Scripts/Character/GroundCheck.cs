using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class GroundCheck : MonoBehaviour
    {
        private IMessageBus _messageBus;
        private string _characterId;
        private CharacterController _controller;
        private Collider2D _collider;

        [Inject]
        public void Construct(IMessageBus messageBus, [Inject(Id = "stats")] CharacterStats stats,
            [Inject(Id = "id")] string characterId)
        {
            _messageBus = messageBus;
            _characterId = characterId;

            _controller = GetComponentInParent<CharacterController>();
            _collider = GetComponent<Collider2D>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            _controller.isGrounded = true;
            _messageBus.RaiseEvent(new OnGrounded(_characterId));
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            _controller.isGrounded = false;
        }

        public void ToggleCollider(bool active)
        {
            _collider.enabled = active;
        }
    }
}