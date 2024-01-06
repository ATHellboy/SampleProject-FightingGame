using UnityEngine;
using AlirezaTarahomi.FightingGame.Character.Behavior.Normal;
using AlirezaTarahomi.FightingGame.Character.Behavior.Complex;
using System;
using ScriptableObjectDropdown;
using AlirezaTarahomi.FightingGame.Character.Powerup;

namespace AlirezaTarahomi.FightingGame.Character
{
    [CreateAssetMenu(menuName = "Custom/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public Transform prefab;
        public GroundMovementValues groundMovementValues;
        public AirMovementValues airMovementValues;
        public CameraValues cameraValues;
        public AvatarCameraValues avatarCameraValues;
        public MiscValues miscValues;
        public Behaviors behaviors;
    }

    [Serializable]
    public class GroundMovementValues
    {
        public float moveSpeed = 10f;
    }

    [Serializable]
    public class AirMovementValues
    {
        public float inAirMoveSpeed = 7f;
        public float jumpHeight = 6f;
        public float lessJumpHeight = 4f;
        public float jumpSpeed = 30f;
        public int jumpNumber = 1;
    }

    [Serializable]
    public class Behaviors
    {
        [ScriptableObjectDropdown(typeof(INormalAttackBehavior))]
        public ScriptableObjectReference normalAttack;
        [ScriptableObjectDropdown(typeof(IComplexAttackBehavior))]
        public ScriptableObjectReference complextAttack;
        [ScriptableObjectDropdown(typeof(IPowerup))]
        public ScriptableObjectReference powerup;
    }

    [Serializable]
    public class CameraValues
    {
        public float cameraRadius = 5.1f;
        public float cameraWeight = 1f;
    }

    [Serializable]
    public class AvatarCameraValues
    {
        public float size = 0.5f;
        public Vector2 offset = Vector2.zero;
    }

    [Serializable]
    public class MiscValues
    {
        public float attackRate = 0.5f;
        public float enteringForce = 4f;
        public float exitVelocity = 70f;
    }
}