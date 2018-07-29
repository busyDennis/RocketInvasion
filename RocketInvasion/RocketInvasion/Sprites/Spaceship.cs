using System;

using CocosSharp;


namespace RocketInvasion.Sprites
{
    public abstract class Spaceship : SpriteNode
    {
        float timeSinceLastLaunch;

        public Spaceship() {
            IsLaunchingRockets = false;
            timeSinceLastLaunch = 0;
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
            if (timeSinceLastLaunch > IntervalBetweenRocketLaunches)
            {
                timeSinceLastLaunch -= IntervalBetweenRocketLaunches;

                LaunchRocket();
            }
        }

        public abstract void LaunchRocket();

        public Action<Rocket> RocketLaunched;

        public bool IsLaunchingRockets { get; set; }

        public float IntervalBetweenRocketLaunches { get; set; }
    }
}
