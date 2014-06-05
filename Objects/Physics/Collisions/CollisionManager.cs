using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SalvagerEngine.Games;
using SalvagerEngine.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SalvagerEngine.Objects.Physics.Collisions
{
    public class CollisionManager : Component
    {
        /* Constructors */

        public CollisionManager(SalvagerGame game)
            : base(game)
        {
        }

        /* Overrides */

        public override void Update(GameTime gameTime)
        {
            /* Call the base method */
            base.Update(gameTime);

            /* Iterate through each level in the game */
            foreach (GameComponent gc in Game.Components)
            {
                if (gc is Level)
                {
                    PerformCollisionChecks(gc as Level);
                }
            }
        }

        /* Utilities */

        void PerformCollisionChecks(Level level)
        {
            /* Retrieve all the collision objects from the level */
            CollisionObject[] objects = level.Root.ForEachAll<CollisionObject>().ToArray();

            /* Iterate through all the objects */
            for (int i = 0; i < objects.Length; i++)
            {
                Parallel.For(i + 1, objects.Length, j =>
                {
                    objects[i].CheckCollision(objects[j]);
                });
            }
        }
    }
}
