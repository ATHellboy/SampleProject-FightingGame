using System;

namespace Assets.Infrastructure.Validation {
	public abstract class BaseTypedValidator<T> : ITypedValidator<T> {
		private readonly Type _tType = typeof(T);

		public abstract ValidationResult IsValid(T obj, ValidationContext context);

		public ValidationResult IsValid(object obj, ValidationContext context) {
			return !_tType.IsAssignableFrom(obj.GetType())
				? ValidationResult.Rejected
				: IsValid((T)obj, context);
		}
	}
}