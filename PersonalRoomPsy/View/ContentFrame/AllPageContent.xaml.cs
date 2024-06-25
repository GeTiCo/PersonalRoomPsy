using LiveCharts.Wpf;
using LiveCharts;
using PersonalRoomPsy.Classes;
using PersonalRoomPsy.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
using System.IO;
using System.Data.Entity.Validation;
using System.Windows.Markup;
using System.Runtime.Remoting.Contexts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using LiveCharts.Definitions.Charts;
using System.Data.Entity;


namespace PersonalRoomPsy.View.ContentFrame
{
    /// <summary>
    /// Логика взаимодействия для AllPageContent.xaml
    /// </summary>
    public partial class AllPageContent : Page
    {
        public static List<Classes.User> listUser;
        public static List<Classes.Orders> listnewOrder;

        public static bool changePhoto = false;
        public static bool changePassport = false;
        public static bool changeEducation = false;

        public static int timeID = 0;
        public static string timeLogin = "";

        public static string changeLogin = "";

        public static int changeRole = 0;
        public static int changeStatus = 0;

        public static bool statusTimeOrder = false;

        public static int typeCloseOrder = 0;


        public AllPageContent()
        {
            InitializeComponent();

            Classes.DBEditor.DB = new Model.personalRoomPsyEntities();
            Classes.DBEditor.User = new Model.user();

            FIOUserPlusProblem.Text = Classes.SaveInformaion.fullName;

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = SupportClass.ShowImageBit(SupportClass.pathexe + SaveInformaion.photo);
            photo.Background = imageBrush;
            this.DataContext = this;

            showActualOrders();
            LoadPieChartData();
            monitoringUsers();
            showNewUser();
            makeListOrder();

            

            //-------------------------Адаптация окна под роль пользователя-----------------------
            if (Classes.SaveInformaion.roleID == 1)
            {
                btnWork1.Content = "Мониторинг специалистов";
                btnWork2.Content = "Статистика";
                mainWindowAdmin.Visibility = Visibility.Visible;

                List<role> userRoles = DBEditor.DB.role.ToList();

                role allRole = new role();
                allRole.idRole = 0;
                allRole.nameRole = "Все группы пользователей";
                userRoles.Insert(0, allRole);
                roleUser.ItemsSource = userRoles;
                roleUser.DisplayMemberPath = "nameRole";
                roleUser.SelectedValuePath = "idRole";

                List<country> countryUsers = DBEditor.DB.country.ToList();

                country allCountry = new country();
                allCountry.idCountry = 0;
                allCountry.nameCountry = "Все страны";
                countryUsers.Insert(0, allCountry);
                countryUser.ItemsSource = countryUsers;
                countryUser.DisplayMemberPath = "nameCountry";
                countryUser.SelectedValuePath = "idCountry";

                roleUser.SelectedIndex = 0;
                countryUser.SelectedIndex = 0;
                cityUser.SelectedIndex = 0;
            }
            if (Classes.SaveInformaion.roleID == 2)
            {
                btnWork1.Content = "Мониторинг специалистов";
                btnWork2.Content = "Заявки на проверку данных";
                mainWindowAdmin.Visibility = Visibility.Visible;

                List<country> countryUsers = DBEditor.DB.country.ToList();

                country allCountry = new country();
                allCountry.idCountry = 0;
                allCountry.nameCountry = "Все страны";
                countryUsers.Insert(0, allCountry);
                countryUser.ItemsSource = countryUsers;
                countryUser.DisplayMemberPath = "nameCountry";
                countryUser.SelectedValuePath = "idCountry";

                List<verification> statusUsers = DBEditor.DB.verification.Where(a => a.idVerification == 1 || a.idVerification == 4).ToList();

                verification allStatus = new verification();
                allStatus.idVerification = 0;
                allStatus.nameVerification = "Все верификационные группы";
                statusUsers.Insert(0, allStatus);
                statusUser.ItemsSource = statusUsers;
                statusUser.DisplayMemberPath = "nameVerification";
                statusUser.SelectedValuePath = "idVerification";


                statusUser.SelectedIndex = 0;
                countryUser.SelectedIndex = 0;
                cityUser.SelectedIndex = 0;
            }
            if (Classes.SaveInformaion.roleID == 4)
            {
                showAtention();
                btnWork1.Content = "Личный кабинет";
                btnWork2.Content = "Редактирование личных данных";
                mainWindowUser.Visibility = Visibility.Visible;

            }
            if (Classes.SaveInformaion.roleID == 3)
            {
                btnWork1.Content = "Мониторинг специалистов";
                btnWork2.Content = "Формирование заявки";
                mainWindowAdmin.Visibility = Visibility.Visible;

                List<country> countryUsers = DBEditor.DB.country.ToList();

                country allCountry = new country();
                allCountry.idCountry = 0;
                allCountry.nameCountry = "Все страны";
                countryUsers.Insert(0, allCountry);
                countryUser.ItemsSource = countryUsers;
                countryUser.DisplayMemberPath = "nameCountry";
                countryUser.SelectedValuePath = "idCountry";

                List<typeProblem> typeProblem = DBEditor.DB.typeProblem.ToList();

                typeProblem allProblem = new typeProblem();
                allProblem.idTypeProblem = 0;
                allProblem.nameTypeProblem = "Выберите запрос";
                typeProblem.Insert(0, allProblem);
                boxProblems.SelectedIndex = 0;
                boxProblems.ItemsSource = typeProblem;
                boxProblems.DisplayMemberPath = "nameTypeProblem";
                boxProblems.SelectedValuePath = "idTypeProblem";

                countryUser.SelectedIndex = 0;
                cityUser.SelectedIndex = 0;
            }
        }

