using System;

namespace Assets.Infrastructure.Validation {
	public class ValidationResultElement {
		public Type ValidatorType { get; }
		public ValidationResult Result { get; }
		public ResultMessage Message { get; }

		public ValidationResultElement(Type validatorType, ValidationResult result, ResultMessage message) {
			ValidatorType = validatorType;
			Result = result;
			Message = message;
		}
	}
}