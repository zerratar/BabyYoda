using UnityEngine;

public class IdleState : CreatureState
{
    private Vector3 targetPosition;
    private float idleTime;
    private float timeBeforeSitting = 30f;

    public IdleState(CreatureController creature) : base(creature)
    {
    }

    public override bool IsCompleted =>
        InPosition && Creature.transform.forward == Vector3.back;

    private bool InPosition => Creature.transform.position.x == targetPosition.x;

    protected override void OnEnter()
    {
        //const float range = 3f;
        //targetPosition = Vector3.Lerp(Vector3.left * range, Vector3.right * range, UnityEngine.Random.value);
        targetPosition = Vector3.zero;
    }

    protected override void OnExit()
    {
    }

    protected override void OnUpdate()
    {
        if (IsCompleted)
        {
            Creature.StopMovement();

            idleTime += Time.deltaTime;
            if (idleTime >= timeBeforeSitting)
            {
                idleTime = 0f;
                Creature.SitDown();
            }

            return;
        }

        if (!InPosition)
        {
            if (Creature.transform.position.x < 0)
            {
                if (!Creature.FaceDirection(Vector3.right))
                    return;
            }
            else
            {
                if (!Creature.FaceDirection(Vector3.left))
                    return;
            }

            Creature.MoveTo(targetPosition);
            return;
        }

        if (!Creature.FaceDirection(Vector3.back))
            return;
    }
}

