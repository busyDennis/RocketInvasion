using CocosSharp;
using System;
using System.Collections.Generic;


namespace RocketInvasion.Sprites
{
    class AlienHive : CCNode
    {
        public List<AlienInvader> AlienInvadersList { get; set; }

        float xMarginLeft, xMarginRight;

        public bool IsLRFloating { get; set; }
        public float LRFloatingVelocity { get; set; }

        public float PhoneScreenWidthVar { get; set; }


        public AlienHive(CCSize contentSize, Action<Rocket> alienRocketHandler) {
            this.LRFloatingVelocity = 0;

            AlienInvadersList = new List<AlienInvader>();
            int alienInvadersCount = 0;

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

        public void NextFrameUpdate() {
            if (this.IsLRFloating)
            {
                this.UpdateHiveXMargins();
                this.UpdateHiveFloatVelocityBasedOnXMargins();
            }
            else
                // set x-velocity to 0 for each alien
                UpdateFloatVelocityForEachNonAttackingAlienInvader(0);
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
            if (this.IsLRFloating) {
                if (this.LRFloatingVelocity == 0) //start floating
                    this.LRFloatingVelocity = GameParameters.ALIEN_HIVE_LR_FLOATING_VELOCITY;
                else if (this.LRFloatingVelocity < 0)
                {
                    if (xMarginLeft < 0)
                        this.LRFloatingVelocity = GameParameters.ALIEN_HIVE_LR_FLOATING_VELOCITY;
                    else
                        return;
                }
                else if (this.LRFloatingVelocity > 0)
                {
                    if (xMarginRight > this.PhoneScreenWidthVar)
                        this.LRFloatingVelocity = -GameParameters.ALIEN_HIVE_LR_FLOATING_VELOCITY;
                    else
                        return;
                }

                UpdateFloatVelocityForEachNonAttackingAlienInvader(this.LRFloatingVelocity);                    
            }
        }

        public void UpdateFloatVelocityForEachNonAttackingAlienInvader(float xVel) {
            foreach (AlienInvader alien in AlienInvadersList)
                if (! alien.IsAttacking)
                    alien.Velocity = new CCVector2(xVel, alien.Velocity.Y);
        }

        // to do:

        public void ExpandAndContract() { }

        public List<AlienInvader> getNewAttackers()
        {
            return null;
        }
    }
}
