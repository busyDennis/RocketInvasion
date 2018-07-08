using CocosDenshion;
using CocosSharp;
using System;
using System.Threading.Tasks;

namespace RocketInvasion.Common.Sprites
{
    public class Player : Spaceship
    {
        public Player()
        {
            this.scaleFactor = GameParameters.PLAYER_PIC_SCALE_FACTOR;
            this.IntervalBetweenRocketLaunches = GameParameters.INTERVAL_BETWEEN_PLAYERS_ROCKET_LAUNCHES;

            this.Lives = 3;
            this.HealthPoints = 100;

            base.DrawSprite("spaceship");
        }

        public int Lives { get; set; }

        public int HealthPoints { get; set; }

        public bool Visibility { get { return this.sprite.Visible; } set { this.sprite.Visible = value; } }

        public override void LaunchRocket()
        {
            Rocket rocket = new PlayersRocket();
            rocket.Position = new CCPoint(this.PositionX, this.PositionY + 80);

            RocketLaunched(rocket);
        }

        public void ResetToOriginalSprite(bool visibility = true)
        {
            this.StopAllActions();
            this.RemoveAllChildren();

            this.scaleFactor = GameParameters.PLAYER_PIC_SCALE_FACTOR;
            this.DrawSprite("spaceship");
            this.sprite.Visible = visibility;
        }

        public void Explode(Action playerExplosionHandler)
        {
            this.StopAllActions();

            this.sprite.Scale = GameParameters.PLAYER_EXPLOSION_SCALE_FACTOR;
            // this.sprite.SpriteFrame = Animations.explosion1AnimationFrames[0];

            CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/playerExplosion");
            this.sprite.RunActions(Animations.explosion1Action, new CCCallFunc(() => this.PlayerExplosionHandlerOnSeparateThread(playerExplosionHandler)));

            

            //if (playerExplosionHandler != null)
              //  Task.Run(() => {
              //      playerExplosionHandler();
              //  });


            //this.sprite.RunActionsAsync(new CCCallFunc(this.Erase), new CCCallFunc(playerDeathHandler));
            //else
            //    this.sprite.RunActions(Animations.explosion1Action, new CCCallFunc(this.Erase));


        }

        private void PlayerExplosionHandlerOnSeparateThread(Action playerExplosionHandler)
        {
            if (playerExplosionHandler != null)
                Task.Run(() => {
                    playerExplosionHandler();
                });
        }

    }
}
