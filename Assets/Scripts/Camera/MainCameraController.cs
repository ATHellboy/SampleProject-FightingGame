using UnityEngine;

namespace AlirezaTarahomi.FightingGame.CameraSystem
{
    public class MainCameraController : MonoBehaviour
    {
        public Camera MainCamera { get; private set; }

        public Vector2 CameraSize
        {
            get
            {
                float height = 2f * MainCamera.orthographicSize;
                float width = height * MainCamera.aspect;

                return new(width, height);
            }
        }

		void Awake()
		{
			MainCamera = GetComponent<Camera>();
		}
	}
}