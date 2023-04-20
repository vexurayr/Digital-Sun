using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribesmanFSM : AIController
{
    protected override void Start()
    {
        base.Start();

        StartingState();
    }

    private void Update()
    {
        if (target == null)
        {
            TargetPlayer();
            return;
        }

        if (!IsDistanceLessThan(GameManager.instance.GetCurrentPlayerController().gameObject, distanceFromPlayerToDisable))
        {
            return;
        }

        MakeDecisions();
    }

    public void StartingState()
    {
        ChangeState(startState);
    }

    public void MakeDecisions()
    {
        switch (currentState)
        {
            case AIState.Idle:
                // Does the actions of the state
                Idle();

                TargetPlayer();

                // Check for transitions
                if (IsDistanceLessThan(target, eyesightDistance))
                {
                    ChangeState(AIState.Chase);
                }
                else if (GetComponent<Health>().GetCurrentValue() / GetComponent<Health>().GetMaxValue() <= percentHealthToFlee)
                {
                    ChangeState(AIState.Flee);
                }

                break;
            case AIState.Chase:
                // Do state actions
                if (target == null)
                {
                    ChangeState(AIState.Idle);
                }
                else
                {
                    Seek(target);
                }

                // Check state transitions
                if (!IsDistanceLessThan(target, eyesightDistance))
                {
                    ChangeState(AIState.Idle);
                }
                else if (GetComponent<Health>().GetCurrentValue() / GetComponent<Health>().GetMaxValue() <= percentHealthToFlee)
                {
                    ChangeState(AIState.Flee);
                }
                /* AI has been in this state for secondsToAttackPlayer amount of time
                else if (lastTimeStateChanged <= Time.time - secondsToAttackPlayer)
                {
                    ChangeState(AIState.SeekAndAttack);
                }
                */

                break;
            case AIState.Flee:
                if (target == null)
                {
                    ChangeState(AIState.Idle);
                }
                else
                {
                    Flee();
                }

                if (!IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Idle);
                }

                break;
            default:
                Debug.LogError("The switch could not determine the current state.");

                break;
        }
    }
}