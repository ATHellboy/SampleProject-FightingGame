namespace Assets.Infrastructure.Validation {
	public interface IValidator {
		ValidationResult IsValid(object obj, ValidationContext context);
	}
}