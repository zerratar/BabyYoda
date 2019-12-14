using UnityEngine;

public class PickupWithHands : CollectBehaviour
{
    public PickupWithHands(
        CreatureState state, CreatureController creature)
        : base(state, creature)
    {
    }

    protected override void OnUpdate(ICollectable collectable)
    {
        if (ItemInHand != null) return;

        if (collectable.transform.position.x < Creature.transform.position.x)
        {
            if (!Creature.FaceDirection(Vector3.left))
                return;
        }
        else
        {
            if (!Creature.FaceDirection(Vector3.right))
                return;
        }

        var targetPosition = collectable.transform.position;
        var distance = Vector3.Distance(targetPosition, Creature.transform.position);
        if (distance <= Creature.HandReachRadius)
        {
            if (Creature.AnimState != AnimationState.Standing)
                return;

            Creature.StopMovement();
            if (collectable.transform.position.y < 0.185) // just ensure it falls to the ground
                CollectAndEatFood(collectable);
        }
        else
        {
            Creature.MoveTo(collectable.transform.position);
        }
    }

    private void CollectAndEatFood(ICollectable collectable)
    {
        collectable.Disable();
        ItemInHand = collectable;
        Creature.Animator.SetInteger("CollectType", 0);
        Creature.Animator.SetTrigger("Collect");
    }
}

