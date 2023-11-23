using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInformation : MonoBehaviour
{
    [SerializeField] private MonsterObserver monsterObserver;

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

    private void Start()
    {
        curHp = maxHp;
    }

}
