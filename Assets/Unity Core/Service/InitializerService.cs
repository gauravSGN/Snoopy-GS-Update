namespace Service
{
    public interface InitializerService : SharedService
    {
        void Register(Initializable target, params Initializable[] dependencies);
    }
}
