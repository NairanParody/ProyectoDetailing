using Proyecto.Models;
using Rotativa.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class VentaController : Controller
    {
        // GET: Venta
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

            List<Venta> oVenta = new List<Venta>();

            using (DetailingEntities db = new DetailingEntities())
            {

                oVenta = (from p in db.Venta
                               select p).ToList();
            }
            return Json(new { data = oVenta }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int idventa)
        {
            Venta venta = new Venta();

            using (DetailingEntities db = new DetailingEntities())
            {

                venta = (from p in db.Venta.Where(x => x.id_venta == idventa)
                           select p).FirstOrDefault();
            }

            return Json(venta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Venta venta)
        {

            bool respuesta = true;
            try
            {

                if (venta.id_venta == 0)
                {
                    using (DetailingEntities db = new DetailingEntities())
                    {
                        db.Venta.Add(venta);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (DetailingEntities db = new DetailingEntities())
                    {
                        Venta tempVenta = (from p in db.Venta
                                               where p.id_venta == venta.id_venta
                                               select p).FirstOrDefault();

                        tempVenta.tipo_cliente = venta.tipo_cliente;
                        tempVenta.fecha = venta.fecha;
                        tempVenta.tipo_pago = venta.tipo_pago;

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

        public JsonResult Eliminar(int idventa)
        {
            bool respuesta = true;
            try
            {
                using (DetailingEntities db = new DetailingEntities())
                {
                    Venta venta = new Venta();
                    venta = (from p in db.Venta.Where(x => x.id_venta == idventa)
                               select p).FirstOrDefault();

                    db.Venta.Remove(venta);

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
                return View(db.Venta.ToList());
            }
        }
        public ActionResult Imprimir()
        {
            return new ActionAsPdf("Listado");
        }
    }
}