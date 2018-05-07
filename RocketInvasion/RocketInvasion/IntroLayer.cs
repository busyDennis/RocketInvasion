using CocosSharp;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using RocketInvasion.Common.Sprites;


namespace RocketInvasion
{
    public class IntroLayer : CCLayerColor
    {
        CCRenderTexture renderTexture;

        Spaceship spaceship;

        List<Rocket> rocketsList;
        // int rocketsListSize;

        AlienInvader[] alienInvadersList;
        int alienInvadersListSize;


        public IntroLayer() : base(CCColor4B.Black)
        {
            renderTexture = new CCRenderTexture(VisibleBoundsWorldspace.Size, VisibleBoundsWorldspace.Size * 2);

            spaceship = new Spaceship();

            spaceship.Position = new CCPoint(300, 50);
            spaceship.RocketLaunched += NewRocketHandler;

            rocketsList = new List<Rocket>();

            alienInvadersList = new AlienInvader[100];

            alienInvadersList[0] = new AlienInvader();
            alienInvadersListSize = 1;

            alienInvadersList[0].Position = new CCPoint(500, 1000);

            renderTexture.BeginWithClear(CCColor4B.Transparent);
            this.Visit();
            renderTexture.End();

            this.AddChild(renderTexture.Sprite);

            renderTexture.Sprite.AddChild(alienInvadersList[0]);

            renderTexture.Sprite.AddChild(spaceship);


            Schedule(GameLoop, 0.02f);
        }

        void GameLoop(float frameTimeInSeconds)
        {
            // System.Diagnostics.Debug.WriteLine("inside game loop");

            spaceship.RocketLaunchingActivity(frameTimeInSeconds);

            // update sprites

            for (int i = 0; i < rocketsList.Count; i++) {
                rocketsList[i].NextFrameUpdate();
            }

            for (int i = 0; i < alienInvadersListSize; i++) {
                alienInvadersList[i].NextFrameUpdate();
            }

            // Handle collisons

            // (1) Rocket collisions - replace with enemy collisions
            
            for (int i = 0; i < rocketsList.Count; i++) {
                RocketVsEnemyCollisionHandler(i);
            }
            

            // (2) Player collisions


        }

        private void RocketVsEnemyCollisionHandler(int index) {
            // collision with container borders 
            if (rocketsList[index].Position.Y > 500) { //this.ContentSize.Height)
                System.Diagnostics.Debug.WriteLine(rocketsList[index].Position.Y);

                //Task.Run(() => {
                //        rocketsList[index].Explode();
                //    }); //.ContinueWith(t => { rocketsList[index].Erase(); });

                Monitor.Enter(rocketsList);
                rocketsList[index].Explode();
                rocketsList.RemoveAt(index);
                Monitor.Exit(rocketsList);

                // CCAudioEngine.SharedEngine.PlayEffect("RocketExplosion");
                // System.Diagnostics.Debug.WriteLine(this.ChildrenCount);
            }

            /*
            for (int i = 0; i < alienInvadersListSize; i++)
            {
                if (rocketsList[index].sprite.TextureRectInPixels.IntersectsRect(alienInvadersList[i].sprite.TextureRectInPixels)) {
                    rocketsList[index].Explode();
                }
            }
            */
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

        void OnTouchesMoved(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
        {
            var locationOnScreen = touches[0].Location;

            spaceship.PositionX = locationOnScreen.X;

            if (!spaceship.IsLaunchingRockets)
                spaceship.IsLaunchingRockets = true;
        }
    }
}

