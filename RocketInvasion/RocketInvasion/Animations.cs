using CocosSharp;
using System.Collections.Generic;


namespace RocketInvasion
{
    public static class Animations
    {
        public static CCSpriteSheet rocketExplosionSpriteSheet;

        // exposed fields
        public static List<CCSpriteFrame> rocketExplosionAnimationFrames;
        public static CCFiniteTimeAction rocketExplosionAction;
        public static CCActionState rocketExplosionActionState;


        static Animations() {
            rocketExplosionSpriteSheet = new CCSpriteSheet("animations/rocket_explosion.plist", "animations/rocket_explosion.png");

            rocketExplosionAnimationFrames = rocketExplosionSpriteSheet.Frames.FindAll(x => x.TextureFilename.StartsWith("frame"));

            CCAnimation rocketExplosionAnimation = new CCAnimation(rocketExplosionAnimationFrames, 0.1f);

            rocketExplosionAction = new CCAnimate(rocketExplosionAnimation);



            // rocketExplosionActionState;

        }
    }
}
