using CocosSharp;


namespace RocketInvasion.Common.Sprites
{
    public class Rocket : SpriteNode
    {
        public Rocket() {
            this.scaleFactor = 0.2f;
            this.VelocityVec = GameParameters.rocketVelocity;

            base.DrawSprite("rocket");
        }

        public void ExplodeAndErase() {
            this.VelocityVec.Y = 0;

            this.sprite.Scale = 2.5f;
            this.sprite.SpriteFrame = Animations.rocketExplosionAnimationFrames[0];

            this.sprite.RunActionsAsync(Animations.rocketExplosionAction, new CCCallFunc(this.Erase));
        }
    }
}
