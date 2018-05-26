using CocosSharp;

namespace RocketInvasion.Common.Sprites
{
    public class AlienInvader : SpriteNode
    {
        private CCAffineTransform affineTransform = new CCAffineTransform();

        public AlienInvader() {
            this.scaleFactor = 0.5f;
            this.VelocityVec = GameParameters.alienInvaderVelocity;

            affineTransform.Rotation = 0;

            base.DrawSprite("alienSpaceship");
        }

        new public void NextFrameUpdate() {
            GameParameters.renderingSurfaceMutex.WaitOne();
            this.Position += new CCPoint(this.VelocityVec.X, this.VelocityVec.Y);
            GameParameters.renderingSurfaceMutex.ReleaseMutex();
        }

        public void ExplodeAndErase()
        {
            this.VelocityVec.Y = 0;

            this.sprite.Scale = 3.0f;
            this.sprite.SpriteFrame = Animations.explosion3AnimationFrames[0];

            this.sprite.RunActionsAsync(Animations.explosion3Action, new CCCallFunc(this.Erase));
        }

        public void LaunchRocket() {

        }
    }
}
