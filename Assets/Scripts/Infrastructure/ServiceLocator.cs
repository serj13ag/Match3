using System;
using System.Collections.Generic;
using Services;

namespace Infrastructure
{
    public class ServiceLocator
    {
        private readonly Dictionary<Type, IService> _services;

        public static ServiceLocator Instance { get; private set; }

        public ServiceLocator()
        {
            _services = new Dictionary<Type, IService>();

            Instance = this;
        }

        public void Register<TService>(TService service) where TService : IService
        {
            _services.Add(typeof(TService), service);
        }

        public TService Get<TService>() where TService : IService
        {
            return (TService)_services[typeof(TService)];
        }
    }
}