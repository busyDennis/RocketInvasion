using CocosSharp;

namespace RocketInvasion.Common.Sprites
{
    class PlayerLifeHpDisplayNode : CCNode
    {
        private Player player;
        private CCSprite[] spriteHolder;

        public PlayerLifeHpDisplayNode(ref Player player)
        {
            this.player = player;


            System.Diagnostics.Debug.WriteLine(player.imgFileName);

            this.UpdateSpriteHolder();
            this.DrawSprite();
        }

        public void UpdateSpriteHolder()
        {
            this.spriteHolder = new CCSprite[player.Lives > 0 ? player.Lives - 1 : 0];

            for (int i = 0; i < spriteHolder.Length; i++)
            {
                spriteHolder[i] = new CCSprite(player.imgFileName);
                spriteHolder[i].Scale = GameParameters.PLAYER_INFO_MINI_PIC_SCALE_FACTOR;
                spriteHolder[i].Position = new CCPoint(50 + i * 50, 50);
            }
        }

        public void DrawSprite()
        {
            CCTexture2D imgTexture = new CCTexture2D(this.player.imgFileName);

            GameParameters.RENDERING_SURFACE_MUTEX.WaitOne();

            this.RemoveAllChildren();

            foreach (CCSprite sprite in this.spriteHolder)
                this.AddChild(sprite);

            GameParameters.RENDERING_SURFACE_MUTEX.ReleaseMutex();
        }
    }
}
