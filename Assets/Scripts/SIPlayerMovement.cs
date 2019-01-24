﻿using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace SpaceInvaders
{
    public class SIPlayerMovement : SIMovement
    {
        [Range(0,1)] [SerializeField] private float _lerpStep;

        private Transform _cachedTransform;
        private Vector2 _startPosition;

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();

            _cachedTransform = transform;
            _startPosition = _cachedTransform.position;
            _currentMovementSpeed = BASIC_SPEED;
        }

        protected override void OnEnable()
        {
            SIEventsHandler.OnPlayerMove += MoveObject;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnPlayerMove -= MoveObject;
        }

        protected override void MoveObject()
        {
            base.MoveObject();

            float dt = Time.deltaTime;
            float horizontalMoveDelta = Input.GetAxis("Horizontal") * _currentMovementSpeed * dt;

            Vector2 currentPosition = _cachedTransform.position;
            Vector2 newPosition = new Vector2(_cachedTransform.position.x + horizontalMoveDelta, _startPosition.y);
            Vector2 smoothedPosition = Vector2.Lerp(currentPosition, newPosition, _lerpStep);

            Vector2 objectInCameraBoundsPos = _mainCamera.WorldToViewportPoint(smoothedPosition);
            objectInCameraBoundsPos.x = Mathf.Clamp(objectInCameraBoundsPos.x, CAMERA_MIN_PERCENT_OFFSET,
                CAMERA_MAX_PERCENT_OFFSET);

            objectInCameraBoundsPos = _mainCamera.ViewportToWorldPoint(objectInCameraBoundsPos);

            _cachedTransform.position = objectInCameraBoundsPos;

        }
    }
}