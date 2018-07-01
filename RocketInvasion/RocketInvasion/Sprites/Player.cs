using System;

using CocosSharp;


namespace RocketInvasion.Common.Sprites
{
    public class Player : Spaceship
    {
        public int lives { get; set; }
        public int healthPoints { get; set; }

        public Player()
        {
            this.scaleFactor = GameParameters.PLAYER_PIC_SCALE_FACTOR;
            this.IntervalBetweenRocketLaunches = GameParameters.INTERVAL_BETWEEN_PLAYERS_ROCKET_LAUNCHES;

            lives = 3;
            healthPoints = 100;

            base.DrawSprite("spaceship");
        }

        public override void LaunchRocket()
        {
            Rocket rocket = new PlayersRocket(); 
            rocket.Position = new CCPoint(this.PositionX, this.PositionY + 80);

            RocketLaunched(rocket);
        }

        public void ExplodeAndErase()
        {
            this.VelocityVec.Y = 0;
            this.sprite.StopAllActions();

            this.sprite.Scale = 0.3f;
            this.sprite.SpriteFrame = Animations.explosion1AnimationFrames[0];
            this.sprite.RunActionsAsync(Animations.explosion1Action, new CCCallFunc(this.Erase));
        }
    }
}
