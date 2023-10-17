using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace FormsApp.Models
{
    public class Product
    {
        [Display (Name="Ürün ID")]
        public int ProductId { get; set; }

        [Required(ErrorMessage ="Zorunlu Alan")] //Hata mesajını türkçe yazmak için
        [Display (Name="Ürün Adı")]
        public string? Name { get; set; }

        [Required]
        [Display (Name="Fiyat")]
        public decimal? Price { get; set; }


        [Display (Name="Resim")]
        public string? Image { get; set; } = string.Empty;
        public bool IsActive { get; set; }

         [Display (Name="Category")]
        public int CategoryId { get; set; }
    }
}