using System;
using System.Reflection;

namespace Assets.Infrastructure.Validation.Reflection {
	public class PropertyValidator<T> : BaseTypedValidator<T> {
		private readonly Type _objectType = typeof(T);
		private readonly IValidator _validator;
		private readonly PropertyInfo _propertyInfo;

		public PropertyValidator(string propertyName, IValidator validator) {
			_validator = validator;
			_propertyInfo = _objectType.GetProperty(propertyName);

			if (null == _propertyInfo) {
				throw new ArgumentException($"Type '{_objectType.FullName}' have not '{propertyName}' property.", propertyName);
			}
		}

		public override ValidationResult IsValid(T obj, ValidationContext context) {
			return _validator.IsValid(_propertyInfo.GetValue(obj), context);
		}
	}
}