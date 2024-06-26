using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace web16.Controllers
{
    public class ProductsController : Controller
    {
        private ConnectDB db = new ConnectDB();

        // GET: Category
        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            return View(categories);
        }

        // GET: Products by Category ID
        public ActionResult GetProductsByCategory(int categoryId)
        {
            var products = db.Products
                             .Where(p => p.CategoryID == categoryId)
                             .ToList();

            return View(products);
        }
        public ActionResult GetProducts()
        {
            var products = db.Products.ToList();
            return PartialView("_ProductList", products);
        }

        // GET: Category Name by Category ID
        public ActionResult GetCategoryNameById(int categoryId)
        {
            var category = db.Categories
                             .Where(c => c.CategoryID == categoryId)
                             .Select(c => c.CategoryName)
                             .FirstOrDefault();

            if (category == null)
            {
                return HttpNotFound();
            }

            return Content(category);
        }

        // GET: Categories
        public ActionResult GetCategories()
        {
            var categories = db.Categories.ToList();
            return PartialView(categories);
        }

        // GET: Product Details
        public ActionResult ProductDetails(int productId)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Search(string searchString)
        {
            var products = from p in db.Products
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString));
            }

            return View(products.ToList());
        }
    }
}
