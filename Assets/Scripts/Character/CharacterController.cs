using AlirezaTarahomi.FightingGame.CameraSystem;
using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.Character.State.Main;
using AlirezaTarahomi.FightingGame.InputSystem;
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
        public OnPowerupStarted OnPowerupStarted { get => _powerupContext.OnPowerupStarted; }
        public OnDied OnDied { get => _mainStateMachineContext.OnDied; }

        [SerializeField] private GameObject _visual;
        [SerializeField] private GameObject _hitbox;

        [HideInInspector] public Vector2 moveAxesRaw;
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
        private Collider2D _hitboxCollider;
        private MainCameraController _mainCameraController;
        private SurfaceCheck _surfaceCheck;
        private SurfaceEnterChecker _surfaceEnterChecker;

        [Inject]
        public void Construct(IObjectResolver container, IOwnershipService ownershipService, StateMachine stateMachine, 
            CharacterContext characterContext, CharacterMainStateMachineContext mainStateMachineContext,
            CharacterSecondaryMovementStateMachineContext secondaryMovementStateMachineContext,
            CharacterCombatStateMachineContext combatStateMachineContext, CharacterBehaviorContext behaviorContext, 
            CharacterPowerupContext powerupContext, CharacterLocomotionHandler locomotionHandler, MainCameraController mainCameraController, 
            SurfaceCheck surfaceCheck, SurfaceEnterChecker surfaceEnterChecker, CharacterAnimatorController animtorController)
        {
            _container = container;
            _ownershipService = ownershipService;
            _stateMachine = stateMachine;
            Context = characterContext;
            _locomotionHandler = locomotionHandler;
            _mainCameraController = mainCameraController;
            _surfaceCheck = surfaceCheck;
            _surfaceEnterChecker = surfaceEnterChecker;
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

            Context.Disposables.Dispose();
            _behaviorContext.Disposables.Dispose();
            _powerupContext.Disposables.Dispose();
        }

        private void InitializeEvents()
        {
            _surfaceCheck.OnGrounded.AddListener(HandleOnGrounded);
            _behaviorContext.OnAttackEnded.AddListener(HandleOnAttackEnded);
            _behaviorContext.OnFlyingToggled.AddListener(HandleOnFlyingToggled);
            _powerupContext.OnPowerupStarted.AddListener(HandleOnPowerupStarted);
            _powerupContext.OnPowerupEnded.AddListener(HandleOnPowerupEnded);
            _mainStateMachineContext.OnDied.AddListener(HandleOnDied);
            _secondaryMovementStateMachineContext.OnChangeMoveSpeedRequested.AddListener(HandleOnChangeMoveSpeedRequested);
            _surfaceEnterChecker.OnEntered.AddListener(HandleOnEntered);
        }

        private void UnsubscribeEvents()
        {
            _surfaceCheck.OnGrounded.RemoveListener(HandleOnGrounded);
            _behaviorContext.OnAttackEnded.RemoveListener(HandleOnAttackEnded);
            _behaviorContext.OnFlyingToggled.RemoveListener(HandleOnFlyingToggled);
            _powerupContext.OnPowerupStarted.RemoveListener(HandleOnPowerupStarted);
            _powerupContext.OnPowerupEnded.RemoveListener(HandleOnPowerupEnded);
            _mainStateMachineContext.OnDied.RemoveListener(HandleOnDied);
            _secondaryMovementStateMachineContext.OnChangeMoveSpeedRequested.RemoveListener(HandleOnChangeMoveSpeedRequested);
            _surfaceEnterChecker.OnEntered.RemoveListener(HandleOnEntered);
        }

        public void HandleInputPressed(InputManager.Type inputType, bool isPressed)
        {
            switch (inputType)
            {
                case InputManager.Type.Jump:
                    _secondaryMovementStateMachineContext.isJumpedPressed = isPressed;
                    break;
                case InputManager.Type.Attack:
                    _combatStateMachineContext.isAttackedPressed = isPressed;
                    break;
                case InputManager.Type.PowerupAttack:
                    _combatStateMachineContext.isPowerupAttackedPressed = isPressed;
                    break;
            }
        }

        public void HandleInputReleased(InputManager.Type inputType, bool isReleased)
        {
            switch (inputType)
            {
                case InputManager.Type.Jump:
                    _secondaryMovementStateMachineContext.isJumpedReleased = isReleased;
                    break;
            }
        }

        public void HandleInputHeld(InputManager.Type inputType, bool isHeld)
        {
            switch (inputType)
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
            if (_hitbox)
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
            if (attackBehavior)
            {
                var clone = Instantiate(attackBehavior) as IAttackBehavior;
                _container.Inject(clone);
                _combatStateMachineContext.AddAttackBehavior(clone);
            }
        }

        private void InitAttackBehaviors()
        {
            InitAttackBehavior((Context.stats.behaviors.powerup.value as IPowerup).PowerupAttackBehavior);
            InitAttackBehavior(Context.stats.behaviors.normalAttack.value);
            InitAttackBehavior(Context.stats.behaviors.complextAttack.value);
        }

        public void Activate()
        {
            _locomotionHandler.SetGroundGravityScale();
            _surfaceEnterChecker.ToggleMovementColliders(true);
        }
        
        public void Deactivate()
        {
            _locomotionHandler.SetNoGravityScale();
            _surfaceEnterChecker.ToggleMovementColliders(false);
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
            _mainStateMachineContext.moveAxesRaw = moveAxesRaw;
            _mainStateMachineContext.moveAxes = moveAxes;
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
            _locomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Continuous);
            _surfaceCheck.ToggleCollider(true);
            _surfaceEnterChecker.ToggleCollider(true);
            Vector2 cameraSize = _mainCameraController.CameraSize;
            _locomotionHandler.Teleport(new Vector2(_mainCameraController.MainCamera.transform.position.x + (int)side * cameraSize.x / 2,
                _mainCameraController.MainCamera.transform.position.y + cameraSize.y / 4));
            _locomotionHandler.ThrowInside(side, targetPos);
        }

        public void ExitStage(Side side)
        {
            _stateMachine.Reset(_mainStateMachineContext);
            _stateMachine.Reset(_secondaryMovementStateMachineContext);
            _stateMachine.Reset(_combatStateMachineContext);
            _animtorController.Reset();
            _surfaceCheck.ToggleCollider(false);
            _surfaceEnterChecker.ToggleMovementColliders(false);
            ToggleStateMachineContexts(false);
            _locomotionHandler.GoOutside(Context.stats.miscValues.exitVelocity, side);
        }

        public void SetAvatarLayer(int layer)
        {
            _visual.layer = layer;
        }

        private void TogglePowerupActivation(bool active)
        {
            _behaviorContext.isPowerupActive = active;
            _combatStateMachineContext.isPowerupActive = active;
        }

        public void HandleOnAttackEnded()
        {
            _combatStateMachineContext.isAttackingEnded = true;
        }

        public void HandleOnPowerupStarted(float time, float cooldown)
        {
            TogglePowerupActivation(true);
        }

        public void HandleOnPowerupEnded()
        {
            TogglePowerupActivation(false);
        }

        public void HandleOnFlyingToggled(bool enable)
        {
            ToggleControls(!enable);
            _secondaryMovementStateMachineContext.isFlying = enable;
        }

        public void HandleOnGrounded()
        {
            _locomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Discrete);
        }

        public void HandleOnDied()
        {
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

        public void HandleOnEntered()
        {
            ToggleStateMachineContexts(true);
            ToggleControls(true);
            _locomotionHandler.Stop();
            _locomotionHandler.SetNoGravityScale();
        }
    }
}