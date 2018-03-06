using CocosSharp;
using System.Collections.Generic;

using RocketInvasion.Common.Sprites;


namespace RocketInvasion
{
    public class IntroLayer : CCLayerColor
    {
        CCRenderTexture renderTexture;

        Spaceship spaceship;

        Rocket[] rocketsList;
        int rocketsListSize;

        AlienInvader[] alienInvadersList;
        int alienInvadersListSize;


        public IntroLayer() : base(CCColor4B.Black)
        {
            renderTexture = new CCRenderTexture(VisibleBoundsWorldspace.Size, VisibleBoundsWorldspace.Size * 2);

            spaceship = new Spaceship();

            spaceship.Position = new CCPoint(300, 50);
            spaceship.RocketLaunched += HandleRocketAdded;

            rocketsList = new Rocket[100];
            rocketsListSize = 0;

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


            //testing space
            //----------------------------------------------
            /*
            CCSpriteSheet spriteSheet = new CCSpriteSheet("animations/rocket_explosion.plist", "animations/rocket_explosion.png");

            List<CCSpriteFrame> animationFrames = spriteSheet.Frames.FindAll((x) => x.TextureFilename.StartsWith("frame"));


            CCAnimation explosionAnimation = new CCAnimation(animationFrames, 0.3f);
            CCAction explosionAction = new CCAnimate(explosionAnimation);

            CCSprite explosionSprite = new CCSprite(animationFrames[0]);
            explosionSprite.Scale = 1.5f;
            explosionSprite.Position = new CCPoint(300, 400);


            renderTexture.Sprite.AddChild(explosionSprite);

            explosionSprite.AddAction(explosionAction);
            */
            //----------------------------------------------


            Schedule(GameLoop);
        }

        void GameLoop(float frameTimeInSeconds)
        {
            // System.Diagnostics.Debug.WriteLine("inside game loop");

            spaceship.RocketLaunchingActivity(frameTimeInSeconds);

            // update sprites
            for (int i = 0; i < alienInvadersListSize; i++)
            {
                alienInvadersList[i].NextTurnMove();
            }

            for (int i = 0; i < rocketsListSize; i++)
            {
                rocketsList[i].NextTurnMove(); //Activity(frameTimeInSeconds);
                HandleRocketCollisions(i);
            }

        }

        private void HandleRocketCollisions(int index)
        {
            float posY = rocketsList[index].Position.Y;

            // collision with container borders 
            if (posY > 500)//this.ContentSize.Height)
            {
                rocketsList[index].Explode();

                rocketsList[index] = rocketsList[rocketsListSize - 1];
                rocketsListSize--;

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

        private void HandleRocketAdded(Rocket rocket)
        {
            rocketsList[rocketsListSize++] = rocket;

            renderTexture.Sprite.AddChild(rocket);
        }

        protected override void AddedToScene()
        {
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

