using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterAI
{
    void Attack();
    void Idle();
    void Detect();
    void Move();
}

public class MonsterAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
