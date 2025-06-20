using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTests
{
    public abstract class Di
    {
        private IServiceProvider? _serviceProvider;
        protected abstract IServiceCollection SutServices { get; }
        protected IServiceProvider ServicesProvider =>
            _serviceProvider ??= SutServices.BuildServiceProvider();
    }
}
