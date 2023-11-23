using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class PlayerHUDController : MonoBehaviour, IPlayerObserver
{
    [SerializeField] private Transform hpFillBar;

    public void Notify()
    {
        float extraHp = (float)PlayerInformation.instance.curHp / (float)PlayerInformation.instance.maxHp;
        Mathf.Clamp(extraHp, 0, 1);
        hpFillBar.localScale = new Vector3(extraHp, hpFillBar.localScale .y, hpFillBar.localScale .z);
    }
}
