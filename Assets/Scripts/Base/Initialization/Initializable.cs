public interface Initializable
{
    // Pre-initialization occurs before anything else.
    void PreInitialize();

    // Initialization for a given object is guaranteed to be called after Initialize has been called on all of its
    // dependencies.
    void Initialize();

    // Post-initialization occurs after all initialization and pre-initialization calls are completed.
    void PostInitialize();
}
