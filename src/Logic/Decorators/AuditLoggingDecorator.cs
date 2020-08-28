using CSharpFunctionalExtensions;
using Logic.Students;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Decorators
{
    public class AuditLoggingDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _commandHandler;

        public AuditLoggingDecorator(ICommandHandler<TCommand> commandHandler)
        {
            _commandHandler = commandHandler;
          
        }

        public Result Handle(TCommand command)
        {
            var commandJson = JsonConvert.SerializeObject(command);

            //in real app logger should be injected into ctor
            Console.WriteLine(command);

            return _commandHandler.Handle(command);
        }
    }
}
