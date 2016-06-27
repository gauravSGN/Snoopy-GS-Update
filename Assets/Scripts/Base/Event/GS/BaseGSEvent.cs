using Config;

public class BaseGSEvent : GameEvent
{
    public GSDescriptor Descriptor { get; private set; }

    public BaseGSEvent(GSDescriptor descriptor)
    {
        Descriptor = descriptor;
    }
}