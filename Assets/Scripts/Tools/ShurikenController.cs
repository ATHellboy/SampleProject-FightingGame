using UnityEngine;
using Infrastructure.ObjectPooling;
using Zenject;
using AlirezaTarahomi.FightingGame.Tool.Event;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Scripts.CQRS.Validators;

namespace AlirezaTarahomi.FightingGame.Tool
{
    public class ShurikenController : MonoBehaviour, IThrowableObject, IPooledObject, IGameObjectProperty,
        IEventHandler<OnThrowableObjectPickedUp>
    {
        [SerializeField] private float _rotatingSpeed = 1;
        [SerializeField] private Sprite _mainSprite = default;
        [SerializeField] private Sprite _illusionSprite = default;
        [SerializeField] private Sprite _stuckSprite = default;

        public GameObject GameObject { get { return gameObject; } }
        public bool IsDeadly { get; private set; } = true;
        public PooledObjectStats PooledObjectStats { get; set; }
        public bool CanPick { get; private set; }

        private static MessageRouteRule _rule = MessageRouteRule.Create<OnThrowableObjectPickedUp,
            ShurikenController>(string.Empty, false, new EventGameObjectValidator<OnThrowableObjectPickedUp>());

        private IMessageBus _messageBus;
        private PoolingSystem _poolSystem;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        private bool _canRotate;
        private bool _isIllusion;

        [Inject]
        public void Construct(IMessageBus messageBus, PoolingSystem poolSystem)
        {
            _messageBus = messageBus;
            _poolSystem = poolSystem;
        }

        void OnEnable()
        {
            InitializeEvents();
        }

        void OnDisable()
        {
            //UnsubscribeEvents();
        }

        private void InitializeEvents()
        {
            _messageBus.AddRule(_rule);
            _messageBus.Subscribe<ShurikenController, OnThrowableObjectPickedUp>(this, new MessageHandlerActionExecutor<OnThrowableObjectPickedUp>(Handle));
        }

        private void UnsubscribeEvents()
        {
            _messageBus.Unsubscribe<ShurikenController, OnThrowableObjectPickedUp>(this);
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

        public void ReInitialize()
        {
            _canRotate = false;
            CanPick = false;
            IsDeadly = true;
            _spriteRenderer.sprite = _mainSprite;
        }

        /// <summary>
        /// Handles the event when the throwable object is picked up
        /// </summary>
        public void Handle(OnThrowableObjectPickedUp @event)
        {
            _poolSystem.Despawn(PooledObjectStats, transform);
        }
    }
}