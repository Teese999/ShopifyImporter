using ExcelDataReader;
using ShopifyImporter.Contracts;
using ShopifyImporter.Contracts.Models;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ShopifyImporter.Services
{
    public class ExcelParserService : IExcelParserService
    {
        private Settings _settings;

        public ExcelParserService(Settings settings)
        {
            _settings = settings;
        }

        public IEnumerable<InventoryDto> ParseFile(string fileName)
        {
            var result = new List<InventoryDto>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(Path.Combine(_settings.IncomingDownloadFolderName, fileName), FileMode.Open, FileAccess.Read))
            {
                var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                DataSet dataSet = excelReader.AsDataSet();

                foreach (DataTable table in dataSet.Tables)
                {
                    if (table.Columns.Count > 0 &&
                        table.Rows.Count > 0 &&
                        table.Rows[0][0].ToString().ToLower() == "sku" &&
                        table.Rows[0][2].ToString().ToLower() == "available")
                    {
                        for (int i = 1; i < table.Rows.Count; i++)
                        {
                            var inventory = new InventoryDto();

                            var sku = table.Rows[i][0]?.ToString()?.Trim();
                            var quantityString = table.Rows[i][2]?.ToString();

                            if (!string.IsNullOrEmpty(sku))
                            {
                                inventory.Sku = sku;
                            }
                            else
                            {
                                inventory.HasError = true;
                                inventory.ErrorMessage = "\"SKU\" column is empty.";
                            }

                            if (int.TryParse(quantityString, out int quantity))
                            {
                                inventory.Quantity = quantity;
                            }
                            else
                            {
                                inventory.HasError = true;
                                inventory.ErrorMessage = "\"Available\" column can't be converted to number.";
                            }

                            result.Add(inventory);
                        }
                    }
                }
            }

            return result;
        }
    }
}
