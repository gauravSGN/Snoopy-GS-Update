using System;

public interface BubbleQueue
{
    BubbleType GetNext();
    BubbleType Peek(int index);
    void Rotate(int count);

    void AddListener(Action<Observable> action);
    void RemoveListener(Action<Observable> action);
    void NotifyListeners();
}