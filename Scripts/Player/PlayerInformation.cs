using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    public static PlayerInformation instance;

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

            PlayerObserver.Instance.NotifyObservers();
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

            PlayerObserver.Instance.NotifyObservers();
        }
    }


    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        curHp = maxHp;
        PlayerObserver.Instance.NotifyObservers();
    }

    public void Damage(int amount)
    {
        curHp -= amount;
    }
}
