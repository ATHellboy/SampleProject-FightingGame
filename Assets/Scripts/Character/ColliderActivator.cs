using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class ColliderActivator : MonoBehaviour
    {
        private int _id;
        private CharacterStats _stats;
        private IMessageBus _messageBus;
        private Collider2D _collider;

        [Inject]
        public void Construct(IMessageBus messageBus, [Inject(Id = "stats")] CharacterStats stats,
            [Inject(Id = "id")] int id)
        {
            _messageBus = messageBus;
            _stats = stats;
            _id = id;
            _collider = GetComponent<Collider2D>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            ToggleCollider(false);
            _messageBus.RaiseEvent(new OnCollideObject(_id, _stats));
        }

        public void ToggleCollider(bool active)
        {
            _collider.enabled = active;
        }
    }
}