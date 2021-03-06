using System;
using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class PlayerMovementSettings : MovementSettings
    {
        public Ease easeType;
        public float easeTime;
        [Range(0, 90)] public float maxRotationAngle;
    }
}