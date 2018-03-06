using CocosSharp;
using System;


namespace RocketInvasion.Common.Sprites
{
    public class SpriteNode : CCNode
    {
        public CCSprite sprite;
        protected float scaleFactor;

        protected CCVector2 VelocityVec;

        public SpriteNode()
        {
            scaleFactor = 1.0f;
            VelocityVec = new CCVector2(0, 0);
        }

        public void DrawSprite(String imgFileName)
        {
            sprite = new CCSprite(imgFileName);
            sprite.Scale = scaleFactor;

            this.AddChild(sprite);
        }

        public void NextTurnMove()
        {
            GameParameters.mutex.WaitOne();
            this.Position += new CCPoint(this.VelocityVec.X, this.VelocityVec.Y);
            GameParameters.mutex.ReleaseMutex();
        }

        public void Erase()
        {
            GameParameters.mutex.WaitOne();
            this.RemoveFromParent();
            GameParameters.mutex.ReleaseMutex();
        }

    }
}
