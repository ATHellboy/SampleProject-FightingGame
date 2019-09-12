using System;

namespace Assets.Infrastructure.Scripts.CQRS {
	public interface IActionExecutor {
		bool CanExecuteForParameters(params object[] parameters);
		void Execute(params object[] parameters);

		Type ParameterType(int index);
	}
}