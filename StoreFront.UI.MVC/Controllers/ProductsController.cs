using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Drawing;//Added for access to Image class.
using StoreFront.Data.EF;
using StoreFront.UI.MVC.Utilities;//Added for access to ImageUtility
using PagedList;
using PagedList.Mvc;//Added for access to Server-Side Paged Listing
using StoreFront.UI.MVC.Models;//Added for access to CartItemViewModel

namespace StoreFront.UI.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private StoreFrontEntities db = new StoreFrontEntities();

        // GET: Products
        public ActionResult Index(string searchString, string categoryFilter, int page = 1)
        {
            //Sets records per page to 6
            int pageSize = 6;

            //Creates products List, ordered by Category then by Release Date.
            var products = db.Products.Include(p => p.Category).Include(p => p.ProductStatus).OrderBy(p => p.CategoryID).ThenBy(p => p.DateReleased).ToList();

            #region Filters

            if (!string.IsNullOrEmpty(searchString) && !string.IsNullOrEmpty(categoryFilter))
            {
                products = db.Products.Include(p => p.Category).Include(p => p.ProductStatus).Where(p => p.Category.CategoryName.ToLower().Contains(categoryFilter) && p.ProductName.ToLower().Contains(searchString.ToLower())).OrderBy(p => p.DateReleased).ToList();
            }
            else if (!string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(categoryFilter))
            {
                products = db.Products.Include(p => p.Category).Include(p => p.ProductStatus).Where(p => p.ProductName.ToLower().Contains(searchString.ToLower())).ToList();
            }
            else if (!string.IsNullOrEmpty(categoryFilter) && string.IsNullOrEmpty(searchString))
            {
                products = db.Products.Include(p => p.Category).Include(p => p.ProductStatus).Where(p => p.Category.CategoryName.ToLower().Contains(categoryFilter)).OrderBy(p => p.DateReleased).ToList();
            }

            ViewBag.SearchString = searchString;

            ViewBag.CategoryFilter = categoryFilter;

            #endregion

            return View(products.ToPagedList(page, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        #region Add-to-Cart Functionality 

        [HttpPost]
        public ActionResult AddToCart(int qty, int productID)
        {
            //Local shopping cart
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            //Check for Session shopping cart
            if (Session["cart"] != null)
            {
                shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];
            }
            else
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }

            //Get the product object
            Product product = db.Products.Where(p => p.ProductID == productID).FirstOrDefault();

            //If returns a null product, redirect to products index
            if (product == null)
            {
                return RedirectToAction("Index");
            }//end if - Product is null. Redirects.
            else
            {
                //Create the cart item.
                CartItemViewModel item = new CartItemViewModel(qty, product);

                //If the item already exists, increase the quantity.
                if (shoppingCart.ContainsKey(product.ProductID))
                {
                    shoppingCart[product.ProductID].Qty += qty;
                }
                //Else, add to the cart.
                else
                {
                    shoppingCart.Add(product.ProductID, item);
                }

                //Update the Global cart
                Session["cart"] = shoppingCart;

            }//end else - Product is not null. Updates cart.

            //Send user to their shopping cart.
            return RedirectToAction("Index", "ShoppingCart");

        }//end AddToCart Action
        

#endregion

// GET: Products/Create
[Authorize(Roles = "Admin")]
public ActionResult Create()
{
    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
    ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "Status");
    return View();
}

