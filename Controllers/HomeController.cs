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
    public async Task<IActionResult> Create(Product model, IFormFile imageFile)
    {
        var extension="";


        if(imageFile != null)
        {
         var allowedExtension = new [] {".jpg", ".jpeg", ".png"};
         extension = Path.GetExtension(imageFile.FileName);

            if(!allowedExtension.Contains(extension))
            {
                ModelState.AddModelError ("", "Geçerli bir belge türü seçiniz..");
            }
        }

        if (ModelState.IsValid) // Bu if bloğu validation sorgusu yapar her alan doğru bir şekilde girilirse çalışır
        {
            if (imageFile !=null)
            {
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                using(var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                model.Image = randomFileName;

                model.ProductId = Repository.Products.Count + 1; //veri tabanı kullanılmadığı için listedeki elemanları sayar 1 fazladsını ekler    
                Repository.CreateProduct(model);
                return RedirectToAction("Index");

            }



        }
        //Aşağıdaki kod bloğu model içinde validasyonu hataları olunca çalışır. 
        // Viewbag categorileri tekrar selectList içine getirir
        // return view mevcut girilen verileri koruması için model olarak döndürülür

        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(model);
    }

    public IActionResult Edit(int? id)
    {
        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);

        if (entity ==null)
        {
            return NotFound();
        }
        
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(entity);
    }

    [HttpPost]

    public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)
    {

        if(id != model.ProductId)
        {
            
           return NotFound();

        }

        if(ModelState.IsValid)
        {

            if (imageFile != null)
            {   
                var extension = Path.GetExtension(imageFile.FileName);
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                using(var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                model.Image = randomFileName;

            }
            

            Repository.EditProduct(model);
            return RedirectToAction("Index");
        }

        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(model);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);

        if (entity == null)
        {
            return NotFound();
        }

        return View("DeleteConfirm", entity);

    }


    [HttpPost]
    public IActionResult Delete(int id, int ProductId)
    {
        if(id != ProductId)
        {
            return NotFound();
        }

        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == ProductId);

        if (entity == null)
        {
            return NotFound();
        }

        Repository.DeleteProduct(entity);
        return RedirectToAction("Index");

    }




}
