using CocosSharp;
using System;


namespace RocketInvasion.Common.Sprites
{
    public class SpriteNode : CCNode
    {
        public CCSprite sprite;
        protected float scaleFactor;

        protected CCVector2 VelocityVec;

        protected int health;


        public SpriteNode() {
            scaleFactor = 1.0f;
            VelocityVec = new CCVector2(0, 0);

            Schedule(AnimationActivity);
        }

        public void DrawSprite(String imgFileName) {
            CCTexture2D imgTexture = new CCTexture2D(imgFileName);

            sprite = new CCSprite(imgTexture);
            sprite.Scale = scaleFactor;

            sprite.AnchorPoint = CCPoint.AnchorMiddle;

            // temporary
            /*
            CCDrawNode drawNode = new CCDrawNode();
            drawNode.Position = CCPoint.AnchorMiddle; //new CCPoint(0, 0);
            drawNode.DrawRect(sprite.BoundingBoxTransformedToWorld, CCColor4B.Blue);
            this.AddChild(drawNode);
            */       

            this.AddChild(sprite);
        }

        public void NextFrameUpdate() {
            GameParameters.renderingSurfaceMutex.WaitOne();
            this.Position += new CCPoint(this.VelocityVec.X, this.VelocityVec.Y);
            GameParameters.renderingSurfaceMutex.ReleaseMutex();
        }

        public void Erase() {
            this.RemoveFromParent();
            this.Dispose();
        }

        protected void AnimationActivity(float timeElapsed) {}

    }
}
