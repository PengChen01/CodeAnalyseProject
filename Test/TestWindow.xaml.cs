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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Product2.Test
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            //for (int i = 0; i < 10; i++)
            //{
            //    //listBook.Add(new Book(0,123456, "testBook"+i, "Math", "qiaobus", "shanghai", "just a book", "none"));
            //    listView1.Items.Add(new User("陈鹏","25"));
 
            //}
        }

        private void listView1_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C))
            {
                string clipboardtext = "";
                System.Collections.IList selectitem = this.listView1.SelectedItems;
                foreach (User item in selectitem)
                {
                   clipboardtext+= item.Name + "\t" + item.Age + "\r\n" ;
                }
                textContent.Text = clipboardtext;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            LoginGrid.OpacityMask = this.Resources["ClosedBrush"] as LinearGradientBrush;
            Storyboard std = this.Resources["ClosedStoryboard"] as Storyboard;
            std.Completed += delegate { this.Close(); };

            std.Begin();
        }

        private void Run_MouseLeftButtonDown3(object sender, MouseButtonEventArgs e)
        {
            //sender = null;
            //e = null;

        }

        private void Run_MouseLeftButtonDown6(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("你按了我");
            //(sender as Run).Text = (sender as Run).Text +"1";
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("你按了我2");
        }
    }
}
