using AutoMapper;
using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation
{
    public class ClassService : IClassService
    {
        private readonly IRepository<Class> _classRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ClassService(IUnitOfWork unitOfWork, ILoggerManager logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _classRepo =  _unitOfWork.GetRepository<Class>();
        }
        public async Task<ServiceResponse<string>> AddClassAsync(ClassRequestDto addClass)
        {
            var classExists = await _classRepo.GetSingleByAsync(s => s.Name == addClass.Name);
            if (classExists != null)
            {
                _logger.LogError("Failed to add the Class. Class already exists.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Failed to add the Class. Class already exists."
                };
            }

            var newclass = new Class
            {
                Name = addClass.Name
            };

            var addedClass = await _classRepo.AddAsync(newclass);
            await _unitOfWork.SaveChangesAsync();

            if (addedClass != null)
            {
                _logger.LogInfo("Class added successfully.");

                var result = new ClassResponseDto
                {
                    Name = addedClass.Name
                };

                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = "Class added successfully!"
                };
            }
            else
            {
                _logger.LogError("Failed to add the class.");
                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = "Failed to add the Class."
                };
            }


        }

        public async Task<ServiceResponse<string>> DeleteClassAsync(int Id)
        {
            var existingClass = await _classRepo.GetByIdAsync(Id);
            if (existingClass == null)
            {
                _logger.LogError($"Failed to delete the class. Class with ID {Id} not found.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = $"Failed to delete the class. Class with ID {Id} not found."
                };
            }

            await _classRepo.DeleteAsync(existingClass);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo($"Class with ID {Id} deleted successfully.");
            return new ServiceResponse<string>
            {
                Success = true,
                Message = $"Class with ID {Id} deleted successfully."
            };
        }


        public async Task<ServiceResponse<string>> UpdateClassAsync(int Id, ClassRequestDto updateClass)
        {
            Class existingClass = await _classRepo.GetByIdAsync(Id);
            if (existingClass == null)
            {
                _logger.LogError($"Failed to update the class. Class with ID {Id} not found.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = $"Failed to update the class. Class with ID {Id} not found."
                };
            }
            string previousName = existingClass.Name;

            existingClass.Name = updateClass.Name;

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInfo($"Class with ID {Id} updated successfully.");

            if (existingClass.Name != previousName)
            {
                _logger.LogInfo($"Class with ID {Id} updated successfully.");

                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = $"Class with ID {Id} updated successfully."
                };
            }
            else
            {
                _logger.LogError("Failed to update the class.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Failed to update the Class."
                };
            }
        }

        public async Task<List<ClassResponseDto>> ViewAllClassesAsync()
        {
            var AllClasses = await _classRepo.GetAllAsync();

            var result = _mapper.Map<List<ClassResponseDto>>(AllClasses);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

    }
}
