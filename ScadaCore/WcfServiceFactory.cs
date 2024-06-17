using ScadaCore.Configuration;
using ScadaCore.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using Unity.Lifetime;
using Unity.Wcf;

namespace ScadaCore
{
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container
                .RegisterType<ITrendingService, TrendingService>()
                .RegisterSingleton<TagRepository>()
                .RegisterSingleton<ScadaConfiguration>()
                .RegisterType<TagContext>(new HierarchicalLifetimeManager());
        }
    }
}