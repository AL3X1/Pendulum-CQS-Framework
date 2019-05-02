using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Pendulum.CQS.Extensions.ServiceProvider
{
    public class PendulumServiceProvider : IPendulumServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        
        public PendulumServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ??
                               throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TService GetService<TService>()
        {
            TService service = _serviceProvider.GetService<TService>();

            if (service == null)
            {
                Type serviceType = typeof(TService);
                
                string clearServiceName = GetClearServiceName(serviceType);
                string errorMessage = $"Unable to get service with name {clearServiceName}";

                Type[] serviceGenericArgs = serviceType.GetGenericArguments();
                
                if (serviceGenericArgs.Any())
                {
                    errorMessage += " and generic arguments ";
                    
                    foreach (var arg in serviceGenericArgs)
                    {
                        errorMessage += arg.Name + ",";
                    }

                    errorMessage = errorMessage.Remove(errorMessage.Length - 1);
                }
                
                throw new Exception(errorMessage);
            }

            return service;
        }

        private string GetClearServiceName(MemberInfo serviceType)
        {
            string serviceName = serviceType.Name;

            if (serviceType.Name.Contains("`"))
            {
                serviceName = serviceName.Substring(0, serviceName.IndexOf("`", StringComparison.Ordinal));
            }

            return serviceName;
        }
    }
}