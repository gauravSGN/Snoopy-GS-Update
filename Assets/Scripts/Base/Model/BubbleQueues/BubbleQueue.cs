using System;

public interface BubbleQueue
{
    BubbleType GetNext();
    BubbleType Peek(int index);
    void Rotate(int count);
    BubbleType GenerateElement();
    void BuildQueue();
    void RemoveInactiveTypes();

    void AddListener(Action<Observable> action);
    void RemoveListener(Action<Observable> action);
    void NotifyListeners();
}