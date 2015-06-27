using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;

using SalvagerEngine.Interfaces.Storage.Content;

namespace SalvagerEngine.Interfaces.Games
{
    public interface ISalvagerGame
    {
        ISalvagerContentManager Content { get; }

        void Log(string message);
        void Log(Exception e);
    }
}
