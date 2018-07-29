using CocosSharp;

namespace RocketInvasion.Sprites
{
    public class AlienInvader : Spaceship
    {
        private CCAffineTransform affineTransform = new CCAffineTransform();

        public AlienInvader() {
            this.scaleFactor = GameParameters.ALIEN_INVADER_PIC_SCALE_FACTOR;
            this.Velocity = GameParameters.ZERO_VELOCITY;
            this.IsAttacking = false;

            affineTransform.Rotation = 0;
            this.IntervalBetweenRocketLaunches = GameParameters.INTERVAL_BETWEEN_ALIEN_INVADER_ROCKET_LAUNCHES;

            base.DrawSprite("alienSpaceship");
        }

        public override void LaunchRocket()
        {
            Rocket rocket = new AlienInvadersRocket();

            rocket.Position = new CCPoint(this.PositionX, this.PositionY - 50);

            RocketLaunched(rocket);
        }

        public void ExplodeAndErase()
        {
            this.Velocity = new CCVector2(0, 0);

            this.sprite.Scale = 3.0f;
            this.sprite.SpriteFrame = Animations.explosion3AnimationFrames[0];

            this.sprite.RunActionsAsync(Animations.explosion3Action, new CCCallFunc(this.Erase));
        }

        public bool IsAttacking
        {
            get;
            set;
        }
    }
}
