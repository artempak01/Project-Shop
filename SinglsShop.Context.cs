﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Магазин_пластинок_ : DbContext
    {
        public Магазин_пластинок_()
            : base("name=Магазин_пластинок_")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Жанры> Жанры { get; set; }
        public virtual DbSet<Издатели> Издатели { get; set; }
        public virtual DbSet<Исполнители> Исполнители { get; set; }
        public virtual DbSet<Покупатели> Покупатели { get; set; }
        public virtual DbSet<Пользователи> Пользователи { get; set; }
        public virtual DbSet<Продажи> Продажи { get; set; }
        public virtual DbSet<Пластинки> Пластинки { get; set; }
    }
}
