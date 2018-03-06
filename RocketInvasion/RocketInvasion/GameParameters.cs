using CocosSharp;
using System.Threading;

namespace RocketInvasion
{
    public static class GameParameters
    {
        public const float intervalBetweenRocketLaunches = 0.5f;

        public static CCVector2 rocketVelocity = new CCVector2 (0, 10);

        public static CCVector2 alienInvaderVelocity = new CCVector2 (0, -2);

        public static Mutex mutex = new Mutex();
    }
}
