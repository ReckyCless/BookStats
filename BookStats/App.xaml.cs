using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BookStats.Models;

namespace BookStats
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static BooksStatsDBEntities Context { get; } = new BooksStatsDBEntities();
        public static Users CurrentUser = null;

        //This code is in model and it's creating the database if it's not exists I've put it here if model erase that code somehow
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        /*class MyContextInitializer : CreateDatabaseIfNotExists<BooksStatsDBEntities>
        {
            protected override void Seed(BooksStatsDBEntities db)
            {
                BookStatuses status1 = new BookStatuses { StatusName = "Не назначен" };
                BookStatuses status2 = new BookStatuses { StatusName = "В обработке" };
                BookStatuses status3 = new BookStatuses { StatusName = "Подтверждена" };
                BookStatuses status4 = new BookStatuses { StatusName = "Отклонена" };

                db.BookStatuses.Add(status1);
                db.BookStatuses.Add(status2);
                db.BookStatuses.Add(status3);
                db.BookStatuses.Add(status4);


                Roles role1 = new Roles { Name = "Администратор" };
                Roles role2 = new Roles { Name = "Пользователь" };
                Roles role3 = new Roles { Name = "Менеджер" };

                db.Roles.Add(role1);
                db.Roles.Add(role2);
                db.Roles.Add(role3);

                Users userAdmin = new Users { Login = "admin", Password = "admin", FirstName = "Л", SecondName = "О", Patronymic = "Х", Phone = "+7 *** *** ** **", Email = "lobrynyaegorich@gmail.com", Role = 1, DateOfAdding = DateTime.Now };
                Users userUser = new Users { Login = "user", Password = "user", FirstName = "Л", SecondName = "О", Patronymic = "Х", Phone = "+7 *** *** ** **", Email = "lobrynyaegorich@gmail.com", Role = 2, DateOfAdding = DateTime.Now };
                Users userManager = new Users { Login = "manager", Password = "manager", FirstName = "Л", SecondName = "О", Patronymic = "Х", Phone = "+7 *** *** ** **", Email = "lobrynyaegorich@gmail.com", Role = 3, DateOfAdding = DateTime.Now };

                db.Users.Add(userAdmin);
                db.Users.Add(userUser);
                db.Users.Add(userManager);

                db.SaveChanges();
            }
        }
        public partial class BooksStatsDBEntities : DbContext
        {
            static BooksStatsDBEntities()
            {
                Database.SetInitializer<BooksStatsDBEntities>(new MyContextInitializer());
            }
        }*/
    }
}
