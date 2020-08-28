using CSharpFunctionalExtensions;
using Logic.Utils;

namespace Logic.Students
{
    public class DisenrollCommand : ICommand
    {

        public DisenrollCommand(long id, int enrollmentNumber, string comment)
        {
            Id = id;
            EnrollmentNumber = enrollmentNumber;
            Comment = comment;
        }

        public long Id { get; }
        public int EnrollmentNumber { get; }
        public string Comment { get; }

        internal class DisenrollCommandHandler : ICommandHandler<DisenrollCommand>
        {
            private readonly UnitOfWork _unitOfWork;

            public DisenrollCommandHandler(UnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public Result Handle(DisenrollCommand command)
            {
                var repository = new StudentRepository(_unitOfWork);

                Student student = repository.GetById(command.Id);
                if (student == null)
                    return Result.Fail($"No student found for Id {command.Id}");

                if (string.IsNullOrWhiteSpace(command.Comment))
                    return Result.Fail("Disenrollment comment is required");

                var enrollment = student.GetEnrollment(command.EnrollmentNumber);
                if (enrollment == null)
                    return Result.Fail($"Enrollment does not exist {command.EnrollmentNumber}");

                student.RemoveEnrollment(enrollment, command.Comment);

                _unitOfWork.Commit();
                return Result.Ok();
            }
        }
    }
}
