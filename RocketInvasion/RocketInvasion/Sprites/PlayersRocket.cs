using CocosSharp;


namespace RocketInvasion.Sprites
{
    class PlayersRocket : Rocket
    {
        public PlayersRocket()
        {
            this.scaleFactor = 0.15f;
            this.Velocity = GameParameters.PLAYERS_ROCKET_VELOCITY;

            base.DrawSprite("rocket");
        }

        public override void ExplodeAndErase()
        {
            this.Velocity = new CCVector2(this.Velocity.X, 0);

            this.sprite.Scale = GameParameters.PLAYERS_ROCKET_PIC_SCALE_FACTOR;
            this.sprite.SpriteFrame = Animations.rocketExplosionAnimationFrames[0];
            this.sprite.RunActionsAsync(Animations.rocketExplosionAction, new CCCallFunc(this.Erase));
        }
    }
}
