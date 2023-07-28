using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMonitorService.StockApi
{
    public class StockApiSettings
    {
        public string Endpoint { get; set; } = default!;
        public string ApiKey { get; set; } = default!;
    }
}
