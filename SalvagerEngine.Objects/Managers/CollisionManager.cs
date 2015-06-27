using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using SalvagerEngine.Interfaces.Objects;
using SalvagerEngine.Interfaces.Components;
using SalvagerEngine.Interfaces.Objects.Physics.Collisions;

namespace SalvagerEngine.Objects.Managers
{
    public class CollisionManager : GameObject
    {
        // Constructors

        public CollisionManager(ILevel component_owner)
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
            var objects = ForEachChild<ICollisionObject>().ToArray();

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
