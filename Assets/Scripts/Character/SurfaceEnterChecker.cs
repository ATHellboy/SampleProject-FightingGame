using AlirezaTarahomi.FightingGame.Character.Event;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class SurfaceEnterChecker : MonoBehaviour
    {
        [SerializeField] private GameObject _movementCollider = default;

        public OnEntered OnEntered { get; private set; } = new();

        private Collider2D _collider;
        private Collider2D[] _movementColliders;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _movementColliders = _movementCollider.GetComponents<Collider2D>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            ToggleCollider(false);
            ToggleMovementColliders(true);
            OnEntered.Invoke();
        }

        public void ToggleMovementColliders(bool active)
        {
            for (int i = 0; i < _movementColliders.Length; i++)
            {
                _movementColliders[i].enabled = active;
            }
        }

        public void ToggleCollider(bool active)
        {
            _collider.enabled = active;
        }
    }
}