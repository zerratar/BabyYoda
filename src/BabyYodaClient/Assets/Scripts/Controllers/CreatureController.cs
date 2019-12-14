using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ObjectRegistry objectRegistry;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    [Header("Bone setup")]
    [SerializeField] public Transform rightHandTransform;
    [SerializeField] public Transform leftHandTransform;

    [Header("Hunger")]
    [SerializeField] private float hungerThreshold = 0.75f;
    [SerializeField] private float hungerIncreaseRate = 0.005f;

    [Header("Happiness")]
    [SerializeField] private float happinessThreshold = 0.50f;
    [SerializeField] private float happinessDecreaseRate = 0.025f;

    [Header("Sadness")]
    [SerializeField] private float sadnessHappinessThreshold = 0.1f;

    private readonly Dictionary<string, ViewerAffection> viewerAffection
        = new Dictionary<string, ViewerAffection>();

    private IdleState stateIdle;
    private PlayState statePlay;
    private FoodCollectState stateFoodCollect;
    private CreatureState currentState;
    private AnimationState animState = AnimationState.Standing;

    public float HandReachRadius = 0.75f;
    public float ForceReachRadius = 10.0f;

    public float Hunger = 0.5f;
    public float Happiness = 0f;
    private bool turning;
    public Animator Animator => animator;
    public GameManager Game => gameManager;
    public bool IsHungry => Hunger >= hungerThreshold;
    public bool IsHappy => Happiness >= happinessThreshold;
    public bool IsSad => Happiness <= sadnessHappinessThreshold;

    public AnimationState AnimState => animState;

    private float hungerAnnounceTimer = 2 * 60f;
    private float hungerAnnounceTime = 2 * 60f;

    private void Awake()
    {
        stateIdle = new IdleState(this);
        statePlay = new PlayState(this);
        stateFoodCollect = new FoodCollectState(this, objectRegistry);
        currentState = stateIdle;
    }

    internal bool Feed(Viewer viewer)
    {
        // add tiny amount of affection towards viewer
        // more will be added if the food was picked up as well later.
        AddAffection(viewer);

        Game.Creature.Wave(viewer);

        return gameManager.Spawner.SpawnFood(viewer);
    }

    internal void Groom(Viewer viewer)
    {
        AddAffection(viewer);
    }

    internal void Play(Viewer viewer)
    {
        AddAffection(viewer);
    }

    internal void Pet(Viewer viewer)
    {
        AddAffection(viewer);
    }

    internal void Wave(Viewer viewer)
    {
        var affection = GetAffection(viewer);

        if (affection?.Greeted ?? false)
            return;

        if (affection != null)
            affection.Greeted = true;

        Animator.SetTrigger("Wave");
        if (viewer != null)
            Game.Message.DisplayWelcomeMessage(viewer.User.DisplayName, 4f);
    }

    private ViewerAffection AddAffection(Viewer viewer)
    {
        if (viewer == null) return null;
        var key = viewer.User.UserId;
        if (!viewerAffection.TryGetValue(key, out var affection))
            affection = viewerAffection[key] = new ViewerAffection();
        affection.AddAffection(1f);
        return affection;
    }

    private ViewerAffection GetAffection(Viewer viewer)
    {
        if (viewer == null) return null;
        var key = viewer.User.UserId;
        viewerAffection.TryGetValue(key, out var affection);
        return affection;
    }

    private void Update()
    {
        Hunger = Mathf.Min(1f, Hunger + hungerIncreaseRate * Time.deltaTime);

        if (IsHungry)
        {
            hungerAnnounceTimer -= Time.deltaTime;
            if (hungerAnnounceTimer <= 0f)
            {
                hungerAnnounceTimer = hungerAnnounceTime;
                Game.Announce("Baby Yoda is hungry. Type !feed to give him an apple.");
            }
        }

        currentState.Update();

        if (currentState.IsCompleted)
        {
            GotoNextState();
        }
    }

    private void GotoNextState()
    {
        if (IsHungry)
        {
            EnterState(stateFoodCollect);
            currentState.Update();

            if (!currentState.IsCompleted)
            {
                return;
            }
        }

        EnterState(statePlay);
        currentState.Update();

        if (!currentState.IsCompleted)
        {
            return;
        }

        EnterState(stateIdle);
        currentState.Update();
    }

    private void EnterState(CreatureState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

    internal bool FaceDirection(Vector3 direction)
    {
        if (transform.forward != direction)
        {
            TurnTo(direction);
            return false;
        }

        return true;
    }

    private void TurnTo(Vector3 direction)
    {
        if (turning) return;
        StartCoroutine(_TurnTo(direction));
    }

    private IEnumerator _TurnTo(Vector3 direction)
    {
        turning = true;
        MoveTo(transform.position);

        var startForward = transform.forward;
        var rotateTimer = 0f;
        var timeToRotate = 1f;

        while (true)
        {
            StartMovementAnimation();
            rotateTimer += Time.deltaTime;
            var progress = rotateTimer / timeToRotate;
            transform.forward = Vector3.Slerp(startForward, direction, progress);
            if (progress >= 1.0f)
            {
                transform.forward = direction;
                break;
            }

            yield return null;
        }

        StopMovementAnimation();
        turning = false;
    }

    public void MoveTo(Vector3 position)
    {
        if (animState != AnimationState.Standing)
        {
            StandUp();
            return;
        }

        if (transform.position == position)
        {
            StopMovementAnimation();
            return;
        }

        StartMovementAnimation();
        agent.SetDestination(position);
    }

    public void StopMovement()
    {
        StopMovementAnimation();
        agent.SetDestination(transform.position);
    }

    public void SitDown()
    {
        if (animState != AnimationState.Standing)
            return;

        animState = AnimationState.SitDown;
        Animator.SetBool("Sitting", true);
    }

    public void StandUp()
    {
        if (animState != AnimationState.Sitting)
            return;

        animState = AnimationState.StandUp;
        Animator.SetBool("Sitting", false);
    }
    public void OnStand()
    {
        animState = AnimationState.Standing;
    }

    public void OnSit()
    {
        animState = AnimationState.Sitting;
    }

    public void OnCollect() => currentState.OnEvent();
    public void OnConsume() => currentState.OnEvent();
    public void OnComplete() => currentState.OnEvent();

    private void StartMovementAnimation()
    {
        animator.SetFloat("MovementSpeed", 1f);
    }
    private void StopMovementAnimation()
    {
        animator.SetFloat("MovementSpeed", 0);
    }
}
