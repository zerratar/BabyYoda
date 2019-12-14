using System;
using UnityEngine;

public abstract class CollectBehaviour
{
    protected readonly CreatureState State;
    protected readonly CreatureController Creature;
    public ICollectable ItemInHand { get; protected set; }
    protected bool IsReady { get; private set; } = true;

    protected CollectBehaviour(CreatureState state, CreatureController creature)
    {
        State = state;
        Creature = creature;
    }
    public void OnCollect(ICollectable obj)
    {
        obj.Disable();
        obj.transform.SetParent(Creature.rightHandTransform);
    }

    internal void Update(ICollectable collectable)
    {
        if (collectable == null)
        {
            return;
        }

        if (!IsReady)
        {
            return;
        }

        OnUpdate(collectable);
    }

    protected abstract void OnUpdate(ICollectable collectable);

    internal void Reset()
    {
        ItemInHand = null;
    }

    internal void SetReady()
    {
        IsReady = true;
    }

    internal void DropItem()
    {
        ItemInHand.Enable();
    }
}

