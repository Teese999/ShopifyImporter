using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Contracts
{
    public interface ICommonService
    {
        Task Execute();
        Task Authenticate();
    }
}
