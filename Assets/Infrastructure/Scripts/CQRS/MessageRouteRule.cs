using System;
using Assets.Infrastructure.Scripts.CQRS.Transformers;
using Assets.Infrastructure.Scripts.CQRS.Validators;

namespace Assets.Infrastructure.Scripts.CQRS {
	public class MessageRouteRule {
		public string Name { get; }
		public Type MessageType { get; }
		public Type HandlerType { get; }

		public IMessageConditionValidator PreCondition { get; }
		public IMessageTransformer Transformer { get; }
		public IMessageConditionValidator PostCondition { get; }
		public bool IncludeDrivedMessageTypes { get; }

		private MessageRouteRule(string name, Type messageType, bool includeDrivedMessageTypes, Type handlerType, IMessageConditionValidator preCondition, IMessageTransformer transformer, IMessageConditionValidator postCondition) {
			Name = name;
			MessageType = messageType;
			IncludeDrivedMessageTypes = includeDrivedMessageTypes;
			HandlerType = handlerType;
			PreCondition = preCondition;
			Transformer = transformer;
			PostCondition = postCondition;
		}

		#region Static Constructor

		public static MessageRouteRule Create<TMessageHandler, TMessage>(string name, bool includeSubMessageTypes, Type handlerType, IMessageConditionValidator<TMessage> preConditionValidator, IMessageTransformer<TMessage, IMessage> transformer, IMessageConditionValidator<TMessageHandler, TMessage> postConditionValidator)
			where TMessageHandler : class, IMessageHandler<TMessage>
			where TMessage : class, IMessage {
			var route = new MessageRouteRule(name, typeof(TMessage), includeSubMessageTypes, handlerType, preConditionValidator, transformer, postConditionValidator);

			return route;
		}

		public static MessageRouteRule Create<TMessage, TMessageHandler>(string name, bool includeSubMessageTypes, IMessageConditionValidator<TMessage> preConditionValidator, IMessageTransformer<TMessage, IMessage> transformer, IMessageConditionValidator<TMessageHandler, TMessage> postConditionValidator)
			where TMessage : class, IMessage
			where TMessageHandler : class, IMessageHandler<TMessage> {
			var route = new MessageRouteRule(name, typeof(TMessage), includeSubMessageTypes, typeof(TMessageHandler),
				preConditionValidator, transformer, postConditionValidator);

			return route;
		}

		public static MessageRouteRule Create<TMessage, TMessageHandler>(string name, bool includeSubMessageTypes, IMessageConditionValidator<TMessageHandler, TMessage> conditionValidator)
			where TMessage : class, IMessage
			where TMessageHandler : class, IMessageHandler<TMessage> {
			var route = new MessageRouteRule(name, typeof(TMessage), includeSubMessageTypes, typeof(TMessageHandler),
				null, null, conditionValidator);

			return route;
		}

		public static MessageRouteRule Create<TMessage, TMessageHandler>(string name, bool includeSubMessageTypes, IMessageConditionValidator<TMessage> conditionValidator)
			where TMessage : class, IMessage
			where TMessageHandler : class, IMessageHandler<TMessage> {
			var route = new MessageRouteRule(name, typeof(TMessage), includeSubMessageTypes, typeof(TMessageHandler),
				null, null, conditionValidator);

			return route;
		}

		public static MessageRouteRule Create<TMessage>(string name, bool includeSubMessageTypes, Type messageHandler, IMessageConditionValidator<TMessage> conditionValidator)
			where TMessage : class, IMessage {
			var route = new MessageRouteRule(name, typeof(TMessage), includeSubMessageTypes, messageHandler,
				null, null, conditionValidator);

			return route;
		}

		public static MessageRouteRule Create<TMessage, TMessageHandler>(string name, bool includeSubMessageTypes)
			where TMessage : class, IMessage
			where TMessageHandler : class, IMessageHandler<TMessage> {
			var route = new MessageRouteRule(name, typeof(TMessage), includeSubMessageTypes, typeof(TMessageHandler),
				null, null, null);

			return route;
		}

		public static MessageRouteRule Create<TTransformerInputMessage, TTransformerOutputMessage, TMessageHandler>(
			string name, bool includeSubMessageTypes, IMessageConditionValidator<TTransformerInputMessage> preConditionValidator,
			IMessageTransformer<TTransformerInputMessage, TTransformerOutputMessage> transformer, IMessageConditionValidator<TMessageHandler, TTransformerOutputMessage> postConditionValidator)

			where TTransformerInputMessage : class, IMessage
			where TTransformerOutputMessage : class, IMessage
			where TMessageHandler : class, IMessageHandler<TTransformerOutputMessage> {
			var route = new MessageRouteRule(name, typeof(TTransformerOutputMessage), includeSubMessageTypes, typeof(TMessageHandler),
				preConditionValidator, transformer, postConditionValidator);

			return route;
		}

		#endregion
	}
}