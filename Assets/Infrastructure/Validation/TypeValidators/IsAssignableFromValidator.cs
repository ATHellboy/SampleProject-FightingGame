using System;
using Assets.Infrastructure.Validation.UnaryOperators;

namespace Assets.Infrastructure.Validation.TypeValidators {
	public class IsAssignableFromValidator : BaseTypedValidator<Type> {
		private readonly Type _baseType;

		public IsAssignableFromValidator(Type baseType) {
			_baseType = baseType;
		}

		public override ValidationResult IsValid(Type obj, ValidationContext context) {
			return new BooleanToValidationResult().IsValid(_baseType.IsAssignableFrom(obj), context);
		}
	}
}