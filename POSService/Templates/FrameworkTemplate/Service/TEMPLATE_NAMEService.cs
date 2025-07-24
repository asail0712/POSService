using AutoMapper;
using Common.DTO.TEMPLATE_NAME;
using Repository.Interface;
using Service.Interface;
using XPlan.Service;

namespace Service
{
    public class TEMPLATE_NAMEService : GenericService<TEMPLATE_NAMEEntity, TEMPLATE_NAMERequest, TEMPLATE_NAMEResponse, ITEMPLATE_NAMERepository>, ITEMPLATE_NAMEService
    {
        public TEMPLATE_NAMEService(ITEMPLATE_NAMERepository repo, IMapper mapper) 
            : base(repo, mapper)
        {
        }
    }
}
