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
    
    public partial class Продажи
    {
        public int Order_ID { get; set; }
        public Nullable<System.DateTime> Дата_продажи { get; set; }
        public Nullable<decimal> Цена { get; set; }
        public Nullable<int> ID_пластинки { get; set; }
        public Nullable<int> ID_покупателя { get; set; }
        public Nullable<int> Количество { get; set; }
    
        public virtual Пластинки Пластинки { get; set; }
        public virtual Покупатели Покупатели { get; set; }
    }
}
