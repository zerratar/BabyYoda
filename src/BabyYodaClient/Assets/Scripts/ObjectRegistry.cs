using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectRegistry : MonoBehaviour, IObjectRegistry
{
    private readonly List<Consumable> foods = new List<Consumable>();

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<IoCContainer>().Container.RegisterCustomShared<IObjectRegistry>(() => this);
    }

    public void AddFood(Consumable consumable)
    {
        foods.Add(consumable);
    }

    public void RemoveFood(Consumable consumable)
    {
        foods.Remove(consumable);
        Destroy(consumable.gameObject);
    }

    public IReadOnlyList<Consumable> GetAvailableFood()
    {
        return foods.Where(x => x != null && x).ToList(); // make a copy
    }
}
