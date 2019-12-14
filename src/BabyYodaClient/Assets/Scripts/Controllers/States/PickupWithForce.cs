using System;
using System.Collections;
using UnityEngine;

public class PickupWithForce : CollectBehaviour
{
    private bool movingTarget;

    public PickupWithForce(
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
        if (distance <= Creature.ForceReachRadius)
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

        if (movingTarget)
            return;

        ItemInHand = collectable;
        Creature.Animator.SetInteger("CollectType", 1);
        Creature.Animator.SetTrigger("Collect");

        MoveTargetItem(collectable);
    }

    private void MoveTargetItem(ICollectable collectable)
    {
        Creature.StartCoroutine(MoveItem(collectable));
    }

    private IEnumerator MoveItem(ICollectable collectable)
    {
        movingTarget = true;
        yield return new WaitForSeconds(0.3f);
        var moveTimer = 0f;
        var moveDuration = 0.43f;
        var startPosition = collectable.transform.position;
        while (true)
        {
            moveTimer += Time.deltaTime;
            var progress = moveTimer / moveDuration;
            collectable.transform.position = Vector3.Lerp(startPosition, Creature.rightHandTransform.position, progress);
            if (progress >= 1.0f)
            {
                collectable.transform.position = Creature.rightHandTransform.position;
                break;
            }

            yield return null;
        }

        collectable.transform.SetParent(Creature.rightHandTransform);
        collectable.transform.localPosition = Vector3.zero;
        movingTarget = false;
    }
}

