using System.Reflection;
using Microsoft.Xna.Framework;
using CocosSharp;
using CocosDenshion;

using RocketInvasion.Scenes;


namespace RocketInvasion
{
    public class AppDelegate : CCApplicationDelegate
    {
        static CCWindow mainWindow;
        static CCDirector sceneDirector;

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            AppDelegate.mainWindow = mainWindow;
            sceneDirector = new CCDirector();
            mainWindow.AddSceneDirector(sceneDirector);

            application.ContentRootDirectory = "Content";


            var windowSize = mainWindow.WindowSizeInPixels;

            var desiredWidth = 768.0f;
            var desiredHeight = 1027.0f;


            // This will set the world bounds to be (0,0, w, h)
            // CCSceneResolutionPolicy.ShowAll will ensure that the aspect ratio is preserved
            CCScene.SetDefaultDesignResolution(desiredWidth, desiredHeight, CCSceneResolutionPolicy.ShowAll);

            // Determine whether to use the high or low def versions of our images
            // Make sure the default texel to content size ratio is set correctly
            // Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
            if (desiredWidth < windowSize.Width)
            {
                application.ContentSearchPaths.Add("hd");
                CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
            }
            else
            {
                application.ContentSearchPaths.Add("ld");
                CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
            }

            LoadIntoductionScene();

            // mainWindow.RunWithScene(scene);
        }

        public override void ApplicationDidEnterBackground(CCApplication application)
        {
            application.Paused = true;
        }

        public override void ApplicationWillEnterForeground(CCApplication application)
        {
            application.Paused = false;
        }

        public void LoadIntoductionScene() {
            var introScreenScene = new IntroductionScene(mainWindow);
            sceneDirector.ReplaceScene(introScreenScene);
        }

        public static void StartNewGame()
        {
            System.Diagnostics.Debug.WriteLine("***** inside AppDelegate.StartNewGame()");

            var scene = new GameScene(mainWindow);
            sceneDirector.ReplaceScene(scene);
        }

        public static void LoadSavedGame() {

        }
    }
}