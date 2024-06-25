using MahApps.Metro.Controls;
using PersonalRoomPsy.View.ContentFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PersonalRoomPsy.View
{
    /// <summary>
    /// Логика взаимодействия для ContentWindow.xaml
    /// </summary>
    public partial class ContentWindow : MetroWindow
    {
        public ContentWindow()
        {
            InitializeComponent();
            AllPageContent userInformationWindow = new AllPageContent();
            mainContentFrame.Navigate(userInformationWindow);
        }
    }
}
