using AutoMapper;
using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Student> _studentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IPhotoUploadService _photoUploadService;

        public StudentService(IMapper mapper, IUnitOfWork unitOfWork, ILoggerManager logger, IPhotoUploadService photoUploadService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _studentRepo =  _unitOfWork.GetRepository<Student>();
            _photoUploadService = photoUploadService;
        }

        public async Task<ServiceResponse<string>> AddStudentAsync(StudentRequestDto addStudent)
        {
            var emailExists = await _studentRepo.GetSingleByAsync(s => s.Email == addStudent.Email);
            if (emailExists != null)
            {
                _logger.LogError("Failed to add the Student. Email already exists.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Failed to add the Student. Email already exists."
                };
            }

            var student = _mapper.Map<Student>(addStudent);

            student.StudentNumber = $"{student.TenantId}/{student.DateRegistered.Year}/{student.DateRegistered.Day}/{GenerateRandomNumber()}";
            student.DateRegistered = DateTime.Now;
            student.IsActive = true;

            if (addStudent.StudentPhoto != null)
            {
                string imagePath = await _photoUploadService.PhotoUpload(addStudent.StudentPhoto);

                student.ImageUrl= imagePath;
            }
            var addedStudent = await _studentRepo.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();


            if (addedStudent != null)
            {
                _logger.LogInfo("Student added successfully.");

                var result = _mapper.Map<StudentResponseDto>(addedStudent);
                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = "Student added successfully!"
                };
            }
            else
            {
                _logger.LogError("Failed to add the student.");
                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = "Failed to add the Student."
                };
            }
        }

        public async Task<ServiceResponse<string>> UpdateStudentAsync(int Id, StudentRequestDto updateStudent)
        {
            var student = await _studentRepo.GetByIdAsync(Id);
            if (student == null)
            {
                _logger.LogError($"Failed to update the student. Student with ID {Id} not found.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = $"Failed to update the student. Student with ID {Id} not found."
                };
            }

            var updatedStudent = _mapper.Map(updateStudent, student);

            if (updateStudent.StudentPhoto != null)
            {
                string imagePath = await _photoUploadService.PhotoUpload(updateStudent.StudentPhoto);

                student.ImageUrl= imagePath;
            }

            await _unitOfWork.SaveChangesAsync();

            if (updatedStudent != null)
            {
                _logger.LogInfo($"Student with ID {Id} updated successfully.");

                var result = _mapper.Map<StudentRequestDto>(student);
                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = $"Student with ID {Id} updated successfully."
                };
            }
            else
            {
                _logger.LogError("Failed to update the student.");
                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = "Failed to update the Student."
                };
            }
        }

        public async Task<List<StudentResponseDto>> ViewAllStudentsAsync()
        {
            var student = await _studentRepo.GetAllAsync();

            var result = _mapper.Map<List<StudentResponseDto>>(student);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<StudentResponseDto> ViewStudentAsync(int Id)
        {
            var student = await _studentRepo.GetByIdAsync(Id);

            if (student == null)
            {
                _logger.LogError($"Failed to view the student. Student with ID {Id} not found.");
                return null;
            }

            var result = _mapper.Map<StudentResponseDto>(student);
            await _unitOfWork.SaveChangesAsync();

            return result;

        }

        public async Task<ServiceResponse<string>> DeactivateStudentAsync(int Id)
        {
            var student = await _studentRepo.GetByIdAsync(Id);
            if (student == null)
            {
                _logger.LogError($"Failed to deactivate the student. Student with ID {Id} not found.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = $"Failed to deactivate the student. Student with ID {Id} not found."
                };
            }

            student.IsActive = false;

            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Success = true,
                Message = $"Student with ID {Id} deactivated successfully."
            };


        }

        private static string GenerateRandomNumber()
        {
            Random random = new Random();

            var randomNumber = random.Next(1000, 9999);


            return randomNumber.ToString();
        }

    }
}
