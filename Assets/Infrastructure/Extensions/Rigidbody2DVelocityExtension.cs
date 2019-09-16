// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;

namespace Infrastructure.Extension
{
    public static class Rigidbody2DVelocityExtension
    {
        public static void SetXVelocity(this Rigidbody2D rigidbody, float x)
        {
            Vector3 newVelocity = new Vector2(x, rigidbody.velocity.y);
            rigidbody.velocity = newVelocity;
        }

        public static void SetYVelocity(this Rigidbody2D rigidbody, float y)
        {
            Vector3 newVelocity = new Vector2(rigidbody.velocity.x, y);
            rigidbody.velocity = newVelocity;
        }
    }
}
