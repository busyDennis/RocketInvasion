using System;
using System.Collections.Generic;
using CocosSharp;


namespace RocketInvasion.UIComponents
{
    public class Button : CCNode
    {
        public CCSize Dimensions { get; set; }

        public CCColor4B FillColor { get; set; }

        public int BorderWidth { get; set; }
        public CCColor4B BorderColor { get; set; }

        public string Text { get; set; }
        public CCColor3B TextColor { get; set; }

        public Single FontSize { get; set; }

        private CCLabel textLabel;
        private CCDrawNode drawNode;

        private CCLayer layer;

        public event EventHandler Clicked;


        public Button(CCPoint position, CCSize dimensions) : base() {
            this.Position = position;
            this.Dimensions = dimensions;
            this.ContentSize = this.Dimensions;

            this.FillColor = CCColor4B.White;

            this.BorderWidth = 2;
            this.BorderColor = CCColor4B.Black;

            this.Text = "";
            this.TextColor = CCColor3B.Black;

            this.FontSize = 5;

            this.drawNode = new CCDrawNode();
            this.AddChild(this.drawNode);

            this.textLabel = new CCLabel("hello", "Arial", this.FontSize, CCTextAlignment.Center);
            this.textLabel.PositionX = this.Dimensions.Width * 0.5f;
            this.textLabel.PositionY = this.Dimensions.Height * 0.5f;
            this.textLabel.Color = CCColor3B.Black;
            this.AddChild(this.textLabel);
        }

        override protected void AddedToScene()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchesBeganHandler;
            layer.AddEventListener(touchListener);
        }

        public void DrawButton(CCLayer layer) {
            this.layer = layer;

            this.drawNode.DrawRect(new CCRect(0, 0, Dimensions.Width, Dimensions.Height),
                fillColor: this.FillColor,
                borderWidth: this.BorderWidth,
                borderColor: this.BorderColor);

            if (this.Text != null)
            {
                this.textLabel.Text = this.Text;
                this.textLabel.SystemFontSize = FontSize;
            }
            
            layer.AddChild(this);
        }

        private void OnTouchesBeganHandler(List<CCTouch> touches, CCEvent touchEvent)
        {
            System.Diagnostics.Debug.WriteLine("***** inside OnTouchesBeganHandler");

            foreach (var touch in touches)
            {
                if (touch != null)
                {
                    System.Diagnostics.Debug.WriteLine("touch.Location.ToString(): " + touch.Location.ToString());
                    System.Diagnostics.Debug.WriteLine("this.BoundingBoxTransformedToWorld.ToString(): " + this.BoundingBoxTransformedToWorld.ToString());

                    if (this.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        System.Diagnostics.Debug.WriteLine("***** user clicked a Button");

                        this.BorderColor = CCColor4B.Red;

                        Clicked(this, null);
                        break;

                    }
                }
            }
        }
    }
}

/*
    System.Diagnostics.Debug.WriteLine("***** Button bounding box coordinates: LL (" + this.BoundingBoxTransformedToWorld.LowerLeft.X + "," + BoundingBoxTransformedToWorld.LowerLeft.Y + "), UR (" + this.BoundingBoxTransformedToWorld.UpperRight.X + "," + BoundingBoxTransformedToWorld.UpperRight.Y + ")");     
     */
