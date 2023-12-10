using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStats.Models
{
    partial class Books
    {

        public string ImagePathInText
        {
            get
            {
                string directory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources"));

                if (Image != null && Image != "")
                {
                    directory = directory + @"/Images/" + Image;
                    return directory;
                }
                else
                {
                    Debug.Print("B");
                    return @"../Resources/default.png";
                }
            }
        }
        public string yearOfPublishing
        {
            get
            {
                if (PublicationDate.HasValue)
                {
                    return "Год выпуска: " + PublicationDate.Value.Year;

                }
                else
                {
                    return "Год выпуска - не указан";
                }
            }
        }
        public string AuthorInText
        {
            get
            {
                return "Автор: " + Author.ToString();
            }
        }
        public string genresInText
        {
            get
            {
                string txt = string.Empty;
                foreach (var elem in BookGenres)
                {
                    txt += elem.Genres.GenreName + ", ";
                }
                txt.TrimEnd(' ', ',');
                if (txt.Length > 0)
                {
                    txt = txt.Insert(0, "Жанры: ");
                    return txt;
                }
                else
                {
                    return "Жанров - не найдено";
                }
            }
        }

        public Visibility visibilityControl
        {
            get
            {
                if (App.CurrentUser != null)
                {
                    switch (App.CurrentUser.Role)
                    {
                        case 1:
                            return Visibility.Visible;
                        case 2:
                            return Visibility.Collapsed;
                        case 3:
                            return Visibility.Visible;
                    }
                }
                return Visibility.Collapsed;
            }
        } 
    }
}
