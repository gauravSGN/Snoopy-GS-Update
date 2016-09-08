public interface UpdateReceiverList<in T>
{
    void Add(T target);
    void Remove(T target);
}
