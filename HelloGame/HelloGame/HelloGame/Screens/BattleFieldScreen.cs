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

        //建筑物的默认最大建筑数是4*9=36个.
        Dictionary<Vector2, Sprite> ConstructionSpriteMap = new Dictionary<Vector2, Sprite>(36);
        List<Sprite> BulletList = new List<Sprite>();

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
            Person.CurrentPosition = Vector2.Zero;
        }

        /// <summary>
        /// 允许游戏组件进行自我更新。
        /// </summary>
        /// <param name="gameTime">提供计时值的快照。</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            foreach (var pair in ConstructionSpriteMap)
            {
                pair.Value.ChangeSpriteAction();
                if (pair.Value.CanAttack(new Point(1000,160)))
                {
                    Sprite Bulletobj = new Sprite(ScreenManager.Game.Content.Load<Texture2D>(@"Images/Bullet/Bullet"));
                    Bulletobj.FrameSize = new Point(20, 20);
                    Bulletobj.CurrentFrame = new Point(0, 0);
                    Bulletobj.SheetSize = new Point(1, 1);
                    Bulletobj.ChangeActionTime = 1;
                    Bulletobj.AttackIntervalTime = 30;
                    Bulletobj.CurrentPosition = new Vector2(pair.Value.CurrentPosition.X + pair.Value.texture2d.Width, pair.Value.CurrentPosition.Y+8);// new Vector2(position.X * 80, position.Y * 80);
                    Bulletobj.AttackStyle = AttackStyle.launch;
                    BulletList.Add(Bulletobj);
                }
            }

            for (int n = BulletList.Count - 1; n >= 0; n--)
            {
                BulletList[n].currentPosition.X += 10;
                if (BulletList[n].currentPosition.X > 1024)
                    BulletList.RemoveAt(n);
            }

            Person.ChangeSpriteAction();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch SpriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(BackGround, Vector2.Zero, Color.White);
            foreach (var pair in ConstructionSpriteMap)
            {
                pair.Value.Draw(ref SpriteBatch);
            }

            foreach (Sprite obj in BulletList)
            {
                obj.Draw(ref SpriteBatch);
            }

            Person.Draw(ref SpriteBatch);
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

            //构造炮塔
            Sprite ConstructionSprite = null;
            if (ConstructionSpriteMap.TryGetValue(position, out ConstructionSprite) == false)
            {
                ConstructionSprite = new Sprite(ScreenManager.Game.Content.Load<Texture2D>(@"Images/Construction/sprite"));
                ConstructionSprite.FrameSize = new Point(80, 80);
                ConstructionSprite.CurrentFrame = new Point(0, 0);
                ConstructionSprite.SheetSize = new Point(1, 1);
                ConstructionSprite.ChangeActionTime = 1;
                ConstructionSprite.AttackIntervalTime = 30;
                ConstructionSprite.CurrentPosition = new Vector2(position.X * 80, position.Y * 80);
                ConstructionSprite.AttackStyle = AttackStyle.launch;

                //ConstructionSprite = ScreenManager.Game.Content.Load<Texture2D>(@"Images/Construction/sprite");
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
