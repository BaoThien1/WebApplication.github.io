using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using WebApplication1.Models;
using System.Net;

namespace WebApplication1.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ConnectDB _context;

        public CustomerController()
        {
            _context = new ConnectDB();
        }
        private bool IsAdmin()
        {
            return Session["AdminID"] != null;
        }
        // GET: Customer
        public ActionResult Index()
        {
            if (!IsAdmin())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            var customers = _context.Customers.ToList();
            return View(customers);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (!IsAdmin())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            // Fetch orders for this customer including InvoiceDetails with associated Product
            var orders = _context.Invoices.Include(i => i.InvoiceDetails.Select(d => d.Product))
                                     .Where(i => i.CustomerID == id)
                                     .ToList();

            ViewBag.Orders = orders;
            return View(customer);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}
