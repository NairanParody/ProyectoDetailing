using Proyecto.Models;
using Rotativa.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }
        }

        public JsonResult Listar()
        {

            List<Proveedor> oCliente = new List<Proveedor>();

            using (DetailingEntities db = new DetailingEntities())
            {

                oCliente = (from p in db.Proveedor
                               select p).ToList();
            }
            return Json(new { data = oCliente }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int idproveedor)
        {
            Proveedor proveedor = new Proveedor();

            using (DetailingEntities db = new DetailingEntities())
            {

                proveedor = (from p in db.Proveedor.Where(x => x.id_proveedor == idproveedor)
                           select p).FirstOrDefault();
            }

            return Json(proveedor, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Proveedor proveedor)
        {

            bool respuesta = true;
            try
            {

                if (proveedor.id_proveedor == 0)
                {
                    using (DetailingEntities db = new DetailingEntities())
                    {
                        db.Proveedor.Add(proveedor);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (DetailingEntities db = new DetailingEntities())
                    {
                        Proveedor tempProveedor = (from p in db.Proveedor
                                               where p.id_proveedor == proveedor.id_proveedor
                                               select p).FirstOrDefault();

                        tempProveedor.nombre = proveedor.nombre;
                        tempProveedor.cuit = proveedor.cuit;
                        tempProveedor.telefono = proveedor.telefono;
                        tempProveedor.correo = proveedor.correo;
                        
                        db.SaveChanges();
                    }

                }
            }
            catch
            {
                respuesta = false;

            }

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Eliminar(int idproveedor)
        {
            bool respuesta = true;
            try
            {
                using (DetailingEntities db = new DetailingEntities())
                {
                    Proveedor proveedor = new Proveedor();
                    proveedor = (from p in db.Proveedor.Where(x => x.id_proveedor == idproveedor)
                               select p).FirstOrDefault();

                    db.Proveedor.Remove(proveedor);

                    db.SaveChanges();
                }
            }
            catch
            {
                respuesta = false;
            }
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Listado()
        {
            using (var db = new DetailingEntities())
            {
                return View(db.Proveedor.ToList());
            }
        }
        public ActionResult Imprimir()
        {
            return new ActionAsPdf("Listado");
        }
    }
}