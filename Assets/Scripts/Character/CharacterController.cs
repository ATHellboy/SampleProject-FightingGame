using AlirezaTarahomi.FightingGame.CameraSystem;
using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.InputSystem;
using AlirezaTarahomi.FightingGame.Player;
using AlirezaTarahomi.FightingGame.Player.Event;
using AlirezaTarahomi.FightingGame.Player.Validator;
using AlirezaTarahomi.FightingGame.Service;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Scripts.CQRS.Validators;
using Infrastructure.Factory;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterController : MonoBehaviour, IEventHandler<OnPushForwardEnded>, IEventHandler<OnPowerupToggled>,
    IEventHandler<OnControlToggled>, IEventHandler<OnCharacterArrivalToggled>, IEventHandler<OnOtherDisabled>,
    IEventHandler<OnGrounded>, IEventHandler<OnCollideObject>, IEventHandler<OnCharacterDied>,
    IEventHandler<OnAttackToggled>, IEventHandler<OnSpeedChanged>, IPlayerIdProperty, IScriptableObjectProperty
    {
        [SerializeField] private GameObject _movementCollider = default;
        [SerializeField] private GameObject _hitbox = default;

        [HideInInspector] public bool isGrounded;

        public CharacterStats Stats { get; private set; }
        public int PlayerId { get; private set; }

        private InputManager _inputManager;
        private IMessageBus _messageBus;
        private IOwnershipService _ownershipService;
        private IResourceFactory _resourceFactory;
        private StateMachine _stateMachine;
        private CharacterMainStateMachineContext _mainStateMachineContext;
        private CharacterSecondaryMovementStateMachineContext _secondaryMovementStateMachineContext;
        private CharacterCombatStateMachineContext _combatStateMachineContext;
        private CharacterBehaviorContext _behaviorContext;
        private CharacterPowerupContext _powerupContext;
        private CharacterLocomotionHandler _locomotionHandler;
        private CharacterHurtBoxHandler _hurtBoxHandler;
        private Collider2D _hitboxCollider;
        private Collider2D[] _movementColliders;
        private Camera _mainCamera;
        private TargetGroupController _targetGroupController;
        private PlayerController _playerController;
        private MainCameraController _mainCameraController;
        private GroundCheck _groundCheck;
        private ColliderActivator _colliderActivator;
        private ThrowingObjectBehavior _throwingObjectBehavior;

        private static MessageRouteRule[] rules = new MessageRouteRule[]
        {
        MessageRouteRule.Create<OnPushForwardEnded, CharacterController>(string.Empty, false,
            new AndValidator<OnPushForwardEnded>(new EventPlayerIdValidator<OnPushForwardEnded>(),
            new EventScriptableObjectValidator<OnPushForwardEnded>())),
        MessageRouteRule.Create<OnPowerupToggled, CharacterController>(string.Empty, false,
            new AndValidator<OnPowerupToggled>(new EventPlayerIdValidator<OnPowerupToggled>(),
            new EventScriptableObjectValidator<OnPowerupToggled>())),
        MessageRouteRule.Create<OnControlToggled, CharacterController>(string.Empty, false,
            new AndValidator<OnControlToggled>(new EventPlayerIdValidator<OnControlToggled>(),
            new EventScriptableObjectValidator<OnControlToggled>())),
        MessageRouteRule.Create<OnCharacterArrivalToggled, CharacterController>(string.Empty, false,
            new AndValidator<OnCharacterArrivalToggled>(new EventPlayerIdValidator<OnCharacterArrivalToggled>(),
            new EventScriptableObjectValidator<OnCharacterArrivalToggled>())),
        MessageRouteRule.Create<OnOtherDisabled, CharacterController>(string.Empty, false,
            new AndValidator<OnOtherDisabled>(new EventPlayerIdValidator<OnOtherDisabled>(),
            new NotValidator<OnOtherDisabled>((new EventScriptableObjectValidator<OnOtherDisabled>())))),
        MessageRouteRule.Create<OnGrounded, CharacterController>(string.Empty, false,
            new AndValidator<OnGrounded>(new EventPlayerIdValidator<OnGrounded>(),
            new EventScriptableObjectValidator<OnGrounded>())),
        MessageRouteRule.Create<OnCollideObject, CharacterController>(string.Empty, false,
            new AndValidator<OnCollideObject>(new EventPlayerIdValidator<OnCollideObject>(),
            new EventScriptableObjectValidator<OnCollideObject>())),
        MessageRouteRule.Create<OnCharacterDied, CharacterController>(string.Empty, false,
            new AndValidator<OnCharacterDied>(new EventPlayerIdValidator<OnCharacterDied>(),
            new EventScriptableObjectValidator<OnCharacterDied>())),
        MessageRouteRule.Create<OnAttackToggled, CharacterController>(string.Empty, false,
            new AndValidator<OnAttackToggled>(new EventPlayerIdValidator<OnAttackToggled>(),
            new EventScriptableObjectValidator<OnAttackToggled>())),
        MessageRouteRule.Create<OnSpeedChanged, CharacterController>(string.Empty, false,
            new AndValidator<OnSpeedChanged>(new EventPlayerIdValidator<OnSpeedChanged>(),
            new EventScriptableObjectValidator<OnSpeedChanged>()))
    };

        [Inject]
        public void Construct(StateMachine stateMachine, InputManager inputManager, IMessageBus messageBus,
            [Inject(Id = "id")] int playerId, [Inject(Id = "stats")] CharacterStats stats,
            CharacterMainStateMachineContext mainStateMachineContext,
            CharacterSecondaryMovementStateMachineContext secondaryMovementStateMachineContext,
            CharacterCombatStateMachineContext combatStateMachineContext,
            CharacterBehaviorContext behaviorContext, CharacterPowerupContext powerupContext,
            CharacterLocomotionHandler locomotionHandler, CharacterHurtBoxHandler hurtBoxHandler,
            IOwnershipService ownershipService, Camera mainCamera,
            TargetGroupController targetGroupController, PlayerController playerController,
            MainCameraController mainCameraController, GroundCheck groundCheck, ColliderActivator colliderActivator,
            IResourceFactory resourceFactory)
        {
            _stateMachine = stateMachine;
            _inputManager = inputManager;
            Stats = stats;
            PlayerId = playerId;
            _messageBus = messageBus;
            _locomotionHandler = locomotionHandler;
            _hurtBoxHandler = hurtBoxHandler;
            _ownershipService = ownershipService;
            _mainCamera = mainCamera;
            _targetGroupController = targetGroupController;
            _playerController = playerController;
            _mainCameraController = mainCameraController;
            _groundCheck = groundCheck;
            _colliderActivator = colliderActivator;
            _resourceFactory = resourceFactory;

            _mainStateMachineContext = mainStateMachineContext;
            _secondaryMovementStateMachineContext = secondaryMovementStateMachineContext;
            _combatStateMachineContext = combatStateMachineContext;
            _behaviorContext = behaviorContext;
            _powerupContext = powerupContext;
        }

        void OnEnable()
        {
            InitializeEvents();
        }

        void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void InitializeEvents()
        {
            _messageBus.AddRule(rules[0]);
            _messageBus.Subscribe<CharacterController, OnPushForwardEnded>(this, new MessageHandlerActionExecutor<OnPushForwardEnded>(Handle));

            _messageBus.AddRule(rules[1]);
            _messageBus.Subscribe<CharacterController, OnPowerupToggled>(this, new MessageHandlerActionExecutor<OnPowerupToggled>(Handle));

            _messageBus.AddRule(rules[2]);
            _messageBus.Subscribe<CharacterController, OnControlToggled>(this, new MessageHandlerActionExecutor<OnControlToggled>(Handle));

            _messageBus.AddRule(rules[3]);
            _messageBus.Subscribe<CharacterController, OnCharacterArrivalToggled>(this, new MessageHandlerActionExecutor<OnCharacterArrivalToggled>(Handle));

            _messageBus.AddRule(rules[4]);
            _messageBus.Subscribe<CharacterController, OnOtherDisabled>(this, new MessageHandlerActionExecutor<OnOtherDisabled>(Handle));

            _messageBus.AddRule(rules[5]);
            _messageBus.Subscribe<CharacterController, OnGrounded>(this, new MessageHandlerActionExecutor<OnGrounded>(Handle));

            _messageBus.AddRule(rules[6]);
            _messageBus.Subscribe<CharacterController, OnCollideObject>(this, new MessageHandlerActionExecutor<OnCollideObject>(Handle));

            _messageBus.AddRule(rules[7]);
            _messageBus.Subscribe<CharacterController, OnCharacterDied>(this, new MessageHandlerActionExecutor<OnCharacterDied>(Handle));

            _messageBus.AddRule(rules[8]);
            _messageBus.Subscribe<CharacterController, OnAttackToggled>(this, new MessageHandlerActionExecutor<OnAttackToggled>(Handle));

            _messageBus.AddRule(rules[9]);
            _messageBus.Subscribe<CharacterController, OnSpeedChanged>(this, new MessageHandlerActionExecutor<OnSpeedChanged>(Handle));
        }

        private void UnsubscribeEvents()
        {
            _messageBus.Unsubscribe<CharacterController, OnPushForwardEnded>(this);
            _messageBus.Unsubscribe<CharacterController, OnPowerupToggled>(this);
            _messageBus.Unsubscribe<CharacterController, OnControlToggled>(this);
            _messageBus.Unsubscribe<CharacterController, OnCharacterArrivalToggled>(this);
            _messageBus.Unsubscribe<CharacterController, OnOtherDisabled>(this);
            _messageBus.Unsubscribe<CharacterController, OnGrounded>(this);
            _messageBus.Unsubscribe<CharacterController, OnCollideObject>(this);
            _messageBus.Unsubscribe<CharacterController, OnCharacterDied>(this);
            _messageBus.Unsubscribe<CharacterController, OnAttackToggled>(this);
            _messageBus.Unsubscribe<CharacterController, OnSpeedChanged>(this);
        }

        void Awake()
        {
            _movementColliders = _movementCollider.GetComponents<Collider2D>();

            GetHitBox();

            CreateThrowingBehaviorInstance();
            _hurtBoxHandler.Inject(_throwingObjectBehavior);

            InitPowerup(Stats.behaviors.powerup.value);
            InitAttackBehaviors();
        }

        private void GetHitBox()
        {
            if (_hitbox != null)
            {
                _ownershipService.Add(_hitbox);
                _hitboxCollider = _hitbox.GetComponent<Collider2D>();
                _hitboxCollider.enabled = false;
                _behaviorContext.hitboxCollider = _hitboxCollider;
            }
        }

        private void CreateThrowingBehaviorInstance()
        {
            ScriptableObject scriptableObject = Stats.behaviors.throwingObjectBehavior.value;
            if (scriptableObject != null)
            {
                _throwingObjectBehavior = _resourceFactory.Instantiate(scriptableObject) as ThrowingObjectBehavior;
            }
        }

        private void InitThrowingBehaviour(ScriptableObject clone)
        {
            IThrowingBehavior castedAsThrowingBehavior = clone as IThrowingBehavior;
            if (castedAsThrowingBehavior != null)
            {
                _throwingObjectBehavior.Inject(_behaviorContext);
                castedAsThrowingBehavior.AssignThrowingObjectBehavior(_throwingObjectBehavior);
            }
        }

        private void InitAttackBehavior(ScriptableObject attackBehavior)
        {
            if (attackBehavior != null)
            {
                ScriptableObject clone = _resourceFactory.Instantiate(attackBehavior) as ScriptableObject;
                InitThrowingBehaviour(clone);
                IAttackBehavior clonedAttackBehavior = clone as IAttackBehavior;
                clonedAttackBehavior.Inject(_behaviorContext);
                _combatStateMachineContext.AddAttackBehavior(clonedAttackBehavior);
            }
        }

        private void InitPowerup(ScriptableObject powerup)
        {
            ScriptableObject clone = _resourceFactory.Instantiate(powerup) as ScriptableObject;
            InitThrowingBehaviour(clone);
            _combatStateMachineContext.powerup = clone as IPowerup;
            _combatStateMachineContext.powerup.Inject(_powerupContext);
        }

        private void InitAttackBehaviors()
        {
            InitAttackBehavior(_combatStateMachineContext.powerup.PowerupAttackBehavior);
            InitAttackBehavior(Stats.behaviors.normalAttack.value);
            InitAttackBehavior(Stats.behaviors.complextAttack.value);
        }

        public ScriptableObject GetScriptableObject()
        {
            return Stats;
        }

        void Start()
        {
            _stateMachine.Start(_mainStateMachineContext);
            _stateMachine.Start(_secondaryMovementStateMachineContext);
            _stateMachine.Start(_combatStateMachineContext);
        }

        void Update()
        {
            UpdateBetweenContexts();

            GetMoveAxes();

            _stateMachine.Update(_mainStateMachineContext);
            _stateMachine.Update(_secondaryMovementStateMachineContext);
            _stateMachine.Update(_combatStateMachineContext);
        }

        void FixedUpdate()
        {
            _stateMachine.FixedUpdate(_mainStateMachineContext);
            _stateMachine.FixedUpdate(_secondaryMovementStateMachineContext);
            _stateMachine.FixedUpdate(_combatStateMachineContext);
        }

        private void UpdateBetweenContexts()
        {
            _secondaryMovementStateMachineContext.isGrounded = isGrounded;
            _combatStateMachineContext.isGrounded = isGrounded;
            _behaviorContext.isGrounded = isGrounded;

            _behaviorContext.jumpCounter = _secondaryMovementStateMachineContext.jumpCounter;
        }

        private void GetMoveAxes()
        {
            _mainStateMachineContext.moveAxes.x = _inputManager.GetAxis("MoveHorizontal_P" + PlayerId);
            _mainStateMachineContext.moveAxes.y = _inputManager.GetAxis("MoveVertical_P" + PlayerId);
        }

        public void ToggleControl(bool active)
        {
            _mainStateMachineContext.ToggleStateMachineUpdate(active);
            _secondaryMovementStateMachineContext.ToggleStateMachineUpdate(active);
            _combatStateMachineContext.ToggleStateMachineUpdate(active);
        }

        // TODO: Move to seprate script
        private void ToggleMovementColliders(bool active)
        {
            for (int i = 0; i < _movementColliders.Length; i++)
            {
                _movementColliders[i].enabled = active;
            }
        }

        private bool CompareStates(IState currentState, IState targetState)
        {
            return currentState == targetState ? true : false;
        }

        public void Handle(OnAttackToggled @event)
        {
            if (!@event.Enter)
            {
                _combatStateMachineContext.goToNextState = true;
            }
        }

        public void Handle(OnPushForwardEnded @event)
        {
            _locomotionHandler.Stop();
            _locomotionHandler.SetAirGravityScale();
        }

        public void Handle(OnPowerupToggled @event)
        {
            if (@event.Active)
            {
                _behaviorContext.isPowerupActive = true;
                _combatStateMachineContext.isPowerupActive = true;
            }
            else
            {
                _behaviorContext.isPowerupActive = false;
                _combatStateMachineContext.isPowerupActive = false;
            }
        }

        public void Handle(OnControlToggled @event)
        {
            if (@event.Enable)
            {
                ToggleControl(true);
            }
            else
            {
                ToggleControl(false);
            }
        }

        public void Handle(OnOtherDisabled @event)
        {
            _locomotionHandler.SetNoGravityScale();
            ToggleMovementColliders(false);
        }

        public void Handle(OnCharacterArrivalToggled @event)
        {
            if (@event.Enter)
            {
                _locomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Continuous);
                _groundCheck.ToggleCollider(true);
                _colliderActivator.ToggleCollider(true);
                Vector2 cameraSize = _mainCameraController.CameraSize;
                _locomotionHandler.Teleport(new Vector2(_mainCamera.transform.position.x + (int)_playerController.side * cameraSize.x / 2, _mainCamera.transform.position.y));
                _locomotionHandler.ThrowInside(Stats.miscValues.enteringForce, _playerController.side);
                _targetGroupController.SwitchTarget(PlayerId - 1,
                    new Cinemachine.CinemachineTargetGroup.Target { target = transform, radius = Stats.cameraValues.cameraRadius, weight = Stats.cameraValues.cameraWeight });
            }
            else
            {
                _messageBus.RaiseEvent(new OnStateMachineReset(_mainStateMachineContext));
                _messageBus.RaiseEvent(new OnStateMachineReset(_secondaryMovementStateMachineContext));
                _messageBus.RaiseEvent(new OnStateMachineReset(_combatStateMachineContext));

                _groundCheck.ToggleCollider(false);
                ToggleMovementColliders(false);
                ToggleControl(false);
                _locomotionHandler.GoOutside(Stats.miscValues.exitVelocity, _playerController.side);
            }
        }

        public void Handle(OnGrounded @event)
        {
            _locomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Discrete);
            _locomotionHandler.SetGroundGravityScale();
            _locomotionHandler.Stop();
            ToggleControl(true);
        }

        public void Handle(OnCharacterDied @event)
        {
            _locomotionHandler.Stop();
            ToggleControl(false);
        }

        public void Handle(OnCollideObject @event)
        {
            ToggleMovementColliders(true);
        }

        // TODO: Remove ifs
        public void Handle(OnSpeedChanged @event)
        {
            if (CompareStates(_mainStateMachineContext.CurrentState, _mainStateMachineContext.RelatedStates.idle))
            {
                _locomotionHandler.ChangeMoveSpeed(0);
            }
            else if (CompareStates(_mainStateMachineContext.CurrentState, _mainStateMachineContext.RelatedStates.walk))
            {
                _locomotionHandler.ChangeMoveSpeed(Stats.groundMovementValues.walkSpeed);
            }
        }
    }
}