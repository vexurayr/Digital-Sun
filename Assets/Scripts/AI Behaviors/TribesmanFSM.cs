using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribesmanFSM : AIController
{
    [SerializeField] private InventoryItem spear;

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
                Debug.Log("Idle");
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
                    Seek(target, attackDistance);
                }
                Debug.Log("Chase");
                // Check state transitions
                if (!IsDistanceLessThan(target, eyesightDistance))
                {
                    ChangeState(AIState.Idle);
                }
                else if (GetComponent<Health>().GetCurrentValue() / GetComponent<Health>().GetMaxValue() <= percentHealthToFlee)
                {
                    ChangeState(AIState.Flee);
                }
                else if (IsDistanceLessThan(target, attackDistance))
                {
                    ChangeState(AIState.SeekAndAttack);
                }
                /* AI has been in this state for secondsToAttackPlayer amount of time
                else if (lastTimeStateChanged <= Time.time - secondsToAttackPlayer)
                {
                    ChangeState(AIState.SeekAndAttack);
                }
                */

                break;
            case AIState.SeekAndAttack:
                if (target == null)
                {
                    ChangeState(AIState.Idle);
                }
                else
                {
                    SeekAndAttack();
                }
                Debug.Log("Seek and attack");
                if (!IsDistanceLessThan(target, attackDistance))
                {
                    ChangeState(AIState.Idle);
                }

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
                Debug.Log("Flee");
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

    public override void Attack()
    {
        Debug.Log("Attacking with spear");
        spear.PrimaryAction(gameObject);
    }
}