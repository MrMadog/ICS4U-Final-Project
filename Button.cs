using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ICS4U_Final_Project
{
    internal class Button
    {
        private MouseState mouseState, prevMouseState;

        private Point mouse;

        private Texture2D _drawTexture;
        private Texture2D _texture;
        private Texture2D _texture2;
        private Rectangle _rectangle;
        private SoundEffect _soundEffect;


        public Button(Texture2D Texture, Texture2D Texture2, Rectangle Rectangle, SoundEffect soundEffect)
        {
            _texture = Texture;
            _texture2 = Texture2;
            _rectangle = Rectangle;
            _soundEffect = soundEffect;

            _drawTexture = _texture;
        }

        public bool IsHovering(Point point)
        {
            return (_rectangle.Contains(point));
        }

        public bool IsPressed()
        {
            if (_rectangle.Contains(mouse))
                return (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released);
            return false;        
        }

        public void Update()
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            mouse = new Point(mouseState.X, mouseState.Y);
            _drawTexture = _texture;

            if (_rectangle.Contains(mouse))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    _drawTexture = _texture2;
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    _soundEffect.Play();
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_drawTexture, _rectangle, Color.White);
        }
    }
}
