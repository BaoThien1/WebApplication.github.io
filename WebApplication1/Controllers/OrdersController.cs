using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;

public class OrdersController : Controller
{
    private ConnectDB _db = new ConnectDB();

    private bool IsAdmin()
    {
        return Session["AdminID"] != null;
    }
    // GET: Orders
    public ActionResult Index()
    {
        if (!IsAdmin())
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        var invoices = _db.Invoices.Include(i => i.InvoiceDetails).ToList();
        return View(invoices);
    }

    // GET: Orders/Details/5
    // GET: Orders/Details/5
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
        // Include both InvoiceDetails and related Products to fetch product names
        Invoice invoice = _db.Invoices
                            .Include(i => i.InvoiceDetails.Select(d => d.Product))
                            .FirstOrDefault(i => i.InvoiceID == id);
        if (invoice == null)
        {
            return HttpNotFound();
        }
        return View(invoice);
    }


    // GET: Invoice/Edit/5
    public ActionResult Edit(int? id)
    {
        if (!IsAdmin())
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        var invoice = _db.Invoices.Find(id);
        if (invoice == null)
        {
            return HttpNotFound();
        }

        return View(invoice);
    }

    // POST: Invoice/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Invoice invoice)
    {
        if (!IsAdmin())
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        if (ModelState.IsValid)
        {
            _db.Entry(invoice).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(invoice);
    }

    // GET: Orders/Delete/5
    public ActionResult Delete(int? id)
    {
        if (!IsAdmin())
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        Invoice invoice = _db.Invoices.Find(id);
        if (invoice == null)
        {
            return HttpNotFound();
        }

        return View(invoice);
    }

    // POST: Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        if (!IsAdmin())
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        Invoice invoice = _db.Invoices.Include(i => i.InvoiceDetails).FirstOrDefault(i => i.InvoiceID == id);
        if (invoice != null)
        {
            // Remove associated InvoiceDetails
            foreach (var detail in invoice.InvoiceDetails.ToList())
            {
                _db.InvoiceDetails.Remove(detail);
            }

            // Remove the invoice itself
            _db.Invoices.Remove(invoice);

            // Save changes
            _db.SaveChanges();
        }

        return RedirectToAction("Index");
    }
    // Dispose the context to avoid memory leaks
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _db.Dispose();
        }
        base.Dispose(disposing);
    }
}
