using PersonalRoomPsy.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using PersonalRoomPsy.Model;
using MahApps.Metro.Controls;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;


using Button = System.Windows.Controls.Button;
using Style = System.Windows.Style;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using MaterialDesignThemes.Wpf;
using PersonalRoomPsy.View;

namespace PersonalRoomPsy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Classes.DBEditor.DB = new Model.personalRoomPsyEntities();


            loginBox.Text = "";
            passwordBox.Password = "";
        }
        //Авторизация
        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text;
            string password = passwordBox.Password;

            login = login.Replace(" ", "");
            password = password.Replace(" ", "");

            loginBox.BorderBrush = System.Windows.Media.Brushes.Gray;
            passwordBox.BorderBrush = System.Windows.Media.Brushes.Gray;

            if (login != "" && password != "")
            {
                //Проверка наличия пользователля
                bool resultFindUser = Classes.DBEditor.findUser(login, password);

                if (resultFindUser == true)
                {
                    View.ContentWindow personalUserWindow = new View.ContentWindow();
                    personalUserWindow.Show();
                    this.Close();
                }
                else
                {
                    markerStatus.Visibility = Visibility.Visible;
                    markerStatus.Text = "Неверный логин или пароль";
                }
            }
            else
            {
                markerStatus.Visibility = Visibility.Visible;
                markerStatus.Foreground = Brushes.Red;
                if (login == "")
                {
                    loginBox.BorderBrush = System.Windows.Media.Brushes.Red;
                }
                if (password == "")
                {
                    passwordBox.BorderBrush = System.Windows.Media.Brushes.Red;
                }

                markerStatus.Text = "Заполнены не все строки";
            }
        }


        //------------------------------------------Окно регистрации---------------------------------------
        //Открывает окно регистрации
        private void registration_Click(object sender, RoutedEventArgs e)
        {

            allCountry();
            CountryComboBox.SelectedItem = CountryComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Tag as string == "Ru");

            imagePhoto.Visibility = Visibility.Visible;

            newLoginBox_text.Foreground = Brushes.White;
            newPassword1Box_text.Foreground = Brushes.White;
            newPassword2Box_text.Foreground = Brushes.White;
            fullNameUser_text.Foreground = Brushes.White;
            dateBirth_text.Foreground = Brushes.White;
            newEmail_text.Foreground = Brushes.White;
            newPhone_text.Foreground = Brushes.White;
            sex_text.Foreground = Brushes.White;
            countryUser_text.Foreground = Brushes.White;
            cityUser_text.Foreground = Brushes.White;
            problemLine_text.Foreground = Brushes.White;
            politic1.Foreground = Brushes.White;
            politic2.Foreground = Brushes.White;

            signInGrid.Visibility = Visibility.Visible;
            logInGrid.Visibility = Visibility.Hidden;
            forgotpass.Visibility = Visibility.Hidden;
        }
        //Функция загрузки аватара в cache и отображения фотографии в форме регистрации
        private void newphotoClient(object sender, MouseButtonEventArgs e)
        {
            clearCachephoto();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName.Replace(" ", "") != "")
            {
                Directory.CreateDirectory(SupportClass.pathexe + $@"/AppData/Cache/cache_photo");
                string url = SupportClass.pathexe + $@"/AppData/Cache/cache_photo/newAvatarPhoto.png";
                File.Copy(openFileDialog.FileName, url);

                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = SupportClass.ShowImageBit(url);
                photoBorder.Background = imageBrush;
                photoBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4AE83A"));
                imagePhoto.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Файл не выбран или отсутствует название!", MessageBox_Window.MessageBoxButton.OK);
            }
        }
        //Функция загрузки диплома в cacheEducation и индикация загруженного файла в форме регистрации
        private void SaveEducation_Click(object sender, RoutedEventArgs e)
        {
            clearCacheEducation();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName.Replace(" ", "") != "")
            {
                Directory.CreateDirectory(SupportClass.pathexe + $@"/AppData/Cache/cache_education");
                string url = SupportClass.pathexe + $@"/AppData/Cache/cache_education/mainEducation.png";
                File.Copy(openFileDialog.FileName, url);

                educationText.Text = "Диплом загружен";
                educationText.Foreground = (Brush)new BrushConverter().ConvertFromString("#4AE83A");

            }
            else
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Файл не выбран или отсутствует название!", MessageBox_Window.MessageBoxButton.OK);
            }
        }
        //Функция загрузки паспорта в cacheEducation и индикация загруженного файла в форме регистрации
        private void SavePasport_Click(object sender, RoutedEventArgs e)
        {
            clearCacheDoc();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName.Replace(" ", "") != "")
            {
                Directory.CreateDirectory(SupportClass.pathexe + $@"/AppData/Cache/cache_doc");
                string url = SupportClass.pathexe + $@"/AppData/Cache/cache_doc/pasport.png";
                File.Copy(openFileDialog.FileName, url);

                pasportText.Text = "Паспорт загружен";
                pasportText.Foreground = (Brush)new BrushConverter().ConvertFromString("#4AE83A");

            }
            else
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Файл не выбран или отсутствует название!", MessageBox_Window.MessageBoxButton.OK);
            }
        }
        private bool IsPasswordValid(string password)
        {
            // Проверяем длину пароля
            if (password.Length < 8)
            {
                return false;
            }

            // Регулярное выражение для проверки, что строка содержит хотя бы одну букву и одну цифру
            Regex regex = new Regex(@"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]*$");
            return regex.IsMatch(password);
        }
        //Функция создания пользователя ожидающего подтверждение
        private void registrationNewUser_click(object sender, RoutedEventArgs e)
        {
            newLoginBox_text.Foreground = Brushes.White;
            newPassword1Box_text.Foreground = Brushes.White;
            newPassword2Box_text.Foreground = Brushes.White;
            fullNameUser_text.Foreground = Brushes.White;
            dateBirth_text.Foreground = Brushes.White;
            newEmail_text.Foreground = Brushes.White;
            newPhone_text.Foreground = Brushes.White;
            sex_text.Foreground = Brushes.White;
            countryUser_text.Foreground = Brushes.White;
            cityUser_text.Foreground = Brushes.White;
            problemLine_text.Foreground = Brushes.White;
            politic1.Foreground = Brushes.White;
            politic2.Foreground = Brushes.White;

            SolidColorBrush buttonColorPhoto = photoBorder.Background as SolidColorBrush;
            SolidColorBrush buttonColorEducation = education.Background as SolidColorBrush;
            SolidColorBrush buttonColorPasport = pasport.Background as SolidColorBrush;

            if (newLoginBox.Text.Replace(" ", "") != "" &&
                newPassword1Box.Password.Replace(" ", "") != "" &&
                newPassword2Box.Password.Replace(" ", "") != "" &&
                newPassword1Box.Password == newPassword2Box.Password &&
                fullNameUser.Text.Replace(" ", "") != "" &&
                dateBirth.Text.Replace(" ", "") != "" &&
                dateBirth.Text.Contains("_") == false &&
                newEmail.Text.Replace(" ", "") != "" &&
                newPhone.Text.Replace(" ", "") != "" &&
                IsValidEmail(newEmail.Text) == true &&
                (int)countryUser.SelectedValue > 0 &&
                cityUser.SelectedItem != null &&
                (maleBox.IsChecked == true || femaleBox.IsChecked == true))
                {
                if (IsPasswordValid(newPassword1Box.Password) == true && newPassword1Box.Password.Length >= 8)
                {
                    var checkUser = Classes.DBEditor.DB.user.Where(a => a.loginUser == newLoginBox.Text || a.loginUser == newEmail.Text).FirstOrDefault();

                    if (checkUser == null)
                    {
                        if (politic1.IsChecked == true && politic2.IsChecked == true)
                        {
                            Model.user user = new Model.user();

                            user.loginUser = newLoginBox.Text;
                            user.passwordUser = newPassword2Box.Password;
                            user.fullNameUser = fullNameUser.Text;
                            user.emailUser = newEmail.Text;
                            user.dateBirth = DateTime.Parse(dateBirth.Text);
                            user.phoneUser = newPhone.Text;
                            user.verificationUser = 1;
                            user.idRoleUser = 4;
                            user.lineWorking = (int)problemLine.SelectedValue;
                            if (maleBox.IsChecked == true) { user.sexUser = "Мужской"; }
                            else { user.sexUser = "Женский"; }
                            user.urlPhoto = $@"/AppData/User/{newLoginBox.Text}/{newLoginBox.Text}.png";

                            var selectedCity = cityUser.SelectedItem as city;
                            var findCity = Classes.DBEditor.DB.city.Where(a => a.nameCity == selectedCity.nameCity).FirstOrDefault();
                            
                            user.countryUser = (int)countryUser.SelectedValue;
                            user.cityUser = findCity.idCity;

                            Directory.CreateDirectory(SupportClass.pathexe + $@"/AppData/User/{newLoginBox.Text}");
                            Directory.CreateDirectory(SupportClass.pathexe + $@"/AppData/User/{newLoginBox.Text}/Data");
                            Directory.CreateDirectory(SupportClass.pathexe + $@"/AppData/User/{newLoginBox.Text}/Data/Documents");
                            try
                            {
                                File.Move(SupportClass.pathexe + $@"/AppData/Cache/cache_photo/newAvatarPhoto.png", SupportClass.pathexe + $@"/AppData/User/{newLoginBox.Text}/{newLoginBox.Text}.png");
                            }
                            catch
                            {
                                File.Copy(SupportClass.pathexe + $@"/AppData/Resources/base_avatar.png", SupportClass.pathexe + $@"/AppData/User/{newLoginBox.Text}/{newLoginBox.Text}.png");
                            }
                            try
                            {
                                File.Move(SupportClass.pathexe + $@"/AppData/Cache/cache_education/mainEducation.png", SupportClass.pathexe + $@"/AppData/User/{newLoginBox.Text}/Data/education{newLoginBox.Text}.png");
                            }
                            catch
                            {
                                File.Copy(SupportClass.pathexe + $@"/AppData/Resources/base_avatar.png", SupportClass.pathexe + $@"/AppData/User/{newLoginBox.Text}/Data/education{newLoginBox.Text}.png");
                            }
                            try
                            {
                                File.Move(SupportClass.pathexe + $@"/AppData/Cache/cache_doc/pasport.png", SupportClass.pathexe + $@"/AppData/User/{newLoginBox.Text}/Data/Documents/pasport{newLoginBox.Text}.png");
                            }
                            catch
                            {
                                File.Copy(SupportClass.pathexe + $@"/AppData/Resources/base_avatar.png", SupportClass.pathexe + $@"/AppData/User/{newLoginBox.Text}/Data/Documents/pasport{newLoginBox.Text}.png");
                            }

                            Classes.DBEditor.DB.user.Add(user);
                            Classes.DBEditor.DB.SaveChanges();
                            MessageBox_Window.ShowDialog("Уведомление", $"Пользователь успешно создан!", MessageBox_Window.MessageBoxButton.OK);
                            Back_Click(this, new RoutedEventArgs());
                        }
                        else
                        {
                            MessageBox_Window.ShowDialog("Уведомление", $"Подтвердите политику конфиденциальности и политику пользования!", MessageBox_Window.MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        MessageBox_Window.ShowDialog("Уведомление", $"Пользователь с таким логином или почтой уже существует!", MessageBox_Window.MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox_Window.ShowDialog("Уведомление", $"Пароль должен содержать минимум 8 символов и иметь в наличии английские буквы и цифры!", MessageBox_Window.MessageBoxButton.OK);
                }
                
            }
            else
            {
                if (newLoginBox.Text.Replace(" ", "") == "")
                {
                    newLoginBox_text.Foreground = Brushes.Red;
                }

                if (newPassword1Box.Password.Replace(" ", "") == "")
                {
                    newPassword1Box_text.Foreground = Brushes.Red;
                }
                if (newPassword2Box.Password.Replace(" ", "") == "")
                {
                    newPassword2Box_text.Foreground = Brushes.Red;
                }

                if (fullNameUser.Text.Replace(" ", "") == "")
                {
                    fullNameUser_text.Foreground = Brushes.Red;
                }

                if (dateBirth.Text.Replace(" ", "") == "" || dateBirth.Text.Contains("_"))
                {
                    dateBirth_text.Foreground = Brushes.Red;
                }

                if (newEmail.Text.Replace(" ", "") == "" || IsValidEmail(newEmail.Text) != true)
                {
                    newEmail_text.Foreground = Brushes.Red;
                }

                if (newPhone.Text.Replace(" ", "") == "" || newPhone.Text.Contains("_"))
                {
                    newPhone_text.Foreground = Brushes.Red;
                }

                if (politic1.IsChecked == false)
                {
                    politic1.Foreground = Brushes.Red;
                }
                if (politic2.IsChecked == false)
                {
                    politic2.Foreground = Brushes.Red;
                }

                if (maleBox.IsChecked == false && femaleBox.IsChecked == false)
                {
                    sex_text.Foreground = Brushes.Red;
                }
                if (countryUser == null || countryUser.SelectedValue == null || (int)countryUser.SelectedValue <= 0)
                {
                    countryUser_text.Foreground = Brushes.Red;
                }
                if (problemLine == null || problemLine.SelectedValue == null || (int)problemLine.SelectedValue <= 0)
                {
                    problemLine_text.Foreground = Brushes.Red;
                }
                try
                {
                    if (cityUser?.SelectedValue == null || (int)cityUser.SelectedValue <= 0)
                    {
                        cityUser_text.Foreground = Brushes.Red;
                    }
                }
                catch
                {
                    cityUser_text.Foreground = Brushes.Red;
                }

                MessageBox_Window.ShowDialog("Уведомление", $"Не все обязательные поля заполнены!", MessageBox_Window.MessageBoxButton.OK);
            }
        }
        //Добавляет страны в комбобокс
        public void allCountry()
        {
            List<Model.lineProblem> allLines = Classes.DBEditor.DB.lineProblem.ToList();
            problemLine.ItemsSource = allLines;
            problemLine.SelectedValuePath = "idLineProblem";
            problemLine.DisplayMemberPath = "nameLineProblem";


            List<Model.country> allCountry = Classes.DBEditor.DB.country.ToList();
            countryUser.ItemsSource = allCountry;
            countryUser.SelectedValuePath = "idCountry";
            countryUser.DisplayMemberPath = "nameCountry";

            if (countryUser.SelectedIndex > -1)
            {
                allCity();
            }
        }
        //Добавляет города в комбобокс на основании выбранной страны
        public void allCity()
        {
            List<Model.city> allCity = Classes.DBEditor.DB.city.Where(a => a.idFromCountry == countryUser.SelectedIndex + 1).ToList();
            cityUser.ItemsSource = allCity;
            cityUser.SelectedValuePath = "idCity";
            cityUser.DisplayMemberPath = "nameCity";
        }
        //Вспомогательная функция, включающая комбобокс городов
        private void countryUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            allCountry();
            cityUser.IsEnabled = true;
        }
        //Вспомогательная функция (без функция)
        private void cityUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        //------------------------------------------Окно Восстановления пароля---------------------------------------
        //Открывает окно восстановления пароля
        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            btnCheckCode.IsEnabled = true;
            codeUpdate.IsEnabled = false;
            btnNewPassword.Visibility = Visibility.Visible;

            forgotpass.Visibility = Visibility.Visible;
            signInGrid.Visibility = Visibility.Hidden;
            logInGrid.Visibility = Visibility.Hidden;
        }
        //Фунция проверки пользователя для будущей смены пароля
        private void CheckMail_click(object sender, RoutedEventArgs e)
        {
            if (codeUpdate.Text.Replace(" ", "") == "" && checkEmail.IsReadOnly == false)
            {
                if (checkEmail.Text.Replace(" ", "") != "" && codeUpdate.IsEnabled == false && SaveInformaion.supportCode == 0)
                {
                    var findUser = Classes.DBEditor.DB.user.Where(a => a.emailUser == checkEmail.Text).FirstOrDefault();
                    if (findUser != null)
                    {
                        SaveInformaion.typeUserAccountDB = 1;
                        checkEmail.IsReadOnly = true;
                        sendMessage();
                        codeUpdate.IsEnabled = true;
                        btnICo.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.CheckCircleOutline;
                    }
                    else
                    {
                        MessageBox_Window.ShowDialog("Уведомление", $"Пользователь с такой почтой не найден!", MessageBox_Window.MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox_Window.ShowDialog("Уведомление", $"Заполните поле почты!", MessageBox_Window.MessageBoxButton.OK);
                }
            }
            if (SaveInformaion.supportCode != 0 && codeUpdate.IsEnabled == true && codeUpdate.Text.Replace(" ", "") != "")
            {
                
                if (codeUpdate.Text == SaveInformaion.supportCode.ToString())
                {
                    codeUpdate.IsReadOnly = true;
                    btnCheckCode.IsEnabled = false;
                    newPassword.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox_Window.ShowDialog("Уведомление", $"Введен неверный код восстановления!", MessageBox_Window.MessageBoxButton.OK);
                }
            }
        }
        //Функция Обновления пароля при совпадении нового
        private void refrash_click(object sender, RoutedEventArgs e)
        {
            btnNewPassword.IsEnabled = true;
            if (newpass.Password == newpassdouble.Password && newpass.Password.Replace(" ", "") != "")
            {
                if (SaveInformaion.typeUserAccountDB == 1)
                {
                    Model.user user = Classes.DBEditor.DB.user.Where(a => a.emailUser == checkEmail.Text).FirstOrDefault();
                    user.passwordUser = newpassdouble.Password;
                    Classes.DBEditor.DB.SaveChanges();
                }
                if (SaveInformaion.typeUserAccountDB == 2)
                {
                    Model.user user = Classes.DBEditor.DB.user.Where(a => a.emailUser == checkEmail.Text).FirstOrDefault();
                    user.passwordUser = newpassdouble.Password;
                    Classes.DBEditor.DB.SaveChanges();
                }
                MessageBox_Window.ShowDialog("Уведомление", $"Пароль обновлен!", MessageBox_Window.MessageBoxButton.OK);
                forgotpass.Visibility = Visibility.Hidden;
                logInGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Пароли не совпадают!", MessageBox_Window.MessageBoxButton.OK);
            }
        }
        //Функция отправки кода сброса пароля + сохранение временного кода в приложении
        public void sendMessage()
        {
            Random rnd = new Random();
            int number = rnd.Next(100000, 999999);
            Classes.SaveInformaion.supportCode = number;

            SmtpClient user = new SmtpClient();
            user.Credentials = new NetworkCredential("sparta22334455@mail.ru", "gVZxemuyf69kt0isTYXe");
            user.Host = "smtp.mail.ru";
            user.Port = 587;
            user.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("sparta22334455@mail.ru");
            mail.To.Add(new MailAddress(checkEmail.Text));
            mail.Subject = "Update password";
            mail.Body = $"Code for reset password {number}";
            user.Send(mail);

            MessageBox_Window.ShowDialog("Уведомление", $"Сообщение отправлено на вашу электронную почту!", MessageBox_Window.MessageBoxButton.OK);
        }

        //------------------------------------------Вспомогательные функции---------------------------------------
        //Возвращает окно авторизации, очищая поля восстановления пароля и поля регистрации
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (forgotpass.Visibility == Visibility.Visible)
            {
                newpass.Clear();
                newpassdouble.Clear();
                checkEmail.Clear();
                codeUpdate.Clear();

                SaveInformaion.supportCode = 0;
                SaveInformaion.typeUserAccountDB = 0;

                newPassword.Visibility = Visibility.Hidden;
                checkEmail.IsReadOnly = false;
                codeUpdate.IsEnabled = false;
            }
            if (signInGrid.Visibility == Visibility.Visible)
            {
                newLoginBox.Clear();
                newPassword1Box.Clear();
                newPassword2Box.Clear();

                politic1.IsChecked = false;
                politic2.IsChecked = false;

                maleBox.IsChecked = false;
                femaleBox.IsChecked = false;

                fullNameUser.Clear();
                newEmail.Clear();
                newPhone.Clear();
                dateBirth.Clear();

                photoBorder.BorderBrush = Brushes.Gray;
                problemLine.SelectedIndex = 0;

                countryUser.SelectedItem = null;
                cityUser.SelectedItem = null;

                educationText.Text = "Загрузите диплом";
                educationText.Foreground = Brushes.White;

                pasportText.Text = "Загрузите паспорт";
                pasportText.Foreground = Brushes.White;

                problemLine.SelectedIndex = 0;

                photoBorder.Background = Brushes.White;
            }

            logInGrid.Visibility = Visibility.Visible;
            forgotpass.Visibility = Visibility.Hidden;
            signInGrid.Visibility = Visibility.Hidden;
        }
        private void CloseTXT(object sender, RoutedEventArgs e)
        {
            textContent.Visibility = Visibility.Hidden;
        }
        //Функция очистки папки cache
        public void clearCacheDoc()
        {
            try
            {
                string folderPath = SupportClass.pathexe + $@"/AppData/Cache/cache_doc";
                // Удаляем все файлы в папке
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    File.Delete(file);
                }

                // Рекурсивно удаляем все подпапки
                DeleteAllDirectories(folderPath);
            }
            catch
            {

            }
            

        }
        public void clearCachephoto()
        {
            try
            {
                string folderPath = SupportClass.pathexe + $@"/AppData/Cache/cache_photo";
                // Удаляем все файлы в папке
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    File.Delete(file);
                }

                // Рекурсивно удаляем все подпапки
                DeleteAllDirectories(folderPath);
            }
            catch
            {

            }
            

        }
        public void clearCacheEducation()
        {
            try
            {
                string folderPath = SupportClass.pathexe + $@"/AppData/Cache/cache_education";
                // Удаляем все файлы в папке
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    File.Delete(file);
                }

                // Рекурсивно удаляем все подпапки
                DeleteAllDirectories(folderPath);
            }
            catch
            {

            }
            

        }
        private void politicPO(object sender, RoutedEventArgs e)
        {
            textContent.Visibility = Visibility.Visible;
            TitleTXT.Text = "Политика конфиденциальности";
            // Укажите путь к файлу
            string filePath = SupportClass.pathexe + @"/AppData/Resources/politic.txt";

            try
            {
                // Чтение содержимого файла
                string fileContent = File.ReadAllText(filePath);
                textContentBlock.Text = fileContent;
            }
            catch (Exception)
            {
                textContentBlock.Text = "Политики конфиденциальности в процессе редактирования";
            }
        }
        private void rulesPO(object sender, RoutedEventArgs e)
        {
            textContent.Visibility = Visibility.Visible;
            TitleTXT.Text = "Правила использования приложения";
            // Укажите путь к файлу
            string filePath = SupportClass.pathexe + @"/AppData/Resources/rules.txt";

            try
            {
                // Чтение содержимого файла
                string fileContent = File.ReadAllText(filePath);
                textContentBlock.Text = fileContent;
            }
            catch (Exception)
            {
                textContentBlock.Text = "Политики конфиденциальности в процессе редактирования";
            }
        }
        //Дополнение к функции очистки папки cache
        static void DeleteAllDirectories(string targetDirectory)
        {
            // Получаем все подпапки
            string[] subdirectories = Directory.GetDirectories(targetDirectory);

            // Рекурсивно удаляем все файлы и подпапки в каждой подпапке
            foreach (string subdirectory in subdirectories)
            {
                // Удаляем все файлы в текущей подпапке
                foreach (string file in Directory.GetFiles(subdirectory))
                {
                    File.Delete(file);
                }

                // Рекурсивно вызываем этот метод для текущей подпапки
                DeleteAllDirectories(subdirectory);

                // Удаляем саму текущую подпапку
                Directory.Delete(subdirectory);
            }
        }

        public bool IsValidEmail(string email)
        {
            // Регулярное выражение для проверки формата адреса электронной почты
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            // Проверяем соответствие введенного текста регулярному выражению
            return Regex.IsMatch(email, pattern);
        }

        public enum Country
        {
            Ru,
            Blr,
            Mng,
            Kz
        }

        public static class CountryMask
        {
            public static Dictionary<Country, string> Masks = new Dictionary<Country, string>
            {
                { Country.Ru, "+7 (000) 000-00-00" },
                { Country.Blr, "+375 (00) 000-00-00" },
                { Country.Mng, "+876 (000) 000-000" },
                { Country.Kz, "+7 (000) 000-00-00" }
            };
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CountryComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is string tag)
            {
                if (Enum.TryParse(tag, out Country selectedCountry))
                {
                    newPhone.Text = string.Empty; // Очистка текста перед изменением маски
                    newPhone.Mask = CountryMask.Masks[selectedCountry];
                }
            }
        }


    }


}
