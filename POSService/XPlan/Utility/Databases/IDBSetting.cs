using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Utility.Databases
{
    public interface IDBSetting
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }        
    }
}
