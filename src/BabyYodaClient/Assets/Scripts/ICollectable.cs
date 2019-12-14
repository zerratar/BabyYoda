using Assets.Scripts.Models;
using UnityEngine;

public interface ICollectable
{
    Viewer Spawner { get; }
    Transform transform { get; }
    void Disable();
    void Enable();
}