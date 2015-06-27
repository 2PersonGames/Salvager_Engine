using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SalvagerEngine.Interfaces.Storage.Content
{
    public interface ISalvagerContentManager
    {
        Texture2D GetTexture(string filename);
        Texture2D GetTexture(string filename, out Rectangle source);
        Texture2D GetTexture(string filename, out Rectangle source, out Rectangle frame);
        SoundEffect GetSoundEffect(string filename);
        SpriteFont GetFont(string filename);
    }
}
