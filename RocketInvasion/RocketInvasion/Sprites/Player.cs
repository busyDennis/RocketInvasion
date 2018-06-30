using System;

using CocosSharp;


namespace RocketInvasion.Common.Sprites
{
    public class Player : SpriteNode
    {
        float timeSinceLastLaunch;

        public Player()
        {
            this.scaleFactor = 0.5f;

            IsLaunchingRockets = false;
            timeSinceLastLaunch = 0;

            base.DrawSprite("spaceship");
        }

        public void LaunchRocket()
        {
            Rocket rocket = new Rocket();

            rocket.Position = new CCPoint(this.PositionX, this.PositionY + 80);

            RocketLaunched(rocket);
        }

        public Action<Rocket> RocketLaunched;

        public bool IsLaunchingRockets
        {
            get;
            set;
        }

        public void RocketLaunchingActivity(float frameTime)
        {
            if (!this.IsLaunchingRockets)
            {
                timeSinceLastLaunch = 0;
                return;
            }

            timeSinceLastLaunch += frameTime;

            if (timeSinceLastLaunch == 0)
                LaunchRocket();
            if (timeSinceLastLaunch > GameParameters.intervalBetweenRocketLaunches)
            {
                timeSinceLastLaunch -= GameParameters.intervalBetweenRocketLaunches;

                LaunchRocket();
            }
        }

        public void ExplodeAndErase()
        {
            this.VelocityVec.Y = 0;

            this.sprite.Scale = 0.4f; //5.5f;
            this.sprite.SpriteFrame = Animations.explosion1AnimationFrames[0];
            this.sprite.RunActionsAsync(Animations.explosion1Action, new CCCallFunc(this.Erase));
        }
    }
}
