using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Interfaces.Games;
using SalvagerEngine.Interfaces.Objects;
using SalvagerEngine.Interfaces.Components;
using SalvagerEngine.Interfaces.Objects.Managers;

namespace SalvagerEngine.Interfaces.Components
{
    public interface ILevel
    {
        GraphicsDevice GraphicsDevice { get; }
        ISalvagerGame Game { get; }
        IGameObject Root { get; }

        ICollisionManager GetCollisionManager();
        void Log(string message);
        void Log(Exception message);
    }
}