// POST: Products/Create
// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
[Authorize(Roles = "Admin")]
public ActionResult Create([Bind(Include = "ProductID,ProductName,Description,CategoryID,Price,ProductStatusID,UnitsInStock,DateReleased,ProductImage")] Product product, HttpPostedFileBase uploadedImage)
{
    if (ModelState.IsValid)
    {

        #region Image Upload

        //Default to the placeholder.
        string imageName = "placeholder.jpg";

        if (uploadedImage != null)
        {
            //Get image and assign to a variable.
            imageName = uploadedImage.FileName;

            //Use the file name and retrieve ONLY the extension and save 
            //it to a variable. We want to check if it's an image.
            string ext = imageName.Substring(imageName.LastIndexOf("."));

            //Create a whitelist of valid extensions.
            string[] goodExts = { ".jpeg", ".jpg", ".png", ".gif" };

            //Compare the ext of the upload to the whitelist.
            //It's always good to convert the casing to lower when
            //checking a string.
            if (goodExts.Contains(ext.ToLower()) && uploadedImage.ContentLength <= 4194304)
            {
                //Rename the file
                imageName = Guid.NewGuid() + ext;

                #region Resize Image

                //Define file path for images
                string savePath = Server.MapPath("~/Content/img/");

                //Convert the uploaded image to an Image object.
                Image convertedImage = Image.FromStream(uploadedImage.InputStream);

                int maxImageSize = 500;
                int maxThumbSize = 100;

                //Pass data to the ResizeImage() on the Image Utility.
                ImageUtility.ResizeImage(savePath, imageName, convertedImage, maxImageSize, maxThumbSize);

                #endregion

            }//end if - Whitelisted extension. Uploads file.
            else
            {

                //Assign the value back to the default.
                imageName = "placeholder.jpg";

            }//end else - File extension was not valid.

        }//end if - Checks if a file was submitted.

        //No matter whether or not the file upload actually contained a file, 
        //update the database with the current value of the file name.
        product.ProductImage = imageName;

        #endregion

        db.Products.Add(product);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
    ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "Status", product.ProductStatusID);
    return View(product);
}

// GET: Products/Edit/5
[Authorize(Roles = "Admin")]
public ActionResult Edit(int? id)
{
    if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
    Product product = db.Products.Find(id);
    if (product == null)
    {
        return HttpNotFound();
    }
    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
    ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "Status", product.ProductStatusID);
    return View(product);
}

// POST: Products/Edit/5
// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
[Authorize(Roles = "Admin")]
public ActionResult Edit([Bind(Include = "ProductID,ProductName,Description,CategoryID,Price,ProductStatusID,UnitsInStock,DateReleased,ProductImage")] Product product, HttpPostedFileBase uploadedImage)
{
    if (ModelState.IsValid)
    {

        #region Image Upload

        string imageName = "placeholder.jpg";

        //Check the input for the file upload and see if it is not null.
        if (uploadedImage != null)
        {
            //Get the file name and assign it to a variable.
            imageName = uploadedImage.FileName;

            //Use the file name and retrieve the extension.
            string ext = imageName.Substring(imageName.LastIndexOf("."));

            //Create a whitelist of valid extensions.
            string[] goodExts = { ".jpeg", ".jpg", ".png", ".gif" };

            //Compare the ext of the upload to the whitelist.
            if (goodExts.Contains(ext.ToLower()) && uploadedImage.ContentLength <= 4194304)
            {

                //Rename the file
                imageName = Guid.NewGuid() + ext;

                #region Resize Image

                string savePath = Server.MapPath("~/Content/img/");

                Image convertedImage = Image.FromStream(uploadedImage.InputStream);

                int maxImageSize = 500;
                int maxThumbSize = 100;

                ImageUtility.ResizeImage(savePath, imageName, convertedImage, maxImageSize, maxThumbSize);

                #endregion

                if (product.ProductImage != "placeholder.jpg" && product.ProductImage != null)
                {
                    string path = Server.MapPath("~/Content/img/");
                    ImageUtility.Delete(path, product.ProductImage);
                }//end if - Deleted previous image if not noImage.png.

                //Only if the image meets all criteria, save to the db.
                product.ProductImage = imageName;

            }//end if - Checks if file was valid extension and size.

        }//end if - Checks if a file was submitted.

        #endregion

        db.Entry(product).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
    }
    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
    ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "Status", product.ProductStatusID);
    return View(product);
}

// GET: Products/Delete/5
[Authorize(Roles = "Admin")]
public ActionResult Delete(int? id)
{
    if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
    Product product = db.Products.Find(id);
    if (product == null)
    {
        return HttpNotFound();
    }
    return View(product);
}

// POST: Products/Delete/5
[Authorize(Roles = "Admin")]
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public ActionResult DeleteConfirmed(int id)
{
    Product product = db.Products.Find(id);

    if (product.ProductImage != null && product.ProductImage != "placeholder.jpg")
    {
        string path = Server.MapPath("~/Content/img/");
        ImageUtility.Delete(path, product.ProductImage);
    }

    db.Products.Remove(product);
    db.SaveChanges();
    return RedirectToAction("Index");
}

protected override void Dispose(bool disposing)
{
    if (disposing)
    {
        db.Dispose();
    }
    base.Dispose(disposing);
}
}
}
