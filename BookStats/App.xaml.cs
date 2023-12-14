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

/*        class MyContextInitializer : CreateDatabaseIfNotExists<BooksStatsDBEntities>
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

                Users userAdmin = new Users { Login = "admin", Password = "admin", FirstName = "Дмитрий", SecondName = "Кудрявцев", Patronymic = "Андреевич", Phone = "+7 *** *** ** **", Email = "lobrynyaegorich@gmail.com", Role = 1, DateOfAdding = DateTime.Now };
                Users userUser = new Users { Login = "user", Password = "user", FirstName = "Дмитрий", SecondName = "Кудрявцев", Patronymic = "Андреевич", Phone = "+7 *** *** ** **", Email = "lobrynyaegorich@gmail.com", Role = 2, DateOfAdding = DateTime.Now };
                Users userManager = new Users { Login = "manager", Password = "manager", FirstName = "Дмитрий", SecondName = "Кудрявцев", Patronymic = "Андреевич", Phone = "+7 *** *** ** **", Email = "lobrynyaegorich@gmail.com", Role = 3, DateOfAdding = DateTime.Now };

                db.Users.Add(userAdmin);
                db.Users.Add(userUser);
                db.Users.Add(userManager);

                Genres genre1 = new Genres { GenreName = "Детектив" };
                Genres genre2 = new Genres { GenreName = "Фантастика" };
                Genres genre3 = new Genres { GenreName = "Трилер" };
                Genres genre4 = new Genres { GenreName = "Фэнтези" };
                Genres genre5 = new Genres { GenreName = "Романтика" };
                Genres genre6 = new Genres { GenreName = "Саморазвитие" };

                db.Genres.Add(genre1);
                db.Genres.Add(genre2);
                db.Genres.Add(genre3);
                db.Genres.Add(genre4);
                db.Genres.Add(genre5);
                db.Genres.Add(genre6);

                Books books1 = new Books { Article = "A12345", Author = "Сагдеев Станислав", Name = "Мышление предпринимателя", Image = "1.png", SourceLink = "https://www.ozon.ru/product/myshlenie-predprinimatelya-sagdeev-stanislav-sagdeev-stanislav-1072648267/?advert=p54LeocgINLQ0GNAGMNK7L8Kj1nz5pdj5XOSnTQ-A-Qjl7urTf8Ty6CTlsW_e3mFBJKqwefddrI7Kbo6Z4qZZS-kohs35tqs9nADwoheFGbHSc9F1CFJGyFwPKOGZZxI5lVKywHDONsAjsJys86gTdZ-Hr7TG4cOC_Dylpiy4MTXWgqf1Z_b-IM-k09_EGW5hgZkX4Zf8VYZY4epFNXGW8Z1eVEc-j6s1qjG5Al3AbcBLwaOpCNGwWV3DPDuOAnRFdbDxNTRr2MrEg&avtc=1&avte=2&avts=1702571141" };
                BookGenres book1Genres = new BookGenres { Books = books1, Genres = genre6 };

                Books books2 = new Books { Article = "B12345", Author = "Фокс Кабейн Оливия", Name = "Харизма", Image = "2.png", SourceLink = "https://www.ozon.ru/product/harizma-kak-vliyat-ubezhdat-i-vdohnovlyat-kniga-po-psihologii-i-samorazvitiyu-lichnaya-241514827/?asb=rbOlDQ47syttAC4XDv4oScBdprT48q1ym5bOrLf95E4%253D&asb2=S_j8YHqmugc1KubDKnX2t8UjDyzX7xo9QJ3CM-32B2kRIoKNVMhKxYFjjhD87K-z&avtc=1&avte=1&avts=1702571141" };
                BookGenres book2Genres = new BookGenres { Books = books2, Genres = genre6 };

                Books books3 = new Books { Article = "C12345", Author = "Мисима Юкио", Name = "Фонтаны под дождем", Image = "3.png", SourceLink = "https://www.ozon.ru/product/fontany-pod-dozhdem-misima-yukio-1268440164/?advert=CsGLQ_c1cn4loqRzeUGU133BfbeV5QtbyUqYxFGe8290992rm_WmeWLoBrmJOfgQtZfMVF2cUbglFiIFBYQNRKtU1iC39eh_Rx3B3lYFBOFpqcW5Bnew22weTNotvmIOZ8rCQtoJPFFG8oOKzYtxM64xZQYzv12nHTgGrDtgXyzE6Nyq3WJ5iXKL-YnAfWBZzzbbgWV5vGTYrI0iF4sSUIGRuFCAt1CsO7MlVnIN135SL_oli0quJmTHzRwfG8cNcB6wBg&avtc=1&avte=2&avts=1702576750" };
                BookGenres book3Genres1 = new BookGenres { Books = books3, Genres = genre1 };
                BookGenres book3Genres2 = new BookGenres { Books = books3, Genres = genre4 };
                BookGenres book3Genres3 = new BookGenres { Books = books3, Genres = genre5 };

                Books books4 = new Books { Article = "D12345", Author = "Рэнд Айн", Name = "Атлант расправил плечи", Image = "4.png", SourceLink = "https://www.ozon.ru/product/atlant-raspravil-plechi-rend-ayn-241512753/?_bctx=CAYQ8pJG&asb=KrHpls%252FCg%252FgW%252B0QuyLijhKnqrMTVruI5GF5RhdWlDb0%253D&asb2=vi7nKGtBw54rpiwZCT0Hw58Q24NPOA1pQWQjoF7IbbjsCKI-WHTgv0T651NF-OYp&avtc=1&avte=1&avts=1702576863" };
                BookGenres book4Genres = new BookGenres { Books = books4, Genres = genre1 };

                Books books5 = new Books { Article = "E12345", Author = "Макконахи Мэттью", Name = "Зеленый свет", Image = null, SourceLink = null };
                BookGenres book5Genres1 = new BookGenres { Books = books5, Genres = genre3 };
                BookGenres book5Genres2 = new BookGenres { Books = books5, Genres = genre5 };

                Books books6 = new Books { Article = "F12345", Author = "Михайлова Татьяна Викторовна", Name = "Вязание крючком: шаг за шагом", Image = null, SourceLink = null };

                db.Books.Add(books1);
                db.BookGenres.Add(book1Genres);

                db.Books.Add(books2);
                db.BookGenres.Add(book2Genres);

                db.Books.Add(books3);
                db.BookGenres.Add(book3Genres1);
                db.BookGenres.Add(book3Genres2);
                db.BookGenres.Add(book3Genres3);

                db.Books.Add(books4);
                db.BookGenres.Add(book4Genres);

                db.Books.Add(books5);
                db.BookGenres.Add(book5Genres1);
                db.BookGenres.Add(book5Genres2);

                db.Books.Add(books6);

                db.SaveChanges();
            }
        }
        public partial class BooksStatsDBEntities : DbContext
        {
            static BooksStatsDBEntities()
            {
                Database.SetInitializer<BooksStatsDBEntities>(new MyContextInitializer());
            }*/
        }
}
