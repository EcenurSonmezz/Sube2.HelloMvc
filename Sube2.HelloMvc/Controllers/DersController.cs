using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sube2.HelloMvc.Models;
using System.Linq;

namespace Sube2.HelloMvc.Controllers
{
    public class DersController : Controller
    {
        public IActionResult Index()
        {
            using (var ctx = new OkulDbContext())
            {
                var dersler = ctx.Dersler.ToList();
                return View(dersler);
            }
        }

        [HttpGet]
        public IActionResult AddDers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDers(Ders ders)
        {
            if (ders != null)
            {
                using (var ctx = new OkulDbContext())
                {
                    // Önce dersi ekleyelim ve kaydedelim
                    ctx.Dersler.Add(ders);
                    var saveResult = ctx.SaveChanges();

                    if (saveResult > 0)
                    {
                        // Kayıt başarılı ise ana sayfaya yönlendirme
                        TempData["SuccessMessage"] = "Ders başarıyla eklendi.";
                        return RedirectToAction("Index");
                    }
                }
            }

            // Kayıt başarısız ise tekrar ders ekle sayfasını göster
            ViewBag.ErrorMessage = "Ders eklenirken bir hata oluştu.";
            return View(ders);
        }

        public IActionResult EditDers(int id)
        {
            using (var ctx = new OkulDbContext())
            {
                var ders = ctx.Dersler.Find(id);
                return View(ders);
            }
        }

        [HttpPost]
        public IActionResult EditDers(Ders ders)
        {
            if (ders != null)
            {
                using (var ctx = new OkulDbContext())
                {
                    ctx.Entry(ders).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(ders);
        }

        public IActionResult DeleteDers(int id)
        {
            using (var ctx = new OkulDbContext())
            {
                var ders = ctx.Dersler.Find(id);
                if (ders != null)
                {
                    ctx.Dersler.Remove(ders);
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ConfirmDelete(int id)
        {
            using (var ctx = new OkulDbContext())
            {
                var ders = ctx.Dersler.Find(id);
                return View(ders);
            }
        }

        [HttpPost]
        public IActionResult ConfirmDelete(Ders ders)
        {
            using (var ctx = new OkulDbContext())
            {
                ctx.Dersler.Remove(ders);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
