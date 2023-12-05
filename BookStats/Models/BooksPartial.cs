using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStats.Models
{
    partial class Books
    {
        public string ImagePathInText
        {
            get
            {
                string directory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources"));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (Image != null && Image != "")
                {
                    directory = directory + @"\products\" + Image;
                    if (!Directory.Exists(ImagePathInText))
                        return directory;
                    else
                        return @"../Resources/default.png";
                }
                else
                {
                    return @"../Resources/default.png";
                }
            }
        }
        public string PriceInText
        {
            get
            {
                return "Цена: " + Price.ToString("N2") + " ₽";
            }
        }
        public string AuthorInText
        {
            get
            {
                return "Автор: " + Author.ToString();
            }
        }
    }
}
