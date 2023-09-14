// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;

namespace Infrastructure.Extension
{
    public static class TransformExtension
    {
        public static void SetXPosition(this Transform transform, float x)
        {
            Vector3 newPosition = new(x, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }

        public static void SetYPosition(this Transform transform, float y)
        {
            Vector3 newPosition = new(transform.position.x, y, transform.position.z);
            transform.position = newPosition;
        }

        public static void SetXYPosition(this Transform transform, float x, float y)
        {
            Vector3 newPosition = new(x, y, transform.position.z);
            transform.position = newPosition;
        }
    }
}
