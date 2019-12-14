public class PlayState : CreatureState
{
    public PlayState(CreatureController creature) : base(creature)
    {
    }

    public override bool IsCompleted => true;

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override void OnUpdate()
    {
    }
}

