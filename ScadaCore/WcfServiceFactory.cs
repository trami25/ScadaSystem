using DriverApi;
using RTDriver;
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
                .RegisterType<IRTUnitService, RTUnitService>()
                .RegisterSingleton<TagRepository>()
                .RegisterType<IDriver, MainSimulationDriver>("SimulationDriver")
                .RegisterType<IDriver, RTDriver.RTDriver>("RTDriver")
                .RegisterSingleton<ScadaConfiguration>(
                    new InjectionConstructor(
                            @"../../scadaConfig.xml",
                            new ResolvedParameter<IDriver>("SimulationDriver"),
                            new ResolvedParameter<IDriver>("RTDriver")
                        )
                )
                .RegisterSingleton<TagProcessor>()
                .RegisterSingleton<TagService>()
                .RegisterType<TagContext>(new HierarchicalLifetimeManager());
        }
    }
}