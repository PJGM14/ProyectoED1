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

        // GET: Editor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Editor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Editor/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
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

        // GET: Editor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Editor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Editor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Editor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
