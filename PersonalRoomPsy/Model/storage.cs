//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PersonalRoomPsy.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class storage
    {
        public int idStorage { get; set; }
        public int idUserStorage { get; set; }
        public string sizeStorage { get; set; }
    
        public virtual user user { get; set; }
    }
}
