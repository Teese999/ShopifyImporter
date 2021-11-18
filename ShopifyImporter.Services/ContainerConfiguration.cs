﻿using ShopifyImporter.Integrations.MicrosoftGraph;
using ShopifyImporter.Integrations.MicrosoftOneDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace ShopifyImporter.Services
{
    public class ContainerConfiguration
    {
        public static void RegisterTypes<TLifetime>(IUnityContainer container)
           where TLifetime : ITypeLifetimeManager, new()
        {
            container.RegisterType<MicrosoftOneDriveWrapper>(new TLifetime());
            container.RegisterType<MicrosoftGraphWrapper>(new TLifetime());
        }
    }
}