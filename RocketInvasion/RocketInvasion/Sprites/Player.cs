﻿using System;
using RocketInvasion.Common.Sprites;

using CocosSharp;


namespace RocketInvasion.Common.Sprites
{
    public class Player : SpriteNode
    {
        float timeSinceLastLaunch;

        public Player()
        {
            this.scaleFactor = 0.7f;

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

            this.sprite.Scale = 5.5f;
            this.sprite.SpriteFrame = Animations.protagonistExplosionAnimationFrames[0];

            this.sprite.RunActionsAsync(Animations.protagonistExplosionAction, new CCCallFunc(this.Erase));
        }
    }
}