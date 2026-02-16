using System;
using System.Collections.Generic;
using System.Text;

namespace HealthApp.Infrastructure
{
    public  class DBConfigurationClass
    {
        public const string SectionName = "ConnectionStrings";
        public string? DefaultConnection { get; set; } = null;
    }
}
