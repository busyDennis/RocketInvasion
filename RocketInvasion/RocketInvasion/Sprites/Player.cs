using CocosDenshion;
using CocosSharp;
using System;
using System.Threading.Tasks;

namespace RocketInvasion.Sprites
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

        public new int HealthPoints { get; set; }

        public bool IsVisible { get { return this.sprite.Visible; } set { this.sprite.Visible = value; } }

        public bool IsTransparent { get { return this.IsTransparent; } set { this.IsTransparent = value; } }

        public override void LaunchRocket()
        {
            Rocket rocket = new PlayersRocket();
            rocket.Position = new CCPoint(this.PositionX, this.PositionY + 80);

            RocketLaunched(rocket);
        }

        public void RestoreAndHide()
        {
            this.StopAllActions();
            this.RemoveAllChildren();

            this.scaleFactor = GameParameters.PLAYER_PIC_SCALE_FACTOR;
            this.DrawSprite("spaceship");
            this.sprite.Visible = false;
        }

        public void EmergeGradually() {
            this.IsVisible = true;

            for (int i = 0; i < 7; i++)
            {
                for (byte j = 200; j > 100; j--)
                {
                    this.sprite.Opacity = j;

                    System.Threading.Tasks.Task.Delay(2).Wait();
                }

                for (byte j = 100; j < 200; j++)
                {
                    this.sprite.Opacity = j;

                    System.Threading.Tasks.Task.Delay(2).Wait();
                }
            }

            this.sprite.Opacity = 255;
        }

        public void Explode(Action playerExplosionHandler)
        {
            this.StopAllActions();

            this.sprite.Scale = GameParameters.PLAYER_EXPLOSION_SCALE_FACTOR;
            this.sprite.Opacity = 255;
            // this.sprite.SpriteFrame = Animations.explosion1AnimationFrames[0];

            CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/playerExplosion");
            this.sprite.RunActions(Animations.explosion1Action, new CCCallFunc(() => this.PlayerExplosionHandlerOnSeparateThread(playerExplosionHandler)));

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
