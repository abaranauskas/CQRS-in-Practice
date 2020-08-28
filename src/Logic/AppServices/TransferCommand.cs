using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using System;

namespace Logic.AppServices
{
    public class TransferCommand : ICommand
    {
        public TransferCommand(long id, int enrollmentNumber, string course, string grade)
        {
            Id = id;
            EnrollmentNumber = enrollmentNumber;
            Course = course;
            Grade = grade;
        }

        public long Id { get; }
        public int EnrollmentNumber { get; }
        public string Course { get; }
        public string Grade { get; }

        internal class TransferCommandHandler : ICommandHandler<TransferCommand>
        {
            private readonly UnitOfWork _unitOfWork;

            public TransferCommandHandler(UnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public Result Handle(TransferCommand command)
            {
                var studentRepository = new StudentRepository(_unitOfWork);
                var courseRepository = new CourseRepository(_unitOfWork);

                Student student = studentRepository.GetById(command.Id);
                if (student == null)
                    return Result.Fail($"No student found for Id {command.Id}");

                Course course = courseRepository.GetByName(command.Course);
                if (course == null)
                    return Result.Fail($"No Course found for name {command.Course}");

                bool success = Enum.TryParse<Grade>(command.Grade, out var grade);
                if (!success)
                    return Result.Fail($"Grade is incorrect {command.Grade}");

                var enrollment = student.GetEnrollment(command.EnrollmentNumber);
                if (enrollment == null)
                    return Result.Fail($"Enrollment does not exist {command.EnrollmentNumber}");

                enrollment.Update(course, grade);

                _unitOfWork.Commit();
                return Result.Ok();
            }
        }
    }
}
