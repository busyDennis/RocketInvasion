using CocosDenshion;
using CocosSharp;
using System.Collections.Generic;
using System.Threading;

using RocketInvasion.Common.Sprites;


namespace RocketInvasion
{
    public class IntroLayer : CCLayerColor
    {
        CCRenderTexture renderTexture;

        Player player;

        List<Rocket> rocketsList;
        List<AlienInvader> alienInvadersList;

        bool playerIsInactive;

        public IntroLayer() : base(CCColor4B.Black)
        {
            renderTexture = new CCRenderTexture(VisibleBoundsWorldspace.Size, VisibleBoundsWorldspace.Size * 2);

            player = new Player();

            player.Position = new CCPoint(300, 50);
            player.RocketLaunched += NewRocketHandler;

            playerIsInactive = false;

            rocketsList = new List<Rocket>();

            alienInvadersList = new List<AlienInvader>();

            alienInvadersList.Add(new AlienInvader());
            alienInvadersList[0].Position = new CCPoint(500, 1000); //fix this

            renderTexture.BeginWithClear(CCColor4B.Transparent);
            this.Visit();
            renderTexture.End();

            this.AddChild(renderTexture.Sprite);

            renderTexture.Sprite.AddChild(player);
            renderTexture.Sprite.AddChild(alienInvadersList[0]);

            Schedule(GameLoop, 0.02f);
        }

        void GameLoop(float frameTimeInSeconds)
        {
            player.RocketLaunchingActivity(frameTimeInSeconds);

            // Update sprites
            for (int i = 0; i < rocketsList.Count; i++) {
                rocketsList[i].NextFrameUpdate();
            }
            for (int i = 0; i < alienInvadersList.Count; i++) {
                alienInvadersList[i].NextFrameUpdate();
            }

            // Handle collisons            
            for (int i = 0; i < rocketsList.Count; i++)
                RocketVsScreenTopCollisionHandler(i);
            for (int i = 0; i < rocketsList.Count; i++)
                RocketVsAlienInvaderCollisionHandler(i);
            if (!playerIsInactive)
                for (int i = 0; i < alienInvadersList.Count; i++)
                    PlayerVsAlienInvaderCollisionHandler(i);
        }

        private void RocketVsScreenTopCollisionHandler(int index) {
            if (rocketsList[index].Position.Y > this.ContentSize.Height) {
                Monitor.Enter(rocketsList);
                rocketsList[index].Erase();
                rocketsList.RemoveAt(index);
                Monitor.Exit(rocketsList);
            }
        }

        private void RocketVsAlienInvaderCollisionHandler(int index) {
            for (int i = 0; i < alienInvadersList.Count; i++) {
                Monitor.Enter(rocketsList);
                Monitor.Enter(alienInvadersList);
                if (rocketsList[index].sprite.BoundingBoxTransformedToWorld.IntersectsRect(alienInvadersList[i].sprite.BoundingBoxTransformedToWorld)) {
                    rocketsList[index].ExplodeAndErase();
                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/rocketExplosion");
                    /* http://soundbible.com/2004-Gun-Shot.html */
                    rocketsList.RemoveAt(index);
                }
                Monitor.Exit(rocketsList);
                Monitor.Exit(alienInvadersList);
            }
        }

        private void PlayerVsAlienInvaderCollisionHandler(int index)
        {
            for (int i = 0; i < alienInvadersList.Count; i++)
            {
                if (player.sprite.BoundingBoxTransformedToWorld.IntersectsRect(alienInvadersList[i].sprite.BoundingBoxTransformedToWorld))
                {
                    player.ExplodeAndErase();
                    playerIsInactive = true;

                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/playerExplosion");
                    /*  */

                    // handlePlayerDeath();
                }
            }
        }

        private void NewRocketHandler(Rocket rocket) {
            Monitor.Enter(rocketsList);
            rocketsList.Add(rocket);
            renderTexture.Sprite.AddChild(rocket);
            Monitor.Exit(rocketsList);
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
            var locationOnScreen = touches[0].Location;

            player.PositionX = locationOnScreen.X;

            if (!player.IsLaunchingRockets)
                player.IsLaunchingRockets = true;
        }

        void handlePlayerDeath() {
            Unschedule(GameLoop);
        }
    }
}

// Code snippets:

// System.Diagnostics.Debug.WriteLine("inside game loop");

// System.Threading.Tasks.Task.Delay(1).Wait();

//Task.Run(() => {
//        rocketsList[index].Explode();
//    }); //.ContinueWith(t => { rocketsList[index].Erase(); });