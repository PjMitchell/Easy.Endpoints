using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints
{
    [EndpointController("Command")]
    [CommandParameterDiscovery]
    [Post("[controller]/[type]")]
    public class CommandEndpointHandler<TCommand> : IJsonBodyAndResponseEndpointHandler<TCommand, CommandResult> where TCommand : ICommand
    {
        public Task<CommandResult> Handle(TCommand body, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CommandResult { Successful = true, Message = "Success" });
        }
    }

    public class CommandParameterDiscoveryAttribute : SingleGenericParameterDiscoveryAttribute
    {
        protected override IEnumerable<Type> GetTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(r => !r.IsAbstract && !r.IsInterface && r.IsAssignableTo(typeof(ICommand)));
        }

        protected override IEnumerable<EndpointRouteValueMetadata> GetRouteVaulesForGenericParameter(Type type)
        {
            var typeValue = type.Name.EndsWith("Command") && type.Name != "Command" ? type.Name[0..^7] : type.Name;

            yield return new EndpointRouteValueMetadata(EndpointRouteKeys.Type, typeValue);
        }
    }

    public interface ICommand
    {
    }

    public class CreateItemCommand: ICommand
    {
        public string Name { get; set; }
    }

    public class DeleteItemCommand : ICommand
    {
        public string Name { get; set; }
    }

    public class RenameItemCommand : ICommand
    {
        public string Name { get; set; }
    }
}
