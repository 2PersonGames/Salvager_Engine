using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SalvagerEngine.Games;

namespace SalvagerEngine.Content
{
    public class ContentManager : Microsoft.Xna.Framework.Content.ContentManager
    {
        /* Typedefs and Constants */

        const string DataFileExtension = "dat";

        struct TextureData
        {
            Texture2D mTexture;
            public Texture2D Texture
            {
                get { return mTexture; }
            }

            Rectangle mSource;
            public Rectangle Source
            {
                get { return mSource; }
            }

            Rectangle mFrame;
            public Rectangle Frame
            {
                get { return mFrame; }
            }

            public TextureData(Texture2D texture, Rectangle source)
            {
                mTexture = texture;
                mSource = source;
                mFrame = source;
            }

            public TextureData(Texture2D texture, Rectangle source, Rectangle frame)
            {
                mTexture = texture;
                mSource = source;
                mFrame = frame;
            }
        };

        /* Class Variables */

        SalvagerGame mGame;

        ReaderWriterLockSlim mTexturesLock;
        Dictionary<string, TextureData> mTextures;
        ReaderWriterLockSlim mSoundsLock;
        Dictionary<string, SoundEffect> mSounds;
        ReaderWriterLockSlim mFontsLock;
        Dictionary<string, SpriteFont> mFonts;

        /* Constructors */

        public ContentManager(Game game, string root_directory)
            : base(game.Content.ServiceProvider, root_directory)
        {
            mGame = game as SalvagerGame;
            mTexturesLock = new ReaderWriterLockSlim();
            mTextures = new Dictionary<string, TextureData>();
            mSoundsLock = new ReaderWriterLockSlim();
            mSounds = new Dictionary<string, SoundEffect>();
            mFontsLock = new ReaderWriterLockSlim();
            mFonts = new Dictionary<string, SpriteFont>();
        }

        /* Accessors */

        public Texture2D GetTexture(string filename)
        {
            Rectangle source = Rectangle.Empty;
            return GetTexture(filename, out source);
        }

        public Texture2D GetTexture(string filename, out Rectangle source)
        {
            Rectangle frame = Rectangle.Empty;
            return GetTexture(filename, out source, out frame);
        }

        public Texture2D GetTexture(string filename, out Rectangle source, out Rectangle frame)
        {
            try
            {
                /* Lock the textures */
                mTexturesLock.EnterReadLock();

                /* Check the file exists */
                if (mTextures.ContainsKey(filename))
                {
                    source = mTextures[filename].Source;
                    frame = mTextures[filename].Frame;
                    return mTextures[filename].Texture;
                }
                else
                {
                    source = Rectangle.Empty;
                    frame = Rectangle.Empty;
                    return null;
                }
            }
            finally
            {
                /* Release the textures */
                mTexturesLock.ExitReadLock();
            }
        }

        public SoundEffect GetSoundEffect(string filename)
        {
            try
            {
                /* Lock the sounds */
                mSoundsLock.EnterReadLock();

                /* Check the sound exists */
                if (mSounds.ContainsKey(filename))
                {
                    return mSounds[filename];
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                /* Exit the sounds lock */
                mSoundsLock.ExitReadLock();
            }
        }

        public SpriteFont GetFont(string filename)
        {
            try
            {
                /* Enter the font lock */
                mFontsLock.EnterReadLock();

                /* Return the font */
                return mFonts[filename];
            }
            finally
            {
                /* Exit the font lock */
                mFontsLock.ExitReadLock();
            }
        }

        /* Utilities */

        public void LoadTextureMap(string filename)
        {
            LoadTextureMap(filename, Path.ChangeExtension(filename, DataFileExtension)); 
        }

        public void LoadTextureMap(string texture_filename, string data_filename)
        {
            /* Load the textures */
            Texture2D texture = null;
            if (string.IsNullOrWhiteSpace(Path.GetExtension(texture_filename)))
            {
                texture = Load<Texture2D>(Path.Combine(RootDirectory, texture_filename));
            }
            else
            {
                texture = Texture2D.FromStream(mGame.GraphicsDevice, new StreamReader(Path.Combine(RootDirectory, texture_filename)).BaseStream);
            }

            /* Open the file stream */
            using (StreamReader reader = new StreamReader(Path.Combine(RootDirectory, data_filename)))
            {
                try
                {
                    /* Read the line */
                    string[] line = reader.ReadLine().Split('\t');

                    /* Read the source rectangle */
                    Rectangle source = new Rectangle(int.Parse(line[1]),
                        int.Parse(line[2]), int.Parse(line[3]), int.Parse(line[4]));

                    try
                    {
                        /* Lock the textures */
                        mTexturesLock.EnterWriteLock();

                        /* Check the length */
                        if (line.Length > 5)
                        {
                            /* Create the animation frame */
                            Rectangle frame = source;
                            frame.Width = int.Parse(line[5]);
                            frame.Height = int.Parse(line[6]);

                            /* Add a texture data with a frame */
                            mTextures.Add(line[0], new TextureData(texture, source, frame));
                        }
                        else
                        {
                            /* Add the texture data without an animation frame */
                            mTextures.Add(line[0], new TextureData(texture, source));
                        }
                    }
                    finally
                    {
                        /* Unlock the textures */
                        mTexturesLock.ExitWriteLock();
                    }
                }
                finally
                {
                    /* Close the stream */
                    reader.Close();
                }
            }
        }

        public void LoadSoundEffect(string filename)
        {
            try
            {
                /* Lock the sound effects */
                mSoundsLock.EnterWriteLock();

                /* Load the sound effect */
                mSounds.Add(filename, Load<SoundEffect>(Path.Combine(RootDirectory, filename)));
            }
            finally
            {
                /* Unlock the sound effects */
                mSoundsLock.ExitWriteLock();
            }
        }

        public void LoadFont(string filename)
        {
            try
            {
                /* Lock the fonts */
                mFontsLock.EnterWriteLock();

                /* Load the font */
                mFonts.Add(filename, Load<SpriteFont>(Path.Combine(RootDirectory, filename)));
            }
            finally
            {
                /* Exit the font lock */
                mFontsLock.ExitWriteLock();
            }
        }
    }
}
