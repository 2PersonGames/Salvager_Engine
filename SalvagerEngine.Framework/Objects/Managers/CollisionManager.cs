﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using SalvagerEngine.Framework.Objects;
using SalvagerEngine.Framework.Components;
using SalvagerEngine.Framework.Objects.Physics.Collisions;

namespace SalvagerEngine.Framework.Objects.Managers
{
    public class CollisionManager : GameObject
    {
        // Constructors

        public CollisionManager(Level component_owner)
            : base(component_owner, 0.0f)
        {
            Visible = false;
        }

        // Overrides

        protected override void Tick(float delta)
        {
            // Call the base method
            base.Tick(delta);

            // Retrieve an array of collision objects
            CollisionObject[] objects = ForEachChild<CollisionObject>().ToArray();

            // Iterate through each object
            for (int i = 0; i < objects.Length; i++)
            {
                Parallel.For(i + 1, objects.Length, j =>
                {
                    objects[i].CheckCollision(objects[j]);
                });
            }
        }

        public override float GetDepth()
        {
            return 1.1f;
        }
    }
}
