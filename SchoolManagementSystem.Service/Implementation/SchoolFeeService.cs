using AutoMapper;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;


namespace SchoolManagementSystem.Service.Implementation
{
    public class SchoolFeeService : ISchoolFeeService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<SchoolFee> _schoolFeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public SchoolFeeService(ILoggerManager logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _schoolFeeRepository = _unitOfWork.GetRepository<SchoolFee>();
        }

        public async Task<SchoolFeeDto> SetSchoolFee(ClassFeeDto classFee)
        {
           var schoolFee = _mapper.Map<SchoolFee>(classFee);

            await _schoolFeeRepository.AddAsync(schoolFee);
            await _unitOfWork.SaveChangesAsync();

            var setSchoolFee = _mapper.Map<SchoolFeeDto>(schoolFee);
            return setSchoolFee;
        }


        public async Task<List<SchoolFeeDto>> ViewSchoolFees()
        {
            var schoolFees = await _schoolFeeRepository.GetAllAsync();
            var schoolFeesDto = _mapper.Map<List<SchoolFeeDto>>(schoolFees);

            return schoolFeesDto;
        }

        public async Task<bool> EditSchoolFee(int feeId, ClassFeeDto updatedFee)
        {
            var schoolFee = await _schoolFeeRepository.GetByIdAsync(feeId);

            if (schoolFee == null)
            {
                return false;
            }
            schoolFee.FeeName = updatedFee.FeeName;
            schoolFee.FeeAmount = updatedFee.FeeAmount;
            schoolFee.SetDate = DateTime.Now;

            await _schoolFeeRepository.UpdateAsync(schoolFee);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
