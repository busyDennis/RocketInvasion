using CocosSharp;
using System;


namespace RocketInvasion.Common.Sprites
{
    public class SpriteNode : CCNode
    {
        public CCSprite sprite;
        public float scaleFactor { get; set;  }

        protected CCVector2 VelocityVec;

        public String imgFileName { get; set;  }


        public SpriteNode() {
            scaleFactor = 1.0f;
            VelocityVec = new CCVector2(0, 0);

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

        public void NextFrameUpdate() {
            GameParameters.RENDERING_SURFACE_MUTEX.WaitOne();
            this.Position += new CCPoint(this.VelocityVec.X, this.VelocityVec.Y);
            GameParameters.RENDERING_SURFACE_MUTEX.ReleaseMutex();
        }

        public void setVelocity(CCVector2 vec) {
            this.VelocityVec = vec;
        }

        protected int HealthPoints { get; set; }

        public void Erase() {
            GameParameters.RENDERING_SURFACE_MUTEX.WaitOne();
            this.RemoveFromParent();
            this.Dispose();
            GameParameters.RENDERING_SURFACE_MUTEX.ReleaseMutex();
        }

        protected void AnimationActivity(float timeElapsed) {}
    }
}
