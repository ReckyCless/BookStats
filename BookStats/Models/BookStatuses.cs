
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
    
public partial class BookStatuses
{

    public BookStatuses()
    {

        this.Requisitions = new HashSet<Requisitions>();

    }


    public int ID { get; set; }

    public string StatusName { get; set; }



    public virtual ICollection<Requisitions> Requisitions { get; set; }

}

}
