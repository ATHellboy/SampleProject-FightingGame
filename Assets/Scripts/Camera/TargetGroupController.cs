using UnityEngine;
using Cinemachine;
using static Cinemachine.CinemachineTargetGroup;

namespace AlirezaTarahomi.FightingGame.CameraSystem
{
    public class TargetGroupController
    {
        private CinemachineTargetGroup _targetGroup;

        public TargetGroupController(CinemachineTargetGroup targetGroup)
        {
            _targetGroup = targetGroup;
        }

        public void SwitchTarget(int index, Target target)
        {
            _targetGroup.m_Targets[index] = target;
        }
    }
}