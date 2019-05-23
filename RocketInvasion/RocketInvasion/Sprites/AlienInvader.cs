using CocosSharp;
using System;

namespace RocketInvasion.Sprites
{
    public class AlienInvader : Spaceship
    {
        public enum SteeringState { Steering, Moving, Stopped };

        private CCAffineTransform affineTransform = new CCAffineTransform();
        private GameParameters.AlienTrajectoryPattern trajectoryPattern;
        private CCPoint destinationPoint;

        private float velocityDirectionAngle;

        // steering and moving
        private int movingTimeInChosenDirectionInFrames; // time before the sprite starts to change direction
        private float steerToAngle; // steers until reaches this angle
        private bool steeringCounterlockwise; // steering direction
        private bool deepSteering; // when deep steering, direction angle will be deeper and moving time longer

        private SteeringState steeringState;

        private Random randGenerator = new Random();
        private int randInt;

        public AlienInvader() {
            this.scaleFactor = GameParameters.ALIEN_INVADER_PIC_SCALE_FACTOR;
            this.Velocity = GameParameters.ZERO_VELOCITY;
            this.IsAttacking = false;

            affineTransform.Rotation = 0;
            this.IntervalBetweenRocketLaunches = GameParameters.INTERVAL_BETWEEN_ALIEN_INVADER_ROCKET_LAUNCHES;

            this.trajectoryPattern = GameParameters.AlienTrajectoryPattern.Primitive;

            this.steeringState = AlienInvader.SteeringState.Stopped;
            movingTimeInChosenDirectionInFrames = 0;
            steeringCounterlockwise = true; // steering angle increases

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
        

        public void SetBehaviorSteer()
        {
            this.trajectoryPattern = GameParameters.AlienTrajectoryPattern.Steer;

            this.steeringState = AlienInvader.SteeringState.Moving;
            this.movingTimeInChosenDirectionInFrames = 0;
            this.steerToAngle = 225f;

            SetVelocityAndRotationByAngle(270f, GameParameters.ALIEN_INVADER_VELOCITY_VAL);
        }

        private void SetVelocityAndRotationByAngle(float angle, float velocityAbsVal)
        {
            // velVec = (velMod * cos, velMod * sin)
            this.Velocity = new CCVector2((float)(velocityAbsVal * Math.Cos(angle * Math.PI / 180.0)), (float)(velocityAbsVal * Math.Sin(angle * Math.PI / 180.0)));
            this.sprite.Rotation = -((int) angle) - 90; // correction of -90 deg; sprite rotation clockwise direction goes against our chosen velocity rotation (counterclockwise)

            this.velocityDirectionAngle = angle;
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
                case GameParameters.AlienTrajectoryPattern.Steer:
                    // should take turns in steering left and right
                    // steers left or right within time limits
                    // steering is an activity
                    UpdatePosition();
                    break;
            }
        }


        /**              
            Steers to one angle for a while (inputs: new angle, angular speed), then stops steering and just moves (input: moving time), then steers to another angle etc.
        */
        public void SteeringActivity(float frameTimeInSeconds) {
            

            if (this.movingTimeInChosenDirectionInFrames > 0) {
                // continue in the same direction
                this.movingTimeInChosenDirectionInFrames -= 1;
            } else {
                // use steeringCounterclockwise and steerToAngle instead of left-right
                // use steeringSpeedCoefficient, steeringTime, make them randomly selected from interval

                // finished moving
                if (this.steeringState == AlienInvader.SteeringState.Moving) {
                    this.steeringState = AlienInvader.SteeringState.Steering;

                    this.steeringCounterlockwise = ! this.steeringCounterlockwise; // changing steering direction

                    randInt = randGenerator.Next(0, 100);

                    if (randInt < 70) {
                        deepSteering = false;
                        this.steerToAngle = this.steeringCounterlockwise ? 315f : 225f;
                    } else {
                        deepSteering = true;
                        this.steerToAngle = this.steeringCounterlockwise ? 335f : 205f;
                    }
                    
                }
                // finished steering step, can do more steering or start moving
                if (this.steeringState == AlienInvader.SteeringState.Steering) {
                    if (this.steeringCounterlockwise)
                    { // steering counterlockwise
                        // steering continues
                        if (this.velocityDirectionAngle + GameParameters.ALIEN_INVADER_STEERING_ANGULAR_INCREMENT < this.steerToAngle)
                        {
                            this.movingTimeInChosenDirectionInFrames = GameParameters.ALIEN_INVADER_TIME_INTERVAL_FOR_ONE_STEERING_STEP_IN_NUM_FRAMES;
                            SetVelocityAndRotationByAngle(this.velocityDirectionAngle + GameParameters.ALIEN_INVADER_STEERING_ANGULAR_INCREMENT, GameParameters.ALIEN_INVADER_VELOCITY_VAL);
                        }
                        // steering done
                        else
                        {
                            this.steeringState = AlienInvader.SteeringState.Moving;
                            this.movingTimeInChosenDirectionInFrames = deepSteering ? GameParameters.ALIEN_INVADER_MAX_TIME_INTERVAL_FOR_MOVING_STAGE_IN_NUM_FRAMES : randGenerator.Next(0, GameParameters.ALIEN_INVADER_MAX_TIME_INTERVAL_FOR_MOVING_STAGE_IN_NUM_FRAMES);
                            SetVelocityAndRotationByAngle(this.steerToAngle, GameParameters.ALIEN_INVADER_VELOCITY_VAL);
                        }
                    } else { // steering clockwise
                        // steering continues
                        if (this.velocityDirectionAngle - GameParameters.ALIEN_INVADER_STEERING_ANGULAR_INCREMENT > this.steerToAngle)
                        {
                            this.movingTimeInChosenDirectionInFrames = GameParameters.ALIEN_INVADER_TIME_INTERVAL_FOR_ONE_STEERING_STEP_IN_NUM_FRAMES;
                            SetVelocityAndRotationByAngle(this.velocityDirectionAngle - GameParameters.ALIEN_INVADER_STEERING_ANGULAR_INCREMENT, GameParameters.ALIEN_INVADER_VELOCITY_VAL);
                        }
                        // steering done
                        else
                        {
                            this.steeringState = AlienInvader.SteeringState.Moving;
                            this.movingTimeInChosenDirectionInFrames = deepSteering ? GameParameters.ALIEN_INVADER_MAX_TIME_INTERVAL_FOR_MOVING_STAGE_IN_NUM_FRAMES : randGenerator.Next(0, GameParameters.ALIEN_INVADER_MAX_TIME_INTERVAL_FOR_MOVING_STAGE_IN_NUM_FRAMES);
                            SetVelocityAndRotationByAngle(velocityDirectionAngle, GameParameters.ALIEN_INVADER_VELOCITY_VAL);
                        }
                    }
                }
            }

            UpdatePosition();
        }

        private void UpdatePosition() {
            GameParameters.RENDERING_SURFACE_MUTEX.WaitOne();
            this.Position += new CCPoint(this.Velocity.X, this.Velocity.Y);
            GameParameters.RENDERING_SURFACE_MUTEX.ReleaseMutex();
        }
    }
}
