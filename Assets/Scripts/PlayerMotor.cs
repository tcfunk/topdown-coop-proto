using MLAPI;
using MLAPI.Prototyping;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : NetworkBehaviour
{
    public bool useAgentSpeed = true;
    public bool useDeltaTime = true;
    public float movementSmoothing = 0.3f;
    
    private NavMeshAgent agent;
    private Interactable target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (target)
        {
            agent.SetDestination(target.interactionTransform.position);
            FaceTarget();
        }
    }

    public void MoveToPoint(Vector3 destination)
    {
        agent.updatePosition = true;
        agent.SetDestination(destination);
    }

    public void MoveInDirection(Vector3 direction)
    {
        var movement = direction;
        if (useAgentSpeed) movement *= agent.speed;
        if (useDeltaTime) movement *= Time.deltaTime;
        
        if (NavMesh.SamplePosition(transform.position + movement, out var navMeshHit, 100f, NavMesh.AllAreas))
        {
            agent.SetDestination(navMeshHit.position);
        }
    }

    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius;
        agent.updateRotation = false;
        target = newTarget;
    }

    public void StopFollowingTarget()
    {
        target = null;
        agent.updateRotation = true;
        agent.stoppingDistance = 0;
    }

    void FaceTarget()
    {
        var direction = (target.interactionTransform.position - transform.position).normalized;

        if (direction.magnitude >= 0.1f)
        {
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
