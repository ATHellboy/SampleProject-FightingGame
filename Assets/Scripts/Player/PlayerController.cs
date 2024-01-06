using UnityEngine;
using VContainer;
using AlirezaTarahomi.FightingGame.InputSystem;
using AlirezaTarahomi.FightingGame.Player.Event;
using System.Collections;
using AlirezaTarahomi.FightingGame.Character;
using ScriptableObjectDropdown;
using AlirezaTarahomi.FightingGame.CameraSystem;
using Infrastructure.Factory;
using Infrastructure.Extension;
using MessagePipe;
using System;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        public OnCharactersConfigured OnCharactersConfigured { get; private set; } = new();

        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Vector2 initialOutsidePos = Vector2.zero;
        [SerializeField] private LayerMask _avatarLayerMask;
        [SerializeField] private RenderTexture _avatarRenderTexture;

        [ScriptableObjectDropdown(typeof(CharacterStats))] public ScriptableObjectReference firstCharacter = default;
        [ScriptableObjectDropdown(typeof(CharacterStats))] public ScriptableObjectReference secondCharacter = default;

        [HideInInspector] public Character.CharacterController currentCharacterController;

        public Side Side
        {
            get => _charactersSwitchingHandler.side;
            set => _charactersSwitchingHandler.side = value;
        }

        private const string PlayerInputSwitch = "Switch_P";
        private const string PlayerInputJump = "Jump_P";
        private const string PlayerInputAttack = "Attack_P";
        private const string PlayerInputPowerAttack = "PowerupAttack_P";
        private const string PlayerInputMoveHorizontal = "MoveHorizontal_P";
        private const string PlayerInputMoveVertical = "MoveVertical_P";

        private IPublisher<OnGameOver> _gameOverPublisher;
        private IPublisher<int, OnAttackCooldownStarted> _attackCooldownPublisher;
        private IPublisher<int, OnPowerupTimerStarted> _powerupTimerPublisher;

        private InputManager _inputManager;
        private PlayersContext _playersContext;
        private PlayerContext _playerContext;
        private CharactersSwitchingHandler _charactersSwitchingHandler;
        private TargetGroupController _targetGroupController;
        private Camera _avatarCamera;
        private bool _canSwitch = true;
        private int _numOfRemainedChars = 2;

        [Inject]
        public void Construct(InputManager inputManager, PlayersContext playersContext, PlayerContext playerContext, 
            CharactersSwitchingHandler charactersSwitchingHandler, TargetGroupController targetGroupController, 
            Camera avatarCamera, IPublisher<OnGameOver> gameOverPublisher, IPublisher<int, OnAttackCooldownStarted> attackCooldownPublisher,
            IPublisher<int, OnPowerupTimerStarted> powerupCooldownPublisher)
        {
            _playersContext = playersContext;
            _playerContext = playerContext;
            _inputManager = inputManager;
            _charactersSwitchingHandler = charactersSwitchingHandler;
            _targetGroupController = targetGroupController;
            _avatarCamera = avatarCamera;
            _gameOverPublisher = gameOverPublisher;
            _attackCooldownPublisher = attackCooldownPublisher;
            _powerupTimerPublisher = powerupCooldownPublisher;
        }

        void Start()
        {
            (Transform target, CharacterStats stats) = InstantiateCharacter(firstCharacter, _spawnPoint.position);
            InstantiateCharacter(secondCharacter, initialOutsidePos);
            _targetGroupController.AssignTarget(_playerContext.index - 1, target, stats.cameraValues.cameraRadius, stats.cameraValues.cameraWeight);
            currentCharacterController = _charactersSwitchingHandler.ConfigCharacters();
            InitAvatarCamera();
            OnCharactersConfigured?.Invoke();
        }

        void Update()
        {
            if (_inputManager.IsPressed(PlayerInputSwitch + _playerContext.index) && _numOfRemainedChars > 1 && _canSwitch)
            {
                Switch();
            }

            currentCharacterController.HandleInputPressed(InputManager.Type.Jump, _inputManager.IsPressed(PlayerInputJump + _playerContext.index));
            currentCharacterController.HandleInputReleased(InputManager.Type.Jump, _inputManager.IsReleased(PlayerInputJump + _playerContext.index));
            currentCharacterController.HandleInputPressed(InputManager.Type.Attack, _inputManager.IsPressed(PlayerInputAttack + _playerContext.index));
            currentCharacterController.HandleInputPressed(InputManager.Type.PowerupAttack, _inputManager.IsPressed(PlayerInputPowerAttack + _playerContext.index));

            currentCharacterController.moveAxesRaw = new Vector2(
                _inputManager.GetAxisRaw(PlayerInputMoveHorizontal + _playerContext.index),
                _inputManager.GetAxisRaw(PlayerInputMoveVertical + _playerContext.index));
            currentCharacterController.moveAxes = new Vector2(
                _inputManager.GetAxis(PlayerInputMoveHorizontal + _playerContext.index), 
                _inputManager.GetAxis(PlayerInputMoveVertical + _playerContext.index));
        }

        void LateUpdate()
        {
            _avatarCamera.transform.SetXYPosition(
                currentCharacterController.transform.position.x + currentCharacterController.Context.stats.avatarCameraValues.offset.x,
                currentCharacterController.transform.position.y + currentCharacterController.Context.stats.avatarCameraValues.offset.y);
        }

        private (Transform, CharacterStats) InstantiateCharacter(ScriptableObjectReference characterStatsReference, Vector2 pos)
        {
            var characterStats = characterStatsReference.value as CharacterStats;
            Transform characterInstance = Instantiate(characterStats.prefab, pos, Quaternion.identity, transform);
            var characterController = characterInstance.GetComponent<Character.CharacterController>();
            _charactersSwitchingHandler.EnqueueCharacter(characterController);
            characterController.SetAvatarLayer(gameObject.layer);
            characterController.OnAttackStarted.AddListener(HandleOnAttackStarted);
            characterController.OnAttackEnded.AddListener(HandleOnAttackEnded);
            characterController.OnPowerupStarted.AddListener(HandleOnPowerupStarted);
            characterController.OnDied.AddListener(HandleOnCharacterDied);
            characterController.Deactivate();
            return (characterInstance, characterStats);
        }

        private void InitAvatarCamera()
        {
            _avatarCamera.cullingMask = _avatarLayerMask.value;
            _avatarCamera.targetTexture = _avatarRenderTexture;
            _avatarCamera.orthographicSize = currentCharacterController.Context.stats.avatarCameraValues.size;
            _avatarCamera.enabled = true;
        }

        public void InitCharacterFace()
        {
            currentCharacterController.InitCharacterFace(Side);
        }

        private void Switch()
        {
            _canSwitch = false;
            StartCoroutine(SwitchingTimer());
            _charactersSwitchingHandler.ExitCurrentCharacter();
            currentCharacterController = _charactersSwitchingHandler.EnterNextCharacter();
            _avatarCamera.orthographicSize = currentCharacterController.Context.stats.avatarCameraValues.size;
        }

        IEnumerator SwitchingTimer()
        {
            yield return new WaitForSeconds(_playersContext.switchingCoolDown);
            _canSwitch = true;
        }

        public void HandleOnCharacterDied()
        {
            _numOfRemainedChars--;

            if (_numOfRemainedChars == 0)
            {
                _gameOverPublisher.Publish(new OnGameOver());
            }
            else
            {
                currentCharacterController = _charactersSwitchingHandler.EnterNextCharacter();
            }
        }

        public void HandleOnAttackStarted(Type attackType, float cooldown)
        {
            _canSwitch = false;
            _attackCooldownPublisher.Publish(_playerContext.index, new OnAttackCooldownStarted(attackType, cooldown));
        }

        public void HandleOnAttackEnded()
        {
            _canSwitch = true;
        }

        public void HandleOnPowerupStarted(float duration, float cooldown)
        {
            _powerupTimerPublisher.Publish(_playerContext.index, new OnPowerupTimerStarted(duration, cooldown));
        }
    }
}