// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;

namespace Infrastructure.Extension
{
    public static class Rigidbody2DExtension
    {
        public static void SetXVelocity(this Rigidbody2D rigidbody, float x)
        {
            rigidbody.velocity = new Vector2(x, rigidbody.velocity.y);
        }

        public static void SetYVelocity(this Rigidbody2D rigidbody, float y)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, y);
        }
    }
}
