using System;
using System.Collections.Generic;
using Ninject;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

namespace SSExec.Button.Core
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;

            AddBindings();
        }

        private void AddBindings()
        {
            _kernel.Bind<ILog>().To<Log>();
            _kernel.Bind<ProcessHelper>().ToSelf();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}