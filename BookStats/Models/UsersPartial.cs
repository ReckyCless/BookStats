using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStats.Models
{
    partial class Users
    {
        public string FullName
        {
            get
            {
                return $"{SecondName} {FirstName} {Patronymic}";
            }
        }
    }
}
