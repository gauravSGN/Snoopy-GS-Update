using System;

public interface BubbleQueue
{
    int ExtrasCount { get; }

    BubbleType GetNext();
    BubbleType Peek(int index);
    void Rotate(int count);

    void AddListener(Action<Observable> action);
    void RemoveListener(Action<Observable> action);
    void NotifyListeners();

    void SwitchToExtras();
}
