using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character
{
    // TODO: Remove Mono
    public class GroundCheck : MonoBehaviour
    {
        private CharacterController _controller;
        private IMessageBus _messageBus;
        private CharacterStats _stats;
        private int _playerId;
        private Collider2D _collider;

        [Inject]
        public void Construct(IMessageBus messageBus, [Inject(Id = "stats")] CharacterStats stats,
            [Inject(Id = "id")] int playerId)
        {
            _messageBus = messageBus;
            _controller = GetComponentInParent<CharacterController>();
            _stats = stats;
            _playerId = playerId;
            _collider = GetComponent<Collider2D>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            _controller.isGrounded = true;
            _messageBus.RaiseEvent(new OnGrounded(_playerId, _stats));
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