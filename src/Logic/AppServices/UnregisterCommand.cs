using CSharpFunctionalExtensions;
using Logic.Utils;

namespace Logic.Students
{
    public sealed class UnregisterCommand : ICommand
    {
        public UnregisterCommand(long id)
        {
            Id = id;
        }
        public long Id { get; set; }

        internal sealed class UnregisterCommandHandler : ICommandHandler<UnregisterCommand>
        {
            private readonly UnitOfWork _unitOfWork;

            public UnregisterCommandHandler(UnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public Result Handle(UnregisterCommand command)
            {
                var repository = new StudentRepository(_unitOfWork);
                Student student = repository.GetById(command.Id);
                if (student == null)
                    return Result.Fail($"No student found for Id {command.Id}");

                repository.Delete(student);
                _unitOfWork.Commit();

                return Result.Ok();
            }
        }
    }
}
