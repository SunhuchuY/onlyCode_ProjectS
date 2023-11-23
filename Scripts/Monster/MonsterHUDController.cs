using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHUDController : MonoBehaviour, IMonsterObserver
{
    [Header("OffSet")]
    [SerializeField] private Vector3 offSet;

    [SerializeField] private MonsterInformation monsterInformation;
    [SerializeField] private Transform hpFillBar;
    [SerializeField] private Transform hpBar;

    private void Start()
    {
        hpBar.position = transform.position + offSet;
    }

    public void Notify()
    {
        float extraHp = monsterInformation.curHp / monsterInformation.maxHp;
        hpFillBar.localScale = new Vector3(extraHp, hpFillBar.localScale.y, hpFillBar.localScale.z);
    }

    private void Update()
    {
        Vector3 direction = transform.position - PlayerManager.instance.gameObject.transform.position;
        hpBar.rotation = Quaternion.LookRotation(direction);
    }
}
