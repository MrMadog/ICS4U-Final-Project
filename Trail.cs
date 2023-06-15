using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ICS4U_Final_Project
{
    internal class Trail
    {
        private Rectangle _rectangle;
        private Rectangle _shadowRectangle;
        private Vector2 _spawnLocation;
        private Texture2D _texture;
        private Color _colour;
        float alpha;
        float alpha2;
        double width;
        double height;
        double locationX;
        double locationY;

        public Trail(Texture2D texture, Vector2 spawnLocation, Color colour)
        {
            _texture = texture;
            _spawnLocation = spawnLocation;
            _colour = colour;

            _rectangle = new Rectangle((int)_spawnLocation.X - _rectangle.Width / 2, (int)_spawnLocation.Y - _rectangle.Height / 2, 5, 5);
            alpha = 1f;
            alpha2 = 0.2f;
            width = 5;
            height = 5;
            locationX = _rectangle.X;
            locationY = _rectangle.Y;
            _shadowRectangle = _rectangle;
        }

        public float getAlpha
        {
            get { return alpha; }
        }

        public Color SetColor
        {
            get { return _colour; }
            set { _colour = value; }
        }

        public void Update()
        {
            width += 0.4;
            height += 0.4;
            locationX -= 0.2;
            locationY -= 0.2;

            _rectangle.Width = (int)Math.Round(width);
            _rectangle.Height = (int)Math.Round(height);

            _rectangle.X = (int)Math.Round(locationX);
            _rectangle.Y = (int)Math.Round(locationY);
            alpha -= 0.007f;

            _shadowRectangle.X = _rectangle.X - 30;
            _shadowRectangle.Y = _rectangle.Y + 70;
            _shadowRectangle.Width = _rectangle.Width;
            _shadowRectangle.Height = _rectangle.Height;
            alpha2 -= 0.002f;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_texture, _shadowRectangle, Color.Black * alpha2);
            _spriteBatch.Draw(_texture, _rectangle, _colour * alpha);
        }
    }
}
