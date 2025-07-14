using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Interface;

namespace XPlan.Utility.Databases
{
    public class MongoDbSettings : IDBSetting
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
