
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Lifetime;
using Unity;

namespace EmployeeMgmt
{
    public static class Factory
    {
        private static IUnityContainer _container;
        static Factory()
        {
            _container = new UnityContainer();

            _container.RegisterType<IEmployeeRepository, WebAPI>(
                new ContainerControlledLifetimeManager());
        }
        public static IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
