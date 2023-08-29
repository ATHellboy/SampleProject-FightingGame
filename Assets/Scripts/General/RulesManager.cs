using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.General
{
    public class RulesManager : MonoBehaviour
    {
        List<MessageBusRules> messageBusesRules = new();

        [Inject]
        public void Constrcut(List<MessageBusRules> _messageBusesRules)
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