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
        private Rectangle _shadowRectangle;
        private SoundEffect _soundEffect;
        private SoundEffectInstance _soundEffectInstance;
        private double _speed;
        private double _index;
        List<Texture2D> _spriteSheet;
        private Stopwatch timer;
        private TimeSpan elapsed;
        private bool ready = false;

        public Bomb(List<Texture2D> spriteSheet, Rectangle rectangle, double speed, SoundEffect soundEffect)
        {
            _spriteSheet = spriteSheet;
            _rectangle = rectangle;
            _speed = speed;
            _shadowRectangle = rectangle;
            _shadowRectangle.X -= 30; _shadowRectangle.Y += 75;
            _soundEffect = soundEffect;

            timer = new Stopwatch();

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

            if (elapsed.TotalSeconds >= 1)
            {
                _index += _speed;
                ready = true;
            }

            if (ready)
                _soundEffectInstance.Play();
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (ready)
                _spriteBatch.Draw(_spriteSheet[(int)Math.Round(_index)], _rectangle, Color.White);
        }
    }
}
