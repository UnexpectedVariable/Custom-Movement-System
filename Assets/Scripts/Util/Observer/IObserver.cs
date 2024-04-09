namespace Assets.Scripts.Util.Observer
{
    public interface IObserver<T>
    {
        void Handle(T observed);
    }
}
