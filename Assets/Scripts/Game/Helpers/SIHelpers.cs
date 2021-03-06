﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpaceInvaders
{
    [Serializable]
    public class SimpleTweenInfo<T>
    {
        public float durationTime;
        public AnimationCurve animationCurve;
        [HideInInspector] public T startValue;
        [HideInInspector] public T endValue;
    }

    [Serializable]
    public class VectorTweenInfo : SimpleTweenInfo<Vector3>{}
    [Serializable]
    public class QuaternionTweenInfo : SimpleTweenInfo<Quaternion> {}

    public static class SIHelpers
    {
        public static float CAMERA_MIN_VIEWPORT_X = 0.075f;
        public static float CAMERA_MAX_VIEWPORT_X = 0.925f;
        public static float CAMERA_MIN_VIEWPORT_Y = 0.0f;
        public static float CAMERA_MAX_VIEWPORT_Y = 1.0f;
        
        public const float VIEWPORT_SPAWN_MIN = -0.25f;
        public const float VIEWPORT_SPAWN_MAX = 1.25f;
        
        public static Vector3 VectorZero = new Vector3(0f, 0f, 0f);
        public static Vector3 VectorDown = new Vector3(0f, -1f, 0f);

        public static bool IsObjectOutOfHorizontalViewportBounds3D(this Vector3 objectViewportPos)
        {
            return objectViewportPos.x >= CAMERA_MAX_VIEWPORT_X || objectViewportPos.x <= CAMERA_MIN_VIEWPORT_X;
        }
        
        public static bool IsObjectVisibleInTheScreen(this Vector3 objectViewportPos)
        {
            return objectViewportPos.x >= VIEWPORT_SPAWN_MIN && objectViewportPos.x <= VIEWPORT_SPAWN_MAX &&
                   objectViewportPos.y >= VIEWPORT_SPAWN_MIN && objectViewportPos.y <= VIEWPORT_SPAWN_MAX;
        }

        public static Vector3 SnapToGrid(Vector3 pos, float offset)
        {
            float x = pos.x;
            float y = pos.y;
            float z = pos.z;
            x = SnapToGrid(x, offset);
            y = SnapToGrid(y, offset);
            z = SnapToGrid(z, offset);
            return new Vector3(x, y, z);
        }

        public static int SnapToGrid(int pos, int offset)
        {
            float x = pos;
            return Mathf.RoundToInt(x / offset) * offset;
        }

        public static float SnapToGrid(float pos, float offset)
        {
            float x = pos;
            return Mathf.Round(x / offset) * offset;
        }

        public static void AddUnique<T>(this List<T> list, T elementToInsert)
        {
            if (list.Contains(elementToInsert))
            {
                Debug.Log("List contains given element - update stopped.");
                return;
            }

            list.Add(elementToInsert);
        }

        public static void SISimpleLogger<T>(T sendingObject, string message, SimpleLoggerTypes logType) where T : MonoBehaviour
        {
#if UNITY_EDITOR
            string formattedMessage = string.Format("[{0}]: {1}(): {2}", Time.realtimeSinceStartup, typeof(T), message);
            switch (logType)
            {
                case SimpleLoggerTypes.Log:
                    Debug.Log(formattedMessage);
                    break;
                case SimpleLoggerTypes.Warning:
                    Debug.LogWarning(formattedMessage);
                    break;
                case SimpleLoggerTypes.Error:
                    Debug.LogError(formattedMessage);
                    break;
            }
#endif
        }
        
        
        // do wywalenia
        
        //

        public static IEnumerator SimpleTween3D(Action<Vector3> onTweenAction, 
                                                VectorTweenInfo tweenInfo,
                                                Action onTweenEnd = null)
        {
            float currentTime = 0.0f;
            float animationProgress = 0.0f;
            float curveProgress = 0.0f;

            while (currentTime < tweenInfo.durationTime)
            {

                animationProgress = Mathf.Clamp01(currentTime / tweenInfo.durationTime);
                curveProgress = tweenInfo.animationCurve.Evaluate(animationProgress);

                onTweenAction?.Invoke(Vector3.Lerp(tweenInfo.startValue, tweenInfo.endValue,
                    currentTime / tweenInfo.durationTime));

                currentTime += Time.deltaTime;
                yield return null;
            }

            onTweenAction?.Invoke(tweenInfo.endValue);
            onTweenEnd.Invoke();
            yield return null;
        }

        public static IEnumerator SimpleTween3D(Action<Quaternion> onTweenAction,
            QuaternionTweenInfo tweenInfo,
            Action onTweenEnd = null)
        {
            float currentTime = 0.0f;
            float animationProgress = 0.0f;
            float curveProgress = 0.0f;

            while (currentTime < tweenInfo.durationTime)
            {
                animationProgress = Mathf.Clamp01(currentTime / tweenInfo.durationTime);
                curveProgress = tweenInfo.animationCurve.Evaluate(animationProgress);

                onTweenAction?.Invoke(Quaternion.Slerp(tweenInfo.startValue, tweenInfo.endValue,
                    currentTime / tweenInfo.durationTime));

                currentTime += Time.deltaTime;
                yield return null;
            }

            onTweenAction?.Invoke(tweenInfo.endValue);
            onTweenEnd?.Invoke();
            yield return null;
        }
    }
}
