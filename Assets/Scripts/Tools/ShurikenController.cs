using UnityEngine;
using Infrastructure.ObjectPool;
using Zenject;
using AlirezaTarahomi.FightingGame.Tool.Event;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Scripts.CQRS.Validators;

namespace AlirezaTarahomi.FightingGame.Tool
{
    public class ShurikenController : MonoBehaviour, IThrowableObject, IPooledObject, IGameObjectProperty,
        IEventHandler<OnThrowableObjectPicked>
    {
        [SerializeField] private float _rotatingSpeed = 1;
        [SerializeField] private Sprite _mainShurikenSprite;
        [SerializeField] private Sprite _stuckShurikenSprite = default;

        [HideInInspector] public bool isIllusion;

        public GameObject GameObject { get { return gameObject; } }
        public bool IsDeadly { get; private set; } = true;
        public PooledObjectStats PooledObjectStats { get; set; }
        public bool IsStuck { get; private set; }

        private IMessageBus _messageBus;
        private PoolSystem _poolSystem;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        private bool _canRotate;
        private static MessageRouteRule _rule = MessageRouteRule.Create<OnThrowableObjectPicked,
            ShurikenController>(string.Empty, false, new EventGameObjectValidator<OnThrowableObjectPicked>());

        [Inject]
        public void Construct(IMessageBus messageBus, PoolSystem poolSystem)
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
            _messageBus.Subscribe<ShurikenController, OnThrowableObjectPicked>(this, new MessageHandlerActionExecutor<OnThrowableObjectPicked>(Handle));
        }

        private void UnsubscribeEvents()
        {
            _messageBus.Unsubscribe<ShurikenController, OnThrowableObjectPicked>(this);
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
            if (collision.CompareTag("Destroyer"))
            {
                _canRotate = false;
                Destroy(gameObject);
            }

            if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
            {
                if (isIllusion)
                {
                    _canRotate = false;
                    Destroy(gameObject);
                }
                else
                {
                    _canRotate = false;
                    IsStuck = true;
                    IsDeadly = false;
                    _spriteRenderer.sprite = _stuckShurikenSprite;
                    _rigidbody.velocity = Vector2.zero;
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

        public void ReInitialize()
        {
            IsStuck = false;
            IsDeadly = true;
            _spriteRenderer.sprite = _mainShurikenSprite;
        }

        public void Handle(OnThrowableObjectPicked @event)
        {
            _poolSystem.Despawn(PooledObjectStats, transform);
        }
    }
}