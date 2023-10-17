using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32.SafeHandles;

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

            products = products.Where(p => p.Name!.ToLower().Contains(searchString)).ToList(); //Name nullable olarak işaretlendiği için altı sarı. Gidermek için Name sonuna ! eklenebilir.
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

    
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product model, IFormFile imageFile)
    {


        if (ModelState.IsValid) // Bu if bloğu validation sorgusu yapar her alan doğru bir şekilde girilirse çalışır
        {

            model.ProductId = Repository.Products.Count + 1; //veri tabanı kullanılmadığı için listedeki elemanları sayar 1 fazladsını ekler    

            Repository.CreateProduct(model);
            return RedirectToAction("Index");

        }
        //Aşağıdaki kod bloğu model içinde validasyonu hataları olunca çalışır. 
        // Viewbag categorileri tekrar selectList içine getirir
        // return view mevcut girilen verileri koruması için model olarak döndürülür

        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(model);
    }


}
