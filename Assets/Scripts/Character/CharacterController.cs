using AlirezaTarahomi.FightingGame.CameraSystem;
using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.Character.State.Main;
using AlirezaTarahomi.FightingGame.Player;
using AlirezaTarahomi.FightingGame.Service;
using Infrastructure.StateMachine;
using UnityEngine;
using VContainer;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterController : MonoBehaviour
    {
        public OnAttackStarted OnAttackStarted { get => _combatStateMachineContext.OnAttackStarted; }
        public OnAttackEnded OnAttackEnded { get => _behaviorContext.OnAttackEnded; }
        public OnDied OnDied { get => _mainStateMachineContext.OnDied; }

        [SerializeField] private GameObject _hitbox;

        [HideInInspector] public bool isJustEntered;
        [HideInInspector] public Vector2 moveAxes;

        public CharacterContext Context { get; private set; }

        private IObjectResolver _container;
        private IOwnershipService _ownershipService;
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
        private MainCameraController _mainCameraController;
        private GroundCheck _groundCheck;
        private MovementColliderActivator _movementColliderActivator;
        private bool _isGrounded;

        [Inject]
        public void Construct(IObjectResolver container, IOwnershipService ownershipService, StateMachine stateMachine, 
            CharacterContext characterContext, CharacterMainStateMachineContext mainStateMachineContext,
            CharacterSecondaryMovementStateMachineContext secondaryMovementStateMachineContext,
            CharacterCombatStateMachineContext combatStateMachineContext, CharacterBehaviorContext behaviorContext, 
            CharacterPowerupContext powerupContext, CharacterLocomotionHandler locomotionHandler, CharacterHurtBoxHandler hurtBoxHandler,
            MainCameraController mainCameraController, GroundCheck groundCheck, MovementColliderActivator colliderActivator, 
            CharacterAnimatorController animtorController)
        {
            _container = container;
            _ownershipService = ownershipService;
            _stateMachine = stateMachine;
            Context = characterContext;
            _locomotionHandler = locomotionHandler;
            _hurtBoxHandler = hurtBoxHandler;
            _mainCameraController = mainCameraController;
            _groundCheck = groundCheck;
            _movementColliderActivator = colliderActivator;
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
            _groundCheck.OnGrounded.AddListener(HandleOnGrounded);
            _behaviorContext.OnAttackEnded.AddListener(HandleOnAttackEnded);
            _behaviorContext.OnFlyingToggled.AddListener(HandleOnFlyingToggled);
            _behaviorContext.OnFlyOverEnded.AddListener(HandleOnFlyOverEnded);
            _powerupContext.OnPowerupToggled.AddListener(HandleOnPowerupToggled);
            _mainStateMachineContext.OnDied.AddListener(HandleOnDied);
            _secondaryMovementStateMachineContext.OnChangeMoveSpeedRequested.AddListener(HandleOnChangeMoveSpeedRequested);
        }

        private void UnsubscribeEvents()
        {
            _groundCheck.OnGrounded.RemoveListener(HandleOnGrounded);
            _behaviorContext.OnAttackEnded.RemoveListener(HandleOnAttackEnded);
            _behaviorContext.OnFlyingToggled.RemoveListener(HandleOnFlyingToggled);
            _behaviorContext.OnFlyOverEnded.RemoveListener(HandleOnFlyOverEnded);
            _powerupContext.OnPowerupToggled.RemoveListener(HandleOnPowerupToggled);
            _mainStateMachineContext.OnDied.RemoveListener(HandleOnDied);
            _secondaryMovementStateMachineContext.OnChangeMoveSpeedRequested.RemoveListener(HandleOnChangeMoveSpeedRequested);
        }

        public void HandleInputPressed(string inputName, bool isPressed)
        {
            switch (inputName)
            {
                case "Jump":
                    _secondaryMovementStateMachineContext.isJumpedPressed = isPressed;
                    break;
                case "Attack":
                    _combatStateMachineContext.isAttackedPressed = isPressed;
                    break;
                case "PowerupAttack":
                    _combatStateMachineContext.isPowerupAttackedPressed = isPressed;
                    break;
            }
        }

        public void HandleInputReleased(string inputName, bool isReleased)
        {
            switch (inputName)
            {
                default:
                    break;
            }
        }

        public void HandleInputHeld(string inputName, bool isHeld)
        {
            switch (inputName)
            {
                default:
                    break;
            }
        }

        void Awake()
        {
            GetHitBox();
            InitPowerup(Context.stats.behaviors.powerup.value);
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

        private void InitPowerup(ScriptableObject powerup)
        {
            var clone = Instantiate(powerup) as IPowerup;
            _container.Inject(clone);
            _combatStateMachineContext.powerup = clone;
        }

        private void InitAttackBehavior(ScriptableObject attackBehavior)
        {
            if (attackBehavior != null)
            {
                var clone = Instantiate(attackBehavior) as IAttackBehavior;
                _container.Inject(clone);
                _combatStateMachineContext.AddAttackBehavior(clone);
            }
        }

        private void InitAttackBehaviors()
        {
            InitAttackBehavior(_combatStateMachineContext.powerup.PowerupAttackBehavior);
            InitAttackBehavior(Context.stats.behaviors.normalAttack.value);
            InitAttackBehavior(Context.stats.behaviors.complextAttack.value);
        }

        public void Activate()
        {
            _locomotionHandler.SetGroundGravityScale();
            _movementColliderActivator.ToggleMovementColliders(true);
        }
        
        public void Deactivate()
        {
            _locomotionHandler.SetNoGravityScale();
            _movementColliderActivator.ToggleMovementColliders(false);
        }

        public void InitCharacterFace(Side side)
        {
            _locomotionHandler.Flip(side);
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
            _mainStateMachineContext.moveAxes = moveAxes;

            _secondaryMovementStateMachineContext.isGrounded = _isGrounded;
            _combatStateMachineContext.isGrounded = _isGrounded;
            _behaviorContext.isGrounded = _isGrounded;

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

        public void EnterStage(Side side, Vector3 targetPos)
        {
            isJustEntered = true;
            _locomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Continuous);
            _groundCheck.ToggleCollider(true);
            _movementColliderActivator.ToggleColliderActivator(true);
            Vector2 cameraSize = _mainCameraController.CameraSize;
            _locomotionHandler.Teleport(new Vector2(_mainCameraController.MainCamera.transform.position.x + (int)side * cameraSize.x / 2,
                _mainCameraController.MainCamera.transform.position.y));
            _locomotionHandler.ThrowInside(side, targetPos);
        }

        public void ExitStage(Side side)
        {
            _stateMachine.Reset(_mainStateMachineContext);
            _stateMachine.Reset(_secondaryMovementStateMachineContext);
            _stateMachine.Reset(_combatStateMachineContext);
            _animtorController.Reset();
            _groundCheck.ToggleCollider(false);
            _movementColliderActivator.ToggleMovementColliders(false);
            ToggleStateMachineContexts(false);
            _locomotionHandler.GoOutside(Context.stats.miscValues.exitVelocity, side);
        }

        public void SetLayer(int layer)
        {
            gameObject.layer = layer;
        }

        /// <summary>
        /// Handles the event when the character attack is ended
        /// </summary>
        public void HandleOnAttackEnded()
        {
            _combatStateMachineContext.isAttackingEnded = true;
        }

        /// <summary>
        /// Handles the event when the character powerup is toggled (activate or disabled)
        /// </summary>
        public void HandleOnPowerupToggled(bool active)
        {
            _behaviorContext.isPowerupActive = active;
            _combatStateMachineContext.isPowerupActive = active;
        }

        /// <summary>
        /// Handles the event when the character flies or stops flying
        /// </summary>
        public void HandleOnFlyingToggled(bool enable)
        {
            ToggleControls(!enable);
            _secondaryMovementStateMachineContext.isFlying = enable;
        }

        public void HandleOnFlyOverEnded()
        {
            _behaviorContext.isPowerupActive = false;
            _combatStateMachineContext.isPowerupActive = false;
        }

        /// <summary>
        /// Handles the event when the character triggers the ground
        /// </summary>
        public void HandleOnGrounded(bool isGrounded)
        {
            _isGrounded = isGrounded;

            if (!_isGrounded)
                return;

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
        public void HandleOnDied()
        {
            _locomotionHandler.Stop();
            ToggleStateMachineContexts(false);
        }

        /// <summary>
        /// Handles the event when the character secondary movement state machine is changed to None
        /// (Retrieve correct movement speed)
        /// </summary>
        public void HandleOnChangeMoveSpeedRequested()
        {
            ChangeMoveSpeed();
        }
    }
}