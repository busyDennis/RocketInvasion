using CocosSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RocketInvasion.Common.Sprites
{
    public class Rocket : SpriteNode
    {
        public Rocket() {
            this.scaleFactor = 0.3f;
            this.VelocityVec = GameParameters.rocketVelocity;

            base.DrawSprite("rocket");
        }

        public void Explode() {
            // this.VelocityVec.X = 0;
            this.VelocityVec.Y = 0;

            this.sprite.Scale = 2.5f;
            this.sprite.SpriteFrame = Animations.rocketExplosionAnimationFrames[0];

            //CCActionState actionState = 
            this.sprite.RunActionsAsync(Animations.rocketExplosionAction);

            //CCSequence mySequence = new CCSequence(Animations.rocketExplosionAction, new Task());
            //this.sprite.RunAction(mySequence);


            // this.Erase();

            // this.StopAction(actionState);

            /*
            while (this.sprite.GetActionState(Animations.rocketExplosionAction.Tag).Target != null)
            {
                System.Threading.Tasks.Task.Delay(1).Wait();
            }
            */
        }

    }
}
