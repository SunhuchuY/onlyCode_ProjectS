using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInformation : MonoBehaviour
{
    [SerializeField] private MonsterObserver monsterObserver;
    [SerializeField] private VMonsterAI vMonsterAI;

    private int _curHp;
    public int curHp
    {
        get
        {
            return _curHp;
        }
        private set
        {
            _curHp = value;

            monsterObserver.NotifyObservers();

            // Dead
            if(value <= 0)
            {
                vMonsterAI.Dead();
            }
        }
    }


    private int _maxHp = 100;
    public int maxHp
    {
        get
        {
            return _maxHp;
        }
        private set
        {
            _maxHp = value;

            monsterObserver.NotifyObservers();
        }
    }


    private void Awake()
    {
        monsterObserver = GetComponent<MonsterObserver>();
        vMonsterAI = GetComponent<VMonsterAI>();   
    }

    private void Start()
    {
        curHp = maxHp;
    }

    public void Damage(int damage)
    {
        curHp -= damage;    
    }
}
