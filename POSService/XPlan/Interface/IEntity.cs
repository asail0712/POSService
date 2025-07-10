using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Interface
{
    public interface IEntity
    {
        ObjectId Id { get; set; }
    }
}
