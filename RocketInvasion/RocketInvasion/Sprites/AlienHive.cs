using CocosSharp;
using System;
using System.Collections.Generic;
using System.Threading;


namespace RocketInvasion.Sprites
{
    public class AlienHive : CCNode
    {
        public List<AlienInvader> AlienInvadersList { get; set; }

        float xMarginLeft, xMarginRight;

        CCPoint[,] slots;

        /* "LR floating" - left-to-right floating */
        public bool IsLRFloating { get; set; }
        public float LRFloatingVelocity { get; set; }
        public float PhoneScreenWidthVar { get; set; }

        private Random randomGenerator = new Random();

        public AlienHive(CCSize contentSize, Action<Rocket> alienRocketHandler) {
            this.LRFloatingVelocity = 0;

            AlienInvadersList = new List<AlienInvader>();
            int alienInvadersCount = 0;

            slots = new CCPoint[2, 6];

            int row = 0, column = 0;
            
            for (int j = 940; j <= 1000; j += 60) // height
            {
                column = 0;
                for (int i = 250; i <= 550; i += 60) // width
                {
                    slots[row, column] = new CCPoint(i, j);

                    AlienInvadersList.Add(new AlienInvader());

                    AlienInvadersList[alienInvadersCount].SetBehaviorDormantInHive();
                    AlienInvadersList[alienInvadersCount].AlienHive = this;
                    AlienInvadersList[alienInvadersCount].PositionInHive = new Tuple<int, int>(row, column);
                    AlienInvadersList[alienInvadersCount].Position = new CCPoint(i, j);
                    AlienInvadersList[alienInvadersCount++].RocketLaunched += alienRocketHandler;

                    column++;
                }
                row++;
            }

            xMarginLeft = 250f;
            xMarginRight = 550f;
            
        }

        public void NextFrameUpdate() {
            if (this.IsLRFloating)
            {
                this.UpdateHiveXMargins();
                this.UpdateLRFloatVelocityBasedOnXMargins();
                this.UpdateSlotPositions();
            }
            else
                // set x-velocity to 0 for each alien
                UpdateFloatVelocityForEachAlienInvader(0);
        }

        public void UpdateHiveXMargins()
        {
            xMarginLeft += this.LRFloatingVelocity;
            xMarginRight += this.LRFloatingVelocity;
        }

        public void UpdateLRFloatVelocityBasedOnXMargins() {
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

                // UpdateFloatVelocityForEachAlienInvader(this.LRFloatingVelocity);                    
            }
        }
        public void UpdateFloatVelocityForEachAlienInvader(float xVel) {
            foreach (AlienInvader alien in AlienInvadersList)
                if (! alien.IsAttacking)
                    alien.Velocity = new CCVector2(xVel, alien.Velocity.Y);
                //else
                    //alien.ParkingSpotVelocity = new CCVector2(xVel, 0);
        }

        public void UpdateSlotPositions() {
            for (int row = 0; row < slots.GetLength(0); row++)
            {
                for (int col = 0; col < slots.GetLength(1); col++)
                {
                    slots[row, col] += new CCPoint(this.LRFloatingVelocity, 0);
                }
            }
        }

        public CCPoint GetPositionOfSlot(Tuple<int, int> slotAddress)
        {
            return new CCPoint(slots[slotAddress.Item1, slotAddress.Item2]);
        }

        public void LaunchAlienAttack() {
            // preliminary version: we choose one random alien for an attack

            if (AlienInvadersList.Count == 0)
                return;

            int randIndex = randomGenerator.Next(0, AlienInvadersList.Count - 1);
            int steps = 1;

            Monitor.Enter(AlienInvadersList);
            // skipping all the attacking alien invaders
            while (AlienInvadersList[randIndex].IsAttacking == true) {
                if(steps == AlienInvadersList.Count)
                {
                    Monitor.Exit(AlienInvadersList);
                    return;
                }
                if (randIndex == 0)
                    randIndex = AlienInvadersList.Count - 1;
                else
                    randIndex--;
                steps++;
            }

            AlienInvadersList[randIndex].IsAttacking = true;
            AlienInvadersList[randIndex].SetBehaviorSteer();
            AlienInvadersList[randIndex].IsLaunchingRockets = true;

            Monitor.Exit(AlienInvadersList);

            /*
            for (int i = 0; i < AlienInvadersList.Count; i++)
            {
                Monitor.Enter(this.AlienInvadersList);
                if (!AlienInvadersList[i].IsAttacking)
                {
                    AlienInvadersList[i].IsAttacking = true;
                    AlienInvadersList[i].SetBehaviorSteer();
                    AlienInvadersList[i].IsLaunchingRockets = true;
                    Monitor.Exit(AlienInvadersList);
                    return;
                }
                Monitor.Exit(AlienInvadersList);
            }
            */
        }

        // to do:

        public void ExpandAndContract() { }

        public List<AlienInvader> getNewAttackers()
        {
            return null;
        }
    }
}
