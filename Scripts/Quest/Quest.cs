using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] private string[] _Conversation;

    public string[] Conversation { get { return _Conversation; } }
}
