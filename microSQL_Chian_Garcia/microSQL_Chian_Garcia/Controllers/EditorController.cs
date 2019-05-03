using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace microSQL_Chian_Garcia.Controllers
{
    public class EditorController : Controller
    {
        // GET: Editor
        public ActionResult Index()
        {
            return View();
        }

        // POST: Editor/Create
        [HttpPost]
        public ActionResult Ejecutar(FormCollection collection)
        {
            try
            {
                var texto = collection["TextoPantalla"];
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
