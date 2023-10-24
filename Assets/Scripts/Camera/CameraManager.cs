using Cinemachine;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.CameraSystem
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Transform _targetGroup = default;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera = default;

        void Awake()
        {
            _cinemachineVirtualCamera.Follow = _targetGroup;
        }
    }
}