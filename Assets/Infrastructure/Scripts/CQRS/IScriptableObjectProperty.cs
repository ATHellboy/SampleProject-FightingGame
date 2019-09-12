using System;
using UnityEngine;

namespace Assets.Infrastructure.Scripts.CQRS {
	public interface IScriptableObjectProperty {
		ScriptableObject GetScriptableObject();
	}
}