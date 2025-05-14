using ProductCatalog.models;
using ProductCatalog.services;
using ProductCatalog.utils;

namespace ProductCatalog
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";
            //Servisleri çağır.
            DBMongo dbMongo = new();
            CategoryService categoryService = new(dbMongo);
            ProductService productService = new(categoryService, dbMongo);
            bool isExit = false;
            while (!isExit)
            {
                Console.WriteLine("\r\nİstediğiniz işlemi seçin:");
                Console.WriteLine("1-)Ürün Ekleme");
                Console.WriteLine("2-)Ürün Güncelleme");
                Console.WriteLine("3-)Ürün Silme");
                Console.WriteLine("4-)Ürün Getir");
                Console.WriteLine("5-)Ürün Listeleme");
                Console.WriteLine("6-)Ürün Arama");
                Console.WriteLine("7-)Ürün Filtreleme");
                Console.WriteLine("8-)Kategori Ekleme");
                Console.WriteLine("9-)Kategori Güncelleme");
                Console.WriteLine("10-)Kategori Silme");
                Console.WriteLine("11-)Kategori Listeleme");
                Console.WriteLine("12-)Hata Loglarını Görüntüleme");
                Console.WriteLine("13-)Hata Loglarında Arama");
                Console.WriteLine("Çıkmak için 'q' veya 'exit' yazın.");
                Console.Write("\nSeçiminiz: ");
                input = Console.ReadLine()!.Trim().ToLower();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("Ürün ekleme işlemi seçildi.");

                        Console.Write("Ürün adı: ");
                        string productName = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(productName))
                        {
                            Console.WriteLine("Ürün adı boş olamaz.");
                            break;
                        }

                        Console.Write("Açıklama: ");
                        string description = Console.ReadLine()?.Trim() ?? "";

                        Console.Write("Fiyat: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
                        {
                            Console.WriteLine("Geçerli ve pozitif bir fiyat girilmelidir.");
                            break;
                        }

                        Console.Write("Stok: ");
                        if (!int.TryParse(Console.ReadLine(), out int stock) || stock < 0)
                        {
                            Console.WriteLine("Geçerli ve negatif olmayan bir stok değeri girilmelidir.");
                            break;
                        }

                        Console.Write("Kategori Id: ");
                        string categoryId = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(categoryId))
                        {
                            Console.WriteLine("Kategori Id boş olamaz.");
                            break;
                        }


                        Product product = new(productName, description, price, stock, categoryId);
                        int result = productService.AddProduct(product);
                        if (result == 1)
                        {
                            Console.WriteLine("Ürün başarıyla eklendi.");
                        }
                        else if (result == -2)
                        {
                            Console.WriteLine("Ürün eklenemedi. Kategori ID'si geçersiz.");
                        }
                        else
                        {
                            Console.WriteLine("Ürün eklenirken bir hata oluştu.");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Ürün güncelleme işlemi seçildi.");
                        Console.WriteLine("Güncellenecek ürünün ID'sini girin: ");
                        string productIdToUpdate = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(productIdToUpdate))
                        {
                            Console.WriteLine("Ürün ID'si boş olamaz.");
                            break;
                        }
                        Console.Write("Ürün adı: ");
                        string productNameToUpdate = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(productNameToUpdate))
                        {
                            Console.WriteLine("Ürün adı boş olamaz.");
                            break;
                        }

                        Console.Write("Açıklama: ");
                        string descriptionToUpdate = Console.ReadLine()?.Trim() ?? "";

                        Console.Write("Fiyat: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal priceToUpdate) || priceToUpdate <= 0)
                        {
                            Console.WriteLine("Geçerli ve pozitif bir fiyat girilmelidir.");
                            break;
                        }

                        Console.Write("Stok: ");
                        if (!int.TryParse(Console.ReadLine(), out int stockToUpdate) || stockToUpdate < 0)
                        {
                            Console.WriteLine("Geçerli ve negatif olmayan bir stok değeri girilmelidir.");
                            break;
                        }

                        Console.Write("Kategori Id: ");
                        string categoryIdToUpdate = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(categoryIdToUpdate))
                        {
                            Console.WriteLine("Kategori Id boş olamaz.");
                            break;
                        }


                        Product productToUpdate = new(productIdToUpdate, productNameToUpdate, descriptionToUpdate, priceToUpdate, stockToUpdate, categoryIdToUpdate);
                        int isUpdated = productService.UpdateProduct(productToUpdate);
                        if (isUpdated == 1)
                        {
                            Console.WriteLine("Ürün başarıyla güncellendi.");
                        }
                        else if (isUpdated == -2)
                        {
                            Console.WriteLine("Ürün güncellenemedi. Kategori ID'si geçersiz.");
                        }
                        else
                        {
                            Console.WriteLine("Ürün güncellenirken bir hata oluştu.");
                        }
                        break;

                    case "3":
                        Console.WriteLine("Ürün silme işlemi seçildi.");
                        Console.WriteLine("Silmek istediğiniz ürünün ID'sini giriniz.");
                        string deleteId = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(deleteId))
                        {
                            Console.WriteLine("ID null olamaz. İşlem başarısız.");
                        }
                        int deleteResult = productService.DeleteProduct(deleteId);
                        if (deleteResult == 0)
                        {
                            Console.WriteLine("Ürün silme işlemi başarısız oldu.");
                        }
                        else
                        {
                            Console.WriteLine("Ürün silindi.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("Ürün Getirme işlemi seçildi.");
                        Console.WriteLine("Ürün ID'si giriniz.");
                        string productId2 = Console.ReadLine()?.Trim() ?? "";
                        var productById = productService.GetProductById(productId2);
                        if (productById != null)
                        {
                            Console.WriteLine("===============***************===============");
                            Console.WriteLine($"ID: {productById.Id}\nName: {productById.Name}\nPrice: {productById.Price}\nCategory: {categoryService.GetCategoryNameById(productById.CategoryId)}\nDescription: {productById.Description}\n ");
                        }
                        break;

                    case "5":
                        Console.WriteLine("Ürün listeleme işlemi seçildi.");
                        var productsList = productService.GetProducts(pageNumber: 1, pageSize: 10);
                        foreach (var productList in productsList)
                        {
                            Console.WriteLine("===============***************===============");
                            Console.WriteLine($"ID: {productList.Id}\nName: {productList.Name}\nPrice: {productList.Price}\nCategory: {categoryService.GetCategoryNameById(productList.CategoryId)}\nDescription: {productList.Description}\n ");
                        }
                        break;

                    case "6":
                        Console.WriteLine("Ürün arama işlemi seçildi.");
                        break;
                    case "7":
                        Console.WriteLine("Ürün filtreleme işlemi seçildi.");
                        break;

                    case "8":
                        Console.WriteLine("Kategori ekleme işlemi seçildi.");
                        Console.Write("Kategori adı: ");
                        string categoryName = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(categoryName))
                        {
                            Console.WriteLine("Kategori adı boş olamaz.");
                            break;
                        }
                        Category category = new(categoryName);
                        int result7 = categoryService.AddCategory(category);
                        if (result7 > 0)
                        {
                            Console.WriteLine("Kategori başarıyla eklendi.");
                        }
                        else
                        {
                            Console.WriteLine("Kategori eklenirken bir hata oluştu.");
                        }
                        break;

                    case "9":
                        Console.WriteLine("Kategori güncelleme işlemi seçildi.");
                        break;

                    case "10":
                        Console.WriteLine("Kategori silme işlemi seçildi.");
                        break;

                    case "11":
                        Console.WriteLine("Kategori listeleme işlemi seçildi.");
                        break;

                    case "12":
                        Console.WriteLine("Hata loglarını görüntüleme işlemi seçildi.");
                        break;

                    case "13":
                        Console.WriteLine("Hata loglarında arama işlemi seçildi.");
                        break;

                    case "q":
                    case "exit":
                        Console.WriteLine("Uygulamadan çıkılıyor...");
                        isExit = true;
                        break;

                    default:
                        Console.WriteLine("Geçersiz seçim yaptınız.");
                        Console.Clear();
                        break;
                }

            }
        }
    }
}
