using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace AlirezaTarahomi.FightingGame.UI
{
    public class UIGameOverPanel : MonoBehaviour, IEventHandler<OnGameOvered>
    {
        private IMessageBus _messageBus;
        private UIVisibilityController _visibilityController;

        [Inject]
        public void Construct(IMessageBus messageBus)
        {
            _messageBus = messageBus;
            _visibilityController = GetComponent<UIVisibilityController>();
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
            _messageBus.AddRule(MessageRouteRule.Create<OnGameOvered, UIGameOverPanel>(string.Empty, false));
            _messageBus.Subscribe<UIGameOverPanel, OnGameOvered>(this, new MessageHandlerActionExecutor<OnGameOvered>(Handle));
        }

        private void UnsubscribeEvents()
        {
            _messageBus.Unsubscribe<UIGameOverPanel, OnGameOvered>(this);
        }

        public void OnClickRestart()
        {
            SceneManager.LoadScene(0);
        }

        public void Handle(OnGameOvered @event)
        {
            _visibilityController.ToggleDisplaying(true);
        }
    }
}