using CocosSharp;


namespace RocketInvasion.Sprites
{
    /**
        Animated space background is utilized by GameplayLayer
         */

    class SpaceBackground : CCNode
    {
        private bool textureIsBeingEnlarged;
        private float scaleFactor;

        private CCRect visibleBoundsWorldspace;

        Star[] stars; // x y visible

        public SpaceBackground(CCRect visibleBoundsWorldspace) : base()
        {
            this.visibleBoundsWorldspace = visibleBoundsWorldspace;

            this.stars = new Star[50];

            for (int i = 0; i < 50; i++)
            {
                this.stars[i] = new Star(visibleBoundsWorldspace.Size.Width, visibleBoundsWorldspace.Size.Height);

                this.AddChild(this.stars[i]);
            }

            //this.Texture = new CCTexture2D(GameParameters.SPACE_BACKGROUND_TEXTURE_FILE_NAME);

                //this.Position = new CCPoint(0, 0);

                //scaleFactor = this.Scale = 5f;

                //textureIsBeingEnlarged = true;
        }

        public void NextFrameUpdate()
        {
            foreach (Star s in this.stars) {
                s.NextFrameUpdate();
            }


            //scaleFactor += textureIsBeingEnlarged ? 0.001f : - 0.001f;
            //if (scaleFactor > 6f) textureIsBeingEnlarged = false;
            //if (scaleFactor < 5f) textureIsBeingEnlarged = true;
            //this.Scale = scaleFactor;

            // System.Diagnostics.Debug.WriteLine("***** this.ContentSize.ToString():" + this.ContentSize.ToString());
            // System.Diagnostics.Debug.WriteLine("***** this.Texture.PixelsWide * scaleFactor:" + this.Texture.PixelsWide * scaleFactor);
        }

    }
}
