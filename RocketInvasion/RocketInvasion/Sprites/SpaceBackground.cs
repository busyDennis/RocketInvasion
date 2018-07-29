using CocosSharp;


namespace RocketInvasion.Sprites
{
    /**
        Animated space background is utilized by GameplayLayer
         */

    class SpaceBackground : CCSprite
    {
        private bool textureIsBeingEnlarged;
        private float scaleFactor;

        public SpaceBackground() : base()
        {
            this.Texture = new CCTexture2D(GameParameters.SPACE_BACKGROUND_TEXTURE_FILE_NAME);

            this.Position = new CCPoint(0, 0);

            scaleFactor = this.Scale = 5f;

            textureIsBeingEnlarged = true;
        }

        public void NextFrameUpdate()
        {
            scaleFactor += textureIsBeingEnlarged ? 0.001f : - 0.001f;
            if (scaleFactor > 6f) textureIsBeingEnlarged = false;
            if (scaleFactor < 5f) textureIsBeingEnlarged = true;
            this.Scale = scaleFactor;

            // System.Diagnostics.Debug.WriteLine("***** this.ContentSize.ToString():" + this.ContentSize.ToString());
            // System.Diagnostics.Debug.WriteLine("***** this.Texture.PixelsWide * scaleFactor:" + this.Texture.PixelsWide * scaleFactor);
        }

    }
}
