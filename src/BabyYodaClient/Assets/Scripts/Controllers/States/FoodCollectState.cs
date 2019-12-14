using System;
using System.Linq;
using UnityEngine;

public class FoodCollectState : CreatureState
{
    private readonly ObjectRegistry objRegistry;
    private readonly PickupWithForce pickupWithForce;
    private readonly PickupWithHands pickupWithHands;
    private CollectBehaviour collectBehaviour;
    private Consumable targetFood;
    private bool collectingFood;
    public FoodCollectState(CreatureController creature, ObjectRegistry objRegistry) : base(creature)
    {
        this.objRegistry = objRegistry;
        pickupWithForce = new PickupWithForce(this, creature);
        pickupWithHands = new PickupWithHands(this, creature);
        collectBehaviour = pickupWithHands;

        RegisterEvent("OnCollect", OnCollect);//() => collectBehaviour.OnCollect(targetFood));
        RegisterEvent("OnConsume", OnConsume);
        RegisterEvent("OnComplete", OnComplete);
    }

    public override bool IsCompleted =>
        !Creature.IsHungry || objRegistry.GetAvailableFood().Count == 0 || !collectingFood;

    private void OnConsume()
    {
        if (targetFood.Spawner != null)
        {
            // add affection to spawner §
        }

        objRegistry.RemoveFood(targetFood);

        // remove from hunger
        Creature.Hunger = Mathf.Max(0f, Creature.Hunger - 0.15f);

        if (!Creature.IsHungry)
        {
            Creature.Game.Announce("Baby Yoda is no longer hungry.");
            if (collectBehaviour.ItemInHand != null)
            {
                collectBehaviour.DropItem();
            }
        }

        targetFood = null;
        collectingFood = false;

        collectBehaviour.Reset();
        SelectNextBehaviour();
    }

    private void OnCollect()
    {
        collectBehaviour.OnCollect(targetFood);
    }

    protected override void OnEnter()
    {
        collectingFood = true;
        collectBehaviour.SetReady();
    }

    protected override void OnExit()
    {
        collectingFood = false;
    }

    protected override void OnUpdate()
    {
        if (!Creature.IsHungry)
        {
            if (collectBehaviour.ItemInHand != null)
            {
                collectBehaviour.DropItem();
            }
            return;
        }

        if (collectBehaviour.ItemInHand == null)
        {
            targetFood = GetNextFood();

            if (!targetFood)
                return;

            if (Vector3.Distance(Creature.transform.position, targetFood.transform.position) > 2.5f)
            {
                // baby yoda is small, lets not make him run super far to fetch
                // the apples. Let him use the force instead.
                collectBehaviour = pickupWithForce;
            }
        }

        if (targetFood)
        {
            collectBehaviour.Update(targetFood);
        }
    }
    private void SelectNextBehaviour()
    {
        CollectBehaviour newBehaviour = pickupWithHands;
        if (UnityEngine.Random.value >= 0.5f)
        {
            newBehaviour = pickupWithForce;
        }

        if (newBehaviour != collectBehaviour)
        {
            collectBehaviour = newBehaviour;
        }
    }

    private void OnComplete()
    {
        collectBehaviour.SetReady();
    }

    private Consumable GetNextFood()
    {
        var foods = objRegistry.GetAvailableFood();
        if (foods.Count == 0) return null;
        return foods.OrderBy(x =>
            Vector3.Distance(x.transform.position, Creature.transform.position))
            .FirstOrDefault();
    }
}

