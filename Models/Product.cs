using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FormsApp.Models
{
    public class Product
    {
        [Display (Name="Ürün ID")]
        public int ProductId { get; set; }

        [Display (Name="Ürün Adı")]
        public string? Name { get; set; }

        [Display (Name="Fiyat")]
        public decimal Price { get; set; }

        [Display (Name="Resim")]
        public string? Image { get; set; }
        public bool IsActive { get; set; }

         [Display (Name="Category")]
        public int CategoryId { get; set; }
    }
}