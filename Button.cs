using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ICS4U_Final_Project
{
    internal class Button
    {
        MouseState mouseState, prevMouseState;

        Point mouse;

        private Texture2D _drawTexture;
        public Texture2D _texture { get; set; }
        public Texture2D _texture2 { get; set; }
        public Rectangle _rectangle { get; set; }


        public Button(Texture2D Texture, Texture2D Texture2, Rectangle Rectangle)
        {
            _texture = Texture;
            _texture2 = Texture2;
            _rectangle = Rectangle;

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
                if (mouseState.LeftButton == ButtonState.Pressed)
                    _drawTexture = _texture2;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_drawTexture, _rectangle, Color.White);
        }
    }
}
