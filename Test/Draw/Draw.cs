using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Product2.Test.Draw
{
    class Draw
    {
        static Canvas graph;
        static Math.Border border;
        static Product2.RichTextProcess richText;
        public Draw(Canvas grid,Product2.RichTextProcess richText){
            Draw.graph = grid;
            Draw.richText = richText;
        }
        public void Line(Math.Vector vec)
        {
            Path path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;
            LineGeometry lineGeometry = new LineGeometry(vec.From.toPoint(), vec.To.toPoint());
            path.Data = lineGeometry;
            graph.Children.Add(path);
        }
        public void Lines(List<Math.Dot2> dots)
        {
            int i;
            int count = dots.Count;
            for (i = 0; i < count - 1; i++)
            {
                Line myline = new Line();
                myline.Stroke = Brushes.Red;
                myline.X1 = dots[i].X;
                myline.Y1 = dots[i].Y;
                myline.X2 = dots[i + 1].X;
                myline.Y2 = dots[i + 1].Y;
                graph.Children.Add(myline);
            }


            ////StreamGeometry geometry = new StreamGeometry();
            //GeometryGroup geometry = new GeometryGroup();
            //if (dots.Count == 0)
            //{
            //    return;
            //}

            //using (StreamGeometryContext ctx = geometry.Open())
            //{
            //    int i = 1;
            //    ctx.BeginFigure(dots[0].toPoint(), true, true);
            //    for (; i < dots.Count - 1; i++)
            //    {
            //        ctx.LineTo(dots[i].toPoint(), true, false);
            //    }
            //    ctx.LineTo(dots[i].toPoint(), false, false);
            //}

            //Path path = new Path();
            //path.Stroke = Brushes.Black;
            //path.StrokeThickness = 1;
            //path.Data = geometry;
            //graph.Children.Add(path);
        }
        public void Border(Math.Border border)
        {
            Draw.border = border;
            StreamGeometry geometry = new StreamGeometry();
            if (border.DotCount == 0)
            {
                return;
            }
            using (StreamGeometryContext ctx = geometry.Open())
            {
                int i = 1;
                ctx.BeginFigure(new Point(border.get(0).X, border.get(0).Y), true, true);
                for (; i < border.DotCount - 1; i++)
                {
                    ctx.LineTo(new Point(border.get(i).X, border.get(i).Y), true, false);
                }
                ctx.LineTo(new Point(border.get(i).X, border.get(i).Y), false, false);
            }
            Path path = new Path();
            //path.Stroke = Brushes.Black;
            path.Fill = Brushes.Red;
            //path.StrokeThickness = 1;
            path.Data = geometry;
            graph.Children.Add(path);
            Appear(border.fetchLog());
        }
        public void Lines(Math.Border border)
        {
            Draw.border = border;
            int i = 0;
            Line myline;
            int count = border.DotCount;
            
            for (; i < count; i++)
            {
                myline = myline = new Line();
                if (i==0)
                {
                    Ring(border.get(i));
                    myline.Stroke = Brushes.Brown;
                }
                else
                {
                    myline.Stroke = Brushes.Red;
                }
                switch (border.getDotType(i))
                {
                    case Product2.Test.Math.DrawType.normal:
                    case Product2.Test.Math.DrawType.Emphasize:
                    case Product2.Test.Math.DrawType.Error:
                    case Product2.Test.Math.DrawType.Info:
                        break;
                    case Product2.Test.Math.DrawType.Ignore:
                        Dot(border.get(i));
                        //myline.Stroke = Brushes.br;
                        break;
                    default:
                        break;
                }
                
                myline.X1 = border.get(i).X;
                myline.Y1 = border.get(i).Y;
                myline.X2 = border.get(i + 1).X;
                myline.Y2 = border.get(i + 1).Y;
                graph.Children.Add(myline);
            }
            //StreamGeometry geometry = new StreamGeometry();
            //if (border.DotCount == 0)
            //{
            //    return;
            //}

            //using (StreamGeometryContext ctx = geometry.Open())
            //{
            //    int i = 1;
            //    ctx.BeginFigure(new Point(border.get(0).X, border.get(0).Y), true, true);
            //    for (; i < border.DotCount - 1; i++)
            //    {
            //        ctx.LineTo(new Point(border.get(i).X, border.get(i).Y), true, false);
            //    }
            //    ctx.LineTo(new Point(border.get(i).X, border.get(i).Y), false, false);
            //}
            //Path path = new Path();
            //path.Stroke = Brushes.Black;
            //path.StrokeThickness = 1;
            //path.Data = geometry;
            //graph.Children.Add(path);

            Appear(border.fetchLog());
        }
        public void Reflesh(Math.Dot2 dot)
        {
            Math.Operate operate = border.fetchLog();
            switch (operate.type)
            {
                case Product2.Test.Math.Type.Append:
                    //border.Modify(dot, operate.index);
                    break;
                case Product2.Test.Math.Type.Insert:
                    //border.Modify(dot, operate.index);
                    break;
                case Product2.Test.Math.Type.Delete:
                case Product2.Test.Math.Type.Read:
                    return;
                case Product2.Test.Math.Type.Modify:
                    //border.Modify(dot, operate.index);
                    break;
                default:
                    break;
            }
            //graph.Children.Clear();
            //Lines(border);
            int index = 0;
            foreach (UIElement item in graph.Children)
            {
                if (item.GetType() == typeof(Line))
                {
                    Line line = (Line)item;
                    index++;
                    if (index == operate.index)
                    {
                        line.X2 = dot.X;
                        line.Y2 = dot.Y;
                    }
                    else if (index == operate.index+1)
                    {
                        line.X1 = dot.X;
                        line.Y1 = dot.Y;
                    }
                }
            }
        }
        public void Dots(List<Math.Dot2> dots)
        {
            for (int i = 0; i < dots.Count; i++)
            {
                Dot(dots[i]);
            }
        }
        public void Dot(Math.Dot2 dot)
        {
            Path path;
            path = new Path();
            path.Stroke = Brushes.Black;
            path.Fill = Brushes.Black;
            path.StrokeThickness = 1;
            path.Data = new EllipseGeometry(dot.toPoint(), 3, 3);
            graph.Children.Add(path);
        }
        public void Ring(Math.Dot2 dot)
        {
            Path path;
            GeometryGroup group = new GeometryGroup();
            path = new Path();
            path.Fill = Brushes.Brown;
            path.StrokeThickness = 1;
            group.Children.Add(new EllipseGeometry(dot.toPoint(), 5, 5));
            group.Children.Add(new EllipseGeometry(dot.toPoint(), 3, 3));
            path.Data = group;
            graph.Children.Add(path);
        }

        #region 动画Animation
        /// <summary>
        /// key关键帧集增加帧frame
        /// </summary>
        /// <param name="kfs">关键帧集</param>
        /// <param name="ms">时间点</param>
        /// <param name="value">数值</param>
        /// <param name="efun">缓动处理</param>
        void KeyAddFrame(DoubleAnimationUsingKeyFrames kfs, double ms, double value, EasingFunctionBase efun)
        {
            EasingDoubleKeyFrame kf = new EasingDoubleKeyFrame();
            kf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(ms));
            kf.Value = value;
            kf.EasingFunction = efun;
            kfs.KeyFrames.Add(kf);
        }
        /// <summary>
        /// story故事版增加key关键帧集
        /// </summary>
        /// <param name="sb">story故事版</param>
        /// <param name="ks">key关键帧集</param>
        /// <param name="dobj">动画目标</param>
        /// <param name="property">动画属性</param>
        void StoryAddKey(Storyboard sb, DoubleAnimationUsingKeyFrames ks, DependencyObject dobj, PropertyPath property)
        {
            sb.Children.Add(ks);
            Storyboard.SetTarget(ks, dobj);
            Storyboard.SetTargetProperty(ks, property);
        }
        Storyboard sb = new Storyboard()

        {
            FillBehavior = FillBehavior.HoldEnd
        };
        /// <summary>
        /// property属性链
        /// </summary>
        DependencyProperty[] propertyChain = new DependencyProperty[]
        {
            Canvas.TopProperty,
            Canvas.LeftProperty
        };
        Ellipse ellipse;
        public void Appear(Math.Operate operate)
        {
            Math.Dot2 dot = operate.dot;
            switch (operate.type)
            {
                case Product2.Test.Math.Type.Append:
                    break;
                case Product2.Test.Math.Type.Insert:
                    break;
                case Product2.Test.Math.Type.Delete:
                case Product2.Test.Math.Type.Read:
                    return;
                case Product2.Test.Math.Type.Modify:
                    break;
                default:
                    break;
            }
            ellipse = new Ellipse();
            ellipse.Width = 14;
            ellipse.Height = 14;
            ellipse.Fill = new SolidColorBrush(Colors.Red);
            ellipse.Stroke = new SolidColorBrush(Colors.Black);
            ellipse.Margin = new Thickness(dot.X - 7, dot.Y - 7, 0, 0);
            graph.Children.Add(ellipse);
            //设置到中心进行旋转
            ellipse.RenderTransformOrigin = new Point(0.5, 0.5);
            ellipse.RenderTransform = new ScaleTransform(1, 1);
            NameScope.SetNameScope(graph, new NameScope());
            graph.RegisterName("mypath", ellipse);
            Storyboard myStoryboard = new Storyboard();
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 1;
            myDoubleAnimation.To = 2;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            Storyboard.SetTargetName(myDoubleAnimation, "mypath");
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath("RenderTransform.ScaleY"));
            myStoryboard.Children.Add(myDoubleAnimation);

            //myDoubleAnimation = new DoubleAnimation();
            //myDoubleAnimation.From = 1;
            //myDoubleAnimation.To = 3;
            //myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            //Storyboard.SetTargetName(myDoubleAnimation, "mypath");
            //Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath("RenderTransform.ScaleY"));
            //myStoryboard.Children.Add(myDoubleAnimation);

            myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0.4;
            myDoubleAnimation.To = 0.8;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            Storyboard.SetTargetName(myDoubleAnimation, "mypath");
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Ellipse.OpacityProperty));
            myStoryboard.Children.Add(myDoubleAnimation);

            ColorAnimation animation = new ColorAnimation();
            animation.From = Colors.Orange;
            animation.To = Colors.Gray;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));

            Storyboard.SetTargetName(animation, "mypath");
            Storyboard.SetTargetProperty(animation, new PropertyPath("Fill.Color"));
            myStoryboard.Children.Add(animation);

            
            ////纵坐标动画
            DoubleAnimationUsingKeyFrames ks1e1 = new DoubleAnimationUsingKeyFrames();
            this.KeyAddFrame(ks1e1, 0.0, 1, null);
            this.KeyAddFrame(ks1e1, 0.5, 2.3, new BackEase() { EasingMode = EasingMode.EaseOut, Amplitude = 1.5 });
            //this.StoryAddKey(sb, ks1e1, path, new PropertyPath("(0)", this.propertyChain));
            Storyboard.SetTargetName(ks1e1, "mypath");
            Storyboard.SetTargetProperty(ks1e1, new PropertyPath("RenderTransform.ScaleX"));
            myStoryboard.Children.Add(ks1e1);
            ////横坐标动画
            //DoubleAnimationUsingKeyFrames ks2e1 = new DoubleAnimationUsingKeyFrames();
            //this.KeyAddFrame(ks2e1, 0.0, 50.0, null);
            //this.KeyAddFrame(ks2e1, 2.0, 350.0, null);
            //this.StoryAddKey(sb, ks2e1, path, new PropertyPath("(1)", this.propertyChain));
            //sb.Begin();

            myStoryboard.Begin(ellipse);
            ellipse.MouseDown += ellipse_MouseDown;
            ellipse.MouseLeftButtonDown += ellipse_MouseLeftButtonDown;
            ellipse.MouseLeftButtonUp += ellipse_MouseLeftButtonUp;
            ellipse.MouseMove += ellipse_MouseMove;
            ellipse.MouseLeave += ellipse_MouseLeave;
            if (richText.AllowDraw)
            {
                richText.appendTitle("MineGame操作日志");
                richText.appendObject(operate);
            }
        }


        
        #endregion

        #region 事件
        Math.Dot2 oldDownPoint = new Math.Dot2();
        public void ellipse_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                //Ellipse ellipse = sender as Ellipse;
                if (ellipse ==null)
                {
                    return;
                }
                Math.Dot2 dot = new Math.Dot2(e.GetPosition(graph)) - oldDownPoint;
                Reflesh(dot);
                ellipse.Margin = new Thickness(e.GetPosition(graph).X -7 - oldDownPoint.X, e.GetPosition(graph).Y  -7-oldDownPoint.Y, 0, 0);

                if (richText.AllowDraw)
                {
                    richText.appendLine("MineGame操作日志:ellipse_MouseMove");
                    //richText.appendObject(dot);
                }
            }
            
        }

        void ellipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            ellipse.RenderTransform = new ScaleTransform(6, 6);
            oldDownPoint.X = e.GetPosition(graph).X;
            oldDownPoint.Y = e.GetPosition(graph).Y;
            //Lines(border);
            if (richText.AllowDraw)
            {
                richText.appendTitle("MineGame操作日志:ellipse_MouseLeftButtonDown");
                richText.appendObject(oldDownPoint);
            }

            oldDownPoint -= new Math.Dot2(ellipse.Margin.Left + ellipse.Width / 2.0, ellipse.Margin.Top + ellipse.Width / 2.0);
            
        }
        private void ellipse_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            ellipse.RenderTransform = new ScaleTransform(2, 2);
            if (richText.AllowDraw)
            {
                richText.appendTitle("MineGame操作日志:ellipse_MouseLeftButtonUp");
                richText.appendObject(oldDownPoint);
            }
        }
        private void ellipse_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (richText.AllowDraw)
            {
                richText.appendTitle("MineGame操作日志:ellipse_MouseDown");
                //richText.appendObject(sender);
            }
        }

        void ellipse_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Math.Operate operate = border.fetchLog();
            //oldDownPoint.X = e.GetPosition(graph).X;
            //oldDownPoint.Y = e.GetPosition(graph).Y;
            oldDownPoint = new Math.Dot2(ellipse.Margin.Left + ellipse.Width / 2.0, ellipse.Margin.Top + ellipse.Width / 2.0);
            border.Modify(oldDownPoint, operate.index);
            if (richText.AllowDraw)
            {
                richText.appendTitle("MineGame操作日志:ellipse_MouseLeave");
                richText.appendObject(oldDownPoint);
            }
        }
        #endregion
    }
}
