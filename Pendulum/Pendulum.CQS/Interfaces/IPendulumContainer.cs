using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Pendulum.CQS.Interfaces
{
    public interface IPendulumContainer
    {
        void Populate(Assembly assembly);
    }
}