using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseServices
{
    public class BdSettings
    {
        public string? Type { get; set; }
        public ConnectionStrings? @ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string? PostgreSQL { get; set; }
        public string? SQLite { get; set; } = "DefaultName.db";
        public string? MSSQL { get; set; }
    }
}
