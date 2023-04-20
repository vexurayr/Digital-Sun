using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIController : MonoBehaviour
{
    #region Variables
    // All potential states in the finite state machine
    public enum AIState
    {
        Idle,
        RandomMovement,
        Chase,
        Flee,
        FaceNoise,
        SeekNoise,
        SeekAndAttack,
        DistanceAttack
    };

    // Set stats like how far away the AI can see or how close they need to be to attack
    [SerializeField] protected float aIFOV;
    [SerializeField] protected float eyesightDistance;
    [SerializeField] protected float earshotDistance;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float fleeDistance;
    [Range(0f, 1f)][SerializeField] protected float percentHealthToFlee;
    [Tooltip("Leave 0 to have no restriction.")][SerializeField] protected float maxDistanceFromSpawn;
    [SerializeField] protected float timeToAgro;
    [SerializeField] protected float distanceFromPlayerToDisable;
    [SerializeField] protected GameObject raycastLocation;
    [SerializeField] protected AIState startState;

    // Last time the AI changed states
    protected float lastTimeStateChanged;

    // State the fsm is currently in
    protected AIState currentState;

    // AI controller's target, likely the player
    protected GameObject target;

    // Used for random movement & random observe
    protected Vector3 randomLocation;

    // Used for Face Noise and Seek Noise
    protected GameObject noiseLocation;

    // Value between 0 and timeToAgro
    protected float currentAlertness;

    private NavMeshAgent navMeshAgent;

    #endregion Variables

    #region MonoBehaviours
    protected virtual void Start()
    {
        noiseLocation = new GameObject();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    #endregion MonoBehaviours

    #region Transitions
    // Option to override later for AI tanks with different personalities
    public virtual void ChangeState(AIState newState)
    {
        // Change current state
        currentState = newState;

        // Saves the time this state changed
        lastTimeStateChanged = Time.time;
    }

    #endregion Transitions

    #region States
    public void Idle()
    { }

    // Polymorphism at its finest
    public void Seek(Vector3 targetVector)
    {
        navMeshAgent.destination = targetVector;
    }

    public void Seek(Transform targetTransform)
    {
        Seek(targetTransform.position);
    }

    public void Seek(GameObject target)
    {
        Seek(target.transform.position);
    }

    public void Flee()
    {
        // Gets vector to target
        Vector3 vectorToTarget = target.transform.position - gameObject.transform.position;

        // Calculates vector away from target (opposite direction of going to target)
        Vector3 vectorAwayFromTarget = -vectorToTarget;

        // Find how far away the AI will travel
        // Compare how close the player is at the beginning of the flee to always be the flee distance away from the player
        // at the end of the flee
        float targetDistance = Vector3.Distance(target.transform.position, gameObject.transform.position);
        float percentOfFleeDistance = targetDistance / fleeDistance;

        // Clamps it between 0 and 1
        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);
        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;

        Vector3 fleeVector = vectorAwayFromTarget.normalized * flippedPercentOfFleeDistance;

        // Seek the point the AI needs to flee to
        Seek(gameObject.transform.position + fleeVector);
    }

    // Will need to be changed based on the AI pawn
    public virtual void Attack()
    {}

    // Virtual so that other scripts can override this for AI personalities
    public virtual void SeekAndAttack()
    {
        // Continually chases and shoots at target
        Seek(target);

        // Only attacks if in range and is lined up with the player
        RaycastHit targetToHit;
        Ray rayToTarget;

        if (!raycastLocation)
        {
            rayToTarget = new Ray(gameObject.transform.position, gameObject.transform.forward);
        }
        else
        {
            rayToTarget = new Ray(raycastLocation.transform.position, raycastLocation.transform.forward);
        }

        if (Physics.Raycast(rayToTarget, out targetToHit, attackDistance))
        {
            // Ray hit the player
            if (targetToHit.collider == target.GetComponent<Collider>())
            {
                Attack();
            }
        }
    }

    public void RandomMovement()
    {
        // This will prevent this tank from constantly looking for a new position to travel to
        if (HasReachedRandomLocation())
        {
            randomLocation = GetRandomDirectionInFrontOfSelf();
        }

        Seek(randomLocation);
    }

    // Will have to be changed based on the AI pawn
    public void FaceNoise()
    { }

    public void SeekNoise()
    {
        // This will prevent the agent from moving when it reaches the source of the noise
        if (!IsDistanceLessThan(noiseLocation, 1))
        {
            navMeshAgent.destination = noiseLocation.transform.position;
        }
    }

    public void DistanceAttack()
    {
        // This agent will always try to remain attack distance away from the player as it attacks
        // +/- 1 so that the agent isn't jittery while trying to remain in one place
        if (IsDistanceLessThan(target, attackDistance - 1))
        {
            
        }
        else if (!IsDistanceLessThan(target, attackDistance + 1))
        {
            
        }

        // Only attacks if in range and is lined up with the player
        RaycastHit targetToHit;
        Ray rayToTarget;

        if (!raycastLocation)
        {
            rayToTarget = new Ray(gameObject.transform.position, gameObject.transform.forward);
        }
        else
        {
            rayToTarget = new Ray(raycastLocation.transform.position, raycastLocation.transform.forward);
        }

        if (Physics.Raycast(rayToTarget, out targetToHit, attackDistance))
        {
            // Ray hit the player
            if (targetToHit.collider == target.GetComponent<Collider>())
            {
                Attack();
            }
        }
    }

    #endregion States

    #region Targeting Options
    public virtual void Target(GameObject newTarget)
    {
        target = newTarget;
    }

    public virtual void TargetPlayer()
    {
        // Must be instance of game manager and list of players, and have players in the list
        if (GameManager.instance != null)
        {
            if (GameManager.instance.GetCurrentPlayerController() != null)
            {
                Target(GameManager.instance.GetCurrentPlayerController().gameObject);
            }
        }
    }

    #endregion Targeting Options

    #region Other Methods
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public Vector3 GetRandomDirectionInFrontOfSelf()
    {
        // This will pick a direction away from the player on the x and z axis
        Vector3 targetPosition = gameObject.transform.position;
        float distance = Random.Range(4, 8);
        targetPosition = targetPosition + gameObject.transform.forward * distance;

        return targetPosition;
    }

    #endregion Other Methods

    #region HelperFunctions
    public bool IsDistanceLessThan(Vector3 target, float distance)
    {
        // Checks distance between 2 vectors
        if (target != null && Vector3.Distance(gameObject.transform.position, target) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsDistanceLessThan(GameObject target, float distance)
    {
        return IsDistanceLessThan(target.transform.position, distance);
    }

    public bool CanHearTarget()
    {
        // If the target has no instance of noise emitter, can't continue
        if (target.GetComponent<NoiseEmitter>() == null)
        {
            return false;
        }

        // Check if target is making no noise
        if (target.GetComponent<NoiseEmitter>().GetCurrentNoiseDistance() == 0)
        {
            return false;
        }

        // If distance between self and target is less than the distance of sound the target is currently emitting
        // plus the distance self can hear from, AI can hear the sound
        if (IsDistanceLessThan(target, target.GetComponent<NoiseEmitter>().GetCurrentNoiseDistance() + earshotDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSeeTarget()
    {
        if (!target)
        {
            return false;
        }

        // Gets the vector between self and target
        Vector3 selfToTargetVector = target.transform.position - gameObject.transform.position;

        // Gets angle between that vector and the direction self is facing
        float angleToTarget = Vector3.Angle(selfToTargetVector, gameObject.transform.forward);

        // Angle is less than AI POV and distance to target is less than eyesight distance
        // meaning the player is within the cone the AI can see
        if (angleToTarget < aIFOV && IsDistanceLessThan(target, eyesightDistance))
        {
            // Now must check if the player is in line of sight
            // Sends ray from self in direction of target location
            RaycastHit targetToHit;
            Vector3 rayPosition;
            if (!raycastLocation)
            {
                rayPosition = gameObject.transform.position;
            }
            else
            {
                rayPosition = raycastLocation.transform.position;
            }

            Ray rayToTarget = new Ray(rayPosition, selfToTargetVector);

            Debug.DrawRay(rayToTarget.origin, selfToTargetVector);

            // Ray is able to hit something
            if (Physics.Raycast(rayToTarget, out targetToHit, eyesightDistance))
            {
                // Check if ray hit target
                if (targetToHit.collider == target.GetComponent<Collider>())
                {
                    //Debug.Log("I can see the " + obj + "!");
                    return true;
                }
                // Ray didn't hit target
                else
                {
                    //Debug.Log("Ray didn't hit target");
                    return false;
                }
            }
            // Ray didn't hit anything
            else
            {
                //Debug.Log("Ray didn't hit anything");
                return false;
            }
        }
        // Target was not in cone of vision
        else
        {
            //Debug.Log("Target was not in cone of vision");
            return false;
        }
    }

    public bool HasReachedRandomLocation()
    {
        if (IsDistanceLessThan(randomLocation, 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion HelperFunctions
}