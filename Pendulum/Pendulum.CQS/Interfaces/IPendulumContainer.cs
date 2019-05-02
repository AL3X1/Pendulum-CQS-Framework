using System.Reflection;

namespace Pendulum.CQS.Interfaces
{
    public interface IPendulumContainer
    {
        void Populate(Assembly assembly);
    }
}