using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.CameraSystem
{
    public class MainCameraController : MonoBehaviour
    {
        public Vector2 CameraSize
        {
            get
            {
                float height = 2f * _mainCamera.orthographicSize;
                float width = height * _mainCamera.aspect;

                return new Vector2(width, height);
            }
        }

        private Camera _mainCamera;

        [Inject]
        public void Contruct(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }
    }
}