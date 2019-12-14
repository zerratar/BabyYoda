using Assets.Scripts.Models;

public class ViewerAffection
{
    private float affection;
    public float Affection => affection;
    public bool Greeted { get; set; }
    public void AddAffection(float value)
    {
        affection += value;
    }
}

