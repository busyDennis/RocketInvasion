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

        List<Rocket> rocketList;
        List<AlienInvader> alienInvaderList;

        bool playerIsInactive;

        public IntroLayer() : base(CCColor4B.Black)
        {
            Animations.Init();

            renderTexture = new CCRenderTexture(VisibleBoundsWorldspace.Size, VisibleBoundsWorldspace.Size * 2);

            player = new Player();

            player.Position = new CCPoint(300, 50);
            player.RocketLaunched += NewRocketHandler;

            playerIsInactive = false;

            rocketList = new List<Rocket>();

            alienInvaderList = new List<AlienInvader>();

            int alienInvaderCount = 0;

            for (int j = 940; j <= 1000; j += 60) {
                for (int i = 250; i <= 550; i += 60) {
                    alienInvaderList.Add(new AlienInvader());
                    alienInvaderList[alienInvaderCount++].Position = new CCPoint(i, j);
                }
            }

            // alienInvaderList.Add(new AlienInvader());
            // alienInvaderList[0].Position = new CCPoint(500, 1000);

            renderTexture.BeginWithClear(CCColor4B.Transparent);
            this.Visit();
            renderTexture.End();

            this.AddChild(renderTexture.Sprite);

            renderTexture.Sprite.AddChild(player);

            foreach (AlienInvader enemy in alienInvaderList)
                renderTexture.Sprite.AddChild(enemy);

            Schedule(GameLoop, 0.02f);
        }

        void GameLoop(float frameTimeInSeconds)
        {
            player.RocketLaunchingActivity(frameTimeInSeconds);

            // Update sprites
            for (int i = 0; i < rocketList.Count; i++) {
                rocketList[i].NextFrameUpdate();
            }
            for (int i = 0; i < alienInvaderList.Count; i++) {
                alienInvaderList[i].NextFrameUpdate();
            }

            // Handle collisons            
            for (int i = 0; i < rocketList.Count; i++)
                RocketVsScreenTopCollisionHandler(i);
            for (int i = 0; i < rocketList.Count; i++)
                RocketVsAlienInvaderCollisionHandler(i);
            if (!playerIsInactive)
                for (int i = 0; i < alienInvaderList.Count; i++)
                    PlayerVsAlienInvaderCollisionHandler(i);
        }

        private void RocketVsScreenTopCollisionHandler(int index) {
            if (rocketList[index].Position.Y > this.ContentSize.Height) {
                Monitor.Enter(rocketList);
                rocketList[index].Erase();
                rocketList.RemoveAt(index);
                Monitor.Exit(rocketList);
            }
        }

        private void RocketVsAlienInvaderCollisionHandler(int index) {
            for (int i = 0; i < alienInvaderList.Count; i++) {
                Monitor.Enter(rocketList);
                Monitor.Enter(alienInvaderList);
                if (rocketList[index].sprite.BoundingBoxTransformedToWorld.IntersectsRect(alienInvaderList[i].sprite.BoundingBoxTransformedToWorld)) {
                    alienInvaderList[i].ExplodeAndErase();
                    alienInvaderList.RemoveAt(i);
                    rocketList[index].ExplodeAndErase();
                    rocketList.RemoveAt(index);
                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/rocketExplosion");
                    /* http://soundbible.com/2004-Gun-Shot.html */
                    break;
                }
                Monitor.Exit(rocketList);
                Monitor.Exit(alienInvaderList);
            }
        }

        private void PlayerVsAlienInvaderCollisionHandler(int index)
        {
            for (int i = 0; i < alienInvaderList.Count; i++)
            {
                if (player.sprite.BoundingBoxTransformedToWorld.IntersectsRect(alienInvaderList[i].sprite.BoundingBoxTransformedToWorld))
                {
                    player.ExplodeAndErase();
                    playerIsInactive = true;

                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/playerExplosion");

                    playerDies();
                }
            }
        }

        private void NewRocketHandler(Rocket rocket) {
            Monitor.Enter(rocketList);
            rocketList.Add(rocket);
            renderTexture.Sprite.AddChild(rocket);
            Monitor.Exit(rocketList);
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

        void playerDies() {
            Unschedule(GameLoop);
        }
    }
}

// Code snippets:

// System.Diagnostics.Debug.WriteLine("inside game loop");

// System.Threading.Tasks.Task.Delay(1).Wait();

//Task.Run(() => {
//        rocketList[index].Explode();
//    }); //.ContinueWith(t => { rocketList[index].Erase(); });