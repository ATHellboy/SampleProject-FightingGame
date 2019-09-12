using System;

namespace Assets.Infrastructure.Scripts.CQRS.Transformers {
	public abstract class BaseTransformer : IMessageTransformer {
		public Type InputType { get; }
		public Type OutpuType { get; }

		protected BaseTransformer(Type inputType, Type outpuType) {
			InputType = inputType;
			OutpuType = outpuType;
		}

		public abstract IMessage Transform(IMessage message);
	}

	public abstract class BaseTransformer<TIn, TOut> : IMessageTransformer<TIn, TOut>
		where TIn : class, IMessage
		where TOut : class, IMessage {
		public Type InputType { get; }
		public Type OutpuType { get; }

		protected BaseTransformer() {
			InputType = typeof(TIn);
			OutpuType = typeof(TOut);
		}

		public abstract TOut Transform(TIn message);

		public IMessage Transform(IMessage message) {
			return Transform((TIn)message);
		}
	}
}