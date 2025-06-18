using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseServices
{
    public class Settings
    {
        public required string FilePath { get; set; } = "Words.db";
        public override string ToString() => FilePath;
    }
}
