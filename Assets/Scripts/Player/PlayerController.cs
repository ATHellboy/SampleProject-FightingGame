using UnityEngine;
using Zenject;
using Assets.Infrastructure.Scripts.CQRS;
using AlirezaTarahomi.FightingGame.InputSystem;
using AlirezaTarahomi.FightingGame.Player.Event;
using System.Collections;
using UniRx;
using AlirezaTarahomi.FightingGame.Character;
using ScriptableObjectDropdown;
using AlirezaTarahomi.FightingGame.CameraSystem;
using Infrastructure.Factory;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        public OnCharactersConfigured OnCharactersConfigured { get; private set; } = new();

        [SerializeField] private Vector2 initialInsidePos = Vector2.zero;
        [SerializeField] private Vector2 initialOutsidePos = Vector2.zero;

        [ScriptableObjectDropdown(typeof(CharacterStats))] public ScriptableObjectReference firstCharacter = default;
        [ScriptableObjectDropdown(typeof(CharacterStats))] public ScriptableObjectReference secondCharacter = default;

        [HideInInspector] public Character.CharacterController currentCharacterController;

        public Side Side
        {
            get => _charactersSwitchingHandler.side;
            set => _charactersSwitchingHandler.side = value;
        }

        private int _playerId;
        private IMessageBus _messageBus;
        private InputManager _inputManager;
        private IResourceFactory _resourceFactory;
        private CharactersSwitchingHandler _charactersSwitchingHandler;
        private TargetGroupController _targetGroupController;
        private float _switchingCoolDown;
        private bool _canSwitch = true;
        private int _numOfRemainedChars = 2;

        [Inject]
        public void Construct(InputManager inputManager, CharactersSwitchingHandler charactersSwitchingHandler, 
            IMessageBus messageBus, IResourceFactory resourceFactory, TargetGroupController targetGroupController, 
            [Inject(Id = "playerId")] int playerId, [Inject(Id = "switchingCoolDown")] float switchingCoolDown)
        {
            _inputManager = inputManager;
            _messageBus = messageBus;
            _resourceFactory = resourceFactory;
            _charactersSwitchingHandler = charactersSwitchingHandler;
            _targetGroupController = targetGroupController;
            _playerId = playerId;
            _switchingCoolDown = switchingCoolDown;
        }

        void Start()
        {
            (Transform target, CharacterStats stats) = InstantiateCharacter(firstCharacter, initialInsidePos);
            InstantiateCharacter(secondCharacter, initialOutsidePos);
            _targetGroupController.AssignTarget(_playerId - 1, target, stats.cameraValues.cameraRadius, stats.cameraValues.cameraWeight);
            currentCharacterController = _charactersSwitchingHandler.ConfigCharacters();
            OnCharactersConfigured?.Invoke();
        }

        // Update is called once per frame
        void Update()
        {
            if (_inputManager.IsPressed("Switch_P" + _playerId) && _numOfRemainedChars > 1 && _canSwitch)
            {
                Switch();
            }

            currentCharacterController.HandleInputPressed("Jump", _inputManager.IsPressed("Jump_P" + _playerId));
            currentCharacterController.HandleInputPressed("Attack", _inputManager.IsPressed("Attack_P" + _playerId));
            currentCharacterController.HandleInputPressed("PowerupAttack", _inputManager.IsPressed("PowerupAttack_P" + _playerId));

            currentCharacterController.moveAxes = 
                new Vector2(_inputManager.GetAxis("MoveHorizontal_P" + _playerId), _inputManager.GetAxis("MoveVertical_P" + _playerId));
        }

        private (Transform, CharacterStats) InstantiateCharacter(ScriptableObjectReference characterStatsReference, Vector2 pos)
        {
            var characterStats = characterStatsReference.value as CharacterStats;
            Transform characterInstance = _resourceFactory.Instantiate(characterStats.prefab, pos, transform);
            var characterController = characterInstance.GetComponent<Character.CharacterController>();
            _charactersSwitchingHandler.EnqueueCharacter(characterController);
            characterController.OnAttackStarted.AddListener(HandleOnAttackStarted);
            characterController.OnAttackEnded.AddListener(HandleOnAttackEnded);
            characterController.OnDied.AddListener(HandleOnCharacterDied);
            characterController.Deactivate();
            return (characterInstance, characterStats);
        }

        public void InitCharacterFace()
        {
            currentCharacterController.InitCharacterFace(Side);
        }

        private void Switch()
        {
            _canSwitch = false;
            Observable.FromCoroutine(_ => SwitchingTimer()).Subscribe();
            _charactersSwitchingHandler.ExitCurrentCharacter();
            currentCharacterController = _charactersSwitchingHandler.EnterNextCharacter();
        }

        IEnumerator SwitchingTimer()
        {
            yield return new WaitForSeconds(_switchingCoolDown);
            _canSwitch = true;
        }

        /// <summary>
        /// Handles the event when each character dies
        /// </summary>
        public void HandleOnCharacterDied()
        {
            _numOfRemainedChars--;

            if (_numOfRemainedChars == 0)
            {
                _messageBus.RaiseEvent(new OnGameOvered());
            }
            else
            {
                currentCharacterController = _charactersSwitchingHandler.EnterNextCharacter();
            }
        }

        /// <summary>
        /// Handles the event when character attack is started
        /// </summary>
        public void HandleOnAttackStarted()
        {
            _canSwitch = false;
        }

        /// <summary>
        /// Handles the event when character attack is ended
        /// </summary>
        public void HandleOnAttackEnded()
        {
            _canSwitch = true;
        }
    }
}