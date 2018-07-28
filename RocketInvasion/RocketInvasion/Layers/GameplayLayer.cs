using CocosDenshion;
using CocosSharp;
using System.Collections.Generic;
using System.Threading;


using RocketInvasion.Common.Sprites;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RocketInvasion.Layers
{
    public class GameplayLayer : CCLayerColor
    {
        CCRenderTexture renderTexture;

        CCSprite background;

        Player player;
        bool playerCannotMove, playerTakesNoDamage;

        AlienHive alienHive;

        int alienAttackMillis;

        List<Rocket> playersRocketList, alienInvadersRocketList;

        PlayerLifeHpDisplayNode playerLifeHpDisplay;

        public GameplayLayer() : base(CCColor4B.Black)
        {
            Animations.Init();

            renderTexture = new CCRenderTexture(VisibleBoundsWorldspace.Size, VisibleBoundsWorldspace.Size * 2);

            background = new CCSprite(new CCTexture2D("spaceBackground.png"));
            background.Position = new CCPoint(0, 0);
            background.Scale = 5f;

            player = new Player();

            player.Position = GameParameters.PLAYER_INITIAL_POSITION;
            player.RocketLaunched += NewRocketHandler;

            playerCannotMove = false;
            playerTakesNoDamage = false;

            playersRocketList = new List<Rocket>();
            alienInvadersRocketList = new List<Rocket>();

            alienAttackMillis = 0;

            alienHive = new AlienHive(this.ContentSize, NewRocketHandler);
            alienHive.IsFloatingHorizontally = true;

            playerLifeHpDisplay = new PlayerLifeHpDisplayNode(ref  player);
            playerLifeHpDisplay.Position = new CCPoint(this.ContentSize.Width, this.ContentSize.Height);

            renderTexture.BeginWithClear(CCColor4B.Transparent);
            this.Visit();
            renderTexture.End();

            this.AddChild(renderTexture.Sprite);

            renderTexture.Sprite.AddChild(background);

            renderTexture.Sprite.AddChild(player);

            foreach (AlienInvader enemy in alienHive.AlienInvadersList)
                renderTexture.Sprite.AddChild(enemy);

            renderTexture.Sprite.AddChild(playerLifeHpDisplay);

            Schedule(GameLoop, 0.02f);
        }

        private void GameLoop(float frameTimeInSeconds)
        {
            // UPDATE SPRITES

            // update player
            player.RocketLaunchingActivity(frameTimeInSeconds);

            // update rockets
            for (int i = 0; i < playersRocketList.Count; i++) {
                playersRocketList[i].NextFrameUpdate();
            }
            for (int i = 0; i < alienInvadersRocketList.Count; i++)
            {
                alienInvadersRocketList[i].NextFrameUpdate();
            }

            // update aliens
            alienHive.NextFrameupdate();
            for (int i = 0; i < alienHive.AlienInvadersList.Count; i++) {
                alienHive.AlienInvadersList[i].NextFrameUpdate();
                alienHive.AlienInvadersList[i].RocketLaunchingActivity(frameTimeInSeconds);
            }

            // HANDLE COLLISIONS         
            for (int i = 0; i < playersRocketList.Count; i++)
                RocketVsScreenTopCollisionHandler(i);
            for (int i = 0; i < playersRocketList.Count; i++)
                RocketVsAlienInvaderCollisionHandler(i);

            if (!playerTakesNoDamage)
            {
                for (int i = 0; i < alienInvadersRocketList.Count; i++)
                    PlayerVsAliensRocketCollisionHandler(i);
                for (int i = 0; i < alienHive.AlienInvadersList.Count; i++)
                    PlayerVsAlienInvaderCollisionHandler(i);
            }

            if (alienAttackMillis > GameParameters.INTERVAL_BETWEEN_ALIEN_INVADER_ATTACKS_MS) {
                alienAttackMillis = 0;
                launchAlienAttack();
            } else {
                alienAttackMillis += 20000;
            }
        }

        private void launchAlienAttack()
        {
            // preliminary version: we choose one random alien for attack

            if (alienHive.AlienInvadersList.Count == 0)
                return;

            for (int i = 0; i < alienHive.AlienInvadersList.Count; i++)
            {
                Monitor.Enter(alienHive.AlienInvadersList);
                if (!alienHive.AlienInvadersList[i].IsAttacking)
                {
                    alienHive.AlienInvadersList[i].IsAttacking = true;
                    alienHive.AlienInvadersList[i].Velocity = GameParameters.ALIEN_INVADERS_VELOCITY;
                    alienHive.AlienInvadersList[i].IsLaunchingRockets = true;
                    return;
                }
                Monitor.Exit(alienHive.AlienInvadersList);
            }
        }

        private void RocketVsScreenTopCollisionHandler(int index) {
            if (playersRocketList[index].Position.Y > this.ContentSize.Height) {
                Monitor.Enter(playersRocketList);
                playersRocketList[index].Erase();
                playersRocketList.RemoveAt(index);
                Monitor.Exit(playersRocketList);
            }
        }

        private void RocketVsAlienInvaderCollisionHandler(int index) {
            for (int i = 0; i < alienHive.AlienInvadersList.Count; i++) {
                Monitor.Enter(playersRocketList);
                Monitor.Enter(alienHive.AlienInvadersList);
                if (playersRocketList[index].sprite.BoundingBoxTransformedToWorld.IntersectsRect(alienHive.AlienInvadersList[i].sprite.BoundingBoxTransformedToWorld)) {
                    alienHive.AlienInvadersList[i].ExplodeAndErase();
                    alienHive.AlienInvadersList.RemoveAt(i);
                    playersRocketList[index].ExplodeAndErase();
                    playersRocketList.RemoveAt(index);
                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/rocketExplosion");
                    /* http://soundbible.com/2004-Gun-Shot.html */
                    break;
                }
                Monitor.Exit(playersRocketList);
                Monitor.Exit(alienHive.AlienInvadersList);
            }
        }

        private void PlayerVsAliensRocketCollisionHandler(int index) {
            Monitor.Enter(alienInvadersRocketList);
            if (alienInvadersRocketList[index].sprite.BoundingBoxTransformedToWorld.IntersectsRect(player.sprite.BoundingBoxTransformedToWorld))
            {
                alienInvadersRocketList[index].ExplodeAndErase();
                alienInvadersRocketList.RemoveAt(index);

                PlayerDies();
            }

            Monitor.Exit(alienInvadersRocketList);
        }

        private void PlayerVsAlienInvaderCollisionHandler(int index)
        {
            for (int i = 0; i < alienHive.AlienInvadersList.Count; i++)
            {
                if (player.sprite.BoundingBoxTransformedToWorld.IntersectsRect(alienHive.AlienInvadersList[i].sprite.BoundingBoxTransformedToWorld))
                {
                    alienHive.AlienInvadersList[i].ExplodeAndErase();
                    alienHive.AlienInvadersList.RemoveAt(i);

                    PlayerDies();
                }
            }
        }

        private void NewRocketHandler(Rocket rocket)
        {
            if (rocket.GetType() == typeof(PlayersRocket))
            {
                Monitor.Enter(playersRocketList);
                playersRocketList.Add(rocket);
                renderTexture.Sprite.AddChild(rocket);
                Monitor.Exit(playersRocketList);
            }
            else if (rocket.GetType() == typeof(AlienInvadersRocket))
            {
                Monitor.Enter(alienInvadersRocketList);
                alienInvadersRocketList.Add(rocket);
                renderTexture.Sprite.AddChild(rocket);
                Monitor.Exit(alienInvadersRocketList);
            }
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            var touchListener = new CCEventListenerTouchAllAtOnce();

            touchListener.OnTouchesEnded = OnTouchesEnded;
            touchListener.OnTouchesMoved = OnTouchesMoved;

            AddEventListener(touchListener, this);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            player.IsLaunchingRockets = false;
        }

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (!playerCannotMove)
            {
                var locationOnScreen = touches[0].Location;

                player.PositionX = locationOnScreen.X;

                if (!player.IsLaunchingRockets)
                    player.IsLaunchingRockets = true;
            }
        }

        void PlayerDies() {
            playerCannotMove = true;
            playerTakesNoDamage = true;

            // we run this on separate thread so that we could pause before player revival

            /*
            Task.Run(() => {
                handlePlayerDeath();
            });*/


            player.Explode(handlePlayerDeath);
        }

        private void handlePlayerDeath()
        {
            player.RestoreAndHide();
            
            //player.Visibility = false;

            // temp
            System.Diagnostics.Debug.WriteLine("player.Lives: " + player.Lives);

            // update player life counter
            if (player.Lives > 0)
            {
                // update player life counter
                player.Lives = player.Lives - 1;
                playerLifeHpDisplay.UpdateSpriteHolder();
                playerLifeHpDisplay.DrawSprite();
            }

            // temp
            System.Diagnostics.Debug.WriteLine("player.Lives after the update: " + player.Lives);

            // revive the player
            if (player.Lives > 0) {
                // clean up explosion trace
                // player.StopAllActions();

                // pause before the player revives
                System.Threading.Tasks.Task.Delay(2000).Wait();

                // revive the player
                player.Position = GameParameters.PLAYER_INITIAL_POSITION;
                playerCannotMove = false;
                player.EmergeGradually();
                playerTakesNoDamage = false;
            }
            else // game over
            {
                Unschedule(GameLoop);
            }

        }
    }
}

// Code snippets:

// System.Diagnostics.Debug.WriteLine("inside game loop");

// System.Threading.Tasks.Task.Delay(1).Wait();

//Task.Run(() => {
//        playersRocketList[index].Explode();
//    }); //.ContinueWith(t => { playersRocketList[index].Erase(); });

//Stopwatch sw = new Stopwatch(); // sw cotructor
//sw.Start();
//while (sw.ElapsedMilliseconds < 5000) ;
//sw.Stop();
