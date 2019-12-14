using Assets.Scripts.Models;
using System.Collections.Generic;

internal interface IViewerManager
{
    bool Contains(string userId);
    IReadOnlyList<Viewer> GetAllViewers();
    IReadOnlyList<Viewer> GetAllModerators();
    Viewer GetViewer(TwitchUser viewer);
    Viewer GetViewerByTwitchUserId(string userId);
    Viewer GetViewerByName(string playerName);
    int GetViewerCount();
    Viewer GetViewerByIndex(int index);
    void Remove(Viewer player);
    Viewer Add(TwitchUser user);
    IReadOnlyList<Viewer> FindPlayers(string query);
}