using CocosSharp;


namespace RocketInvasion.Sprites
{
    class AlienInvadersRocket : Rocket
    {
        public AlienInvadersRocket()
        {
            this.scaleFactor = GameParameters.ALIEN_INVADERS_ROCKET_PIC_SCALE_FACTOR;
            this.Velocity = GameParameters.ALIEN_INVADERS_ROCKET_VELOCITY;

            base.DrawSprite("aliendropping0001");

            this.sprite.SpriteFrame = Animations.alienBombAnimationFrames[0];

            this.sprite.RunAction(Animations.alienBombAction);
        }

        public override void ExplodeAndErase()
        {
            this.Velocity = new CCVector2(this.Velocity.X, 0);
            this.sprite.StopAllActions();

            this.sprite.Scale = 2.0f;
            this.sprite.SpriteFrame = Animations.rocketExplosionAnimationFrames[0];

            this.sprite.RunActionsAsync(Animations.rocketExplosionAction, new CCCallFunc(this.Erase));
        }
    }
}
