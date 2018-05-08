﻿using CocosSharp;

namespace RocketInvasion.Common.Sprites
{
    public class AlienInvader : SpriteNode
    {
        private CCAffineTransform affineTransform = new CCAffineTransform();

        public AlienInvader() {
            this.scaleFactor = 0.5f;
            this.VelocityVec = GameParameters.alienInvaderVelocity;

            affineTransform.Rotation = 0;

            base.DrawSprite("alienSpaceship");
        }

        new public void NextFrameUpdate() {
            GameParameters.renderingSurfaceMutex.WaitOne();
            this.Position += new CCPoint(this.VelocityVec.X, this.VelocityVec.Y);
            GameParameters.renderingSurfaceMutex.ReleaseMutex();
        }

        public void LaunchRocket() {

        }
    }
}