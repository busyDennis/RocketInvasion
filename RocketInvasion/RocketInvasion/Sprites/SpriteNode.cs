using CocosSharp;

using System;


namespace RocketInvasion.Sprites
{
    public class SpriteNode : CCNode
    {
        public CCSprite sprite;
        public float scaleFactor { get; set; }
        public CCVector2 Velocity { get; set; }
        public String imgFileName { get; set; }


        public SpriteNode() {
            scaleFactor = 1.0f;
            this.Velocity = new CCVector2(0, 0);

            Schedule(AnimationActivity);
        }

        public void DrawSprite(String imgFileName) {
            this.imgFileName = imgFileName;

            CCTexture2D imgTexture = new CCTexture2D(imgFileName);

            sprite = new CCSprite(imgTexture);
            sprite.Scale = scaleFactor;

            sprite.AnchorPoint = CCPoint.AnchorMiddle; 

            this.AddChild(sprite);
        }

        public virtual void NextFrameUpdate() {
            GameParameters.RENDERING_SURFACE_MUTEX.WaitOne();
            this.Position += new CCPoint(this.Velocity.X, this.Velocity.Y);
            GameParameters.RENDERING_SURFACE_MUTEX.ReleaseMutex();
        }

        protected int HealthPoints { get; set; }

        public void Erase() {            
            GameParameters.RENDERING_SURFACE_MUTEX.WaitOne();
            this.RemoveFromParent();
            this.RemoveAllListeners();
            this.Dispose();
            GameParameters.RENDERING_SURFACE_MUTEX.ReleaseMutex();
        }

        protected void AnimationActivity(float timeElapsed) {}
    }
}
