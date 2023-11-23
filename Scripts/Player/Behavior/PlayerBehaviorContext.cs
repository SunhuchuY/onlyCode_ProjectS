using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerBehaviorContext : MonoBehaviour
{
    private IPlayerBehaviorState CurrentState;

    public void BehaviorStart()
    {
        if (CurrentState == null)
            return;

        CurrentState.Handle();
    }

    public void Transition(IPlayerBehaviorState state)
    {
        CurrentState = state;
    }
}
