using DriverApi;
using ScadaCore.Configuration;
using ScadaCore.Tags;
using SimulationDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Resolution;
using Unity.Wcf;

namespace ScadaCore
{
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container
                .AddExtension(new Diagnostic())
                .RegisterType<ITrendingService, TrendingService>()
                .RegisterSingleton<TagRepository>()
                .RegisterSingleton<ScadaConfiguration>()
                .RegisterType<IDriver, MainSimulationDriver>()
                .RegisterType<TagContext>(new HierarchicalLifetimeManager());

            container
                .Resolve<ScadaConfiguration>(new ParameterOverride("configPath", @"../../scadaConfig.xml"));
        }
    }
}