using UniRx;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterContext : MonoBehaviour
    {
        public bool debugStateMachine = false;
        public CharacterStats stats;
        public float throwingAngle = 30;
        public float dieFadeOutDelay = 0.5f;
        public float dieFadeOutDuration = 0.5f;

        public CompositeDisposable Disposables { get; private set; } = new();
    }
}