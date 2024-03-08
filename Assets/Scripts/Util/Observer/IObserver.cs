namespace Assets.Scripts.Util.Observer
{
    public interface IObserver<T>
    {
        public void Handle(T observed);
    }
}
