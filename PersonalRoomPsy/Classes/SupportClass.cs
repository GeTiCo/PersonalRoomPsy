using PersonalRoomPsy.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PersonalRoomPsy.Classes
{
    internal class SupportClass
    {
        public static string pathexe = Environment.CurrentDirectory;

        public static BitmapImage ShowImageBit(string fileName)
        {
            BitmapImage bit = null;
            byte[] photo = File.ReadAllBytes(fileName);
            System.IO.MemoryStream strm = new System.IO.MemoryStream(photo);
            bit = new System.Windows.Media.Imaging.BitmapImage();
            bit.BeginInit();
            bit.StreamSource = strm;
            bit.EndInit();
            return bit;
        }
        public static int findWorker(int typeProblem, string time, DateTime date)
        {

            var findUser = Classes.DBEditor.DB.user
            .Where(a => a.verificationUser == 3 && a.idRoleUser == 4 && a.lineWorking == typeProblem)
            .ToList();

            var userOrderCounts = new List<Tuple<int, int>>();

            TimeSpan parsedTime = TimeSpan.Parse(time);

            foreach (var worker in findUser)
            {
                // Проверяем, есть ли у специалиста запись на указанное время и дату
                bool hasOrder = Classes.DBEditor.DB.order.Any(order =>
                    order.idWorker == worker.idUser && order.dateStart == date && order.timeStart == parsedTime);

                if (!hasOrder)
                {
                    // Если у специалиста нет записи на это время, добавляем его в список
                    int count = Classes.DBEditor.DB.order.Count(order => order.idWorker == worker.idUser);
                    userOrderCounts.Add(Tuple.Create(worker.idUser, count));
                }
            }

            if (userOrderCounts.Any())
            {
                userOrderCounts.Sort((x, y) => x.Item2.CompareTo(y.Item2));
                return userOrderCounts.First().Item1;
            }
            else
            {
                return 0;
            }
        }
    }
    public class User
    {
        public int idUser { get; set; }

        public BitmapImage Photo { get; set; }
        public string PhotoURL { get; set; }

        public string login { get; set; }
        public string fullName { get; set; }
        public string birth { get; set; }
        public string emailUser { get; set; }
        public string phoneUser { get; set; }

        public int idRole { get; set; }
        public string nameRole { get; set; }

        public int lineWorkingID { get; set; }
        public string lineWorkingName { get; set; }

        public int countryID { get; set; }
        public string countryName { get; set; }

        public int cityID { get; set; }
        public string cityName { get; set; }

        public string sex { get; set; }

        public int verificationID { get; set; }
        public string verificationName { get; set; }
    }
    public class Orders
    {
        public int idOrder { get; set; }
        public string FioClient { get; set; }
        public string dateMake { get; set; }
        public string dateStart { get; set; }
        public string timeStart { get; set; }

        public int typeProblemID { get; set; }
        public string typeProblemName { get; set; }

        public int lineWorkerID { get; set; }
        public string lineWorkerName { get; set; }

        public int WorkerID { get; set; }
        public string WorkerName { get; set; }

        public int statusOrderID { get; set; }
        public string statusOrderName { get; set; }

        public string infoFromClient { get; set; }

        public BitmapImage Photo { get; set; }
    }
   
}
