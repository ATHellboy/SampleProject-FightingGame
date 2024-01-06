using AlirezaTarahomi.FightingGame.Character.Event;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class SurfaceCheck : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider2D _movementCollider;
        [SerializeField] private LayerMask _slopeRaycastLayerMask;
        [SerializeField] private float _slopeRaycastDistance;
        [SerializeField] private float _heightOffset = 1.0f;
        [SerializeField] private bool _debug = false;

        [HideInInspector] public bool onGround;
        [HideInInspector] public int face;

        public OnGrounded OnGrounded { get; private set; } = new();

        public bool OnSlope { get; private set; }
        public Vector2 SlopeNormalPerp { get; private set; }

        private CircleCollider2D _collider;

        void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        void FixedUpdate()
        {
            SlopeCheck();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            onGround = true;
            OnGrounded?.Invoke();
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            onGround = false;
        }

        private void SlopeCheck()
        {
            var raycastOrigin = new Vector2(transform.position.x + _movementCollider.offset.x, _movementCollider.bounds.min.y + _heightOffset);
            RaycastHit2D frontHit = Physics2D.Raycast(raycastOrigin + Vector2.right * face * _movementCollider.bounds.size.x / 4,
                Vector2.down, _slopeRaycastDistance + _heightOffset, _slopeRaycastLayerMask);
            RaycastHit2D centerHit = Physics2D.Raycast(raycastOrigin, Vector2.down, _slopeRaycastDistance + _heightOffset, _slopeRaycastLayerMask);
            RaycastHit2D backHit = Physics2D.Raycast(raycastOrigin - Vector2.right * face * _movementCollider.bounds.size.x / 4,
                Vector2.down, _slopeRaycastDistance + _heightOffset, _slopeRaycastLayerMask);

            if (!frontHit && !centerHit && !backHit)
            {
                OnSlope = false;
                return;
            }

            Vector2 frontNormalPerp = Vector2.Perpendicular(frontHit.normal);
            Vector2 centerNormalPerp = Vector2.Perpendicular(centerHit.normal);
            Vector2 backNormalPerp = Vector2.Perpendicular(backHit.normal);
            SlopeNormalPerp = frontNormalPerp;
            // No jump or stickiness (No downward) at the start of going downward 
            SlopeNormalPerp = (Vector2.Dot(centerHit.normal, Vector2.right) == 0 && frontHit.point.y < centerHit.point.y) ?
                centerNormalPerp : SlopeNormalPerp;
            // No fall (No straightforward) at the end of going downward
            SlopeNormalPerp = (Vector2.Dot(frontHit.normal, Vector2.right) == 0 && frontHit.point.y < backHit.point.y) ?
                backNormalPerp : SlopeNormalPerp;
            OnSlope = onGround && !(Vector2.Dot(frontHit.normal, Vector2.right) == 0 && Vector2.Dot(backHit.normal, Vector2.right) == 0);

            if (_debug)
            {
                Debug.DrawRay(frontHit.point, frontHit.normal, Color.red);
                Debug.DrawRay(frontHit.point, frontNormalPerp, Color.white);
                Debug.DrawRay(centerHit.point, centerHit.normal, Color.white);
                Debug.DrawRay(centerHit.point, centerNormalPerp, Color.white);
                Debug.DrawRay(backHit.point, backHit.normal, Color.blue);
                Debug.DrawRay(backHit.point, backNormalPerp, Color.black);
            }
        }

        public void ToggleCollider(bool active)
        {
            _collider.enabled = active;
        }
    }
}