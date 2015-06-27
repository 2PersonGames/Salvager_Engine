using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using SalvagerEngine.Interfaces.Objects.Effects;
using SalvagerEngine.Interfaces.Objects.Graphics;

namespace SalvagerEngine.Interfaces.Objects.Physics.Collisions
{
    /* Typedefs and Constants */

    public delegate void CollisionHandler(ICollisionObject me, ICollisionObject other);

    public interface ICollisionObject : IEffectObject
    {
        CollisionHandler OnCollision { get; set; }
        new IGraphicsObject Parent { get; }

        float CalculateAngularMass(float mass);
        void CheckCollision(ICollisionObject obj);
        bool IsCollideableType(Type type);
        bool CheckForCollision(ICollisionObject obj);
    }
}
