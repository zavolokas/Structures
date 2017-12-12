﻿namespace Zavolokas.Structures
{
    public struct Rectangle
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"Rect({X}, {Y}, {Width}, {Height})";
        }
    }
}
