using System;

namespace Pendulum.CQS.Extensions.ServiceProvider
{
    public interface IPendulumServiceProvider
    {
        TService GetService<TService>();
    }
}