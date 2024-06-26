using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;

public class CategoryController : Controller
{
    private ConnectDB _db = new ConnectDB();
    private bool IsAdmin()
    {
        return Session["AdminID"] != null;
    }
    // GET: Category
    public ActionResult Index()
    {
        if (!IsAdmin())
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        var categories = _db.Categories.ToList();
        return View(categories);
    }

    // GET: Category/Create
    public ActionResult Create()
    {
        if (!IsAdmin())
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        return View();
    }

    // POST: Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "CategoryName, Description")] Category category)
    {
        if (!IsAdmin())
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        if (ModelState.IsValid)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(category);
    }

    // GET: Category/Edit/5
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
        Category category = _db.Categories.Find(id);
        if (category == null)
        {
            return HttpNotFound();
        }
        return View(category);
    }

    // POST: Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "CategoryID, CategoryName, Description")] Category category)
    {
        if (!IsAdmin())
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        if (ModelState.IsValid)
        {
            _db.Entry(category).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }

    // GET: Category/Delete/5
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
        Category category = _db.Categories.Find(id);
        if (category == null)
        {
            return HttpNotFound();
        }
        return View(category);
    }

    // POST: Category/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        if (!IsAdmin())
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        Category category = _db.Categories.Find(id);
        _db.Categories.Remove(category);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _db.Dispose();
        }
        base.Dispose(disposing);
    }
}
