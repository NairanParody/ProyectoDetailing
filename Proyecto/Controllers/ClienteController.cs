using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using Rotativa.MVC;

namespace Proyecto.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
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

            List<Cliente> oLstPersona = new List<Cliente>();

            using (DetailingEntities db = new DetailingEntities())
            {

                oLstPersona = (from p in db.Cliente
                               select p).ToList();
            }
            return Json(new { data = oLstPersona }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int idcliente)
         {
             Cliente cliente = new Cliente();

             using (DetailingEntities db = new DetailingEntities())
             {

                 cliente = (from p in db.Cliente.Where(x => x.id_cliente == idcliente)
                             select p).FirstOrDefault();
             }

             return Json(cliente, JsonRequestBehavior.AllowGet);
         }

         [HttpPost]
         public JsonResult Guardar(Cliente cliente)
         {

             bool respuesta = true;
             try
             {

                 if (cliente.id_cliente == 0)
                 {
                     using (DetailingEntities db = new DetailingEntities())
                     {
                         db.Cliente.Add(cliente);
                         db.SaveChanges();
                     }
                 }
                 else
                 {
                     using (DetailingEntities db = new DetailingEntities())
                     {
                         Cliente tempCliente = (from p in db.Cliente
                                                 where p.id_cliente == cliente.id_cliente
                                                 select p).FirstOrDefault();

                         tempCliente.nombre = cliente.nombre;
                         tempCliente.dni = cliente.dni;
                         tempCliente.correo = cliente.correo;
                         tempCliente.direccion = cliente.direccion;
                         tempCliente.telefono = cliente.telefono;

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

         public JsonResult Eliminar(int idcliente)
         {
             bool respuesta = true;
             try
             {
                 using (DetailingEntities db = new DetailingEntities())
                 {
                     Cliente cliente = new Cliente();
                     cliente = (from p in db.Cliente.Where(x => x.id_cliente == idcliente)
                                 select p).FirstOrDefault();

                     db.Cliente.Remove(cliente);

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
                return View(db.Cliente.ToList());
            }
        }
        public ActionResult Imprimir()
        {
            return new ActionAsPdf("Listado");
        }
    }
}