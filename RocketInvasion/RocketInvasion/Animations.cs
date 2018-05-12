using CocosSharp;
using System.Collections.Generic;


namespace RocketInvasion
{
    public static class Animations
    {
        public static CCSpriteSheet rocketExplosionSpriteSheet,
            protagonistExplosionSpriteSheet;
        public static List<CCSpriteFrame> rocketExplosionAnimationFrames,
            protagonistExplosionAnimationFrames;
        public static CCFiniteTimeAction rocketExplosionAction,
            protagonistExplosionAction;
        public static CCActionState rocketExplosionActionState,
            protagonistExplosionActionState;


        static Animations() {
            //rocket explosion animation
            rocketExplosionSpriteSheet = new CCSpriteSheet("animations/rocket_explosion.plist", "animations/rocket_explosion.png");
            rocketExplosionAnimationFrames = rocketExplosionSpriteSheet.Frames.FindAll(x => x.TextureFilename.StartsWith("frame"));
            CCAnimation rocketExplosionAnimation = new CCAnimation(rocketExplosionAnimationFrames, 0.1f);
            rocketExplosionAction = new CCAnimate(rocketExplosionAnimation);

            //protagonist explosion animation
            protagonistExplosionSpriteSheet = new CCSpriteSheet("animations/player_explosion.plist");//, "animations/protagonist_explosion.png");
            protagonistExplosionAnimationFrames = protagonistExplosionSpriteSheet.Frames.FindAll(x => x.TextureFilename.StartsWith("frame"));
            CCAnimation protagonistExplosionAnimation = new CCAnimation(protagonistExplosionAnimationFrames, 0.028f);
            protagonistExplosionAction = new CCAnimate(protagonistExplosionAnimation);
        }
    }
}
