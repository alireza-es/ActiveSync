using System;
using Autofac;

namespace ActiveSync.Core.Injection
{
    public static class ServiceResolver
    {
        public static T GetService<T>()
        {
            //TODO: Use your own ContainerBuilder
            //return ContainerBuilder.Container.Build<T>();


            throw new NotImplementedException();
        }
    }
}
