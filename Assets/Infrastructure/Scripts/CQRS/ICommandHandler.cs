namespace Assets.Infrastructure.Scripts.CQRS {
	public interface ICommandHandler<in TCommand> : IMessageHandler<TCommand>
		where TCommand : ICommand {
		new void Handle(TCommand cmd);
	}
}