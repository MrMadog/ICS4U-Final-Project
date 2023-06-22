using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ICS4U_Final_Project
{
    internal class Bomb
    {
        private Rectangle _rectangle;
        private SoundEffect _soundEffect;
        private SoundEffectInstance _soundEffectInstance;
        List<Texture2D> _spriteSheet;
        private Texture2D _bombTexture;
        private Stopwatch timer;
        private TimeSpan elapsed;
        private double _index;
        private double _bombAlt;
        private double _bombShadowY;
        private double _bombShadowX;
        private float _size;
        private float _rotation;
        private Vector2 _rotationOrigin;
        private Vector2 _location;
        private Vector2 _ShadowLocation;
        private bool explode = false;

        public Bomb(List<Texture2D> explosion, Vector2 location, Texture2D bomb, float rotation, SoundEffect soundEffect)
        {
            _spriteSheet = explosion;
            _bombTexture = bomb;
            _soundEffect = soundEffect;
            _rotation = rotation;
            _location = location;

            timer = new Stopwatch();

            _size = 1f;

            _rectangle = new Rectangle((int)_location.X - 16, (int)_location.Y - 16 + 30, 32, 32);

            _bombAlt = _location.Y;
            _bombShadowY = _location.Y + 75;
            _bombShadowX = _location.X - 30;

            _ShadowLocation = new Vector2((float)_bombShadowX, (float)_bombShadowY);

            _soundEffectInstance = _soundEffect.CreateInstance();

            _rotationOrigin = new Vector2(16, 16);
        }

        public bool Done()
        {
            return( _index >= _spriteSheet.Count - 0.5);
        }

        public void Update(GameTime gameTime)
        {
            timer.Start();
            elapsed = timer.Elapsed;
            
            if (!explode)
            {
                _bombAlt += 0.52;
                _bombShadowY -= 0.65;
                _bombShadowX += 0.50;
                _size -= 0.005f;
            }

            if (elapsed.TotalSeconds >= 1)
            {
                _index += 0.05;
                explode = true;
            }

            if (explode)
                _soundEffectInstance.Play();

            _ShadowLocation.X = (int)_bombShadowX;
            _ShadowLocation.Y = (int)_bombShadowY;

            _location.Y = (int)_bombAlt;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (!explode)
            {
                _spriteBatch.Draw(_bombTexture, _ShadowLocation, null, Color.Black * 0.3f, _rotation, _rotationOrigin, _size, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_bombTexture, _location, null, Color.White, _rotation, _rotationOrigin, _size, SpriteEffects.None, 0f);
            }
            if (explode)
                _spriteBatch.Draw(_spriteSheet[(int)Math.Round(_index)], _rectangle, Color.White);
        }
    }
}
