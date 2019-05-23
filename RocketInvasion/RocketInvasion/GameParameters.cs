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
        public static CCPoint PLAYER_INITIAL_POSITION = new CCPoint(350, 50);
        public const float PLAYER_PIC_SCALE_FACTOR = 0.5f;
        public const float PLAYER_EXPLOSION_SCALE_FACTOR = 0.3f;

        // variables relevant to Player's rocket launching activity
        public const float PLAYERS_ROCKET_PIC_SCALE_FACTOR = 2.0f;
        public static CCVector2 PLAYERS_ROCKET_VELOCITY = new CCVector2(0, 35);
        public const float INTERVAL_BETWEEN_PLAYERS_ROCKET_LAUNCHES = 0.9f;

        // variables relevant to Player's life and HP display
        public const float PLAYER_INFO_MINI_PIC_SCALE_FACTOR = 0.3f;

        // variables relevant to AlienHive
        public const float ALIEN_HIVE_LR_FLOATING_VELOCITY = 1.2f;

        // variables relevant to AlienInvader
        public static float ALIEN_INVADER_PIC_SCALE_FACTOR = 0.25f;
        public static int ALIEN_INVADER_VELOCITY_VAL = 14;
        public const int INTERVAL_BETWEEN_ALIEN_INVADER_ATTACKS_MS = 3000000;
        public enum AlienTrajectoryPattern { StraightToDest, Steer, Primitive };
        public const int ALIEN_INVADER_STEERING_ANGULAR_INCREMENT = 10; // deg
        public const int ALIEN_INVADER_TIME_INTERVAL_FOR_ONE_STEERING_STEP_IN_NUM_FRAMES = 0;
        public const int ALIEN_INVADER_MAX_TIME_INTERVAL_FOR_MOVING_STAGE_IN_NUM_FRAMES = 25;

        // variables relevant to AlienInvaders' rocket launching activity
        public static float ALIEN_INVADERS_ROCKET_PIC_SCALE_FACTOR = 0.5f;
        public static CCVector2 ALIEN_INVADERS_ROCKET_VELOCITY = new CCVector2(0, -8);
        public const float INTERVAL_BETWEEN_ALIEN_INVADER_ROCKET_LAUNCHES = 2.0f;

        // variables relevant to SpaceBackground and Star
        public const string SPACE_BACKGROUND_TEXTURE_FILE_NAME = "spaceBackground.png";
        public const float STAR_BLINKING_INTERVAL = 0.8f;
        public static CCVector2 STARS_MOVING_VELOCITY = new CCVector2(0, -15);

        //********************MISCELLANEOUS VARIABLES*******************

        public static float ANIMATION_FRAME_CHANGE_INTERVAL_SECONDS = 0.0417f; // 24 frames per second
        public static CCVector2 ZERO_VELOCITY = new CCVector2(0, 0);
        public static Mutex RENDERING_SURFACE_MUTEX = new Mutex();
    }
}
