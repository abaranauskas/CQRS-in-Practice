using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Decorators
{
    public sealed class DataBaseRetryDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _commandHandler;
        private readonly Config _config;

        public DataBaseRetryDecorator(ICommandHandler<TCommand> commandHandler, Config config)
        {
            _commandHandler = commandHandler;
            _config = config;
        }

        public Result Handle(TCommand command)
        {
            for (int i = 0; ; i++)
            {
                try
                {                    
                    return _commandHandler.Handle(command);
                }
                catch (Exception ex)
                {
                    if (i >= _config.NumberOfDatabaseRetries || !IsDatabaseException(ex))
                        throw;                 
                }
            }           
        }

        private bool IsDatabaseException(Exception ex)
        {
            var message = ex.InnerException?.Message;

            if (message == null)
                return false;

            return message.Contains("The connection is broken and recovery is not possible") ||
                message.Contains("error occured while establishing a connection");
        }
    }
}
