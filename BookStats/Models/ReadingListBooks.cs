
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
    
public partial class ReadingListBooks
{

    public string BookID { get; set; }

    public int ReadingListID { get; set; }

    public Nullable<int> ChapterPageNumber { get; set; }

    public string Remark { get; set; }



    public virtual Books Books { get; set; }

    public virtual ReadingLists ReadingLists { get; set; }

}

}
