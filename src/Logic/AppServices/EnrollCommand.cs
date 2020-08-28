using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using System;

namespace Logic.AppServices
{
    public class EnrollCommand : ICommand
    {
        public EnrollCommand(long id, string course, string grade)
        {
            Id = id;
            Course = course;
            Grade = grade;
        }

        public long Id { get; }
        public string Course { get; }
        public string Grade { get; }

        internal class EnrollCommandHandler : ICommandHandler<EnrollCommand>
        {
            private readonly UnitOfWork _unitOfWork;

            public EnrollCommandHandler(UnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public Result Handle(EnrollCommand command)
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

                student.Enroll(course, grade);
                _unitOfWork.Commit();
                return Result.Ok();
            }
        }
    }

   
}
