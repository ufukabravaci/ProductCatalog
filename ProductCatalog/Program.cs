﻿using ProductCatalog.models;
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
            FileService fileService = new();
            CategoryService categoryService = new(dbMongo, fileService);
            ProductService productService = new(categoryService, dbMongo, fileService);
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
            Console.WriteLine("\nSeçiminiz: ");
            input = Console.ReadLine()!.Trim().ToLower();
            switch (input)
            {
                case "1":
                    Console.WriteLine("Ürün ekleme işlemi seçildi.");

                    Console.WriteLine("Ürün adı: ");
                    string productName = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(productName))
                    {
                        Console.WriteLine("Ürün adı boş olamaz.");
                        break;
                    }

                    Console.WriteLine("Açıklama: ");
                    string description = Console.ReadLine()?.Trim() ?? "";

                    Console.WriteLine("Fiyat: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
                    {
                        Console.WriteLine("Geçerli ve pozitif bir fiyat girilmelidir.");
                        break;
                    }

                    Console.WriteLine("Stok: ");
                    if (!int.TryParse(Console.ReadLine(), out int stock) || stock < 0)
                    {
                        Console.WriteLine("Geçerli ve negatif olmayan bir stok değeri girilmelidir.");
                        break;
                    }

                    Console.WriteLine("Kategori Id: ");
                    string categoryId = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(categoryId))
                    {
                        Console.WriteLine("Kategori Id boş olamaz.");
                        break;
                    }


                    Product product = new()
                    {
                        Name = productName,
                        Description = description,
                        Price = price,
                        Stock = stock,
                        CategoryId = categoryId
                    };
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
                    Console.WriteLine("Ürün adı: ");
                    string productNameToUpdate = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(productNameToUpdate))
                    {
                        Console.WriteLine("Ürün adı boş olamaz.");
                        break;
                    }

                    Console.WriteLine("Açıklama: ");
                    string descriptionToUpdate = Console.ReadLine()?.Trim() ?? "";

                    Console.WriteLine("Fiyat: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal priceToUpdate) || priceToUpdate <= 0)
                    {
                        Console.WriteLine("Geçerli ve pozitif bir fiyat girilmelidir.");
                        break;
                    }

                    Console.WriteLine("Stok: ");
                    if (!int.TryParse(Console.ReadLine(), out int stockToUpdate) || stockToUpdate < 0)
                    {
                        Console.WriteLine("Geçerli ve negatif olmayan bir stok değeri girilmelidir.");
                        break;
                    }

                    Console.WriteLine("Kategori Id: ");
                    string categoryIdToUpdate = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(categoryIdToUpdate))
                    {
                        Console.WriteLine("Kategori Id boş olamaz.");
                        break;
                    }


                    Product productToUpdate = new()
                    {
                        Id = productIdToUpdate,
                        Name = productNameToUpdate,
                        Price = priceToUpdate,
                        Stock = stockToUpdate,
                        CategoryId = categoryIdToUpdate,

                    };
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
                    Console.WriteLine("Sayfalı ürün listeleme işlemi seçildi.");

                    Console.Write("Sayfa numarasını giriniz (örnek: 1): ");
                    int.TryParse(Console.ReadLine(), out int pageNumber);

                    Console.Write("Sayfa başına ürün sayısını giriniz (örnek: 10): ");
                    int.TryParse(Console.ReadLine(), out int pageSize);

                    var pagedProducts = productService.GetProducts(pageNumber, pageSize);

                    if (pagedProducts.Any())
                    {
                        Console.WriteLine($"{pagedProducts.Count} ürün listelendi (Sayfa: {pageNumber}).");
                        foreach (var productPaged in pagedProducts)
                        {
                            Console.WriteLine("===============***************===============");
                            Console.WriteLine($"ID: {productPaged.Id}\nName: {productPaged.Name}\nPrice: {productPaged.Price}\nCategory: {categoryService.GetCategoryNameById(productPaged.CategoryId)}\nDescription: {productPaged.Description}\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Bu sayfada listelenecek ürün bulunamadı.");
                    }
                    break;
                case "6":
                    Console.WriteLine("Ürün arama işlemi seçildi.");
                    Console.WriteLine("Arama yapmak istediğiniz kelimeyi giriniz: ");
                    string keyword = Console.ReadLine()?.Trim() ?? "";
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        var searchList = productService.GetSearchProducts(keyword);
                        if (searchList.Any())
                        {
                            Console.WriteLine($"{searchList.Count} adet ürün bulundu.");
                            foreach (var sl in searchList)
                            {
                                Console.WriteLine("===============***************===============");
                                Console.WriteLine($"ID: {sl.Id}\nName: {sl.Name}\nPrice: {sl.Price}\nCategory: {categoryService.GetCategoryNameById(sl.CategoryId)}\nDescription: {sl.Description}\n ");

                            }
                        }
                        else
                        {
                            Console.WriteLine("Aradığınız kritere uygun ürün bulunamadı.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Arama yapmak istediğiniz kelime boş olamaz.");
                    }
                    break;
                case "7":
                    Console.WriteLine("Ürün filtreleme işlemi seçildi.");
                    Console.WriteLine("Minimum fiyat giriniz (boş bırakabilirsiniz):");
                    string? minInput = Console.ReadLine();
                    //boş bırakınca default olarak değeri sıfıra atıyor olması sorun yaratıyordu. Bu sebeple eğer boşsa null olsun dedik.
                    decimal? minVal = string.IsNullOrWhiteSpace(minInput) ? null : decimal.Parse(minInput);
                    Console.WriteLine("Maksimum fiyat giriniz (boş bırakabilirsiniz):");
                    string? maxInput = Console.ReadLine();
                    decimal? maxVal = string.IsNullOrWhiteSpace(maxInput) ? null : decimal.Parse(maxInput);
                    Console.WriteLine("KategoriID giriniz: ");
                    string catId = Console.ReadLine()?.Trim() ?? "";
                    var filteredList = productService.FilterProducts(minVal, maxVal, catId);
                    if (filteredList.Any())
                    {
                        Console.WriteLine($"{filteredList.Count} adet ürün bulundu.");
                        foreach (var sl in filteredList)
                        {
                            Console.WriteLine("===============***************===============");
                            Console.WriteLine($"ID: {sl.Id}\nName: {sl.Name}\nPrice: {sl.Price}\nCategory: {categoryService.GetCategoryNameById(sl.CategoryId)}\nDescription: {sl.Description}\n ");

                        }
                    }
                    else
                    {
                        Console.WriteLine("Aradığınız kritere uygun ürün bulunamadı.");
                    }
                    break;
                case "8":
                    Console.WriteLine("Kategori ekleme işlemi seçildi.");
                    Console.WriteLine("Kategori adı: ");
                    string categoryName = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(categoryName))
                    {
                        Console.WriteLine("Kategori adı boş olamaz.");
                        break;
                    }
                    Category category = new()
                    {
                        Name = categoryName
                    };
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
                    Console.WriteLine("Güncellenecek Kategoriye ait ID giriniz:");
                    string cateId = Console.ReadLine()?.Trim() ?? "";
                    if (!string.IsNullOrWhiteSpace(cateId))
                    {
                        if (categoryService.CategoryExists(cateId))
                        {
                            Console.WriteLine("Yeni Kategori ismini giriniz:");
                            string cateName = Console.ReadLine()?.Trim() ?? "";
                            if (!string.IsNullOrWhiteSpace(cateName))
                            {
                                if (categoryService.UpdateCategory(cateId, cateName) == -1)
                                {
                                    Console.WriteLine("Kategori Güncellenirken bir hata oluştu.");
                                }
                                if (categoryService.UpdateCategory(cateId, cateName) == 1)
                                {
                                    Console.WriteLine($"Kategori güncellendi. {cateId} numaralı kategorinin yeni kategori ismi {cateName} olarak değiştirildi.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Kategori ismi boş olamaz.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Girmiş olduğunuz kategoryId bir kategoriye ait değil. Lütfen doğru bir kategoryId ile tekrar deneyin.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("KategoryId boş olamaz.");
                    }
                    break;
                case "10":
                    Console.WriteLine("Kategori silme işlemi seçildi.");
                    Console.Write("Silmek istediğiniz kategori ID'sini giriniz: ");
                    string delId = Console.ReadLine()?.Trim() ?? "";

                    if (!string.IsNullOrWhiteSpace(delId))
                    {
                        var delres = categoryService.DeleteCategory(delId);
                        if (delres == 1)
                        {
                            Console.WriteLine("Kategori başarıyla silindi.");
                        }
                        else if (delres == 0)
                        {
                            Console.WriteLine("Kategori bulunamadı. Lütfen geçerli bir ID girin.");
                        }
                        else
                        {
                            Console.WriteLine("Kategori silinirken bir hata oluştu.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Kategori ID boş olamaz.");
                    }
                    break;
                case "11":
                    Console.WriteLine("Kategori listeleme işlemi seçildi.");
                    var allCategories = categoryService.GetAllCategories();

                    if (allCategories.Any())
                    {
                        Console.WriteLine($"{allCategories.Count} adet kategori bulundu:");
                        foreach (var cat in allCategories)
                        {
                            Console.WriteLine($"ID: {cat.Id} Name: {cat.Name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hiç kategori bulunamadı.");
                    }
                    break;
                case "12":
                    Console.WriteLine("Hata loglarını görüntüleme işlemi seçildi.");
                    var logs = fileService.ReadLogs();
                    foreach (var log in logs)
                    {
                        Console.WriteLine($"{log}{Environment.NewLine}");
                    }
                    break;
                case "13":
                    Console.WriteLine("Hata loglarında arama işlemi seçildi.");

                    Console.Write("Başlangıç tarihi giriniz (örnek: 13.05.2025) veya boş bırakın: ");
                    string startInput = Console.ReadLine()?.Trim() ?? "";
                    DateTime? startDate = null;
                    //datetime'a parse edilebilir bir yapıda input gelmişse startDate'e atama yapıyoruz.
                    if (!string.IsNullOrEmpty(startInput) && DateTime.TryParse(startInput, out DateTime startParsed))
                    {
                        startDate = startParsed;
                    }
                    //EndDate ataması
                    Console.Write("Bitiş tarihi giriniz (örnek: 15.05.2025) veya boş bırakın: ");
                    string endInput = Console.ReadLine()?.Trim() ?? "";
                    DateTime? endDate = null;
                    if (!string.IsNullOrEmpty(endInput) && DateTime.TryParse(endInput, out DateTime endParsed))
                    {
                        endDate = endParsed;
                    }

                    Console.Write("Aramak istediğiniz anahtar kelimeyi giriniz (veya boş bırakın): ");
                    string? searchKeyword = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(searchKeyword))
                    {
                        searchKeyword = null;
                    }

                    var filteredLogs = fileService.FilterLogs(startDate, endDate, searchKeyword);
                    if (filteredLogs.Any())
                    {
                        Console.WriteLine($"\n{filteredLogs.Count} adet log bulundu:\n");
                        foreach (var log in filteredLogs)
                        {
                            Console.WriteLine(log);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Belirtilen kriterlere uygun log bulunamadı.");
                    }

                    break;
                case "q":
                case "exit":
                    Console.WriteLine("Uygulamadan çıkılıyor...");
                    break;

                default:
                    Console.WriteLine("Geçersiz seçim yaptınız.");
                    Console.Clear();
                    break;
            }

        }
    }

}
