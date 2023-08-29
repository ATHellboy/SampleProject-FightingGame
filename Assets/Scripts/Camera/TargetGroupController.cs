using UnityEngine;
using Cinemachine;

namespace AlirezaTarahomi.FightingGame.CameraSystem
{
    public class TargetGroupController
    {
        private CinemachineTargetGroup _targetGroup;

        public TargetGroupController(CinemachineTargetGroup targetGroup)
        {
            _targetGroup = targetGroup;
        }

        public void AssignTarget(int index, Transform target, float radius, float weight)
        {
            _targetGroup.m_Targets[index] = new CinemachineTargetGroup.Target { target = target, radius = radius, weight = weight };
        }
    }
}