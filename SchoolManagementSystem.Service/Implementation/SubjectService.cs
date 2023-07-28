using AutoMapper;
using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation
{
    public class SubjectService : ISubjectService
    {
        private readonly IRepository<Subject> _subjectRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork unitOfWork, ILoggerManager logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _subjectRepo =  _unitOfWork.GetRepository<Subject>();
        }
        public async Task<ServiceResponse<string>> AddSubjectAsync(SubjectRequestDto addSubject)
        {
            var subjectExists = await _subjectRepo.GetSingleByAsync(s => s.Name == addSubject.Name);
            if (subjectExists != null)
            {
                _logger.LogError("Failed to add the subject. Subject already exists.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Failed to add the subject. Subject already exists."
                };
            }

            var newsubject = new Subject
            {
                Name = addSubject.Name
            };

            var addedSubject = await _subjectRepo.AddAsync(newsubject);
            await _unitOfWork.SaveChangesAsync();

            if (addedSubject != null)
            {
                _logger.LogInfo("Subject added successfully.");

                var result = new SubjectResponseDto
                {
                    Name = addedSubject.Name
                };

                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = "Subject added successfully!"
                };
            }
            else
            {
                _logger.LogError("Failed to add the subject.");
                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = "Failed to add the subject."
                };
            }


        }

        public async Task<ServiceResponse<string>> DeleteSubjectAsync(int Id)
        {
            var existingSubject = await _subjectRepo.GetByIdAsync(Id);
            if (existingSubject == null)
            {
                _logger.LogError($"Failed to delete the subject. Subject with ID {Id} not found.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = $"Failed to delete the subject. Subject with ID {Id} not found."
                };
            }

            await _subjectRepo.DeleteAsync(existingSubject);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo($"Subject with ID {Id} deleted successfully.");
            return new ServiceResponse<string>
            {
                Success = true,
                Message = $"Subject with ID {Id} deleted successfully."
            };
        }


        public async Task<ServiceResponse<string>> UpdateSubjectAsync(int Id, SubjectRequestDto updateSubject)
        {
            Subject existingSubject = await _subjectRepo.GetByIdAsync(Id);
            if (existingSubject == null)
            {
                _logger.LogError($"Failed to update the subject. Subject with ID {Id} not found.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = $"Failed to update the subject. Subject with ID {Id} not found."
                };
            }
            string previousName = existingSubject.Name;

            existingSubject.Name = updateSubject.Name;

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInfo($"Subject with ID {Id} updated successfully.");

            if (existingSubject.Name != previousName)
            {
                _logger.LogInfo($"Subject with ID {Id} updated successfully.");

                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = $"Subject with ID {Id} updated successfully."
                };
            }
            else
            {
                _logger.LogError("Failed to update the subject.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Failed to update the Subject."
                };
            }
        }

        public async Task<List<SubjectResponseDto>> ViewAllSubjectsAsync()
        {
            var AllSubjects = await _subjectRepo.GetAllAsync();

            var result = _mapper.Map<List<SubjectResponseDto>>(AllSubjects);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

    }
}
