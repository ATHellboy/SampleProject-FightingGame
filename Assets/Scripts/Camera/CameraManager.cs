using Cinemachine;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.CameraSystem
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Transform _targetGroup;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        void Awake()
        {
            _cinemachineVirtualCamera.Follow = _targetGroup;
        }
    }
}