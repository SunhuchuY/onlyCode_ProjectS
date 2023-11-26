using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    public static PlayerSword instance;

    private BoxCollider boxCollider;
    private HashSet<Transform> monsters = new HashSet<Transform>();
    private int damage = 10;

    private void Awake()
    {
        InitializeComponents();
        InitializeSingleton();
    }

    private void InitializeComponents()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    private void InitializeSingleton()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryAddMonster(other);
    }

    private void TryAddMonster(Collider other)
    {
        if (IsMonsterCollider(other))
        {
            Debug.Log("Monster Enter");
            monsters.Add(other.transform);
        }
    }

    private bool IsMonsterCollider(Collider collider)
    {
        return collider.CompareTag("Monster");
    }

    public void AttackStart()
    {
        monsters.Clear();
        EnableCollider();
    }

    public void AttackEnd()
    {
        DisableCollider();
        ApplyDamageToMonsters();
    }

    private void EnableCollider()
    {
        boxCollider.enabled = true;
    }

    private void DisableCollider()
    {
        boxCollider.enabled = false;
    }

    private void ApplyDamageToMonsters()
    {
        foreach (var monster in monsters)
        {
            ApplyDamageToMonster(monster);
        }
    }

    private void ApplyDamageToMonster(Transform monster)
    {
        MonsterInformation monsterInfo = monster.GetComponent<MonsterInformation>();
        if (monsterInfo != null)
        {
            monsterInfo.Damage(damage);
        }
    }
}
