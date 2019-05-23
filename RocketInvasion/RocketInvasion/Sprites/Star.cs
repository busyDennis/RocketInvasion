using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketInvasion.Sprites
{
    public class Star : SpriteNode
    {
        private static Random rGen = new Random();
        private float timeSinceLastBlink;

        public Star()
        {
            this.scaleFactor = 0.1f;

            base.DrawSprite("star");

            this.sprite.Opacity = Convert.ToByte(rGen.Next(50, 255)); // opacity is random
            this.sprite.Visible = rGen.NextDouble() > 0.5 ? true : false; // visibility is random

            timeSinceLastBlink = (float)(rGen.NextDouble() * GameParameters.STAR_BLINKING_INTERVAL); // blinking starts at random time within the interval
        }

        public void BlinkingActivity(float frameTime)
        {
            timeSinceLastBlink += frameTime;

            if (timeSinceLastBlink == 0)
                Blink();

            if (timeSinceLastBlink > GameParameters.STAR_BLINKING_INTERVAL)
            {
                timeSinceLastBlink -= GameParameters.STAR_BLINKING_INTERVAL;

                Blink();
            }
        }

        private void Blink() {
            this.sprite.Visible = ! this.sprite.Visible;
        }
    }
}
