using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Product2.Test
{
    /// <summary>
    /// MineGame.xaml 的交互逻辑
    /// </summary>
    public partial class MineGame : Window
    {
        
        public MineGame()
        {
            InitializeComponent();
        }
        public MineGame(Product2.RichTextProcess process)
        {
            InitializeComponent();
            draw = new Draw.Draw(this.MineCanvas, process);
        }
        Math.Border border;
        Draw.Draw draw;
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Math.Vector vec = new Math.Vector(new Math.Dot2(100, 200), new Math.Dot2(200, 100));
            

            //draw.Line(vec);
            //List<Math.Dot2> dots = Math.Math.CreateRandomDots(10, (int)this.MineCanvas.Width - 10, (int)this.MineCanvas.Height - 10);
            //draw.Dots(dots);
            //draw.Lines(dots);
            //List<Math.Dot2> interActive;
            //Math.Border border = new Math.Border(20, (int)this.MineCanvas.Width - 10, (int)this.MineCanvas.Height - 10, out interActive, draw);
            border = new Math.Border(3, (int)this.MineCanvas.Width - 10, (int)this.MineCanvas.Height - 10);
            draw.Lines(border);
            //draw.Dots(interActive);


            //Math.Dot2 ss;
            //List<Math.Dot2> source = new List<Math.Dot2>() { new Math.Dot2(500, 500),new Math.Dot2(1, 1) ,new Math.Dot2(500, 1),new Math.Dot2(1, 500) };
            //if (Math.Math.isIntersect(new Math.Vector(source[0], source[1]), new Math.Vector(source[2], source[3]), out ss))
            //{
            //    draw.Lines(source);
            //    interActive = new List<Math.Dot2>() { ss };
            //    draw.Dots(interActive);
            //}
        }

        private void Reflesh_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            Math.Dot2 dot = Test.Math.Math.CreateRandomDot(rand, (int)this.MineCanvas.Width - 10, (int)this.MineCanvas.Height - 10);
            this.MineCanvas.Children.Clear();
            //draw.Dots(new List<Math.Dot2>() { dot });
            border.put(dot);
            if (IsFill.IsChecked==true)
            {
                draw.Border(border);
            }
            else
            {
                draw.Lines(border);
            }
            
            //draw.Lines(border);
            //draw.Appear(border.fetchLog().dot);
        }
        
        private void Bebug_Click(object sender, RoutedEventArgs e)
        {
            this.MineCanvas.Children.Clear();
            Math.Operate operate;
            operate = border.fetchLog();
            //olddot = operate.dot;
            switch (operate.type)
            {
                case Product2.Test.Math.Type.Append:
                    border.Delete(operate.index);
                    //border.Insert(operate.dot, operate.index);
                    break;
                case Product2.Test.Math.Type.Insert:
                    border.Delete(operate.index);
                    break;
                case Product2.Test.Math.Type.Delete:
                    //border.Insert(operate.dot, operate.index);
                    border.put(operate.dot);
                    break;
                case Product2.Test.Math.Type.Read:
                    break;
                case Product2.Test.Math.Type.Modify:
                    break;
                default:
                    break;
            }
            if (IsFill.IsChecked == true)
            {
                draw.Border(border);
            }
            else
            {
                draw.Lines(border);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.MineCanvas.Children.Clear();
            border = new Math.Border(3, (int)this.MineCanvas.Width - 10, (int)this.MineCanvas.Height - 10);
            draw.Lines(border);
        }
    }
}
