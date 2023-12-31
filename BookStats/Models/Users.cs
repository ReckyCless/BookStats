
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
    
public partial class Users
{

    public Users()
    {

        this.ReadingLists = new HashSet<ReadingLists>();

        this.RequisitionManagers = new HashSet<RequisitionManagers>();

        this.Requisitions = new HashSet<Requisitions>();

        this.NotificationTable = new HashSet<NotificationTable>();

    }


    public int ID { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string SecondName { get; set; }

    public string Patronymic { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public System.DateTime DateOfAdding { get; set; }

    public int Role { get; set; }



    public virtual ICollection<ReadingLists> ReadingLists { get; set; }

    public virtual ICollection<RequisitionManagers> RequisitionManagers { get; set; }

    public virtual ICollection<Requisitions> Requisitions { get; set; }

    public virtual Roles Roles { get; set; }

    public virtual ICollection<NotificationTable> NotificationTable { get; set; }

}

}
