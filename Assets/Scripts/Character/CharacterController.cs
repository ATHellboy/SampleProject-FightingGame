using AlirezaTarahomi.FightingGame.CameraSystem;
using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.Character.State.Main;
using AlirezaTarahomi.FightingGame.InputSystem;
using AlirezaTarahomi.FightingGame.Player;
using AlirezaTarahomi.FightingGame.Player.Event;
using AlirezaTarahomi.FightingGame.Player.Validator;
using AlirezaTarahomi.FightingGame.Service;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Scripts.CQRS.Validators;
using Infrastructure.Factory;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterController : MonoBehaviour, IEntityIdProperty, IEventHandler<OnPowerupToggled>,
    IEventHandler<OnControlToggled>, IEventHandler<OnCharacterArrivalToggled>, IEventHandler<OnOtherDisabled>,
    IEventHandler<OnGrounded>, IEventHandler<OnCharacterDied>, IEventHandler<OnCharacterFlownToggled>,
    IEventHandler<OnAttackToggled>, IEventHandler<OnSecondaryMovementNoneStateEntered>, IPlayerIdProperty, IScriptableObjectProperty
    {
        [SerializeField] private GameObject _hitbox = default;

        [HideInInspector] public bool isGrounded;
        [HideInInspector] public bool isJustEntered;

        public string EntityId { get; private set; }
        public CharacterStats Stats { get; private set; }
        public int PlayerId { get; private set; }

        private static MessageRouteRule[] _rules = new MessageRouteRule[]
        {
        MessageRouteRule.Create<OnPowerupToggled, CharacterController>(string.Empty, false,
            new EventEntityIdValidator<OnPowerupToggled>()),
        MessageRouteRule.Create<OnControlToggled, CharacterController>(string.Empty, false,
            new EventEntityIdValidator<OnControlToggled>()),
        MessageRouteRule.Create<OnCharacterArrivalToggled, CharacterController>(string.Empty, false,
            new EventEntityIdValidator<OnCharacterArrivalToggled>()),
        MessageRouteRule.Create<OnOtherDisabled, CharacterController>(string.Empty, false,
            new AndValidator<OnOtherDisabled>(new EventPlayerIdValidator<OnOtherDisabled>(),
            new NotValidator<OnOtherDisabled>((new EventEntityIdValidator<OnOtherDisabled>())))),
        MessageRouteRule.Create<OnGrounded, CharacterController>(string.Empty, false,
            new EventEntityIdValidator<OnGrounded>()),
        MessageRouteRule.Create<OnCharacterDied, CharacterController>(string.Empty, false,
            new EventEntityIdValidator<OnCharacterDied>()),
        MessageRouteRule.Create<OnAttackToggled, CharacterController>(string.Empty, false,
            new AndValidator<OnAttackToggled>(new EventEntityIdValidator<OnAttackToggled>(),
                                              new EventPlayerIdValidator<OnAttackToggled>())),
        MessageRouteRule.Create<OnSecondaryMovementNoneStateEntered, CharacterController>(string.Empty, false,
            new EventEntityIdValidator<OnSecondaryMovementNoneStateEntered>()),
        MessageRouteRule.Create<OnCharacterFlownToggled, CharacterController>(string.Empty, false,
            new EventEntityIdValidator<OnCharacterFlownToggled>())
        };

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
        private CharacterAnimatorController _animtorController;
        private CharacterHurtBoxHandler _hurtBoxHandler;
        private Collider2D _hitboxCollider;
        private Camera _mainCamera;
        private TargetGroupController _targetGroupController;
        private PlayerController _playerController;
        private MainCameraController _mainCameraController;
        private GroundCheck _groundCheck;
        private MovementColliderActivator _movementColliderActivator;
        private ThrowingObjectBehavior _throwingObjectBehavior;

        [Inject]
        public void Construct(StateMachine stateMachine, InputManager inputManager, IMessageBus messageBus,
            [Inject(Id = "id")] string entityId, [Inject(Id = "playerId")] int playerId, [Inject(Id = "stats")] CharacterStats stats,
            CharacterMainStateMachineContext mainStateMachineContext,
            CharacterSecondaryMovementStateMachineContext secondaryMovementStateMachineContext,
            CharacterCombatStateMachineContext combatStateMachineContext,
            CharacterBehaviorContext behaviorContext, CharacterPowerupContext powerupContext,
            CharacterLocomotionHandler locomotionHandler, CharacterHurtBoxHandler hurtBoxHandler,
            IOwnershipService ownershipService, Camera mainCamera,
            TargetGroupController targetGroupController, PlayerController playerController,
            MainCameraController mainCameraController, GroundCheck groundCheck, MovementColliderActivator colliderActivator,
            IResourceFactory resourceFactory, CharacterAnimatorController animtorController)
        {
            _stateMachine = stateMachine;
            _inputManager = inputManager;
            EntityId = entityId;
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
            _movementColliderActivator = colliderActivator;
            _resourceFactory = resourceFactory;
            _animtorController = animtorController;

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
            _messageBus.AddRule(_rules[0]);
            _messageBus.Subscribe<CharacterController, OnPowerupToggled>(this, new MessageHandlerActionExecutor<OnPowerupToggled>(Handle));

            _messageBus.AddRule(_rules[1]);
            _messageBus.Subscribe<CharacterController, OnControlToggled>(this, new MessageHandlerActionExecutor<OnControlToggled>(Handle));

            _messageBus.AddRule(_rules[2]);
            _messageBus.Subscribe<CharacterController, OnCharacterArrivalToggled>(this, new MessageHandlerActionExecutor<OnCharacterArrivalToggled>(Handle));

            _messageBus.AddRule(_rules[3]);
            _messageBus.Subscribe<CharacterController, OnOtherDisabled>(this, new MessageHandlerActionExecutor<OnOtherDisabled>(Handle));

            _messageBus.AddRule(_rules[4]);
            _messageBus.Subscribe<CharacterController, OnGrounded>(this, new MessageHandlerActionExecutor<OnGrounded>(Handle));

            _messageBus.AddRule(_rules[5]);
            _messageBus.Subscribe<CharacterController, OnCharacterDied>(this, new MessageHandlerActionExecutor<OnCharacterDied>(Handle));

            _messageBus.AddRule(_rules[6]);
            _messageBus.Subscribe<CharacterController, OnAttackToggled>(this, new MessageHandlerActionExecutor<OnAttackToggled>(Handle));

            _messageBus.AddRule(_rules[7]);
            _messageBus.Subscribe<CharacterController, OnSecondaryMovementNoneStateEntered>(this, new MessageHandlerActionExecutor<OnSecondaryMovementNoneStateEntered>(Handle));

            _messageBus.AddRule(_rules[8]);
            _messageBus.Subscribe<CharacterController, OnCharacterFlownToggled>(this, new MessageHandlerActionExecutor<OnCharacterFlownToggled>(Handle));
        }

        private void UnsubscribeEvents()
        {
            _messageBus.Unsubscribe<CharacterController, OnPowerupToggled>(this);
            _messageBus.Unsubscribe<CharacterController, OnControlToggled>(this);
            _messageBus.Unsubscribe<CharacterController, OnCharacterArrivalToggled>(this);
            _messageBus.Unsubscribe<CharacterController, OnOtherDisabled>(this);
            _messageBus.Unsubscribe<CharacterController, OnGrounded>(this);
            _messageBus.Unsubscribe<CharacterController, OnCharacterDied>(this);
            _messageBus.Unsubscribe<CharacterController, OnAttackToggled>(this);
            _messageBus.Unsubscribe<CharacterController, OnSecondaryMovementNoneStateEntered>(this);
            _messageBus.Unsubscribe<CharacterController, OnCharacterFlownToggled>(this);
        }

        void Awake()
        {
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

            _stateMachine.Update(Time.deltaTime, _mainStateMachineContext);
            _stateMachine.Update(Time.deltaTime, _secondaryMovementStateMachineContext);
            _stateMachine.Update(Time.deltaTime, _combatStateMachineContext);
        }

        void FixedUpdate()
        {
            _stateMachine.FixedUpdate(Time.deltaTime, _mainStateMachineContext);
            _stateMachine.FixedUpdate(Time.deltaTime, _secondaryMovementStateMachineContext);
            _stateMachine.FixedUpdate(Time.deltaTime, _combatStateMachineContext);
        }


        void LateUpdate()
        {
            _stateMachine.LateUpdate(Time.deltaTime, _mainStateMachineContext);
            _stateMachine.LateUpdate(Time.deltaTime, _secondaryMovementStateMachineContext);
            _stateMachine.LateUpdate(Time.deltaTime, _combatStateMachineContext);
        }

        private void UpdateBetweenContexts()
        {
            _secondaryMovementStateMachineContext.isGrounded = isGrounded;
            _combatStateMachineContext.isGrounded = isGrounded;
            _behaviorContext.isGrounded = isGrounded;

            _behaviorContext.jumpCounter = _secondaryMovementStateMachineContext.jumpCounter;
        }

        public void ToggleStateMachineContexts(bool active)
        {
            _mainStateMachineContext.ToggleContext(active);
            _secondaryMovementStateMachineContext.ToggleContext(active);
            _combatStateMachineContext.ToggleContext(active);
        }

        public void ToggleControls(bool active)
        {
            _mainStateMachineContext.ToggleControl(active);
            _secondaryMovementStateMachineContext.ToggleControl(active);
            _combatStateMachineContext.ToggleControl(active);
        }

        private void ChangeMoveSpeed()
        {
            (_mainStateMachineContext.CurrentState as IMainState).ChangeMoveSpeed(_mainStateMachineContext);
        }

        private void EnterStage()
        {
            isJustEntered = true;
            _locomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Continuous);
            _groundCheck.ToggleCollider(true);
            _movementColliderActivator.ToggleColliderActivator(true);
            Vector2 cameraSize = _mainCameraController.CameraSize;
            _locomotionHandler.Teleport(new Vector2(_mainCamera.transform.position.x + (int)_playerController.side * cameraSize.x / 2, _mainCamera.transform.position.y));
            _locomotionHandler.ThrowInside(_playerController.side);
            _targetGroupController.SwitchTarget(PlayerId - 1,
                new Cinemachine.CinemachineTargetGroup.Target { target = transform, radius = Stats.cameraValues.cameraRadius, weight = Stats.cameraValues.cameraWeight });
        }

        private void ExitStage()
        {
            _stateMachine.Reset(_mainStateMachineContext);
            _stateMachine.Reset(_secondaryMovementStateMachineContext);
            _stateMachine.Reset(_combatStateMachineContext);
            _animtorController.Reset();
            _groundCheck.ToggleCollider(false);
            _movementColliderActivator.ToggleMovementColliders(false);
            ToggleStateMachineContexts(false);
            _locomotionHandler.GoOutside(Stats.miscValues.exitVelocity, _playerController.side);
        }

        /// <summary>
        /// Handles the event when the character attack is ended
        /// </summary>
        public void Handle(OnAttackToggled @event)
        {
            if (!@event.Enable)
            {
                _combatStateMachineContext.isAttackingEnded = true;
            }
        }

        /// <summary>
        /// Handles the event when the character powerup is toggled (activate or disabled)
        /// </summary>
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

        /// <summary>
        /// Handles the event when the character controling by player is toggled (enables or disabled)
        /// </summary>
        public void Handle(OnControlToggled @event)
        {
            ToggleControls(@event.Enable);
        }

        /// <summary>
        /// Handles the event when other characters of the player should be disabled (Some stuff are disabled by diffult)
        /// </summary>
        public void Handle(OnOtherDisabled @event)
        {
            _locomotionHandler.SetNoGravityScale();
            _movementColliderActivator.ToggleMovementColliders(false);
        }

        /// <summary>
        /// Hanldes the event for bringing the character in and out
        /// </summary>
        public void Handle(OnCharacterArrivalToggled @event)
        {
            if (@event.Enter)
            {
                EnterStage();
            }
            else
            {
                ExitStage();
            }
        }

        /// <summary>
        /// Handles the event when the character triggers the ground
        /// </summary>
        public void Handle(OnGrounded @event)
        {
            _locomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Discrete);
            _locomotionHandler.SetGroundGravityScale();
            _locomotionHandler.Stop();

            if (isJustEntered)
            {
                isJustEntered = false;
                ToggleStateMachineContexts(true);
                ToggleControls(true);
            }
        }

        /// <summary>
        /// Handles the event when the character is dead
        /// </summary>
        public void Handle(OnCharacterDied @event)
        {
            _locomotionHandler.Stop();
            ToggleStateMachineContexts(false);
        }

        /// <summary>
        /// Handles the event when the character secondary movement state machine is changed to None
        /// (Retrieve correct movement speed)
        /// </summary>
        public void Handle(OnSecondaryMovementNoneStateEntered @event)
        {
            ChangeMoveSpeed();
        }

        /// <summary>
        /// Handles the event when the character flies or stops flying
        /// </summary>
        public void Handle(OnCharacterFlownToggled @event)
        {
            _secondaryMovementStateMachineContext.isFlying = @event.Enable;
        }
    }
}