using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;
using Logic.Utils;

namespace Logic.AppServices
{
    public sealed class EditPersonalInfoCommand : ICommand
    {
        public EditPersonalInfoCommand(string email, string name, long id)
        {
            Email = email;
            Name = name;
            Id = id;
        }

        public long Id { get; }
        public string Name { get; }
        public string Email { get; }

        [AuditLog]
        [DatabaseRetry]
        internal sealed class EditPersonalInfoCommandHandler : ICommandHandler<EditPersonalInfoCommand>
        {
            private readonly SessionFactory _sessionFactory;

            public EditPersonalInfoCommandHandler(SessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Result Handle(EditPersonalInfoCommand command)
            {
                var unitOfWork = new UnitOfWork(_sessionFactory);
                var repository = new StudentRepository(unitOfWork);
                Student student = repository.GetById(command.Id);
                if (student == null)
                    return Result.Fail($"No student found for Id {command.Id}");

                student.Name = command.Name;
                student.Email = command.Email;

                unitOfWork.Commit();

                return Result.Ok();
            }
        }
    }
}
