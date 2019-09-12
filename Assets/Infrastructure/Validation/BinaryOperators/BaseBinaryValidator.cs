namespace Assets.Infrastructure.Validation.BinaryOperators {
	public abstract class BaseBinaryValidator : IValidator {
		protected readonly IValidator LeftOperand;
		protected readonly IValidator RightOperand;

		protected BaseBinaryValidator(IValidator leftOperand, IValidator rightOperand) {
			LeftOperand = leftOperand;
			RightOperand = rightOperand;
		}

		public abstract ValidationResult IsValid(object obj, ValidationContext context);
	}
}