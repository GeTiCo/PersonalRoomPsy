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
    /// Логика взаимодействия для MessageBox_Window.xaml
    /// </summary>
    public partial class MessageBox_Window : Window
    {
        public enum MessageBoxButton
        {
            YesNoCancel,
            OK,
            YesNo,
            OkCancel
        }
        public enum ButtonResult
        {
            NULL,
            YES,
            NO,
            CANCEL,
            OK
        }
        public static ButtonResult buttonResultClicked;
        public MessageBox_Window(string message, string title, MessageBoxButton button)
        {
            InitializeComponent();
            TBLOCK_Title.Text = title;
            TBLOCK_Message.Text = message;
            buttonResultClicked = ButtonResult.NULL;
            if (button == MessageBoxButton.OK)
            {
                SP_ContainsButton.Children.Remove(BTN_NO);
                SP_ContainsButton.Children.Remove(BTN_YES);

            }
            else if (button == MessageBoxButton.YesNo)
            {
                SP_ContainsButton.Children.Remove(BTN_OK);

            }
        }
        public static void Show(string text, string title = "", MessageBox_Window.MessageBoxButton button = MessageBox_Window.MessageBoxButton.OK)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                new MessageBox_Window(text, title, button).Show();
            });
        }
        public static void ShowDialog(string text, string title = "", MessageBox_Window.MessageBoxButton button = MessageBox_Window.MessageBoxButton.OK)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                new MessageBox_Window(text, title, button).ShowDialog();
            });
        }

        private void Button_Click_OK(object sender, RoutedEventArgs e)
        {
            buttonResultClicked = ButtonResult.OK;
            this.Close();
        }

        private void Button_Click_YES(object sender, RoutedEventArgs e)
        {
            buttonResultClicked = ButtonResult.YES;
            this.Close();
        }

        private void Button_Click_NO(object sender, RoutedEventArgs e)
        {
            buttonResultClicked = ButtonResult.NO;
            this.Close();
        }

        private void Button_Click_CANCEL(object sender, RoutedEventArgs e)
        {
            buttonResultClicked = ButtonResult.CANCEL;
            this.Close();
        }
    }
}
