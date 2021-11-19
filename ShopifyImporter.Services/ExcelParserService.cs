using ExcelDataReader;
using ShopifyImporter.Contracts;
using ShopifyImporter.Integrations.Shopify.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Services
{
    public class ExcelParserService : IExcelParserService
    {
        public List<(string, int)> GetUpdatingList(string filePath, ReportDto report)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var listScu = new List<(string, string)>();


            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
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
                            listScu.Add((table.Rows[i][0].ToString(), table.Rows[i][2].ToString()));
                        }
                    }
                }
            }

            var skuPardesList = new List<(string, int)>();

            foreach (var skuTouple in listScu)
            {
                int parsedItemAvailable;
                bool parseReuslt = Int32.TryParse(skuTouple.Item2, out parsedItemAvailable);

                if (parseReuslt)
                {
                    skuPardesList.Add((skuTouple.Item1.Replace(" ", ""), parsedItemAvailable));
                }
                else
                {
                    report.SkuFailed.Add((skuTouple.Item1, "can't parse available count")); 
                }
            }

            return skuPardesList;
        }
    }
}
