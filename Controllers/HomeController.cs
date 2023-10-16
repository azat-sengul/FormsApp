using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormsApp.Controllers;

public class HomeController : Controller
{
  

    public HomeController()
    {
        
    }

    public IActionResult Index(string searchString, string category) // searchString ile get metodu yardımı ile sayfa arama filtreleme yapılır
    {
        var products = Repository.Products;

        if (!String.IsNullOrEmpty(searchString))
        {
            ViewBag.SearchString = searchString;

            products = products.Where(p => p.Name.ToLower().Contains(searchString)).ToList();
        }
        // View üzerinde kategori listeleme için yapılacak
        // ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name", category); //Görünür name, value CategoryId, selected category olur

        if(!String.IsNullOrEmpty(category) && category !="0")
        {
           
            products = products.Where(p => p.CategoryId == int.Parse(category)).ToList();
        }

        // ProductViewModel içerisinde gönderilmek istenen bilgiler paketlendi. bu durumda ViewBag kısmına buarada artık gerek kalmadı.
        
        var model = new ProductViewModel{ 
            Products = products,
            Categories = Repository.Categories,
            SelectedCategory = category
        }; 


        return View(model); //artık sayfaya products değil model gönderilecek
    }

    public IActionResult Privacy()
    {
        return View();
    }


}
