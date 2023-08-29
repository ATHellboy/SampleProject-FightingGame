using AlirezaTarahomi.FightingGame.Character.Event;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class GroundCheck : MonoBehaviour
    {
        public OnGrounded OnGrounded { get; private set; } = new();

        private Collider2D _collider;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            OnGrounded?.Invoke(true);
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            OnGrounded?.Invoke(false);
        }

        public void ToggleCollider(bool active)
        {
            _collider.enabled = active;
        }
    }
}