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
                .RegisterSingleton<MainSimulationDriver>()
                .RegisterSingleton<RTDriver.RTDriver>();

            var simulationDriver = container.Resolve<MainSimulationDriver>();
            var rtDriver = container.Resolve<RTDriver.RTDriver>();

            container
                .RegisterSingleton<ScadaConfiguration>(
                    new InjectionConstructor(
                            @"../../scadaConfig.xml",
                            simulationDriver,
                            rtDriver
                        )
                )
                .RegisterSingleton<TagRepository>()
                .RegisterSingleton<TagProcessor>()
                .RegisterSingleton<TagService>()
                .RegisterType<TagContext>(new HierarchicalLifetimeManager())
                .RegisterType<RTUnitContext>()
                .RegisterType<ITrendingService, TrendingService>()
                .RegisterType<IRTUnitService, RTUnitService>(
                    new InjectionConstructor(
                            rtDriver
                        )
                );
        }
    }
}