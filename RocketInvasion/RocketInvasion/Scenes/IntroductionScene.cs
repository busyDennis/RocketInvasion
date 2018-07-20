using System;

using CocosSharp;

using RocketInvasion.UIComponents;


namespace RocketInvasion.Scenes
{
    class IntroductionScene : CCScene
    {
        private CCLayer thisLayer;

        public IntroductionScene(CCWindow window) : base(window)
        {
            CreateLayer();
            DrawButtons();
        }

        public void CreateLayer()
        {
            thisLayer = new CCLayer();
            this.AddChild(thisLayer);
        }

        public void DrawButtons() {
            Button btnNewGame = new Button(GameParameters.BTN_NEW_GAME_POSITION, GameParameters.BTN_NEW_GAME_DIMENSIONS);
            btnNewGame.FillColor = GameParameters.BTN_NEW_GAME_FILL_COLOR;
            btnNewGame.BorderColor = GameParameters.BTN_NEW_GAME_BORDER_COLOR;
            btnNewGame.TextColor = GameParameters.BTN_NEW_GAME_TEXT_COLOR;
            btnNewGame.Text = "New Game";
            btnNewGame.FontSize = GameParameters.BTN_NEW_GAME_FONT_SIZE;
            btnNewGame.Clicked += ClkHandlerNewGame;
            btnNewGame.DrawButton(thisLayer);
            
            Button btnLoadGame = new Button(GameParameters.BTN_LOAD_GAME_POSITION, GameParameters.BTN_LOAD_GAME_DIMENSIONS);
            btnLoadGame.FillColor = GameParameters.BTN_LOAD_GAME_FILL_COLOR;
            btnLoadGame.BorderColor = GameParameters.BTN_LOAD_GAME_BORDER_COLOR;
            btnLoadGame.TextColor = GameParameters.BTN_LOAD_GAME_TEXT_COLOR;
            btnLoadGame.Text = "Load Game";
            btnLoadGame.FontSize = GameParameters.BTN_LOAD_GAME_FONT_SIZE;
            btnLoadGame.Clicked += ClkHandlerLoadSavedGame;
            btnLoadGame.DrawButton(thisLayer);
        }

        private void ClkHandlerNewGame(object sender, EventArgs args)
        {
            thisLayer.RemoveAllChildren();



            AppDelegate.StartNewGame();

            System.Diagnostics.Debug.WriteLine("inside BtnClkHandlerNewGame");
        }

        private void ClkHandlerLoadSavedGame(object sender, EventArgs args)
        {
            AppDelegate.LoadSavedGame();
            System.Diagnostics.Debug.WriteLine("inside BtnClkHandlerLoadSavedGame");
        }
    }
}
