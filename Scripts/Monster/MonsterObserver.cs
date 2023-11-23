using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterObserver
{
    void Notify();
}

public interface IMonsterSubject
{
    void RegisterObserver(IMonsterObserver observer);
    void RemoveObserver(IMonsterObserver observer);
    void NotifyObservers();
}

public class MonsterObserver : MonoBehaviour, IMonsterSubject
{
    public List<IMonsterObserver> observers = new List<IMonsterObserver>();

    [SerializeField] private MonsterHUDController monsterHUDController;

    private void Start()
    {
        RegisterObserver(monsterHUDController);
        NotifyObservers();
    }


    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Notify();
        }
    }

    public void RegisterObserver(IMonsterObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IMonsterObserver observer)
    {
        observers.Remove(observer);
    }
}
