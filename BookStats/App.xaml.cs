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
    }
}
