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
    
    public partial class NotificationTable
    {
        public int RequisitionID { get; set; }
        public int UserID { get; set; }
        public bool IsChecked { get; set; }
    
        public virtual Requisitions Requisitions { get; set; }
        public virtual Users Users { get; set; }
    }
}
