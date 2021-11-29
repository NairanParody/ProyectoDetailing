using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
                return View();
        }

        //[HttpPost]
        public ActionResult Login(string nombre, string clave)
        {
            using (DetailingEntities db = new DetailingEntities())
            {
                var obj = db.Usuario.FirstOrDefault(p => p.nombre == nombre && p.clave == clave);

                if (obj != null)
                {
                    Session["Usuario"] = obj;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Usuario o contraseña no correcta";
                    return RedirectToAction("Index","Login");
                } 
            }
        }

        public ActionResult CerrarSesion()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }

   
}