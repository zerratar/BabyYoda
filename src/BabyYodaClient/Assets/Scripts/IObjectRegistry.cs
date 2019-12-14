using System.Collections.Generic;

internal interface IObjectRegistry
{
    void AddFood(Consumable consumable);
    void RemoveFood(Consumable consumable);
    IReadOnlyList<Consumable> GetAvailableFood();
}