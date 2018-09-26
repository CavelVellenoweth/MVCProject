using System;
using System.Collections.Generic;
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
