using ShopifyImporter.Contracts;
using ShopifyImporter.Integrations.MicrosoftGraph;
using ShopifyImporter.Integrations.MicrosoftGraph.Contracts;
using ShopifyImporter.Integrations.MicrosoftOneDrive;
using ShopifyImporter.Integrations.MicrosoftOneDrive.Contracts;
using ShopifyImporter.Integrations.Shopify;
using ShopifyImporter.Integrations.Shopify.Contracts;
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
            container.RegisterType<IMicrosoftOneDriveWrapper, MicrosoftOneDriveWrapper>(new TLifetime());
            container.RegisterType<IMicrosoftGraphWrapper, MicrosoftGraphWrapper>(new TLifetime());
            container.RegisterType<IShopifyWrapper, ShopifyWrapper>(new TLifetime());

            container.RegisterType<IFileService, FileService>(new TLifetime());
            container.RegisterType<IExcelParserService, ExcelParserService>(new TLifetime());
            container.RegisterType<IShopifyService, ShopifyService>(new TLifetime());
            container.RegisterType<ICommonService, CommonService>(new TLifetime());
            container.RegisterType<IEmailService, EmailService>(new TLifetime());
            container.RegisterType<IReportService, ReportService>(new TLifetime());

        }
    }
}
