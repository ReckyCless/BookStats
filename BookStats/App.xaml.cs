using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
                BookStatuses status1 = new BookStatuses { StatusName = "Доступна" };
                BookStatuses status2 = new BookStatuses { StatusName = "Используется" };
                BookStatuses status3 = new BookStatuses { StatusName = "В архиве" };

                db.BookStatuses.Add(status1);
                db.BookStatuses.Add(status2);
                db.BookStatuses.Add(status3);


                Roles role1 = new Roles { Name = "Администратор" };
                Roles role2 = new Roles { Name = "Пользователь" };

                db.Roles.Add(role1);
                db.Roles.Add(role1);

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
