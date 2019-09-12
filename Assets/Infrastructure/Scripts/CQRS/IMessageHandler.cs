namespace Assets.Infrastructure.Scripts.CQRS {
	public interface IMessageHandler {
	}


	public interface IMessageHandler<in TMessage> : IMessageHandler
		where TMessage : IMessage {
		void Handle(TMessage message);
	}
}