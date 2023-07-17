using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;


namespace SchoolManagementSystem.Service.Implementation
{
    public class SchoolFeeService : ISchoolFeeService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<SchoolFee> _schoolFeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SchoolFeeService> _logger;

        public SchoolFeeService(ILogger<SchoolFeeService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _schoolFeeRepository = _unitOfWork.GetRepository<SchoolFee>();
        }

        public async Task AddSchoolFee(SchoolFeeDto schoolFeeDto)
        {
            var schoolFee = _mapper.Map<SchoolFee>(schoolFeeDto);
            await _schoolFeeRepository.AddAsync(schoolFee);
            _unitOfWork.SaveChanges();
            _logger.LogInformation("School fee added successfully.");
        }

        public async Task<string> UpdateSchoolFee(int id, SchoolFeeDto schoolFeeDto)
        {
            var existingSchoolFee = await _schoolFeeRepository.GetByIdAsync(id);
            if (existingSchoolFee == null)
            {
                return "school fee not exists";
            }
            var updatedFee = _mapper.Map(schoolFeeDto, existingSchoolFee);
            await _schoolFeeRepository.UpdateAsync(updatedFee);

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("School fee updated successfully.");

            return "school fee updated successful!";
        }

        public async Task<List<SchoolFeeResponse>> GetAllSchoolFees()
        {
            var schoolFees = await _schoolFeeRepository.GetAllAsync();
            var schoolFeeDtos = _mapper.Map<List<SchoolFeeResponse>>(schoolFees);

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Retrieved all school fees.");
            return schoolFeeDtos;
        }


        public async Task DeleteSchoolFee(int id)
        {
            var schoolFee = await _schoolFeeRepository.GetByIdAsync(id);
            if (schoolFee == null)
            {
                return;
            }
            await _schoolFeeRepository.DeleteAsync(schoolFee);

            _unitOfWork.SaveChanges();
            _logger.LogInformation("School fee deleted successfully.");
        }


        public async Task<decimal> GetTotalSchoolFees()
        {
            var totalFee = await _schoolFeeRepository.SumAsync(fee => fee.FeeAmount);
            _logger.LogInformation("Retrieved total school fees.");
            return totalFee;
        }


        public async Task<decimal> GetTotalSchoolFeesOfClass(string className)
        {
            //var feesOfClass = await _schoolFeeRepository.GetByAsync(fee => fee.Class ==className.ToLower());

            var feesOfClass = await _schoolFeeRepository.Where(fee => fee.Class == className);
            var totalClassFee = feesOfClass.Sum(fee => fee.FeeAmount);

            _logger.LogInformation($"Retrieved total school fees of class {className}.");
            return totalClassFee;
        }


        public async Task<List<SchoolFeeResponse>> GetAllSchoolFeesOfClass(string className)
        {
            var schoolFeesOfClass = await _schoolFeeRepository.Where(fee => fee.Class == className);
            var schoolFeeDtos = _mapper.Map<List<SchoolFeeResponse>>(schoolFeesOfClass);

            _logger.LogInformation($"Retrieved all school fees of class {className}.");
            return schoolFeeDtos;
        }
    }
}
