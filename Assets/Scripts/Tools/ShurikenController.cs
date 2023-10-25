using UnityEngine;
using Infrastructure.ObjectPooling;
using VContainer;
using AlirezaTarahomi.FightingGame.General.Variable;

namespace AlirezaTarahomi.FightingGame.Tool
{
    public class ShurikenController : MonoBehaviour, IThrowableObject, IPooledObject
    {
        [SerializeField] private IntVariable _objectCounter;
        [SerializeField] private float _rotatingSpeed = 1;
        [SerializeField] private Sprite _mainSprite;
        [SerializeField] private Sprite _illusionSprite;
        [SerializeField] private Sprite _stuckSprite;

        public bool IsDeadly { get; private set; } = true;
        public PooledObjectStats PooledObjectStats { get; set; }
        public bool CanPick { get; private set; }
        public IntVariable ObjectCounter
        {
            get => _objectCounter;
            private set => _objectCounter = value;
        }

        private PoolingSystem _poolSystem;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        private bool _canRotate;
        private bool _isIllusion;

        [Inject]
        public void Construct(PoolingSystem poolSystem)
        {
            _poolSystem = poolSystem;
        }

        // Use this for initialization
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (!_canRotate)
                return;

            Rotate();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Destroyer))
            {
                _poolSystem.Despawn(PooledObjectStats, transform);
            }

            if (collision.CompareTag(Tags.Ground) || collision.CompareTag(Tags.Wall))
            {
                if (_isIllusion)
                {
                    _isIllusion = false;
                    _poolSystem.Despawn(PooledObjectStats, transform);
                }
                else
                {
                    MakeItStuck();
                }
            }
        }

        public void Throw(float force, Vector2 direction)
        {
            _canRotate = true;
            _rigidbody.AddForce(force * 1000 * direction);
        }

        void Rotate()
        {
            transform.Rotate(Vector3.forward, _rotatingSpeed * 1000 * Time.deltaTime);
        }

        public void MakeItStuck()
        {
            _canRotate = false;
            CanPick = true;
            IsDeadly = false;
            _spriteRenderer.sprite = _stuckSprite;
            _rigidbody.velocity = Vector2.zero;
        }

        public void PrepareForPowerup()
        {
            _isIllusion = true;
            _spriteRenderer.sprite = _illusionSprite;
        }

        public void ResetValues()
        {
            _canRotate = false;
            CanPick = false;
            IsDeadly = true;
            _spriteRenderer.sprite = _mainSprite;
        }
    }
}