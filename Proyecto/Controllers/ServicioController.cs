using Proyecto.Models;
using Rotativa.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class ServicioController : Controller
    {
        // GET: Servicio
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

            List<Servicio> oServicio = new List<Servicio>();

            using (DetailingEntities db = new DetailingEntities())
            {

                oServicio = (from p in db.Servicio
                               select p).ToList();
            }
            return Json(new { data = oServicio }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int idservicio)
        {
            Servicio servicio = new Servicio();

            using (DetailingEntities db = new DetailingEntities())
            {

                servicio = (from p in db.Servicio.Where(x => x.id_servicio == idservicio)
                           select p).FirstOrDefault();
            }

            return Json(servicio, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Servicio servicio)
        {

            bool respuesta = true;
            try
            {

                if (servicio.id_servicio == 0)
                {
                    using (DetailingEntities db = new DetailingEntities())
                    {
                        db.Servicio.Add(servicio);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (DetailingEntities db = new DetailingEntities())
                    {
                        Servicio tempServicio = (from p in db.Servicio
                                               where p.id_servicio == servicio.id_servicio
                                               select p).FirstOrDefault();

                        tempServicio.nombre = servicio.nombre;
                        tempServicio.descripcion = servicio.descripcion;

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

        public JsonResult Eliminar(int idservicio)
        {
            bool respuesta = true;
            try
            {
                using (DetailingEntities db = new DetailingEntities())
                {
                    Servicio servicio = new Servicio();
                    servicio = (from p in db.Servicio.Where(x => x.id_servicio == idservicio)
                               select p).FirstOrDefault();

                    db.Servicio.Remove(servicio);

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
                return View(db.Servicio.ToList());
            }
        }
        public ActionResult Imprimir()
        {
            return new ActionAsPdf("Listado");
        }
    }
}