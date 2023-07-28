using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class QueueNames
    {
        public const string MonitorQueue = "stock_monitor";
        public const string AlertQueue = "quote_alert";
        public const string SendMailQueue = "send_mail";
    }
}
