using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Quest Data", menuName = "Scriptable Object/Quest Data", order = int.MaxValue)]
public class QuestData : ScriptableObject
{
    [SerializeField] private string[] _Conversation;

    public string[] Conversation { get{ return _Conversation; } }


}
