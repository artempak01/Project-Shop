//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Музыкальный_магазин_пластинок
{
    using System;
    using System.Collections.Generic;
    
    public partial class Покупатели
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Покупатели()
        {
            this.Пластинки = new HashSet<Пластинки>();
            this.Продажи = new HashSet<Продажи>();
        }
    
        public int Id { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public Nullable<decimal> Сумма_покупок { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Пластинки> Пластинки { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Продажи> Продажи { get; set; }
    }
}
