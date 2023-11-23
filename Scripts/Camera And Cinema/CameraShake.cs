using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void CameraShakeFuntion(float power, float duration)
    {
        transform.DOShakePosition
                    (duration, strength: Vector3.one * power,
                    vibrato: 10, randomness: 90, fadeOut: true);
    }

}

public enum CameraShakeType
{
    Shake,
    Zoom
}
