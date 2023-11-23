using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public abstract class IPlayerBehaviorState
{
    public virtual void Handle() { }
}

public class BasicAttackCombo_Cancle : IPlayerBehaviorState
{
    public override void Handle()
    {
        PlayerManager.instance.animator.SetBool("BasicAttackCombo", false); 
    }
}

public class BasicAttackCombo_Execute : IPlayerBehaviorState
{
    public override void Handle()
    {
        PlayerManager.instance.animator.SetBool("BasicAttackCombo", true);
    }
}


public class PlayerBehaviorState : MonoBehaviour
{
    
}
