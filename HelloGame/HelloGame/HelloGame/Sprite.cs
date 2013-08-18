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

namespace HelloGame
{
    public class Sprite
    {
        public Sprite(Texture2D texture2d)
        {
            this.texture2d = texture2d;
        }

        #region 字段
        /// <summary>
        /// 引用的精灵对象
        /// </summary>
        public Texture2D texture2d = null;

        //精灵的绘制坐标信息
        Vector2 currentPosition = Vector2.Zero;

        //下面是关于精灵动作改变的信息
        int changeActionTime = 1;
        int currentActionTime = 1;
        Point frameSize = new Point();
        Point currentFrame = new Point();
        Point sheetSize = new Point();
        #endregion

        #region 属性
        /// <summary>
        /// 每个精灵的尺寸.
        /// 例如:一个200*76的整体大图中画了4个画面来形成一个动画精灵
        /// 那么frameSize就应该是:
        ///         frameSize = new Point(50, 74);
        /// </summary>
        public Point FrameSize
        {
            get { return frameSize; }
            set { frameSize = value; }
        }

        /// <summary>
        /// 当前精灵动画进行到第几帧
        /// 例如:一个200*76的整体大图中画了4个画面来形成一个动画精灵
        /// 那么初始化时的currentFrame就应该是:
        ///         currentFrame = new Point(0, 0);
        /// </summary>
        public Point CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        /// <summary>
        /// 精灵大图的动画帧排布行列
        /// 例如:一个200*76的整体大图中画了4个动作,一共是四列一行,来形成一个动画精灵
        /// 那么SheetSize就应该是:
        ///         SheetSize = new Point(4, 1);
        /// </summary>
        public Point SheetSize
        {
            get { return sheetSize; }
            set { sheetSize = value; }
        }

        /// <summary>
        /// 当前精灵的位置
        /// </summary>
        public Vector2 CurrentPosition
        {
            get { return currentPosition; }
            set { currentPosition = value; }
        }

        /// <summary>
        /// 经过多少帧的时间,动画改变一次
        /// </summary>
        public int ChangeActionTime
        {
            get { return changeActionTime; }
            set { changeActionTime = value; }
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 这里做动态精灵的位置变化时的函数,供子类覆盖使用
        /// 原理:
        ///     函数没经过一帧会进一次函数,这里有一个变量记录了
        /// </summary>
        public virtual void ChangeSpriteAction()
        {
            if (currentActionTime >= changeActionTime)
            {
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
                currentActionTime = 1;
            }
            else
            {
                ++currentActionTime;
            }
        }

        /// <summary>
        /// 这里写单独的Draw函数用来给框架下的Draw调用
        /// </summary>
        /// <param name="SpriteBatch">框架下的Draw调用时传入的参数</param>
        public virtual void Draw(ref SpriteBatch SpriteBatch)
        {
            if (texture2d == null)
            {
                System.Diagnostics.Debug.WriteLine("没有引用的精灵对象,所以没有执行精灵自己的绘制信息.");
            }
            SpriteBatch.Draw(texture2d, currentPosition, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }
        #endregion
    }
}