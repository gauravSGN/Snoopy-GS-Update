namespace Service
{
    public interface UpdateService : SharedService
    {
        UpdateReceiverList<UpdateReceiver> Updates { get; }
        UpdateReceiverList<LateUpdateReceiver> LateUpdates { get; }
        UpdateReceiverList<FixedUpdateReceiver> FixedUpdates { get; }

        void Reset();
    }
}
