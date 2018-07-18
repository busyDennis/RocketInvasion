﻿using CocosSharp;
using System.Threading;

namespace RocketInvasion
{
    public static class GameParameters
    {
        // variables relevant to Player

        public const float PLAYER_PIC_SCALE_FACTOR = 0.5f;
        public const float PLAYERS_ROCKET_PIC_SCALE_FACTOR = 2.0f;
        public const float PLAYER_EXPLOSION_SCALE_FACTOR = 0.3f;

        public const float PLAYER_INFO_MINI_PIC_SCALE_FACTOR = 0.3f;

        public static CCPoint PLAYER_INITIAL_POSITION = new CCPoint(350, 50);

        public static CCVector2 PLAYERS_ROCKET_VELOCITY = new CCVector2 (0, 17);

        public const float INTERVAL_BETWEEN_PLAYERS_ROCKET_LAUNCHES = 0.5f;


        // variables relevant to AlienInvader

        public static float ALIEN_INVADER_PIC_SCALE_FACTOR = 0.3f;
        public static float ALIEN_INVADERS_ROCKET_PIC_SCALE_FACTOR = 0.5f;

        public static CCVector2 ALIEN_INVADERS_VELOCITY = new CCVector2(0, -3);

        public static CCVector2 ALIEN_INVADERS_ROCKET_VELOCITY = new CCVector2(0, -8);

        public const float INTERVAL_BETWEEN_ALIEN_INVADER_ROCKET_LAUNCHES = 2.0f;

        public const int INTERVAL_BETWEEN_ALIEN_INVADER_ATTACKS_MS = 3000000;


        // miscellaneous variables

        public static CCVector2 ZERO_VELOCITY = new CCVector2(0, 0);

        public static Mutex RENDERING_SURFACE_MUTEX = new Mutex();
    }
}
