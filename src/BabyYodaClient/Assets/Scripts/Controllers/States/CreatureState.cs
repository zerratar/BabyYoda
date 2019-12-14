using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public abstract class CreatureState
{
    private readonly Dictionary<string, Action> eventCallback = new Dictionary<string, Action>();

    protected readonly CreatureController Creature;

    public abstract bool IsCompleted { get; }

    protected CreatureState(CreatureController creature)
    {
        Creature = creature;
    }

    public void Update()
    {
        OnUpdate();
    }
    public void Enter()
    {
        OnEnter();
    }
    public void Exit()
    {
        OnExit();
    }

    protected abstract void OnUpdate();
    protected abstract void OnEnter();
    protected abstract void OnExit();

    public void OnEvent([CallerMemberName] string eventName = null)
    {
        if (eventCallback.TryGetValue(eventName, out var action))
        {
            action();
        }
    }

    protected void RegisterEvent(string name, Action action)
    {
        eventCallback[name] = action;
    }
}

