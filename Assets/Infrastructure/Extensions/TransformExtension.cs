// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;

namespace Infrastructure.Extension
{
    public static class TransformExtension
    {
        public static void SetXPosition(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        public static void SetYPosition(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        public static void SetXYPosition(this Transform transform, float x, float y)
        {
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
