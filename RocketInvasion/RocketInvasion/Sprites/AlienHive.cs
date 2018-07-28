using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;


namespace RocketInvasion.Common.Sprites
{
    class AlienHive : CCNode
    {
        public List<AlienInvader> AlienInvadersList { get; set; }

        float xMarginLeft, xMarginRight;
        CCSize contentSize;

        public bool IsFloatingHorizontally { get; set; }
        public float FloatVelocity { get; set; }


        public AlienHive(CCSize contentSize, Action<Rocket> alienRocketHandler) {
            this.FloatVelocity = 0;

            AlienInvadersList = new List<AlienInvader>();
            int alienInvadersCount = 0;

            this.contentSize = contentSize;

            for (int j = 940; j <= 1000; j += 60) // height
            {
                for (int i = 250; i <= 550; i += 60) // width
                {
                    AlienInvadersList.Add(new AlienInvader());
                    AlienInvadersList[alienInvadersCount].Position = new CCPoint(i, j);
                    AlienInvadersList[alienInvadersCount++].RocketLaunched += alienRocketHandler;
                }
            }
        }

        public void NextFrameupdate() {
            if (this.IsFloatingHorizontally)
            {
                this.UpdateHiveXMargins();
                this.UpdateHiveFloatVelocityBasedOnXMargins();
            }
            else
                // set x-velocity to 0 for each alien
                UpdateFloatVelocityForEachAlienInvader(0);
        }

        public void UpdateHiveXMargins()
        {
            if (AlienInvadersList.Count == 0)
                return;

            float minX = AlienInvadersList[0].PositionX, maxX = AlienInvadersList[0].PositionX;

            foreach (AlienInvader alien in AlienInvadersList)
            {
                if (minX > alien.PositionX)
                    minX = alien.PositionX;
                if (maxX < alien.PositionX)
                    maxX = alien.PositionX;
            }

            this.xMarginLeft = minX;
            this.xMarginRight = maxX;
        }

        public void UpdateHiveFloatVelocityBasedOnXMargins() {
            if (this.IsFloatingHorizontally) {
                if (this.FloatVelocity == 0) //start floating
                    this.FloatVelocity = 0.5f;
                else if (this.FloatVelocity < 0)
                {
                    if (xMarginLeft < 0)
                        this.FloatVelocity = 0.5f;
                    else
                        return;
                }
                else if (this.FloatVelocity > 0)
                {
                    if (xMarginRight > contentSize.Width)
                        this.FloatVelocity = -0.5f;
                    else
                        return;
                }

                UpdateFloatVelocityForEachAlienInvader(this.FloatVelocity);                    
            }
        }

        public void UpdateFloatVelocityForEachAlienInvader(float xVel) {
            foreach (AlienInvader alien in AlienInvadersList)
                alien.Velocity = new CCVector2(xVel, alien.Velocity.X);
        }

        // to do:

        public void ExpandAndContract() { }

        public List<AlienInvader> getNewAttackers()
        {
            return null;
        }
    }
}
