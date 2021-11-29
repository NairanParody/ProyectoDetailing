using Proyecto.Models;
using Rotativa.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
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

            List<Producto> oProducto = new List<Producto>();

            using (DetailingEntities db = new DetailingEntities())
            {
                oProducto = (from p in db.Producto
                            select p).ToList();
            }
            return Json(new { data = oProducto }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int idproducto)
        {
            Producto producto = new Producto();

            using (DetailingEntities db = new DetailingEntities())
            {

                producto = (from p in db.Producto.Where(x => x.id_producto == idproducto)
                             select p).FirstOrDefault();
            }

            return Json(producto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Producto producto)
        {

            bool respuesta = true;
            try
            {

                if (producto.id_producto == 0)
                {
                    using (DetailingEntities db = new DetailingEntities())
                    {
                        db.Producto.Add(producto);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (DetailingEntities db = new DetailingEntities())
                    {
                        Producto tempProducto = (from p in db.Producto
                                                   where p.id_producto == producto.id_producto
                                                 select p).FirstOrDefault();

                        tempProducto.nombre = producto.nombre;
                        tempProducto.marca = producto.marca;
                        tempProducto.descripcion = producto.descripcion;
                        tempProducto.precio = producto.precio;

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

        public JsonResult Eliminar(int idproducto)
        {
            bool respuesta = true;
            try
            {
                using (DetailingEntities db = new DetailingEntities())
                {
                    Producto producto = new Producto();
                    producto = (from p in db.Producto.Where(x => x.id_producto == idproducto)
                                 select p).FirstOrDefault();

                    db.Producto.Remove(producto);

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
                return View(db.Producto.ToList());
            }
        }
        public ActionResult Imprimir()
        {
            return new ActionAsPdf("Listado");
        }
    }
}