        private ConcurrentDictionary<string, BitmapImage> _imageCache = new ConcurrentDictionary<string, BitmapImage>();
        //------------------------------------------Окно администратора---------------------------------------
        //Функция мониторинга пользователей(фильтры)
        public void monitoringUsers()
        {
            if (SaveInformaion.roleID == 1)
            {
                roleUser.Visibility = Visibility.Visible;
                statusUser.Visibility = Visibility.Hidden;

                List<Model.user> user = DBEditor.DB.user.ToList();

                switch (roleUser.SelectedIndex)
                {
                    case 0:
                        break;
                    default:
                        user = user.Where(a => a.idRoleUser == roleUser.SelectedIndex).ToList();
                        break;
                }

                switch (countryUser.SelectedIndex)
                {
                    case 0:
                        break;
                    default:
                        user = user.Where(a => a.countryUser == countryUser.SelectedIndex).ToList();
                        choseCity();
                        break;
                }

                if (cityUser.SelectedIndex > 0)
                {
                    try
                    {
                        var selectedCity = cityUser.SelectedItem as city;
                        user = user.Where(a => a.cityUser == (int)cityUser.SelectedValue).ToList();
                    }
                    catch
                    {
                        // Handle exception if needed
                    }
                }

                if (findName.Text.Length > 0)
                {
                    user = user.Where(pr => pr.fullNameUser.Contains(findName.Text)).ToList();
                }

                choseCity();
                
                listUser = new List<Classes.User>();

                foreach (Model.user newUser in user)
                {
                    Classes.User newUsercl = new Classes.User
                    {
                        idUser = newUser.idUser,
                        login = newUser.loginUser,
                        fullName = newUser.fullNameUser,
                        birth = newUser.dateBirth.ToString(),
                        emailUser = newUser.emailUser,
                        phoneUser = newUser.phoneUser,
                        idRole = newUser.idRoleUser,
                        nameRole = newUser.role.nameRole,
                        lineWorkingID = (int)newUser.lineWorking,
                        lineWorkingName = newUser.lineProblem.nameLineProblem,
                        countryID = newUser.countryUser,
                        countryName = newUser.country.nameCountry,
                        cityID = newUser.cityUser,
                        cityName = newUser.city.nameCity,
                        sex = newUser.sexUser,
                        verificationID = (int)newUser.verificationUser,
                        verificationName = newUser.verification.nameVerification,
                        PhotoURL = newUser.urlPhoto
                    };

                    string imagePath = SupportClass.pathexe + newUsercl.PhotoURL;
                    newUsercl.Photo = GetCachedImage(imagePath);

                    listUser.Add(newUsercl);
                }

                listUsers.ItemsSource = listUser;
            }

            if (SaveInformaion.roleID == 2 || SaveInformaion.roleID == 3)
            {
                List<Model.user> user;

                if (SaveInformaion.roleID == 2)
                {
                    roleUser.Visibility = Visibility.Hidden;
                    statusUser.Visibility = Visibility.Visible;

                    user = DBEditor.DB.user.Where(a => a.idRoleUser == 4).ToList();

                    switch (statusUser.SelectedIndex)
                    {
                        case 0:
                            break;
                        default:
                            user = statusUser.SelectedIndex == 1
                                ? user.Where(a => a.verificationUser == 1).ToList()
                                : user.Where(a => a.verificationUser == 4).ToList();
                            choseCity();
                            break;
                    }

                    switch (countryUser.SelectedIndex)
                    {
                        case 0:
                            break;
                        default:
                            user = user.Where(a => a.countryUser == countryUser.SelectedIndex).ToList();
                            choseCity();
                            break;
                    }

                    if (cityUser.SelectedIndex > 0)
                    {
                        try
                        {
                            user = user.Where(a => a.cityUser == (int)cityUser.SelectedValue).ToList();
                        }
                        catch
                        {
                            // Handle exception if needed
                        }
                    }

                    if (findName.Text.Length > 0)
                    {
                        user = user.Where(pr => pr.fullNameUser.Contains(findName.Text)).ToList();
                    }

                    choseCity();
                }
                else
                {
                    roleUser.Visibility = Visibility.Hidden;
                    statusUser.Visibility = Visibility.Hidden;

                    user = DBEditor.DB.user.Where(a => a.idRoleUser == 4 && a.verificationUser == 3).ToList();

                    switch (countryUser.SelectedIndex)
                    {
                        case 0:
                            break;
                        default:
                            user = user.Where(a => a.countryUser == countryUser.SelectedIndex).ToList();
                            choseCity();
                            break;
                    }

                    if (cityUser.SelectedIndex > 0)
                    {
                        try
                        {
                            user = user.Where(a => a.cityUser == (int)cityUser.SelectedValue).ToList();
                        }
                        catch
                        {
                            // Handle exception if needed
                        }
                    }

                    if (findName.Text.Length > 0)
                    {
                        user = user.Where(pr => pr.fullNameUser.Contains(findName.Text)).ToList();
                    }

                    choseCity();
                }

                listUser = new List<Classes.User>();

                foreach (Model.user newUser in user)
                {
                    Classes.User newUsercl = new Classes.User
                    {
                        idUser = newUser.idUser,
                        login = newUser.loginUser,
                        fullName = newUser.fullNameUser,
                        birth = newUser.dateBirth.ToString(),
                        emailUser = newUser.emailUser,
                        phoneUser = newUser.phoneUser,
                        idRole = newUser.idRoleUser,
                        nameRole = newUser.role.nameRole,
                        lineWorkingID = (int)newUser.lineWorking,
                        lineWorkingName = newUser.lineProblem.nameLineProblem,
                        countryID = newUser.countryUser,
                        countryName = newUser.country.nameCountry,
                        cityID = newUser.cityUser,
                        cityName = newUser.city.nameCity,
                        sex = newUser.sexUser,
                        verificationID = (int)newUser.verificationUser,
                        verificationName = newUser.verification.nameVerification,
                        PhotoURL = newUser.urlPhoto
                    };

                    string imagePath = SupportClass.pathexe + newUsercl.PhotoURL;
                    newUsercl.Photo = GetCachedImage(imagePath);

                    listUser.Add(newUsercl);
                }

                listUsers.ItemsSource = listUser;
            }
        }
        private BitmapImage GetCachedImage(string imagePath)
        {
            if (_imageCache.TryGetValue(imagePath, out BitmapImage cachedImage))
            {
                return cachedImage;
            }

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(imagePath);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            image.Freeze();

            _imageCache[imagePath] = image;
            return image;
        }

        //Функция отображения дополнительной информации о пользователе + изменение его прав

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                // Получаем элемент, по которому был произведен клик
                var clickedItem = GetListBoxItemAt(listBox, e.GetPosition(listBox));

