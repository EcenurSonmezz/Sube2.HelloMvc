using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sube2.HelloMvc.Models;

namespace Sube2.HelloMvc.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            using (var ctx = new OkulDbContext())
            {
                var lst = ctx.Ogrenciler.ToList();
                return View(lst);
            }

        }
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Ogrenci ogr)
        {
            if (ogr != null)
            {
                using (var ctx = new OkulDbContext())
                {
                    ctx.Ogrenciler.Add(ogr);
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Index");//burada index actionına gönderme yapıyoruz ki kaydete basınca yeni kayıtla beraber listeyi göstersin
        }

        public IActionResult EditStudent(int id)
        {
            using (var ctx = new OkulDbContext())
            {
                var ogr = ctx.Ogrenciler.Find(id);
                return View(ogr);
            }

        }
        [HttpPost]
        public IActionResult EditStudent(Ogrenci ogr)
        {
            if (ogr != null)
            {
                using (var ctx = new OkulDbContext())
                {
                    ctx.Entry(ogr).State = EntityState.Modified;
                    ctx.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }
        public IActionResult DeleteStudent(int id)
        {
            using (var ctx = new OkulDbContext())
            {
                ctx.Ogrenciler.Remove(ctx.Ogrenciler.Find(id));
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Detay(int id)
        {
            using (var ctx = new OkulDbContext())
            {
                var ogrenci = ctx.Ogrenciler
                    .Include(o => o.OgrenciDersler)
                    .ThenInclude(od => od.Ders)
                    .FirstOrDefault(o => o.OgrenciId == id);

                if (ogrenci == null)
                {
                    return NotFound();
                }

                var mevcutDersler = ctx.Dersler.ToList();

                var model = new OgrenciDetayViewModel
                {
                    Ogrenci = ogrenci,
                    MevcutDersler = mevcutDersler
                };

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult EkleDers(int ogrenciId, string dersIds)
        {
            var dersIdArray = dersIds.Split(',').Select(int.Parse).ToArray();
            using (var ctx = new OkulDbContext())
            {
                var ogrenci = ctx.Ogrenciler
                    .Include(o => o.OgrenciDersler)
                    .FirstOrDefault(o => o.OgrenciId == ogrenciId);

                if (ogrenci != null)
                {
                    // Önce mevcut dersleri temizle
                    var mevcutDersler = ctx.OgrenciDersler.Where(od => od.OgrenciId == ogrenciId).ToList();
                    ctx.OgrenciDersler.RemoveRange(mevcutDersler);

                    // Yeni dersleri ekle
                    foreach (var dersId in dersIdArray)
                    {
                        ogrenci.OgrenciDersler.Add(new OgrenciDers
                        {
                            OgrenciId = ogrenciId,
                            DersId = dersId
                        });
                    }

                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Detay", new { id = ogrenciId });
        }
    }
}