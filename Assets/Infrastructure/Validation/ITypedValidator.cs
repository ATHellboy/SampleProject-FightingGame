namespace Assets.Infrastructure.Validation {
	public interface ITypedValidator<in T> : IValidator {
		ValidationResult IsValid(T obj, ValidationContext context);
	}
}