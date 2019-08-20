namespace Pendulum.CQS.Interfaces
{
    public interface IPendulumServiceProvider
    {
        TService GetService<TService>();
    }
}