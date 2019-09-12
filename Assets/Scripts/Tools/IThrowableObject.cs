using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowableObject : ITool
{
    void Throw(float force, Vector2 direction);
}