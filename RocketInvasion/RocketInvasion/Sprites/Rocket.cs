using CocosSharp;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RocketInvasion.Common.Sprites
{
    public class Rocket : SpriteNode
    {
        public Rocket()
        {
            this.scaleFactor = 0.3f;
            this.VelocityVec = GameParameters.rocketVelocity;

            base.DrawSprite("rocket");
        }

        public void Explode() {
            this.sprite.SpriteFrame = Animations.rocketExplosionAnimationFrames[0];
            this.sprite.Scale = 1.5f;
            this.sprite.AddAction(Animations.rocketExplosionAction);
            //this.Erase();
        }
    }
}
