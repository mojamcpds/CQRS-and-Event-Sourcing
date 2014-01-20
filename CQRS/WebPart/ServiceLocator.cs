using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Bus;
using CQRS.Reporting;

namespace WebPart
{
    public static class ServiceLocator
    {
        public static IEventBus EventBus { get; set; }
        public static ICommandBus CommandBus {get; set;}
        public static IReportingRepository ReportingRepository { get; set; }
    }
}