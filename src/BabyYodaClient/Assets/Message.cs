using System.Collections.Concurrent;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI label;

    private readonly ConcurrentQueue<MessageItem> messageQueue
        = new ConcurrentQueue<MessageItem>();

    private MessageItem activeMessage;
    private float visibleTime;

    // Update is called once per frame
    void Update()
    {
        if (activeMessage == null && messageQueue.TryDequeue(out activeMessage))
        {
            visibleTime = 0;
        }

        if (activeMessage == null)
        {
            label.gameObject.SetActivePerf(false);
            return;
        }

        visibleTime += Time.deltaTime;
        if (visibleTime >= activeMessage.Duration)
        {
            activeMessage = null;
            return;
        }

        label.SetText(activeMessage.Text);
        label.gameObject.SetActivePerf(true);
    }

    public void DisplayWelcomeMessage(string name, float duration)
    {
        messageQueue.Enqueue(new MessageItem($"Welcome <b>{name}</b>!", duration));
    }

    private class MessageItem
    {
        public MessageItem(string text, float duration)
        {
            Text = text;
            Duration = duration;
        }

        public string Text { get; }
        public float Duration { get; }
    }
}

public static class UIExtensions
{
    public static void SetActivePerf(this GameObject obj, bool value)
    {
        if (!obj || obj == null) return;
        if (obj.activeSelf == value) return;
        obj.SetActive(value);
    }
}
