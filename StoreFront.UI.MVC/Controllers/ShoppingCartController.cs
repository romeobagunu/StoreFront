using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreFront.UI.MVC.Models;
using StoreFront.Data.EF;

namespace StoreFront.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            //Create a local version of the shopping cart from the global cart.
            var shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            //If there was no global version, initialize a local one.
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
                ViewBag.Message = "There are no products in your cart.";
            }//end if - No global cart, creates local cart and sends message.
            else
            {
                ViewBag.Message = null;
            }//end else - Global cart exists, nulls message for empty cart.

            //Return the cart view and pass through the cart.
            return View(shoppingCart);
        }

        public ActionResult UpdateCart(int productID, int qty)
        {
            //If they set quantity to zero, remove the item from the cart.
            if (qty == 0)
            {
                RemoveFromCart(productID);

                return RedirectToAction("Index");
            }

            //Retrieve the session cart and open a local cart
            Dictionary<int, CartItemViewModel> shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            //Update the quantity in the local cart.
            shoppingCart[productID].Qty = qty;

            //Push the local cart to the session cart
            Session["cart"] = shoppingCart;

            //Pass through a message if they completely emptied the cart.
            if (shoppingCart.Count == 0)
            {
                ViewBag.Message = "Your cart is empty.";
            }

            return RedirectToAction("Index");
        }//end UpdateCart

        public ActionResult RemoveFromCart(int id)
        {
            //Retrieve the global cart and create a local cart
            Dictionary<int, CartItemViewModel> shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            shoppingCart.Remove(id);

            //If the cart is now empty, null out the session cart 
            if (shoppingCart.Count == 0)
            {
                Session["cart"] = null;
            }

            return RedirectToAction("Index");
        }

    }
}