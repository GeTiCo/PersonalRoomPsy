using PersonalRoomPsy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRoomPsy.Classes
{
    internal class DBEditor
    {
        public static Model.personalRoomPsyEntities DB { get; set; }
        public static Model.user User { get; set; }

        public static bool findUser(string login, string password)
        {
            bool result = false;

            var findUser = Classes.DBEditor.DB.user.Where(a => a.loginUser == login && a.passwordUser == password).Select(a => new
            {
                idUser = a.idUser,
                login = a.loginUser,

                fullName = a.fullNameUser,
                birth = a.dateBirth,
                email = a.emailUser,
                phone = a.phoneUser,

                roleID = a.idRoleUser,
                roleName = a.role.nameRole,

                countryID = a.countryUser,
                countryName = a.country.nameCountry,
                cityID = a.cityUser,
                cityName = a.city.nameCity,

                sex = a.sexUser,

                photo = a.urlPhoto,

                verificationID = a.verificationUser,
                verificationName = a.verification.nameVerification,

                linkWorkingID = a.lineWorking,
                lineWorkingName = a.lineProblem.nameLineProblem,
            }).FirstOrDefault();

            if (findUser != null)
            {
                result = true;

                Classes.SaveInformaion.idUser = findUser.idUser;
                Classes.SaveInformaion.login = findUser.login;
                Classes.SaveInformaion.fullName = findUser.fullName;
                Classes.SaveInformaion.birth = findUser.birth.ToString();
                Classes.SaveInformaion.email = findUser.email;
                Classes.SaveInformaion.phone = findUser.phone;
                Classes.SaveInformaion.roleID = findUser.roleID;
                Classes.SaveInformaion.roleName = findUser.roleName;
                Classes.SaveInformaion.countryID = findUser.countryID;
                Classes.SaveInformaion.countryName = findUser.countryName;
                Classes.SaveInformaion.cityID = findUser.cityID;
                Classes.SaveInformaion.cityName = findUser.cityName;
                Classes.SaveInformaion.sex = findUser.sex;
                Classes.SaveInformaion.photo = findUser.photo;
                Classes.SaveInformaion.verificationID = (int)findUser.verificationID;
                Classes.SaveInformaion.verificationName = findUser.verificationName;
                try
                {
                    Classes.SaveInformaion.linkWorkingID = (int)findUser.linkWorkingID;
                    Classes.SaveInformaion.lineWorkingName = findUser.lineWorkingName;
                }
                catch
                {

                }
                
            }



            return result;
        }

        public static int findNewUserID(string login)
        {
            int result = 0;

            var findUser = Classes.DBEditor.DB.user.Where(a => a.loginUser == login).Select(a => new
            {
                idUser = a.idUser,
            }).FirstOrDefault();

            result = findUser.idUser;


            return result;
        }
    }
}
