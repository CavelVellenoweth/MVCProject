using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Context;
using MVCProject.Models;
using PayPal.Api;

namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        MVCPetsContext db = new MVCPetsContext();

        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View(db.Categories.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View(db.Categories.ToList());
        }
        public ActionResult Products()
        {
            ViewBag.Message = "Your Products page.";
            ViewBag.Products = db.Products.ToList();
            ViewData["Products"] = db.Products.ToList();
            return View(db.Categories.ToList());
        }
        public ActionResult Product()
        {
            ViewBag.Message = "Your Product page.";
            ViewBag.Products = db.Products.ToList();
            ViewData["Products"] = db.Products.ToList();
            return View(db.Categories.ToList());
        }
        [HttpPost]
        public ActionResult UpdateCategories(string categoryName, string categoryDescription)
        {
            if (ModelState.IsValid)
            {
                if(db.Categories.Where(m=> m.CategoryName == categoryName).Count() > 0)
                {
                    Category category = db.Categories.FirstOrDefault(m => m.CategoryName == categoryName);
                    category.CategoryName = categoryName;
                    category.Description = categoryDescription;
                    db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    var newCategory = new Category();
                    newCategory.CategoryName = categoryName;
                    newCategory.Description = categoryDescription;
                    db.Categories.Add(newCategory);
                }                    
                db.SaveChanges();
            }
            return RedirectToAction("Admin");
        }
        [HttpPost]
        public ActionResult GetSelectedCategory()
        {
            TempData["SelectedCategory"] = Request.Form["SelectedCategory"];
            return RedirectToAction("Admin");
        }
        [HttpPost]
        public ActionResult UpdateProducts(string productName, string productDescription, HttpPostedFileBase image,string price, string categoryID,string currentStock,string discountPct)
        {
            if (ModelState.IsValid)
            {
                if (db.Products.Where(m => m.ProductName == productName).Count() > 0)
                {
                    Product product = db.Products.FirstOrDefault(m => m.ProductName == productName);
                    product.ProductName = productName;
                    product.Description = productDescription;
                    if (image != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Catalog/Images/Thumbs"), Path.GetFileName(image.FileName));
                        image.SaveAs(path);
                        product.ImagePath = image.FileName.ToString();
                    }
                    product.Price = Decimal.Parse(price);
                    product.CategoryID = Int32.Parse(categoryID);
                    product.CurrentStock = Int32.Parse(currentStock);
                    product.DiscountPct = Int32.Parse(discountPct);
                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    var newProduct = new Product();
                    newProduct.ProductName = productName;
                    newProduct.Description = productDescription;
                    var path = Path.Combine(Server.MapPath("~/Catalog/Images/Thumbs"), Path.GetFileName(image.FileName));
                    image.SaveAs(path);
                    newProduct.ImagePath = image.FileName.ToString();
                    newProduct.Price = Decimal.Parse(price);
                    newProduct.CategoryID = Int32.Parse(categoryID);
                    newProduct.CurrentStock = Int32.Parse(currentStock);
                    newProduct.DiscountPct = Int32.Parse(discountPct);
                    db.Products.Add(newProduct);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Admin");
        }
        [HttpPost]
        public ActionResult GetSelectedProduct()
        {
            TempData["SelectedProduct"] = Request.Form["SelectedProduct"];
            return RedirectToAction("Admin");
        }
        public ActionResult Admin()
        {
            ViewBag.SelectedCategory = TempData["SelectedCategory"];
            ViewBag.SelectedProduct = TempData["SelectedProduct"];
            ViewBag.Message = "Your Admin page.";
            ViewData["Products"] = db.Products;
            ViewData["Categories"] = db.Categories;
            return View(db.Categories.ToList());
        }
        public ActionResult Payment()
        {
            var apiContext = GetApiContext();
            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "paypal"
                },
                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        description = "You bought an animal",
                        amount = new Amount
                        {
                            currency = "GBP",
                            total = 99.99M.ToString(),
                        },
                        item_list = new ItemList()
                        {
                            items = new List<Item>()
                            {
                                new Item()
                                {
                                    description = "You bought an animal",
                                    currency = "GBP",
                                    price = 99.99M.ToString(),
                                    quantity = "1",
                                }
                            }
                        }
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    return_url = Url.Action("Return", "Home", null, Request.Url.Scheme),
                    cancel_url = Url.Action("Cancel", "Single", null, Request.Url.Scheme),
                }
            };

            //send payment to paypal
            var createdPayment = payment.Create(apiContext);

            //save reference to the paypal payment

            //find the approval url to send our user to
            var approvalUrl =
                createdPayment.links.FirstOrDefault(
                    x => x.rel.Equals("approval_url",
                    StringComparison.OrdinalIgnoreCase));

            //send the user to paypal to approve payment
            return Redirect(approvalUrl.href);
        }
        public ActionResult Return(string payerId, string paymentId)
        {
            var apiContext = GetApiContext();

            //set payer for payment
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            //identify the payment to execute
            var payment = new Payment()
            {
                id = paymentId
            };
            var executedpayement = payment.Execute(apiContext, paymentExecution);
            return RedirectToAction("Index");
        }

        //return View(db.Categories.ToList());
        private APIContext GetApiContext()
        {
            //authenticate with paypal
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;

        }
    }
}
