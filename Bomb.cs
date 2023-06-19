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
        private Rectangle _bombRect;
        private Rectangle _bombShadowRect;
        private SoundEffect _soundEffect;
        private SoundEffectInstance _soundEffectInstance;
        private double _speed;
        private double _index;
        List<Texture2D> _spriteSheet;
        private Texture2D _bombTexture;
        private Stopwatch timer;
        private TimeSpan elapsed;
        private double _bombSize;
        private double _bombAlt;
        private double _bombShadowY;
        private double _bombShadowX;
        private double _bombX;
        private bool explode = false;

        public Bomb(List<Texture2D> explosion, Texture2D bomb, Rectangle rectangle, double speed, SoundEffect soundEffect)
        {
            _spriteSheet = explosion;
            _bombTexture = bomb;
            _rectangle = rectangle;
            _speed = speed;
            _soundEffect = soundEffect;

            timer = new Stopwatch();
            _bombSize = 32;
            _bombAlt = _rectangle.Y;
            _bombX = _rectangle.X;
            _bombShadowY = _rectangle.Y + 75;
            _bombShadowX = _rectangle.X - 30;

            _bombRect = new Rectangle(_rectangle.X, _rectangle.Y, 32, 32);
            _bombShadowRect = new Rectangle(_rectangle.X - 30, _rectangle.Y + 75, 32, 32);

            _rectangle.Y += 30;

            _soundEffectInstance = _soundEffect.CreateInstance();
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
                _bombX += 0.05;
                _bombSize -= 0.1;
                _bombShadowY -= 0.6;
                _bombShadowX += 0.5;
            }

            if (elapsed.TotalSeconds >= 1)
            {
                _index += _speed;
                explode = true;
            }

            if (explode)
                _soundEffectInstance.Play();

            _bombRect.Y = (int)_bombAlt;
            _bombRect.X = (int)_bombX;

            _bombRect.Width = (int)_bombSize;
            _bombRect.Height = (int)_bombSize;

            _bombShadowRect.Y = (int)_bombShadowY;
            _bombShadowRect.X = (int)_bombShadowX;

            _bombShadowRect.Width = (int)_bombSize;
            _bombShadowRect.Height = (int)_bombSize;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (!explode)
            {
                _spriteBatch.Draw(_bombTexture, _bombShadowRect, Color.Black * 0.3f);
                _spriteBatch.Draw(_bombTexture, _bombRect, Color.White);
            }
            if (explode)
                _spriteBatch.Draw(_spriteSheet[(int)Math.Round(_index)], _rectangle, Color.White);
        }
    }
}
