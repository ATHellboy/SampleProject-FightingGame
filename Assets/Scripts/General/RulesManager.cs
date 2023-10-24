using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace AlirezaTarahomi.FightingGame.General
{
    public class RulesManager : MonoBehaviour
    {
        IEnumerable<MessageBusRules> messageBusesRules = new List<MessageBusRules>();

        [Inject]
        public void Constrcut(IEnumerable<MessageBusRules> _messageBusesRules)
        {
            messageBusesRules = _messageBusesRules;
        }

        void Awake()
        {
            foreach (MessageBusRules messageBusRules in messageBusesRules)
            {
                messageBusRules.InitRules();
                messageBusRules.AddRules();
            }
        }
    }
}