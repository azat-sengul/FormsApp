namespace FormsApp.Models
{
    public class ProductViewModel
    {
        public List<Product> Products {get; set;} = null!;

        public List<Category> Categories {get; set; } =null!;

        public string? SelectedCategory {get; set;}
    }
}

// Model sayfasına bir liste (IEnumerable) ve
// beraberinde viewbag ile ek bilgiler göndermek yerine bu 
// ProductViewModel ile hepsine tek bir yerden göndermek için bu class üretildi