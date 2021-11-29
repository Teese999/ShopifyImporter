using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.MicrosoftOneDrive.Models
{
    public class DriveModel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerName { get; set; }
        public string OwnerId { get; set; }
        public string OriginalData { get; set; }
    }
}
