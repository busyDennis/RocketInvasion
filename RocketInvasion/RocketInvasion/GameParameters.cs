using CocosSharp;
using System.Threading;

namespace RocketInvasion
{
    public static class GameParameters
    {
        //*********************INTRODUCTORY SCREEN*******************

        public static CCPoint BTN_NEW_GAME_POSITION = new CCPoint(300, 600);
        public static CCSize BTN_NEW_GAME_DIMENSIONS = new CCSize(250, 90);
        public static CCColor4B BTN_NEW_GAME_FILL_COLOR = CCColor4B.Yellow;
        public static CCColor4B BTN_NEW_GAME_BORDER_COLOR = CCColor4B.LightGray;
        public static CCColor3B BTN_NEW_GAME_TEXT_COLOR = CCColor3B.Black;
        public static int BTN_NEW_GAME_FONT_SIZE = 50;

        public static CCPoint BTN_LOAD_GAME_POSITION = new CCPoint(300, 400);
        public static CCSize BTN_LOAD_GAME_DIMENSIONS = new CCSize(250, 90);
        public static CCColor4B BTN_LOAD_GAME_FILL_COLOR = CCColor4B.Yellow;
        public static CCColor4B BTN_LOAD_GAME_BORDER_COLOR = CCColor4B.LightGray;
        public static CCColor3B BTN_LOAD_GAME_TEXT_COLOR = CCColor3B.Black;
        public static int BTN_LOAD_GAME_FONT_SIZE = 50;


        //*************************GAME LEVELS***********************

        // variables relevant to Player

        public const float PLAYER_PIC_SCALE_FACTOR = 0.5f;
        public const float PLAYERS_ROCKET_PIC_SCALE_FACTOR = 2.0f;
        public const float PLAYER_EXPLOSION_SCALE_FACTOR = 0.3f;

        public const float PLAYER_INFO_MINI_PIC_SCALE_FACTOR = 0.3f;

        public static CCPoint PLAYER_INITIAL_POSITION = new CCPoint(350, 50);

        public static CCVector2 PLAYERS_ROCKET_VELOCITY = new CCVector2 (0, 17);

        public const float INTERVAL_BETWEEN_PLAYERS_ROCKET_LAUNCHES = 0.5f;


        // variables relevant to AlienInvaders

        public static float ALIEN_INVADER_PIC_SCALE_FACTOR = 0.3f;
        public static float ALIEN_INVADERS_ROCKET_PIC_SCALE_FACTOR = 0.5f;

        public static CCVector2 ALIEN_INVADERS_VELOCITY = new CCVector2(0, -3);

        public static CCVector2 ALIEN_INVADERS_ROCKET_VELOCITY = new CCVector2(0, -8);

        public const float INTERVAL_BETWEEN_ALIEN_INVADER_ROCKET_LAUNCHES = 2.0f;

        public const int INTERVAL_BETWEEN_ALIEN_INVADER_ATTACKS_MS = 3000000;

        public const float ALIEN_HIVE_LR_FLOATING_VELOCITY = 0.7f;


        // miscellaneous variables

        public static CCVector2 ZERO_VELOCITY = new CCVector2(0, 0);

        public static Mutex RENDERING_SURFACE_MUTEX = new Mutex();
    }
}
