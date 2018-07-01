using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;


namespace RocketInvasion.Common.Sprites
{
    class AlienHive : CCNode
    {
        public AlienHive(CCSize contentSize, Action<Rocket> alienRocketHandler) {
            int alienInvaderCount = 0;

            AlienInvadersList = new List<AlienInvader>();

            for (int j = 940; j <= 1000; j += 60) // height
            {
                for (int i = 250; i <= 550; i += 60)
                // width
                {
                    AlienInvadersList.Add(new AlienInvader());
                    AlienInvadersList[alienInvaderCount].Position = new CCPoint(i, j);
                    AlienInvadersList[alienInvaderCount++].RocketLaunched += alienRocketHandler;
                }
            }
        }

        public List<AlienInvader> AlienInvadersList { get; set; }

        public List<AlienInvader> getNewAttackers()
        {
            return null;
        }
    }
}
