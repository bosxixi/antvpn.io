﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API
{
    public class AppSettings
    {
        public Connectionstrings ConnectionStrings { get; set; }
        public string CatalogUrl { get; set; }
        public string AccountingUrl { get; set; }
        public string ServerUrl { get; set; }
        public string OrderingUrl { get; set; }
        public string IdentityUrl { get; set; }
        public string MvcClient { get; set; }
        public string BasketUrl { get; set; }
        public Logging Logging { get; set; }
    }

    public class Connectionstrings
    {
        public string DefaultConnection { get; set; }
    }

    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }
}
