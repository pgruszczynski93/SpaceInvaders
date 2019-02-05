﻿using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerShieldBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _shield;

        public void EnableShield(bool canBeEnabled)
        {
            if (_shield == null)
            {
                SIHelpers.SISimpleLogger(this, "Shield is not assigned.", SimpleLoggerTypes.Error);
                return;
            }

            if (_shield.activeSelf && canBeEnabled == false)
            {
                _shield.SetActive(false);
                return;
            }
            _shield.SetActive(canBeEnabled);
        }
    }
}