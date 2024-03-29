using AutoMapper;
using GenZStyleAPP.BAL.Repository.Interfaces;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RoleRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }
    }
}
