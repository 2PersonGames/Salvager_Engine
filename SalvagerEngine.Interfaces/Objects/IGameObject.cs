using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using SalvagerEngine.Interfaces.Components;
using SalvagerEngine.Interfaces.Objects.Graphics;

namespace SalvagerEngine.Interfaces.Objects
{
    public interface IGameObject
    {
        ILevel ComponentOwner { get; }
        bool Enabled { get; set; }
        bool Visible { get; set; }
        long Identifier { get; }

        void Destroy();
        void Update(float delta);
        void Draw(ICamera camera);
        bool IsChildOf(IGameObject obj);
        bool IsParentOf(IGameObject obj);
        IEnumerable<IGameObject> ForEachChild();
        IEnumerable<T> ForEachChild<T>() where T : IGameObject;
        IEnumerable<IGameObject> ForEachAll();
        IEnumerable<T> ForEachAll<T>() where T : IGameObject;
        IGameObject FindParent();
        float GetDepth();
        bool AddChild(IGameObject obj);
        bool RemoveChild(IGameObject obj);
        bool RemoveChild(long identifier);
    }
}
