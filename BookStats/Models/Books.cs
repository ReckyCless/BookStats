//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookStats.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Books
    {
        public Books()
        {
            this.BookGenres = new HashSet<BookGenres>();
            this.ReadingListBooks = new HashSet<ReadingListBooks>();
        }
    
        public string Article { get; set; }
        public int UserID { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> PublicationDate { get; set; }
        public string Image { get; set; }
        public string Remark { get; set; }
    
        public virtual ICollection<BookGenres> BookGenres { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<ReadingListBooks> ReadingListBooks { get; set; }
    }
}
