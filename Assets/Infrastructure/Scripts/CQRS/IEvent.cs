using System;

namespace Assets.Infrastructure.Scripts.CQRS {
	public interface IEvent : IMessage {
		string EntityId { get; }
		DateTime UtcOccureTime { get; }
	}
}