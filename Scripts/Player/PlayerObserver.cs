using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerObserver
{
    void Notify();
}

public interface IPlayerSubject
{
    void RegisterObserver(IPlayerObserver observer);
    void RemoveObserver(IPlayerObserver observer);
    void NotifyObservers();
}


public class PlayerObserver : MonoBehaviour, IPlayerSubject
{
    public static PlayerObserver Instance;

    public List<IPlayerObserver> observers = new List<IPlayerObserver>();

    [SerializeField] private PlayerHUDController playerHUDController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        RegisterObserver(playerHUDController);
        NotifyObservers();
    }


    public void NotifyObservers()
    {
        foreach (var observer in observers) 
        {
            observer.Notify();
        }
    }

    public void RegisterObserver(IPlayerObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IPlayerObserver observer)
    {
        observers.Remove(observer);
    }
}
