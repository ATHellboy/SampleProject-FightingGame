namespace Assets.Infrastructure.Scripts.CQRS {
	public interface IEventHandler<in TEvent> : IMessageHandler<TEvent>
		where TEvent : IEvent {
		new void Handle(TEvent evt);
	}
}