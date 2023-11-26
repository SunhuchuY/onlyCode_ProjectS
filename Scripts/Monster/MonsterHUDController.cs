using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterHUDController : MonoBehaviour, IMonsterObserver
{
    [Header("Offset")]
    [SerializeField] private Vector3 offset;

    [SerializeField] private MonsterInformation monsterInformation;
    [SerializeField] private Transform fillBar;
    [SerializeField] private Transform backgroundBar;

    private void Start()
    {
        backgroundBar.position = transform.position + offset;
    }

    public void Notify()
    {
        float healthPercentage = Mathf.Clamp01((float)monsterInformation.curHp / (float)monsterInformation.maxHp);
        fillBar.localScale = new Vector3(healthPercentage, fillBar.localScale.y, fillBar.localScale.z);
    }

    private void Update()
    {
        Vector3 directionToPlayer = PlayerManager.instance.gameObject.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        backgroundBar.rotation = Quaternion.Slerp(backgroundBar.rotation, targetRotation, Time.deltaTime * 5f);
    }
}