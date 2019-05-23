using CocosSharp;
using System;


namespace RocketInvasion.Sprites
{
    /**
        Animated space background is utilized by GameplayLayer
    */

    class SpaceBackground : CCNode
    {
        private Star[] stars;
        private CCSize contentSize;
        public bool IsMoving { get; set; }

        public SpaceBackground() : base() {
            this.stars = new Star[50];
            IsMoving = false;
        }

        /***
         * SpaceBackground must be initialized before it can be used
         * parentContentSize - parent layer content size
         */
        public void Initialize(CCSize parentContentSize) {
            this.contentSize = parentContentSize;

            Random rGen = new Random();

            for (int i = 0; i < 50; i++)
            {
                this.stars[i] = new Star();
                this.stars[i].Position = new CCPoint(rGen.Next(0, (int) parentContentSize.Width), rGen.Next(0, (int)parentContentSize.Height));
                this.AddChild(this.stars[i]);
            }
        }

        public void StartMoving() {
            if (this.IsMoving) return;

            foreach (Star s in this.stars)
                s.Velocity = GameParameters.STARS_MOVING_VELOCITY;
        }

        public void StopMoving() {
            if (! this.IsMoving) return;

            foreach (Star s in this.stars)
                s.Velocity = GameParameters.ZERO_VELOCITY;
        }

        public void NextFrameUpdate() {
            foreach (Star s in this.stars) {
                s.NextFrameUpdate();
                if (s.PositionY < 0)
                    s.PositionY = this.contentSize.Height + s.PositionY;
            }
        }

        public void BlinkingActivity(float frameTimeInSeconds)
        {
            foreach (Star s in this.stars)
            {
                s.BlinkingActivity(frameTimeInSeconds);
            }
        }

    }
}
