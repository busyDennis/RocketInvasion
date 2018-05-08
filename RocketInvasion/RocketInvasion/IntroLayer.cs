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

        Spaceship spaceship;

        List<Rocket> rocketsList;
        List<AlienInvader> alienInvadersList;


        public IntroLayer() : base(CCColor4B.Black)
        {
            renderTexture = new CCRenderTexture(VisibleBoundsWorldspace.Size, VisibleBoundsWorldspace.Size * 2);

            spaceship = new Spaceship();

            spaceship.Position = new CCPoint(300, 50);
            spaceship.RocketLaunched += NewRocketHandler;

            rocketsList = new List<Rocket>();

            alienInvadersList = new List<AlienInvader>();

            alienInvadersList.Add(new AlienInvader());
            alienInvadersList[0].Position = new CCPoint(500, 1000); //fix this

            renderTexture.BeginWithClear(CCColor4B.Transparent);
            this.Visit();
            renderTexture.End();

            this.AddChild(renderTexture.Sprite);

            renderTexture.Sprite.AddChild(spaceship);
            renderTexture.Sprite.AddChild(alienInvadersList[0]);

            Schedule(GameLoop, 0.02f);
        }

        void GameLoop(float frameTimeInSeconds)
        {
            spaceship.RocketLaunchingActivity(frameTimeInSeconds);

            // Update sprites

            for (int i = 0; i < rocketsList.Count; i++) {
                rocketsList[i].NextFrameUpdate();
            }

            for (int i = 0; i < alienInvadersList.Count; i++) {
                alienInvadersList[i].NextFrameUpdate();
            }

            // Handle collisons

            // (1) Rocket collisions - replace with enemy collisions
            
            for (int i = 0; i < rocketsList.Count; i++)
                RocketVsScreenTopCollisionHandler(i);

            for (int i = 0; i < rocketsList.Count; i++)
                RocketVsEnemyCollisionHandler(i);

            // (2) Player collisions


        }

        private void RocketVsScreenTopCollisionHandler(int index) {
            if (rocketsList[index].Position.Y > this.ContentSize.Height) {
                Monitor.Enter(rocketsList);
                rocketsList[index].Erase();
                rocketsList.RemoveAt(index);
                Monitor.Exit(rocketsList);
            }
        }

        private void RocketVsEnemyCollisionHandler(int index) {
            for (int i = 0; i < alienInvadersList.Count; i++) {

                Monitor.Enter(rocketsList);

                if (rocketsList[index].sprite.BoundingBoxTransformedToWorld.IntersectsRect(alienInvadersList[i].sprite.BoundingBoxTransformedToWorld)) {
                    rocketsList[index].ExplodeAndErase();
                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/rocketExplosion");
                    /* http://soundbible.com/tags-gun.html */
                    rocketsList.RemoveAt(index);
                }

                Monitor.Exit(rocketsList);
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
            spaceship.IsLaunchingRockets = false;
        }

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            var locationOnScreen = touches[0].Location;

            spaceship.PositionX = locationOnScreen.X;

            if (!spaceship.IsLaunchingRockets)
                spaceship.IsLaunchingRockets = true;
        }
    }
}

// Code snippets:

// System.Diagnostics.Debug.WriteLine("inside game loop");

// System.Threading.Tasks.Task.Delay(1).Wait();

//Task.Run(() => {
//        rocketsList[index].Explode();
//    }); //.ContinueWith(t => { rocketsList[index].Erase(); });