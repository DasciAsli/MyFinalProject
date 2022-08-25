using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;

internal class Program
{
    //SOLID
    //O harfi:Open Closed Princip(Yaptığın yazılıma yeni bri özellik ekliyorsan mevcuttaki hiçbir koduna dokunamazsın)
    private static void Main(string[] args)
    {
        ProductManager productManager = new ProductManager(new EFProductDal());
        foreach (var product in productManager.GetAllByCategoryId(2))
        {
            Console.WriteLine(product.ProductName);
        }
    }
}