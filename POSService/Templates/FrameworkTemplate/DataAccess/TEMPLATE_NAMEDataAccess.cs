using AutoMapper;
using Common.DTO.TEMPLATE_NAME;
using DataAccess.Interface;
using XPlan.DataAccess;

namespace DataAccess
{
    public class TEMPLATE_NAMEDataAccess : MongoEntityDataAccess<TEMPLATE_NAMEEntity, TEMPLATE_NAMEDocument>, ITEMPLATE_NAMEDataAccess
    {
        public TEMPLATE_NAMEDataAccess(IMapper mapper)
            : base(mapper)
        {
        }
    }
}
