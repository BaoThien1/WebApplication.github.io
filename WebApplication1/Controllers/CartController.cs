using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        private ConnectDB _db = new ConnectDB();


        [HttpGet]
        public ActionResult Index()
        {
            if (Session["CustomerID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var customerID = Convert.ToInt32(Session["CustomerID"]);
            var cart = _db.Carts.FirstOrDefault(c => c.CustomerID == customerID);
            if (cart == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }

            var cartDetails = _db.CartDetails.Where(cd => cd.CartID == cart.CartID).ToList();
            return View(cartDetails);
        }

        [HttpPost]
        public ActionResult AddToCart(int? productId)
        {
            if (Session["AdminID"] != null)
            {
                return View();
            }

            if (Session["CustomerID"] == null )
            {

                return RedirectToAction("Login", "Home");
            }

                var product = _db.Products.Find(productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            var customerID = Convert.ToInt32(Session["CustomerID"]);
            var cart = _db.Carts.FirstOrDefault(c => c.CustomerID == customerID);
            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerID = customerID,
                    CreatedDate = DateTime.Now,
                    Total = 0
                };
                _db.Carts.Add(cart);
                _db.SaveChanges();
            }

            var cartDetail = _db.CartDetails.FirstOrDefault(cd => cd.CartID == cart.CartID && cd.ProductID == productId);
            if (cartDetail == null)
            {
                cartDetail = new CartDetail
                {
                    CartID = cart.CartID,
                    ProductID = productId.Value,
                    Quantity = 1
                };
                _db.CartDetails.Add(cartDetail);
            }
            else
            {
                cartDetail.Quantity += 1;
            }

            cart.Total += product.Price;
            _db.SaveChanges();

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public ActionResult UpdateCart(int? CartDetailID, int Quantity)
        {
            if (!CartDetailID.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "CartDetailID is required.");
            }

            var cartDetail = _db.CartDetails.Find(CartDetailID.Value);
            if (cartDetail == null)
            {
                return HttpNotFound("CartDetail not found.");
            }

            cartDetail.Quantity = Quantity;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteFromCart(int CartDetailID)
        {
            var cartDetail = _db.CartDetails.Find(CartDetailID);
            if (cartDetail == null)
            {
                return HttpNotFound("CartDetail not found.");
            }

            _db.CartDetails.Remove(cartDetail);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult Checkout()
        {
            if (Session["CustomerID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var customerID = Convert.ToInt32(Session["CustomerID"]);
            var cart = _db.Carts.Include("CartDetails.Product").FirstOrDefault(c => c.CustomerID == customerID);
            if (cart == null || cart.CartDetails.Count == 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CartTotal = cart.Total; // Truyền tổng số tiền của giỏ hàng vào ViewBag

            var cartDetails = cart.CartDetails.ToList();
            return View(cartDetails);
        }


        [HttpPost]
        public ActionResult ProcessCheckout(string shipAddress, string phone)
        {
            if (Session["CustomerID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var customerID = Convert.ToInt32(Session["CustomerID"]);
            var cart = _db.Carts.Include("CartDetails.Product").FirstOrDefault(c => c.CustomerID == customerID);
            if (cart == null || cart.CartDetails.Count == 0)
            {
                return RedirectToAction("Index"); // Redirect to cart or home page if cart is empty
            }

             if (string.IsNullOrEmpty(phone) || phone.Length != 10 || !phone.All(char.IsDigit))
            {
                ModelState.AddModelError("Phone", "Phone number must be exactly 10 digits.");
            }

            // Check if there are any validation errors
            if (!ModelState.IsValid)
            {
                // Restore shipAddress and re-render the Checkout view with errors
                ViewBag.ShipAddress = shipAddress;
                return View("Checkout", cart.CartDetails);
            }

            try
            {
                decimal total = 0;
                foreach (var cartDetail in cart.CartDetails)
                {
                    decimal price = cartDetail.Product.Price; // Default to 0 if Price is null
                    int quantity = cartDetail.Quantity ?? 0; // Default to 0 if Quantity is null

                    total += price * quantity;
                }
                // Create new Invoice
                var invoice = new Invoice
                {
                    CustomerID = customerID,
                    InvoiceDate = DateTime.Now,
                    ShipAddress = shipAddress,
                    Phone = phone,
                    Status = "Pending",
                    Total = total
                };
                _db.Invoices.Add(invoice);
                _db.SaveChanges();

                // Create InvoiceDetails for each cart item
                foreach (var cartDetail in cart.CartDetails)
                {
                    var invoiceDetail = new InvoiceDetail
                    {
                        InvoiceID = invoice.InvoiceID,
                        ProductID = cartDetail.ProductID,
                        Quantity = cartDetail.Quantity,
                        UnitPrice = cartDetail.Product.Price // Assuming Product has Price property
  
                    };
                    _db.InvoiceDetails.Add(invoiceDetail);
                }
                _db.SaveChanges();

                // Optionally, clear the cart after successful checkout
                _db.CartDetails.RemoveRange(cart.CartDetails);
                cart.Total = 0;
                _db.SaveChanges();

                ViewBag.Message = "Checkout successful! Your order has been placed.";

                return View("CheckoutConfirmation");

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                ViewBag.Message = "Checkout failed due to validation errors.";
                return View("Checkout");
            }

        }
        
    }
}