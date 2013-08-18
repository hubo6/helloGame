using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;


namespace HelloGame.Screens
{
    /// <summary>
    /// 这是一个实现 IUpdateable 的游戏组件。
    /// </summary>
    public class BattleFieldScreen : GameScreen
    {
        Sprite Person;
        Texture2D BackGround;

        public ScreenState oldState { get; set; }

        public int difficultMeter { get; set; }
        public int timeSpan { get; set; }

        //建筑物的默认最大建筑数是4*9=36个.
        Dictionary<Vector2, Texture2D> ConstructionSpriteMap = new Dictionary<Vector2, Texture2D>(36);
        List<Sprite> enemyList = new List<Sprite>(36);

        public override void LoadContent()
        {
            ContentManager Content = ScreenManager.Game.Content;

            EnabledGestures = EnabledGestures | GestureType.Tap;

            BackGround = Content.Load<Texture2D>(@"Images/BackGround/backGround_800_480");

            //建立动态精灵,并且设置动画信息
            Person = new Sprite(Content.Load<Texture2D>(@"Images/enemy/person"));
            Person.FrameSize = new Point(50, 74);
            Person.CurrentFrame = new Point(0, 0);
            Person.SheetSize = new Point(4, 1);
            Person.ChangeActionTime = 5;
            Person.CurrentPosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, 0.0f);
            difficultMeter = 50;
            timeSpan = 30 * 5;
            //for (int n = 0; n < 15; n++)
                enemyList.Add(Person);
        }

        /// <summary>
        /// 允许游戏组件进行自我更新。
        /// </summary>
        /// <param name="gameTime">提供计时值的快照。</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            for (int n = enemyList.Count - 1; n >=0; n--)
            {

                if (enemyList[n].CurrentPosition.X + enemyList[n].texture2d.Width <= 0)
                {
                    enemyList.RemoveAt(n);
                }
                else
                {
                    enemyList[n].ChangeSpriteAction();
                    enemyList[n].CurrentPosition += new Vector2(-150.0f * 0.03f, 0.00f);
                }

            }
            

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        protected void drawEnemyList()
        {
            SpriteBatch SpriteBatch = ScreenManager.SpriteBatch;
            foreach (Sprite enemy in enemyList)
            {
                enemy.Draw(ref SpriteBatch);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch SpriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(BackGround, Vector2.Zero, Color.White);
            foreach (var pair in ConstructionSpriteMap)
            {
                Texture2D sprite = pair.Value;
                Vector2 vector = pair.Key;
                Vector2 location = new Vector2(vector.X * 80, vector.Y * 80);
                ScreenManager.SpriteBatch.Draw(sprite, location, Color.Yellow);
            }

            drawEnemyList();
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void alertOk(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null,new BackgroundScreen(), new MainMenuScreen());
        }

        private void alertCancel(object sender, PlayerIndexEventArgs e)
        {
            ScreenState = oldState;
        }
        public override void HandleInput(InputState input)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                oldState = ScreenState;
                ScreenState = ScreenState.Pause;
                MessageBoxScreen alert = new MessageBoxScreen("ALERT");
                alert.ok.Selected += alertOk;
                alert.cancel.Selected += alertCancel;
                ScreenManager.AddScreen(alert, null);
            }
            else
            {
                foreach (GestureSample gesture in input.Gestures)
                {
                    switch (gesture.GestureType)
                    {
                        case GestureType.Tap:
                            {
                                OnTouchUpEven(gesture);
                                System.Diagnostics.Debug.WriteLine("**点击事件:坐标为'{0},{1}'.", gesture.Position.X, gesture.Position.Y);
                            }
                            break;
                        //case :
                        //    break;
                        //case :
                        //    break;
                        default:
                            break;
                    }
                }
            }
        }

        #region 事件
        /// <summary>
        /// 触摸按下时触发的事件
        /// </summary>
        private void OnTouchDownEven(TouchLocation tl)
        {

        }

        /// <summary>
        /// 手指在屏幕上滑动时触发的事件
        /// </summary>
        private void OnMoveEven(TouchLocation tl)
        {

        }

        /// <summary>
        /// 手指抬起时触发事件
        /// </summary>
        private void OnTouchUpEven(GestureSample gesture)
        {
            //判断如果不再建筑范围内,那么就可以将消息传送到其他PANEL了.
            if (gesture.Position.X < 80 || gesture.Position.X > 800)
            {
                System.Diagnostics.Debug.WriteLine("X轴坐标点击不在建筑范围内.");
                return;
            }

            if (gesture.Position.Y < 80 || gesture.Position.Y > 400)
            {
                System.Diagnostics.Debug.WriteLine("Y轴坐标点击不在建筑范围内.");
                return;
            }

            Vector2 position = new Vector2((int)gesture.Position.X / 80, (int)gesture.Position.Y / 80);
            System.Diagnostics.Debug.WriteLine("建筑物区域坐标'{0},{1}'.", (int)position.X, (int)position.Y);

            Texture2D ConstructionSprite = null;
            if (ConstructionSpriteMap.TryGetValue(position, out ConstructionSprite) == false)
            {
                ConstructionSprite = ScreenManager.Game.Content.Load<Texture2D>(@"Images/Construction/sprite");
                ConstructionSpriteMap.Add(position, ConstructionSprite);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("当前坐标已经存在建筑物,需要换其他地方见建了.");
            }
        }
#endregion
    }
}
