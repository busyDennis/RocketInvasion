using CocosSharp;

using RocketInvasion.Layers;


namespace RocketInvasion.Scenes
{
    class GameScene : CCScene
    {
        GameplayLayer gameplayLayer;

        public GameScene(CCWindow window) : base(window)
        {
            gameplayLayer = new GameplayLayer();
            gameplayLayer.ContentSize = this.ContentSize;
            this.AddChild(gameplayLayer);
        }
    }
}
