using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;

internal class Program
{
    //SOLID
    //O harfi:Open Closed Princip(Yaptığın yazılıma yeni bir özellik ekliyorsan mevcuttaki hiçbir koduna dokunamazsın)
    //DTO : Data Transformation Object
    private static void Main(string[] args)
    {
        ProductTest();

        //CategoryTest();

    }

    private static void CategoryTest()
    {
        CategoryManager categoryManager = new CategoryManager(new EFCategoryDal());
        foreach (var category in categoryManager.GetAll())
        {
            Console.WriteLine(category.CategoryName);
        }
    }

    private static void ProductTest()
    {
        ProductManager productManager = new ProductManager(new EFProductDal());
        foreach (var product in productManager.GetProductDetails())
        {
            Console.WriteLine(product.CategoryName);
        }
    }
}