                if (clickedItem != null)
                {
                    // Устанавливаем выбранный элемент
                    listBox.SelectedItem = clickedItem;

                    // Здесь выполняем нужные действия с выбранным элементом
                    HandleListBoxItemClick(clickedItem);
                }
            }
        }
        private object GetListBoxItemAt(ListBox listBox, Point position)
        {
            var element = listBox.InputHitTest(position) as UIElement;

            while (element != null && !(element is ListBoxItem))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
            }

            return (element as ListBoxItem)?.DataContext;
        }
        private void HandleListBoxItemClick(object item)
        {
            DataContext = this;

            Classes.User selectedUser = (Classes.User)listUsers.SelectedItem;

            if (selectedUser != null)
            {
                filters.Visibility = Visibility.Hidden;
                if (SaveInformaion.roleID == 1)
                {
                    oneUsersMonitoring.Visibility = Visibility.Visible;
                    allUsersMonitoring.Visibility = Visibility.Hidden;

                    changeUserPrivate.Visibility = Visibility.Visible;
                    changeUserStatus.Visibility = Visibility.Hidden;

                    List<role> userRoles = DBEditor.DB.role.ToList();

                    changeRoleUser.ItemsSource = userRoles;
                    changeRoleUser.SelectedValue = selectedUser.idRole;
                    changeRoleUser.DisplayMemberPath = "nameRole";
                    changeRoleUser.SelectedValuePath = "idRole";

                    //changeRoleUser.SelectedValue = selectedUser.nameRole;

                    changeLogin = selectedUser.login;

                }
                if (SaveInformaion.roleID == 2)
                {
                    oneUsersMonitoring.Visibility = Visibility.Visible;
                    allUsersMonitoring.Visibility = Visibility.Hidden;

                    changeUserPrivate.Visibility = Visibility.Hidden;
                    changeUserStatus.Visibility = Visibility.Visible;

                    List<verification> status = DBEditor.DB.verification.ToList();

                    changeStatusUser.ItemsSource = status;
                    changeStatusUser.DisplayMemberPath = "nameVerification";
                    changeStatusUser.SelectedValuePath = "idVerification";

                    changeStatusUser.SelectedValue = selectedUser.verificationID;

                    changeLogin = selectedUser.login;
                }
                if (SaveInformaion.roleID == 3)
                {
                    changeUserPrivate.Visibility = Visibility.Hidden;
                    changeUserStatus.Visibility = Visibility.Hidden;
                }

                string imagePath = SupportClass.pathexe + $"/AppData/User/{selectedUser.login}/{selectedUser.login}.png";
                PhotoInfo.Source = SupportClass.ShowImageBit(imagePath);

                FIOInfo.Text = $"{selectedUser.fullName}";
                RoleInfo.Text = $"Должность: {selectedUser.nameRole}";
                status.Text = $"Статус: {selectedUser.verificationName}";
                if (selectedUser.verificationID == 1)
                {
                    status.Foreground = Brushes.Blue;
                }
                if (selectedUser.verificationID == 2)
                {
                    status.Foreground = Brushes.Red;
                }
                if (selectedUser.verificationID == 3)
                {
                    status.Foreground = (Brush)new BrushConverter().ConvertFromString("#1DD300");
                }
                if (selectedUser.verificationID == 4)
                {
                    status.Foreground = Brushes.Orange;
                }
                sexInfo.Text = $"Пол: {selectedUser.sex}";
                emailInfo.Text = $"Электронная почта: {selectedUser.emailUser}";
                phoneInfo.Text = $"Мобильный телефон: {selectedUser.phoneUser}";
                CountryCityInfo.Text = $"Место проживания: {selectedUser.countryName}, {selectedUser.cityName}";

                using (var dbContext = new personalRoomPsyEntities()) // Замените на свой контекст базы данных
                {
                    var rate = DBEditor.DB.historyOrder.Where(a => a.idWorker == selectedUser.idUser && a.rateWorker != null).Select(a => (double)a.rateWorker).ToList();

                    // Вызываем функцию для расчета рейтинга
                    ratingUser.Text = CalculatePerformerRating(rate).ToString();

                }

                try
                {
                    LoadChartData(selectedUser.idUser);
                }
                catch
                {
                    View.MessageBox_Window.ShowDialog("Ошибка таблицы", "Таблица ОЛВ временно недоступна", MessageBox_Window.MessageBoxButton.OK);
                }

            }
            else
            {
                oneUsersMonitoring.Visibility = Visibility.Hidden;
                allUsersMonitoring.Visibility = Visibility.Visible;
            }
        }
        public static string CalculatePerformerRating(List<double> ratings)
        {
            // Проверка на случай пустого списка оценок
            if (ratings.Count == 0)
                return "0.00";

            // Суммируем все оценки
            double sum = ratings.Sum();

            // Вычисляем средний рейтинг
            double averageRating = sum / ratings.Count;

            // Ограничиваем до двух знаков после запятой и возвращаем как строку
            return averageRating.ToString("F2");
        }
        private int LoadChartData(int idUser)
        {
            SeriesCollection seriesCollection = new SeriesCollection();
            string[] labels = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToArray();
            Func<double, string> formatter = value => value.ToString("N");

            using (var dbCont = new personalRoomPsyEntities())
            {
                // Создаем коллекцию значений для гистограммы выполненных заявок
                ChartValues<int> completedValues = new ChartValues<int>();
                for (int i = 0; i < 12; i++)
                {
                    completedValues.Add(0);
                }

                // Получаем количество выполненных заявок для каждого месяца
                var queryCompleted = from order in dbCont.historyOrder
                                     where order.dateCompete.Year == DateTime.Now.Year && order.statusOrder == 3 && order.idWorker ==  idUser
                                     group order by order.dateCompete.Month into g
                                     select new { Month = g.Key, Count = g.Count() };

                foreach (var item in queryCompleted.OrderBy(x => x.Month))
                {
                    completedValues[item.Month - 1] = item.Count;
                }

                // Создаем коллекцию значений для гистограммы отмененных заявок
                ChartValues<int> canceledValues = new ChartValues<int>();
                for (int i = 0; i < 12; i++)
                {
                    canceledValues.Add(0);
                }

                // Получаем количество отмененных заявок для каждого месяца
                var queryCanceled = from order in dbCont.historyOrder
                                    where order.dateCompete.Year == DateTime.Now.Year && order.statusOrder == 4 && order.idWorker == idUser
                                    group order by order.dateCompete.Month into g
                                    select new { Month = g.Key, Count = g.Count() };

                foreach (var item in queryCanceled.OrderBy(x => x.Month))
                {
                    canceledValues[item.Month - 1] = item.Count;
                }

                // Создаем коллекцию значений для гистограммы еще одного типа заявок (например, отклоненных заявок)
                ChartValues<int> rejectedValues = new ChartValues<int>();
                for (int i = 0; i < 12; i++)
                {
                    rejectedValues.Add(0);
                }

                // Получаем количество отклоненных заявок для каждого месяца
                var queryRejected = from order in dbCont.order
                                    where order.dateMaking.Year == DateTime.Now.Year && order.statusOrder == 2 && order.idWorker == idUser
                                    group order by order.dateMaking.Month into g
                                    select new { Month = g.Key, Count = g.Count() };

                foreach (var item in queryRejected.OrderBy(x => x.Month))
                {
                    rejectedValues[item.Month - 1] = item.Count;
                }

                // Находим максимальное количество заказов за месяц
                int maxOrders = Math.Max(completedValues.Max(), Math.Max(canceledValues.Max(), rejectedValues.Max()));

                // Добавляем гистограммы в коллекцию данных
                seriesCollection.Add(new ColumnSeries
                {
                    Title = "Выполненные заявки",
                    Values = completedValues,
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#7120ea"),
                });

                seriesCollection.Add(new ColumnSeries
                {
                    Title = "Отмененные заявки",
                    Values = canceledValues,
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#ef98d0"),
                });

                seriesCollection.Add(new ColumnSeries
                {
                    Title = "Актуальные заявки",
                    Values = rejectedValues,
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#00bcd4"),
                });

                // Устанавливаем месяцы как метки по оси X
                LabelsAxisX.Labels = labels;

                // Устанавливаем форматтер для оси Y
                AxisY.LabelFormatter = formatter;

                // Устанавливаем максимальное значение оси Y
                AxisY.MaxValue = maxOrders;
            }
            // Устанавливаем данные в элемент управления Chart
            cartesianChart.Series = seriesCollection;

            return 0;
        }

        private void Deluser_click(object sender, RoutedEventArgs e)
        {
            Classes.User selectedUser = (Classes.User)listUsers.SelectedItem;
            MessageBox_Window.ShowDialog("ВНИМАНИЕ", "Вы точно хотите безвозвратно удалить этого пользователя?", MessageBox_Window.MessageBoxButton.YesNo);
            if (MessageBox_Window.buttonResultClicked == MessageBox_Window.ButtonResult.YES)
            {
                var orders = DBEditor.DB.order.Where(o => o.idWorker == selectedUser.idUser).ToList().Count();
                if (orders == 0)
                {
                    using (var dbContext = new personalRoomPsyEntities()) // Замените на свой контекст базы данных
                    {
                        var user = dbContext.user.FirstOrDefault(u => u.idUser == selectedUser.idUser);
                        if (user != null)
                        {
                            // Удаление всех связанных сообщений
                            var messages = dbContext.messageProblem.Where(m => m.idUser == selectedUser.idUser).ToList();
                            foreach (var message in messages)
                            {
                                dbContext.messageProblem.Remove(message);
                            }
                            // Удаление всех связанных старых заказов
                            var hisorders = dbContext.historyOrder.Where(o => o.idWorker == selectedUser.idUser).ToList();
                            foreach (var historder in hisorders)
                            {
                                dbContext.historyOrder.Remove(historder);
                            }

                            // Удаление самого пользователя
                            dbContext.user.Remove(user);

                            string folderPath = System.IO.Path.Combine(SupportClass.pathexe, $"AppData/User/{user.loginUser}");

                            try
                            {
                                // Проверка существования папки
                                if (Directory.Exists(folderPath))
                                {
                                    // Сброс атрибутов файлов и директорий перед удалением
                                    SetAttributesNormal(new DirectoryInfo(folderPath));

                                    // Рекурсивное удаление папки и её содержимого
                                    Directory.Delete(folderPath, true);
                                    // Сохранение изменений в базе данных
                                    dbContext.SaveChanges();
                                    MessageBox_Window.ShowDialog("Уведомление", $"Пользователь успешно удален!", MessageBox_Window.MessageBoxButton.OK);
                                }
                                else
                                {
                                    MessageBox_Window.ShowDialog("Уведомление", $"Ошибка удаления пользователя!", MessageBox_Window.MessageBoxButton.OK);
                                }
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Console.WriteLine($"Отказано в доступе");
                            }

                            oneUsersMonitoring.Visibility = Visibility.Hidden;
                            filters.Visibility = Visibility.Visible;
                            allUsersMonitoring.Visibility= Visibility.Visible;
                            monitoringUsers();
                        }


                    }
                }
                else
                {
                    MessageBox_Window.ShowDialog("Уведомление", $"У специалиста есть незакрытые заявки!", MessageBox_Window.MessageBoxButton.OK);
                }
            }
            else
            {
            }
        }
        static void SetAttributesNormal(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
            {
                SetAttributesNormal(subDir);
            }

            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }

            dir.Attributes = FileAttributes.Normal;
        }
        //Вспомогательная функция для мониторинга (Статус пользователя)
        private void statusUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            monitoringUsers();
        }
        //Кнопка сохранения изменений пользователя (админ и кадровик)
        private void saveChangedUser_Click(object sender, RoutedEventArgs e)
        {
            if (SaveInformaion.roleID == 1)
            {
                changeRole = (int)changeRoleUser.SelectedValue;

                Model.user upUser = DBEditor.DB.user.Where(a => a.loginUser == changeLogin).FirstOrDefault();
                upUser.idRoleUser = changeRole;

                if (SaveInformaion.login == changeLogin)
                {
                    MessageBox_Window.ShowDialog("Уведомление", $"Вы не можете изменить свою роль!", MessageBox_Window.MessageBoxButton.OK);
                }
                else
                {
                    Classes.DBEditor.DB.SaveChanges();
                    MessageBox_Window.ShowDialog("Уведомление", $"Роль сотрудника изменена!", MessageBox_Window.MessageBoxButton.OK);
                    monitoringUsers();
                }
            }
            if (SaveInformaion.roleID == 2)
            {
                changeStatus = (int)changeStatusUser.SelectedValue;

                Model.user upUser = DBEditor.DB.user.Where(a => a.loginUser == changeLogin).FirstOrDefault();
                upUser.verificationUser = changeStatus;

                if (SaveInformaion.login == changeLogin)
                {
                    MessageBox_Window.ShowDialog("Уведомление", $"Вы не можете изменить свой статус!", MessageBox_Window.MessageBoxButton.OK);
                }
                else
                {
                    Classes.DBEditor.DB.SaveChanges();
                    MessageBox_Window.ShowDialog("Уведомление", $"Роль сотрудника изменена!", MessageBox_Window.MessageBoxButton.OK);
                    monitoringUsers();
                }
                Classes.DBEditor.DB.SaveChanges();
                monitoringUsers();
            }

        }
        //Функция закрытия окна дополнительной информации
        private void closeInformationUser(object sender, RoutedEventArgs e)
        {
            filters.Visibility = Visibility.Visible;
            allUsersMonitoring.Visibility = Visibility.Visible;
            oneUsersMonitoring.Visibility = Visibility.Hidden;
        }
        //Вспомогательная функция для мониторинга (Роль пользователей)
        private void roleUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            monitoringUsers();
        }
        //Вспомогательная функция для мониторинга (Страна пользователей)
        private void countryUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            monitoringUsers();
            if (countryUser.SelectedIndex > 0)
            {
                cityUser.SelectedIndex = 0;
            }
        }
        //Вспомогательная функция для мониторинга (Город пользователей)
        private void cityUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            monitoringUsers();
        }
        //Вспомогательная функция для мониторинга (Поиск по ФИО)
        private void findName_TextChanged(object sender, TextChangedEventArgs e)
        {
            monitoringUsers();
        }
        //Вспомогательная функция для мониторинга (Отображение города в зависимости от выбранной страны)
        public void choseCity()
        {
            if (countryUser.SelectedIndex > 0)
            {
                cityUser.IsEnabled = true;

                List<city> cityUsers = DBEditor.DB.city.Where(a => a.idFromCountry == countryUser.SelectedIndex).ToList();
                city allCity = new city();
                allCity.nameCity = "Все города";
                cityUsers.Insert(0, allCity);
                cityUser.ItemsSource = cityUsers;
                cityUser.DisplayMemberPath = "nameCity";
                cityUser.SelectedValuePath = "idCity";
            }
            if (countryUser.SelectedIndex <= 0)
            {
                List<city> cityUsers = DBEditor.DB.city.ToList();
                city allCity = new city();
                allCity.nameCity = "Все города";
                cityUsers.Insert(0, allCity);
                cityUser.ItemsSource = cityUsers;
                cityUser.DisplayMemberPath = "nameCity";
                cityUser.SelectedValuePath = "idCity";
            }
        }
        //Функция отображения графов статистики (не готов 3 - объем данных)
        private void LoadPieChartData()
        {
            var roleColors = new[] { "#7120ea", "#ef98d0", "#2db8e7", "#aa5fe1" };
            var sexColors = new[] { "#7120ea", "#ef98d0" };
            var statusColors = new[] { "#7120ea", "#ef98d0", "#2db8e7", "#aa5fe1" };

            pieChartRole.Series = new SeriesCollection
    {
        new PieSeries
        {
            Title = "Администрация",
            Values = new ChartValues<double> { Classes.DBEditor.DB.user.Count(a => a.idRoleUser == 1) },
            Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(roleColors[0]),
            Stroke = Brushes.Transparent,
            Foreground = Brushes.White,
            DataLabels = true,
        },
        new PieSeries
        {
            Title = "Кадровый контроль",
            Values = new ChartValues<double> { Classes.DBEditor.DB.user.Count(a => a.idRoleUser == 2) },
            Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(roleColors[1]),
            Stroke = Brushes.Transparent,
            Foreground = Brushes.White,
            DataLabels = true
        },
        new PieSeries
        {
            Title = "менеджер заявок",
            Values = new ChartValues<double> { Classes.DBEditor.DB.user.Count(a => a.idRoleUser == 3) },
            Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(roleColors[2]),
            Stroke = Brushes.Transparent,
            Foreground = Brushes.White,
            DataLabels = true
        },
        new PieSeries
        {
            Title = "Специалист",
            Values = new ChartValues<double> { Classes.DBEditor.DB.user.Count(a => a.idRoleUser == 4) },
            Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(roleColors[3]),
            Stroke = Brushes.Transparent,
            Foreground = Brushes.White,
            DataLabels = true
        }
    };
            pieChartRole.LegendLocation = LegendLocation.Right;

            pieChartSex.Series = new SeriesCollection
    {
        new PieSeries
        {
            Title = "Мужчины",
            Values = new ChartValues<double> { Classes.DBEditor.DB.user.Count(a => a.sexUser == "Мужской") },
            Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(sexColors[0]),
            Stroke = Brushes.Transparent,
            DataLabels = true
        },
        new PieSeries
        {
            Title = "Женщины",
            Values = new ChartValues<double> { Classes.DBEditor.DB.user.Count(a => a.sexUser == "Женский") },
            Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(sexColors[1]),
            Stroke = Brushes.Transparent,
            DataLabels = true
        }
    };
            pieChartSex.LegendLocation = LegendLocation.Right;

            pieChartStatus.Series = new SeriesCollection
    {
        new PieSeries
        {
            Title = "Верифицирован",
            Values = new ChartValues<double> { Classes.DBEditor.DB.user.Count(a => a.verificationUser == 3) },
            Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(statusColors[0]),
            Stroke = Brushes.Transparent,
            DataLabels = true
        },
        new PieSeries
        {
            Title = "Не пройдена",
            Values = new ChartValues<double> { Classes.DBEditor.DB.user.Count(a => a.verificationUser == 2) },
            Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(statusColors[1]),
            Stroke = Brushes.Transparent,
            DataLabels = true
        },
        new PieSeries
        {
            Title = "В ожидании",
            Values = new ChartValues<double> { Classes.DBEditor.DB.user.Count(a => a.verificationUser == 1) },
            Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(statusColors[2]),
            Stroke = Brushes.Transparent,
            DataLabels = true
        },
        new PieSeries
        {
            Title = "Требуется повторная",
            Values = new ChartValues<double> { Classes.DBEditor.DB.user.Count(a => a.verificationUser == 4) },
            Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(statusColors[3]),
            Stroke = Brushes.Transparent,
            DataLabels = true
        },
    };
            pieChartStatus.LegendLocation = LegendLocation.Right;

            DataContext = this;

            // Инициализируем коллекцию для хранения данных о выполненных заявках
            SeriesCollection = new SeriesCollection();

            Labels = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToArray();
            Formatter = value => value.ToString("N");
            // Получаем данные о выполненных заявках из базы данных
            using (var dbContext = new personalRoomPsyEntities())
            {
                // Инициализируем месяцы и устанавливаем нулевое количество заявок для каждого месяца
                DataContext = this;

                SeriesCollection = new SeriesCollection();
                Labels = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToArray();
                Formatter = value => value.ToString("N");

                using (var dbCont = new personalRoomPsyEntities())
                {
                    // Создаем коллекцию значений для гистограммы выполненных заявок
                    ChartValues<int> completedValues = new ChartValues<int>();
                    for (int i = 0; i < 12; i++)
                    {
                        completedValues.Add(0);
                    }

                    // Получаем количество выполненных заявок для каждого месяца
                    var queryCompleted = from order in dbCont.historyOrder
                                         where order.dateCompete.Year == DateTime.Now.Year && order.statusOrder == 3
                                         group order by order.dateCompete.Month into g
                                         select new { Month = g.Key, Count = g.Count() };

                    foreach (var item in queryCompleted.OrderBy(x => x.Month))
                    {
                        completedValues[item.Month - 1] = item.Count;
                    }

                    // Создаем коллекцию значений для гистограммы отмененных заявок
                    ChartValues<int> canceledValues = new ChartValues<int>();
                    for (int i = 0; i < 12; i++)
                    {
                        canceledValues.Add(0);
                    }

                    // Получаем количество отмененных заявок для каждого месяца
                    var queryCanceled = from order in dbCont.historyOrder
                                        where order.dateCompete.Year == DateTime.Now.Year && order.statusOrder == 4
                                        group order by order.dateCompete.Month into g
                                        select new { Month = g.Key, Count = g.Count() };

                    foreach (var item in queryCanceled.OrderBy(x => x.Month))
                    {
                        canceledValues[item.Month - 1] = item.Count;
                    }

                    // Находим максимальное количество заказов за месяц
                    int maxOrders = Math.Max(completedValues.Max(), canceledValues.Max());

                    // Добавляем гистограммы в коллекцию данных
                    SeriesCollection.Add(new ColumnSeries
                    {
                        Title = "Выполненные заявки",
                        Values = completedValues,
                        Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#7120ea"),
                    });

                    SeriesCollection.Add(new ColumnSeries
                    {
                        Title = "Отмененные заявки",
                        Values = canceledValues,
                        Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#ef98d0"),
                    });

                    // Устанавливаем месяцы как метки по оси X
                    Labels = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToArray();

                    // Устанавливаем форматтер для оси Y
                    Formatter = value => value.ToString("N");

                    // Устанавливаем максимальное значение оси Y
                    MaxY = maxOrders;

                    // Обновляем представление
                    OnPropertyChanged(nameof(SeriesCollection));
                    OnPropertyChanged(nameof(Labels));
                    OnPropertyChanged(nameof(Formatter));
                    OnPropertyChanged(nameof(MaxY));

                    int activeOrdersCount = dbCont.order.Where(order => order.statusOrder == 2).Count();

                    countOrders.Text = activeOrdersCount.ToString();

                }
            }

        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }

        private string[] _labels;
        public string[] Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }

        private Func<double, string> _formatter;
        public Func<double, string> Formatter
        {
            get { return _formatter; }
            set
            {
                _formatter = value;
                OnPropertyChanged(nameof(Formatter));
            }
        }

        private double _maxY;
        public double MaxY
        {
            get { return _maxY; }
            set
            {
                _maxY = value;
                OnPropertyChanged(nameof(MaxY));
            }
        }
        //------------------------------------------Окно кадрового специалиста---------------------------------------
        //monitoringUsers + сопровождающие функции в разделе администратора
        //Функция отображения списка актуальных заяовк новых специалситов
        public void showNewUser()
        {
            List<Model.user> newUsers = new List<Model.user>();
            newUsers = DBEditor.DB.user.Where(a => a.verificationUser == 1).ToList();
            listUser = new List<Classes.User>();

            foreach (Model.user newUser in newUsers)
            {
                Classes.User newUsercl = new Classes.User();

                newUsercl.idUser = newUser.idUser;
                newUsercl.login = newUser.loginUser;
                newUsercl.fullName = newUser.fullNameUser;
                newUsercl.birth = newUser.dateBirth.ToShortDateString();
                newUsercl.emailUser = newUser.emailUser;
                newUsercl.phoneUser = newUser.phoneUser;
                newUsercl.idRole = newUser.idRoleUser;
                newUsercl.nameRole = newUser.role.nameRole;
                newUsercl.lineWorkingID = (int)newUser.lineWorking;
                newUsercl.lineWorkingName = newUser.lineProblem.nameLineProblem;
                newUsercl.countryID = newUser.countryUser;
                newUsercl.countryName = newUser.country.nameCountry;
                newUsercl.cityID = newUser.cityUser;
                newUsercl.cityName = newUser.city.nameCity;
                newUsercl.sex = newUser.sexUser;
                newUsercl.verificationID = (int)newUser.verificationUser;
                newUsercl.verificationName = newUser.verification.nameVerification;
                newUsercl.PhotoURL = newUser.urlPhoto;

                string imagePath = SupportClass.pathexe + newUsercl.PhotoURL;
                newUsercl.Photo = SupportClass.ShowImageBit(imagePath);

                listUser.Add(newUsercl);
            }
            listNewUser.ItemsSource = listUser;
        }
        //Функция отображения диплома пользователя
        private void showEducation_click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (clickedButton.Content.ToString() == "Диплом")
            {
                gridEducationInfo.Visibility = Visibility.Visible;
                Classes.User newUser = (sender as Button).DataContext as Classes.User;

                string imagePath = SupportClass.pathexe + $"/AppData/User/{newUser.login}/Data/education{newUser.login}.png";
                photoMainEducation.Source = SupportClass.ShowImageBit(imagePath);
            }
            else
            {
                gridEducationInfo.Visibility = Visibility.Visible;
                Classes.User newUser = (sender as Button).DataContext as Classes.User;

                string imagePath = SupportClass.pathexe + $"/AppData/User/{newUser.login}/Data/Documents/pasport{newUser.login}.png";
                photoMainEducation.Source = SupportClass.ShowImageBit(imagePath);
            }


        }
        //Функция закрывающая окно диплома пользователя
        private void closeDoc_Click(object sender, RoutedEventArgs e)
        {
            gridEducationInfo.Visibility = Visibility.Hidden;
            photoMainEducation.Source = null;
        }
        //Функция открытия шаблона отказа
        private void userHaveProblem(object sender, RoutedEventArgs e)
        {
            messageProblem.Text = "";

            var button = sender as Button;
            var selectedUser = button.DataContext as User;

            timeID = selectedUser.idUser;

            problemMessage.Visibility = Visibility.Visible;
            FIOCpes.Text = $"Специалист: {selectedUser.fullName}";

            messageProblem.Text = "";
        }

        //Функция принятия специалиста
        private void userCompete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedUser = button.DataContext as User;

            Model.user user = Classes.DBEditor.DB.user.Where(a => a.idUser == selectedUser.idUser).FirstOrDefault();

            user.verificationUser = 3;

            DBEditor.DB.SaveChanges();

            MessageBox_Window.ShowDialog("Уведомление", $"Кандидатура одобрена!", MessageBox_Window.MessageBoxButton.OK);

            problemMessage.Visibility = Visibility.Hidden;

            showNewUser();
        }
        //Функция сохранения сообщения-проблемы специалиста (не готов)
        private void sendMessageProblem_click(object sender, RoutedEventArgs e)
        {
            if (messageProblem.Text.Replace(" ", "") != "")
            {
                Model.messageProblem messageProb = new Model.messageProblem();

                messageProb.commentProblem = messageProblem.Text;
                messageProb.idUser = timeID;

                Classes.DBEditor.DB.messageProblem.Add(messageProb);

                Model.user upUser = DBEditor.DB.user.Where(a => a.idUser == messageProb.idUser).FirstOrDefault();
                upUser.verificationUser = 2;

                Classes.DBEditor.DB.SaveChanges();
                MessageBox_Window.ShowDialog("Уведомление", $"Заявка отклонена на редактирование!", MessageBox_Window.MessageBoxButton.OK);
                problemMessage.Visibility = Visibility.Hidden;
                showNewUser();
            }
            else
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Заполнены не все поля!", MessageBox_Window.MessageBoxButton.OK);
            }
        }

        //------------------------------------------Окно менеджера---------------------------------------
        //Функция формирования заявки (не готов)
        private void saveNewOrder(object sender, RoutedEventArgs e)
        {
            if (orderNameClient.Text.Replace(" ", "") != "" &&
                orderDateComplete.SelectedDate.HasValue == true &&
                orderTimeComplete.Text.Replace(" ", "") != "" &&
                boxProblems.SelectedIndex > 0 &&
                orderTimeComplete.Text.Contains("_") == false)
            {
                Model.order order = new Model.order();

                order.FioClient = orderNameClient.Text;
                order.dateMaking = DateTime.Now;
                order.dateStart = orderDateComplete.SelectedDate.Value.Date;

                order.timeStart = TimeSpan.Parse(orderTimeComplete.Text);
                order.idProblemOrder = (int)boxProblems.SelectedValue;

                var lineWorker = Classes.DBEditor.DB.typeProblem.Where(a => a.idTypeProblem == (int)boxProblems.SelectedValue).Select(a => new
                {
                    lineWork = a.idLineProblem,
                }).FirstOrDefault();

                order.idLineProblemOrder = lineWorker.lineWork;

                int resultFindUser = SupportClass.findWorker(lineWorker.lineWork, orderTimeComplete.Text, orderDateComplete.SelectedDate.Value.Date);

                if (resultFindUser == 0)
                {
                    MessageBox_Window.ShowDialog("Уведомление", $"Специалистов на данное время нет!", MessageBox_Window.MessageBoxButton.OK);
                }
                else
                {
                    order.idWorker = resultFindUser;

                    order.statusOrder = 2;
                    order.infoFromUserOrder = informationFromClient.Text;

                    Classes.DBEditor.DB.order.Add(order);
                    Classes.DBEditor.DB.SaveChanges();

                    MessageBox_Window.ShowDialog("Уведомление", $"Заявка успешно сформирована!", MessageBox_Window.MessageBoxButton.OK);

                    makeListOrder();
                }
            }
            else
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Не все поля введены корректно!", MessageBox_Window.MessageBoxButton.OK);
            }

        }
        //Функция отображения изменений в датагрид
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            makeListOrder();
        }
        //Функция отображения наполнения в датагрид
        public void makeListOrder()
        {
            boxProblems.SelectedIndex = 0;
            List<Model.order> newUsers = new List<Model.order>();
            newUsers = DBEditor.DB.order.Where(a => a.statusOrder == 1 || a.statusOrder == 2).ToList();

            if (findClient.Text.Length > 0)
            {
                newUsers = DBEditor.DB.order.Where(pr => pr.FioClient.Contains(findClient.Text)).ToList();
            }

            listnewOrder = new List<Classes.Orders>();

            foreach (Model.order newUser in newUsers)
            {
                Classes.Orders newUsercl = new Classes.Orders();

                newUsercl.idOrder = newUser.idOrder;
                newUsercl.FioClient = newUser.FioClient;
                newUsercl.dateMake = newUser.dateMaking.ToShortDateString();
                newUsercl.dateStart = newUser.dateStart.ToShortDateString();
                newUsercl.timeStart = newUser.timeStart.ToString();
                newUsercl.typeProblemName = newUser.typeProblem.nameTypeProblem;
                newUsercl.typeProblemID = newUser.idProblemOrder;
                newUsercl.lineWorkerID = newUser.idLineProblemOrder;
                newUsercl.lineWorkerName = newUser.lineProblem.nameLineProblem;
                try
                {
                    newUsercl.WorkerID = (int)newUser.idWorker;
                    newUsercl.WorkerName = $"{newUser.user.fullNameUser}";
                }
                catch
                {

                }
                newUsercl.statusOrderID = newUser.statusOrder;
                newUsercl.statusOrderName = newUser.statusOrder1.nameStatusOrder;
                newUsercl.infoFromClient = newUser.infoFromUserOrder;

                listnewOrder.Add(newUsercl);
            }
            dataGridOrders.ItemsSource = listnewOrder;

            // where dg is my data grid's name...
            foreach (DataGridColumn column in dataGridOrders.Columns)
            {
                //if you want to size your column as per the cell content
                column.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToCells);
                //if you want to size your column as per the column header
                column.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToHeader);
                //if you want to size your column as per both header and cell content
                column.Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto);
            }
        }
        //Кнопка сохранения нового образования
        private void closeOrder_Click(object sender, RoutedEventArgs e)
        {
            // Получить кнопку, на которую нажали
            var button = sender as Button;

            // Получить строку DataGrid, к которой принадлежит кнопка
            var dataGridRow = FindVisualParent<DataGridRow>(button);

            if (dataGridRow != null)
            {
                // Получить данные строки DataGrid
                var selectedRow = dataGridRow.Item as Orders; // YourDataType замените на тип данных вашей строки (например, Orders)

                if (selectedRow != null)
                {
                    // Извлечь данные из выбранной строки и сделать с ними что-то
                    SaveInformaion.idOrder = selectedRow.idOrder;
                    SaveInformaion.idWorker = selectedRow.WorkerID;
                    SaveInformaion.FioClient = selectedRow.FioClient;
                    SaveInformaion.dateComlete = selectedRow.dateStart;
                    SaveInformaion.statusOrder = selectedRow.statusOrderID;
                    // Продолжайте работу с вашими данными...
                }
            }

            if (button.Name == "close" && SaveInformaion.statusOrder == 2)
            {
                closeStatus.Visibility = Visibility.Visible;
                titleCloseOrder.Text = "Форма заявки(Закрытие)";
                Rating.IsEnabled = true;
                FioClient.Text = SaveInformaion.FioClient;
                typeCloseOrder = 1;

            }
            if (button.Name == "cancel")
            {
                closeStatus.Visibility = Visibility.Visible;
                titleCloseOrder.Text = "Форма заявки(Отмена)";
                Rating.IsEnabled = false;
                FioClient.Text = SaveInformaion.FioClient;
                typeCloseOrder = 2;
            }
        }
        private void orderOrder_click(object sender, RoutedEventArgs e)
        {
            Model.historyOrder historyOrder = new Model.historyOrder();

            if (typeCloseOrder == 1)
            {
                historyOrder.idOldOrder = SaveInformaion.idOrder;
                historyOrder.idWorker = SaveInformaion.idWorker;
                historyOrder.FioClient = SaveInformaion.FioClient;
                historyOrder.dateCompete = DateTime.Parse(SaveInformaion.dateComlete);
                historyOrder.commentClient = commentClient.Text;
                historyOrder.rateWorker = Rating.SelectedIndex + 1;
                historyOrder.statusOrder = 3;
            }
            if(typeCloseOrder == 2)
            {
                historyOrder.idOldOrder = SaveInformaion.idOrder;
                historyOrder.idWorker = SaveInformaion.idWorker;
                historyOrder.FioClient = SaveInformaion.FioClient;
                historyOrder.dateCompete = DateTime.Parse(SaveInformaion.dateComlete);
                historyOrder.commentClient = commentClient.Text;
                historyOrder.statusOrder = 4;
            }

            Model.order order = DBEditor.DB.order.Where(a => a.idOrder == SaveInformaion.idOrder).FirstOrDefault();
            DBEditor.DB.historyOrder.Add(historyOrder);
            DBEditor.DB.SaveChanges();
            DBEditor.DB.order.Remove(order);
            DBEditor.DB.SaveChanges();
            if (typeCloseOrder == 1)
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Заявка {SaveInformaion.idOrder} закрыта!", MessageBox_Window.MessageBoxButton.OK);
            }
            if (typeCloseOrder == 2)
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Заявка {SaveInformaion.idOrder} отменена!", MessageBox_Window.MessageBoxButton.OK);
            }
            makeListOrder();
            FioClient.Clear();
            Rating.SelectedIndex = 0;
            commentClient.Clear();
            closeStatus.Visibility = Visibility.Hidden;
        }
        private void findClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            makeListOrder();
        }

        //------------------------------------------Окно пользователя---------------------------------------
        //отображение главной страницы пользователя
        public void showActualOrders()
        {
            FIOUserPlusProblem.Text = $"{SaveInformaion.fullName} ({SaveInformaion.lineWorkingName})";
            verification.Text = $"Статус верификации: {SaveInformaion.stausAccount}";
            sex.Text = $"Пол: {SaveInformaion.sex}";
            role.Text = $"Должность: {SaveInformaion.roleName}";
            email.Text = $"Электронная почта: {SaveInformaion.email}";
            phone.Text = $"Мобильный телефон: {SaveInformaion.phone}";
            countryPlusCity.Text = $"Место проживания: {SaveInformaion.countryName}, {SaveInformaion.cityName}";

            using (var dbContext = new personalRoomPsyEntities()) // Замените на свой контекст базы данных
            {
                var rate = DBEditor.DB.historyOrder.Where(a => a.idWorker == SaveInformaion.idUser && a.rateWorker != null).Select(a => (double)a.rateWorker).ToList();

                // Вызываем функцию для расчета рейтинга
                localRate.Text = CalculatePerformerRating(rate).ToString();

            }

            List<order> orderList = Classes.DBEditor.DB.order.Where(a => a.idWorker == SaveInformaion.idUser && (a.statusOrder == 1 || a.statusOrder == 2)).ToList();

            listnewOrder = new List<Orders>();

            foreach (Model.order actualOrder in orderList)
            {
                Classes.Orders actual = new Classes.Orders();

                actual.idOrder = actualOrder.idOrder;
                actual.FioClient = actualOrder.FioClient;
                actual.dateStart = actualOrder.dateStart.ToShortDateString();

                DateTime time = DateTime.Parse(actualOrder.timeStart.ToString());

                actual.timeStart = time.ToString("HH:mm");
                actual.typeProblemName = actualOrder.typeProblem.lineProblem.nameLineProblem;
                actual.infoFromClient = actualOrder.infoFromUserOrder;

                
                
                listnewOrder.Add(actual);
            }

            listActualOrders.ItemsSource = listnewOrder;

            
        }
        //Кнопка сохранения нового образования
        private void takeorder_click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedUser = button.DataContext as Orders;

            Model.order order = DBEditor.DB.order.Where(a => a.idOrder == selectedUser.idOrder).FirstOrDefault();

            order.statusOrder = 2;

            DBEditor.DB.SaveChanges();
            showActualOrders();
        }
        //Функция отображения обновления листа заявок
        private void listActualOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showActualOrders();
        }
        //Кнопка сохранения нового образования
        private void saveNewEducation_Click(object sender, RoutedEventArgs e)
        {
            clearCacheEducation();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName.Replace(" ", "") != "")
            {
                Directory.CreateDirectory(SupportClass.pathexe + $@"/AppData/Cache/cache_education");
                string url = SupportClass.pathexe + $@"/AppData/Cache/cache_education/mainEducation.png";
                File.Copy(openFileDialog.FileName, url);

                changeEducation = true;
                newEducation.Background = Brushes.LightGreen;
                newEducation.Content = "Диплом загружен";

            }
            else
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Файл не выбран или отсутствует название!", MessageBox_Window.MessageBoxButton.OK);
            }
        }
        //Кнопка сохранения проверки паспорта
        private void saveNewPassport_Click(object sender, RoutedEventArgs e)
        {
            clearCacheDoc();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName.Replace(" ", "") != "")
            {
                Directory.CreateDirectory(SupportClass.pathexe + $@"/AppData/Cache/cache_doc");
                string url = SupportClass.pathexe + $@"/AppData/Cache/cache_doc/pasport.png";
                File.Copy(openFileDialog.FileName, url);

                changePassport = true;
                newPassport.Background = Brushes.LightGreen;
                newPassport.Content = "Паспорт загружен";
            }
            else
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Файл не выбран или отсутствует название!", MessageBox_Window.MessageBoxButton.OK);
            }
        }
        //Функция загрузки аватара в cache и отображения фотографии в форме регистрации
        private void newphotoClient(object sender, MouseButtonEventArgs e)
        {
            if(SaveInformaion.roleID == 4)
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
                    photo.Background = imageBrush;
                    changePhoto = true;
                }
                else
                {
                    MessageBox_Window.ShowDialog("Уведомление", $"Файл не выбран или отсутствует название!", MessageBox_Window.MessageBoxButton.OK);
                }
            }
        }
        //Функция отображения данных пользователя для редактирования
        public void showActualInfoUser()
        {
            fName.Text = SaveInformaion.fullName;
            login.Text = SaveInformaion.login;
            mail.Text = SaveInformaion.email;
            phones.Text = SaveInformaion.phone;

            List<Model.country> allCountry = Classes.DBEditor.DB.country.ToList();
            newCountry.ItemsSource = allCountry;
            newCountry.SelectedValue = SaveInformaion.countryID;
            newCountry.SelectedValuePath = "idCountry";
            newCountry.DisplayMemberPath = "nameCountry";

            List<Model.lineProblem> allLines = Classes.DBEditor.DB.lineProblem.ToList();
            newLine.ItemsSource = allLines;
            newLine.SelectedValue = SaveInformaion.linkWorkingID;
            newLine.SelectedValuePath = "idLineProblem";
            newLine.DisplayMemberPath = "nameLineProblem";
        }
        
        //Добавляет страны в комбобокс
        private void newCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                List<Model.city> allCity = Classes.DBEditor.DB.city.Where(a => a.idFromCountry == newCountry.SelectedIndex + 1).ToList();
                newCity.ItemsSource = allCity;
                newCity.SelectedValue = SaveInformaion.cityID;
                newCity.SelectedValuePath = "idCity";
                newCity.DisplayMemberPath = "nameCity";
        }
        //Сохранение личных данных
        private void SaveNewPersonalData_Click(object sender, RoutedEventArgs e)
        {
            if (changePhoto == true)
            {
                File.Delete(SupportClass.pathexe + $@"/AppData/User/{SaveInformaion.login}/{SaveInformaion.login}.png");
                try
                {
                    File.Move(SupportClass.pathexe + $@"/AppData/Cache/cache_photo/newAvatarPhoto.png", SupportClass.pathexe + $@"/AppData/User/{SaveInformaion.login}/{SaveInformaion.login}.png");
                }
                catch
                {
                    File.Copy(SupportClass.pathexe + $@"/AppData/Resources/base_avatar.png", SupportClass.pathexe + $@"/AppData/User/{SaveInformaion.login}/{SaveInformaion.login}.png");
                }
            }
            if (fName.Text != SaveInformaion.fullName ||
            login.Text != SaveInformaion.login ||
            mail.Text != SaveInformaion.email ||
            phones.Text != SaveInformaion.phone ||
            (int)newCountry.SelectedValue != SaveInformaion.countryID ||
            (int)newCity.SelectedValue != SaveInformaion.cityID ||
            (int)newLine.SelectedValue != SaveInformaion.linkWorkingID)
            {
                if (changePassport == true && changeEducation == true)
                {
                    try
                    {
                        Model.user user = Classes.DBEditor.DB.user.Where(a => a.idUser == SaveInformaion.idUser).FirstOrDefault();

                        user.fullNameUser = fName.Text;
                        user.emailUser = mail.Text;
                        user.phoneUser = phones.Text;
                        user.verificationUser = 1;
                        user.lineWorking = (int)newLine.SelectedValue;
                        user.countryUser = (int)newCountry.SelectedValue;
                        user.cityUser = (int)newCity.SelectedValue;

                        File.Delete(SupportClass.pathexe + $@"/AppData/User/{login.Text}/Data/education{login.Text}.png");
                        try
                        {
                            File.Move(SupportClass.pathexe + $@"/AppData/Cache/cache_education/mainEducation.png", SupportClass.pathexe + $@"/AppData/User/{login.Text}/Data/education{login.Text}.png");
                        }
                        catch
                        {
                            File.Copy(SupportClass.pathexe + $@"/AppData/Resources/base_avatar.png", SupportClass.pathexe + $@"/AppData/User/{login.Text}/Data/education{login.Text}.png");
                        }
                        File.Delete(SupportClass.pathexe + $@"/AppData/User/{login.Text}/Data/Documents/pasport{login.Text}.png");
                        try
                        {
                            File.Move(SupportClass.pathexe + $@"/AppData/Cache/cache_doc/pasport.png", SupportClass.pathexe + $@"/AppData/User/{login.Text}/Data/Documents/pasport{login.Text}.png");
                        }
                        catch
                        {
                            File.Copy(SupportClass.pathexe + $@"/AppData/Resources/base_avatar.png", SupportClass.pathexe + $@"/AppData/User/{login.Text}/Data/Documents/pasport{login.Text}.png");
                        }

                        Classes.DBEditor.DB.SaveChanges();

                        MessageBox_Window.ShowDialog("Уведомление", $"Ваши данные успешно изменены, авторизируйтесь повторно!", MessageBox_Window.MessageBoxButton.OK);

                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();

                        ContentWindow contentWindow = Application.Current.Windows.OfType<ContentWindow>().FirstOrDefault();

                        if (contentWindow != null)
                        {
                            // Вызываем метод закрытия окна
                            contentWindow.Close();
                        }
                    }
                    catch
                    {
                        
                    }
                }
                else
                {
                    MessageBox_Window.ShowDialog("Уведомление", $"Внимание, вы внесли изменения в личные данные, для подтверждения повторно загрузите фото паспорта и диплома об образовнии, соответствующего выбранному направлению!", MessageBox_Window.MessageBoxButton.OK);
                }
            }
        }
        //Отмена изменений
        private void closeChanged_Click(object sender, RoutedEventArgs e)
        {
            fName.Text = SaveInformaion.fullName;
            login.Text = SaveInformaion.login;
            mail.Text = SaveInformaion.email;
            phones.Text = SaveInformaion.phone;
            newLine.SelectedValue = SaveInformaion.linkWorkingID;
            newCountry.SelectedValue = SaveInformaion.countryID;
            newCity.SelectedValue = SaveInformaion.cityID;

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = SupportClass.ShowImageBit(SupportClass.pathexe + SaveInformaion.photo);
            photo.Background = imageBrush;

            newPassport.Background = Brushes.White;
            newPassport.Content = "Паспорт";

            newEducation.Background = Brushes.White;
            newEducation.Content = "Диплом";

            clearCacheDoc();
            clearCachephoto();
            clearCacheEducation();
        }
        //Обновление пароля
        private void SaveNewPassword_Click(object sender, RoutedEventArgs e)
        {
            var pass = Classes.DBEditor.DB.user.Where(a => a.idUser == SaveInformaion.idUser).FirstOrDefault(); 
            if (oldpass.Password == pass.passwordUser)
            {
                if (pass1.Password == pass2.Password)
                {
                    Model.user user = Classes.DBEditor.DB.user.Where(a => a.idUser == SaveInformaion.idUser).FirstOrDefault();

                    user.passwordUser = pass2.Password;

                    Classes.DBEditor.DB.SaveChanges();

                    MessageBox_Window.ShowDialog("Уведомление", $"Ваши данные успешно изменены, авторизируйтесь повторно!", MessageBox_Window.MessageBoxButton.OK);

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    ContentWindow contentWindow = Application.Current.Windows.OfType<ContentWindow>().FirstOrDefault();

                    if (contentWindow != null)
                    {
                        // Вызываем метод закрытия окна
                        contentWindow.Close();
                    }
                }
                else
                {
                    MessageBox_Window.ShowDialog("Уведомление", $"Новые пароли не совпадают!", MessageBox_Window.MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Неверный актуальынй пароль!", MessageBox_Window.MessageBoxButton.OK);
            }
        }
        //Уведомление о верификации
        public void showAtention()
        {
            Atention.Visibility = Visibility.Visible;

            if (SaveInformaion.verificationID == 1)
            {
                //Ожидает проверку(синий)
                Atention.BorderBrush = Brushes.Blue;
                titleAlert.Text = "Внимание";
                titleAlert.Text = "Ваш профиль ожидает верификации";
                textAllert.Text = "Верификация новых профилей осуществляется в течении 24 часов";
            }
            if (SaveInformaion.verificationID == 2)
            {
                var message = Classes.DBEditor.DB.messageProblem.Where(a => a.idUser == SaveInformaion.idUser).FirstOrDefault();

                //Не прошел проверку(красный)
                Atention.BorderBrush = Brushes.Red;
                titleAlert.Text = "Внимание";
                titleAlert.Text = "Ваш профиль не прошел верификацию, зайдите в раздел редактирования профиля и повторно внесите личную информацию";
                try
                {
                    textAllert.Text = "Комментарий от кадрового специалиста: " + message.commentProblem;
                }
                catch
                {
                    textAllert.Text = "Комментарий от кадрового специалиста: не указан";
                }
                
            }
            if (SaveInformaion.verificationID == 3)
            {
                //Прошел проверку(зеленый)
                Atention.BorderBrush = Brushes.Green;
                titleAlert.Text = "Внимание";
                titleAlert.Text = "Ваш профиль верифицирован";
                textAllert.Text = "Хорошего дня";
            }
            if (SaveInformaion.verificationID == 4)
            {
                //Требуется повторная проверка(оранжевый)
                Atention.BorderBrush = Brushes.Orange;
                titleAlert.Text = "Внимание";
                titleAlert.Text = "Ваши личные данные устарели, обновите личные данные в разделе редактирования данных и направьте форму на повторную верификацию";
                textAllert.Text = "Повторная верификация профилей осуществляется в течении 24";
            }
        }
        //Закрытие уведомления о верификации
        private void CloseAllert_Click(object sender, RoutedEventArgs e)
        {
            Atention.Visibility = Visibility.Hidden;
        }
        //Окно личного кабинета
        private void personalRoomUser(object sender, RoutedEventArgs e)
        {
            mainWindowUser.Visibility = Visibility.Visible;
        }

        //------------------------------------------Дополнительные функции---------------------------------------
        //Функция перехода к окну авторизации
        private void close_personalRoom(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Window window = Window.GetWindow(this);
            mainWindow.Show();
            window.Close();
        }
        //Функция открытия окна обратной связи
        private void questionUser_click(object sender, RoutedEventArgs e)
        {
            questionWindow.Visibility = Visibility.Visible;
        }
        //Функция отправки сообщения из окна обратной связи
        private void sendMessage_click(object sender, RoutedEventArgs e)
        {
            if (nameUser.Text.Replace(" ","") != "" && contentMessage.Text.Replace(" ", "") != "" && titleMessage.Text.Replace(" ", "") != "")
            {
                try
                {
                    SmtpClient user = new SmtpClient();
                    user.Credentials = new NetworkCredential("sparta22334455@mail.ru", "gVZxemuyf69kt0isTYXe");
                    user.Host = "smtp.mail.ru";
                    user.Port = 587;
                    user.EnableSsl = true;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("sparta22334455@mail.ru");
                    mail.To.Add(new MailAddress("sparta22334455@mail.ru"));
                    mail.Subject = titleMessage.Text;
                    mail.Body = $"Сообщение от {nameUser.Text} \n {contentMessage.Text}";
                    user.Send(mail);

                    MessageBox_Window.ShowDialog("Уведомление", $"Сообщение успешно отправлено!", MessageBox_Window.MessageBoxButton.OK);
                }
                catch
                {
                    SmtpClient user = new SmtpClient();
                    user.Credentials = new NetworkCredential("sparta22334455@mail.ru", "gVZxemuyf69kt0isTYXe");
                    user.Host = "smtp.gmail.ru";
                    user.Port = 587;
                    user.EnableSsl = true;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("sparta22334455@mail.ru");
                    mail.To.Add(new MailAddress("sparta22334455@mail.ru"));
                    mail.Subject = titleMessage.Text;
                    mail.Body = $"Сообщение от {nameUser.Text} \n {contentMessage.Text}";
                    user.Send(mail);

                    MessageBox_Window.ShowDialog("Уведомление", $"Сообщение успешно отправлено!", MessageBox_Window.MessageBoxButton.OK);

                }
                nameUser.Clear();
                titleMessage.Clear();
                contentMessage.Clear();
                questionWindow.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox_Window.ShowDialog("Уведомление", $"Заполните все информационные поля!", MessageBox_Window.MessageBoxButton.OK);
            }

        }
        //Функция закрытия окна обратной связи
        private void closeMessageGrid(object sender, RoutedEventArgs e)
        {
            questionWindow.Visibility = Visibility.Hidden;

            nameUser.Clear();
            titleMessage.Clear();
            contentMessage.Clear();
        }
        //Функция закрытия окна обратной связи проблемы
        private void closeMessageproblem(object sender, RoutedEventArgs e)
        {
            FioClient.Clear();
            Rating.SelectedIndex = 0;
            commentClient.Clear();
            problemMessage.Visibility = Visibility.Hidden;
            closeStatus.Visibility = Visibility.Hidden;
        }
        //Функции очистки
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

        //------------------------------------------Основные функциональные кнопки---------------------------------------
        //Функциональная кнопка 1 (админ и кадровый специалист - мониторинг пользователей / пользователь - личный кабинет)
        private void work1_click(object sender, RoutedEventArgs e)
        {
            
            switch (SaveInformaion.roleID)
            {
                case 1:
                    mainWindowAdmin.Visibility = Visibility.Visible;
                    statistics.Visibility = Visibility.Hidden;
                    monitoringUsers();
                    break;
                case 2:
                    mainWindowAdmin.Visibility = Visibility.Visible;
                    newUserCheck.Visibility = Visibility.Hidden;
                    monitoringUsers();
                    break;
                case 3:
                    mainWindowAdmin.Visibility = Visibility.Visible;
                    newOrder.Visibility = Visibility.Hidden;
                    break;
                case 4:
                    mainWindowUser.Visibility = Visibility.Visible;
                    editUserInfo.Visibility = Visibility.Hidden;
                    
                    monitoringUsers();
                    break;
            }
            if (questionWindow.Visibility == Visibility.Visible)
            {
                questionWindow.Visibility = Visibility.Hidden;

                nameUser.Clear();
                titleMessage.Clear();
                contentMessage.Clear();
            }
        }
        //Функциональная кнопка 1 (админ - статистика и настройки политик / кадровик - заявки на пользователей / пользователь - редактирование личного кабинета)
        private void work2_click(object sender, RoutedEventArgs e)
        {
            showNewUser();
            switch (SaveInformaion.roleID)
            {
                case 1:
                    mainWindowAdmin.Visibility = Visibility.Hidden;
                    statistics.Visibility = Visibility.Visible;
                    LoadPieChartData();
                    break;
                case 2:
                    mainWindowAdmin.Visibility = Visibility.Hidden;
                    newUserCheck.Visibility = Visibility.Visible;
                    break;
                case 3:
                    mainWindowAdmin.Visibility = Visibility.Hidden;
                    newOrder.Visibility = Visibility.Visible;
                    boxProblems.SelectedIndex = 0;
                    break;
                case 4:
                    mainWindowUser.Visibility = Visibility.Hidden;
                    editUserInfo.Visibility = Visibility.Visible;
                    showActualInfoUser();
                    break;
            }

            if (questionWindow.Visibility == Visibility.Visible)
            {
                questionWindow.Visibility = Visibility.Hidden;

                nameUser.Clear();
                titleMessage.Clear();
                contentMessage.Clear();
            }
        }
        //------------------------------------------Спижено---------------------------------------
        //функции вписывания времени
        private void TimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешить только цифры и двоеточие
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9:]+");
            return !regex.IsMatch(text);
        }

        private void TimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Применить шаблон формата hh:mm
            var textBox = sender as TextBox;
            if (TimeSpan.TryParseExact(textBox.Text, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan result))
            {
                statusTimeOrder = true;
            }
            else
            {
                statusTimeOrder = false;
            }
        }
        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }
        private T FindVisualParent1<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }


    }
    

}

