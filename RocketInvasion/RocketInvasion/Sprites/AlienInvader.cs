using CocosSharp;
using System;

namespace RocketInvasion.Sprites
{
    public class AlienInvader : Spaceship
    {
        private CCAffineTransform affineTransform = new CCAffineTransform();
        private GameParameters.AlienTrajectoryPattern trajectoryPattern;
        private CCPoint destinationPoint;


        public AlienInvader() {
            this.scaleFactor = GameParameters.ALIEN_INVADER_PIC_SCALE_FACTOR;
            this.Velocity = GameParameters.ZERO_VELOCITY;
            this.IsAttacking = false;

            affineTransform.Rotation = 0;
            this.IntervalBetweenRocketLaunches = GameParameters.INTERVAL_BETWEEN_ALIEN_INVADER_ROCKET_LAUNCHES;

            this.trajectoryPattern = GameParameters.AlienTrajectoryPattern.Primitive;

            base.DrawSprite("alienSpaceship");
        }

        public override void LaunchRocket()
        {
            Rocket rocket = new AlienInvadersRocket();

            rocket.Position = new CCPoint(this.PositionX, this.PositionY - 50);

            RocketLaunched(rocket);
        }

        public void ExplodeAndErase()
        {
            this.Velocity = new CCVector2(0, 0);

            this.sprite.Scale = 3.0f;
            this.sprite.SpriteFrame = Animations.explosion3AnimationFrames[0];

            this.sprite.RunActionsAsync(Animations.explosion3Action, new CCCallFunc(this.Erase));
        }

        public bool IsAttacking
        {
            get;
            set;
        }

        public void SetBehaviorStraightToDest(CCPoint destinationPoint, double speedAbs) {
            this.trajectoryPattern = GameParameters.AlienTrajectoryPattern.StraightToDest;

            this.destinationPoint = destinationPoint;

            double tanAlpha = Math.Abs(destinationPoint.Y - this.Position.Y) / Math.Abs(destinationPoint.X - this.Position.X);
            double speedX = Math.Sqrt(speedAbs / (tanAlpha * tanAlpha + 1));
            double speedY = tanAlpha * speedX;

            if (destinationPoint.X < this.Position.X)
                speedX = -speedX;

            if (destinationPoint.Y < this.Position.Y)
                speedY = -speedY;

            this.Velocity = new CCVector2((float)speedX, (float)speedY);
        }

        public void SetBehaviorSteerInChosenDirection(CCVector2 direction, double speedAbs)
        {
            this.trajectoryPattern = GameParameters.AlienTrajectoryPattern.SteerInChosenDirection;



        }

        public override void NextFrameUpdate()
        {
            switch (this.trajectoryPattern) {
                case GameParameters.AlienTrajectoryPattern.Primitive:
                    UpdatePosition();
                    break;
                case GameParameters.AlienTrajectoryPattern.StraightToDest:
                    if (Math.Abs(this.Position.X - this.destinationPoint.X) < 2.0 && Math.Abs(this.Position.Y - this.destinationPoint.Y) < 2.0)
                        this.Velocity = new CCVector2(0, 0);
                    
                    UpdatePosition();
                    break;
                case GameParameters.AlienTrajectoryPattern.SteerInChosenDirection:



                    UpdatePosition();
                    break;
            }
        }

        private void UpdatePosition() {
            GameParameters.RENDERING_SURFACE_MUTEX.WaitOne();
            this.Position += new CCPoint(this.Velocity.X, this.Velocity.Y);
            GameParameters.RENDERING_SURFACE_MUTEX.ReleaseMutex();
        }
    }
}
