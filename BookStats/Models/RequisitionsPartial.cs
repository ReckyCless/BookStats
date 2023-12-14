using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStats.Models
{
    partial class Requisitions
    {
        public Visibility AdminManagerVisibility
        {
            get
            {
                if (App.CurrentUser != null)
                {
                    if (App.CurrentUser.Role == 2)
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        return Visibility.Visible;
                    }
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
    }
}
