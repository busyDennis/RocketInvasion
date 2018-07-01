using CocosSharp;
using System.Collections.Generic;


namespace RocketInvasion
{
    public static class Animations
    {
        public static CCSpriteSheet rocketExplosionSpriteSheet,
            explosion1SpriteSheet, explosion2SpriteSheet, explosion3SpriteSheet, alienBombSpriteSheet;
        public static List<CCSpriteFrame> rocketExplosionAnimationFrames,
            explosion1AnimationFrames, explosion2AnimationFrames, explosion3AnimationFrames, alienBombAnimationFrames;
        public static CCFiniteTimeAction rocketExplosionAction,
            explosion1Action, explosion2Action, explosion3Action, alienBombAction;
        public static CCAnimation alienBombAnimation;

        public static void Init() {
            // EXPLOSIONS

            // rocket explosion animation
            rocketExplosionSpriteSheet = new CCSpriteSheet("animations/rocket_explosion.plist", "animations/rocket_explosion.xnb");
            rocketExplosionAnimationFrames = rocketExplosionSpriteSheet.Frames.FindAll(x => x.TextureFilename.StartsWith("frame"));
            CCAnimation rocketExplosionAnimation = new CCAnimation(rocketExplosionAnimationFrames, 0.1f);
            rocketExplosionAction = new CCAnimate(rocketExplosionAnimation);

            // explosion_1 animation
            explosion1SpriteSheet = new CCSpriteSheet("animations/explosion_1.plist", "animations/explosion_1.xnb");
            explosion1AnimationFrames = explosion1SpriteSheet.Frames.FindAll(x => x.TextureFilename.StartsWith("frame"));
            CCAnimation explosion1Animation = new CCAnimation(explosion1AnimationFrames, 0.1f);
            explosion1Action = new CCAnimate(explosion1Animation);

            // explosion_2 animation
            explosion2SpriteSheet = new CCSpriteSheet("animations/explosion_2.plist", "animations/explosion_2.xnb");
            explosion2AnimationFrames = explosion2SpriteSheet.Frames.FindAll(x => x.TextureFilename.StartsWith("frame"));
            CCAnimation explosion2Animation = new CCAnimation(explosion2AnimationFrames, 0.1f);
            explosion2Action = new CCAnimate(explosion2Animation);

            // explosion_3 animation
            explosion3SpriteSheet = new CCSpriteSheet("animations/explosion_3.plist", "animations/explosion_3.xnb");
            explosion3AnimationFrames = explosion3SpriteSheet.Frames.FindAll(x => x.TextureFilename.StartsWith("frame"));
            CCAnimation explosion3Animation = new CCAnimation(explosion3AnimationFrames, 0.12f);
            explosion3Action = new CCAnimate(explosion3Animation);


            // Alien bomb
            alienBombSpriteSheet = new CCSpriteSheet("animations/bomb_gimped.plist", "animations/bomb_gimped.xnb");
            alienBombAnimationFrames = alienBombSpriteSheet.Frames.FindAll(x => x.TextureFilename.StartsWith("frame"));
            alienBombAnimation = new CCAnimation(alienBombAnimationFrames, 0.08f);
            alienBombAction = new CCRepeatForever(new CCAnimate(alienBombAnimation));
            // alienBombAction = new CCAnimate(alienBombAnimation);
        }
    }
}
