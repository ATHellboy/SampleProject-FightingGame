using AlirezaTarahomi.FightingGame.Character.Event;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class GroundCheck : MonoBehaviour
    {
        public OnGrounded OnGrounded { get; private set; } = new();
        public bool OnGround { get; private set; }

        private Collider2D _collider;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            OnGround = true;
            OnGrounded?.Invoke();
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            OnGround = false;
            OnGrounded?.Invoke();
        }

        public void ToggleCollider(bool active)
        {
            _collider.enabled = active;
        }
    }
}