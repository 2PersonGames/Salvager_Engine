using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalvagerEngine.Interfaces.Objects.Effects
{
    public interface IEffectObject : IGameObject
    {
        IGameObject Parent { get; }
    }
}
