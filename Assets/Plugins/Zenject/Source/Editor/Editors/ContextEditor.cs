using UnityEditor;

#if !ODIN_INSPECTOR

namespace Zenject
{
    [NoReflectionBaking]
    public class ContextEditor : UnityInspectorListEditor
    {
        private SerializedProperty _findSiblingInstallers;

        protected override string[] PropertyNames
        {
            get
            {
                return new string[]
                {
                    "_scriptableObjectInstallers",
                    "_monoInstallers",
                    "_installerPrefabs",
                };
            }
        }

        protected override string[] PropertyDisplayNames
        {
            get
            {
                return new string[]
                {
                    "Scriptable Object Installers",
                    "Mono Installers",
                    "Prefab Installers",
                };
            }
        }

        protected override string[] PropertyDescriptions
        {
            get
            {
                return new string[]
                {
                    "Drag any assets in your Project that implement ScriptableObjectInstaller here",
                    "Drag any MonoInstallers that you have added to your Scene Hierarchy here.",
                    "Drag any prefabs that contain a MonoInstaller on them here",
                };
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            
            _findSiblingInstallers = serializedObject.FindProperty("_findSiblingMonoInstallers");
        }

        protected override void OnGui()
        {
            base.OnGui();
            
            EditorGUILayout.PropertyField(_findSiblingInstallers);
        }
    }
}

#endif
