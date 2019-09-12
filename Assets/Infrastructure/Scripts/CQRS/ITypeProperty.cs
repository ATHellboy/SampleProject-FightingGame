using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Infrastructure.Scripts.CQRS {
	public interface ITypeEvent {
		Type Type { get; }
	}
}