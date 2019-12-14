using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewerManager : MonoBehaviour, IViewerManager
{
    private readonly List<Viewer> activeViewers = new List<Viewer>();
    private readonly object mutex = new object();

    [SerializeField] private GameSettings settings;
    [SerializeField] private IoCContainer ioc;

    void Start()
    {
        if (!settings) settings = GetComponent<GameSettings>();
        if (!ioc) ioc = GetComponent<IoCContainer>();
        ioc.Container.RegisterCustomShared<IViewerManager>(() => this);
    }

    public bool Contains(string userId)
    {
        return GetViewerByTwitchUserId(userId) != null;
    }

    public IReadOnlyList<Viewer> GetAllViewers()
    {
        return activeViewers;
    }

    public IReadOnlyList<Viewer> GetAllModerators()
    {
        return activeViewers.Where(x => x.User.IsModerator).ToList();
    }

    public Viewer GetViewer(TwitchUser viewer)
    {
        var player = GetViewerByName(viewer.Username) ?? GetViewerByTwitchUserId(viewer.UserId);
        if (player == null) player = Add(viewer);
        return player;
    }

    public Viewer GetViewerByTwitchUserId(string userId)
    {
        lock (mutex)
        {
            return activeViewers.FirstOrDefault(x =>
                x.User.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public Viewer GetViewerByName(string playerName)
    {
        playerName = playerName.StartsWith("@") ? playerName.Substring(1) : playerName;
        lock (mutex)
        {
            return activeViewers.FirstOrDefault(x =>
            x.User.Username.Equals(playerName, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public int GetViewerCount()
    {
        lock (mutex)
            return activeViewers?.Count ?? 0;
    }

    public Viewer GetViewerByIndex(int index)
    {
        lock (mutex)
        {
            if (activeViewers == null || activeViewers.Count <= index)
            {
                return null;
            }

            return activeViewers[index];
        }
    }

    public void Remove(Viewer player)
    {
        lock (mutex)
        {
            if (!activeViewers.Contains(player))
            {
                return;
            }

            activeViewers.Remove(player);
        }
    }

    public Viewer Add(TwitchUser user)
    {
        lock (mutex)
        {
            var viewer = activeViewers.FirstOrDefault(x => x.User.UserId == user.UserId);
            if (viewer != null)
            {
                return viewer;
            }

            viewer = new Viewer(user);
            activeViewers.Add(viewer);
            return viewer;
        }
    }

    public IReadOnlyList<Viewer> FindPlayers(string query)
    {
        lock (mutex)
            return activeViewers.Where(x => x.User.Username.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
    }
}