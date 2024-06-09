using System.Collections.Generic;

namespace Sube2.HelloMvc.Models
{
    public class OgrenciDetayViewModel
    {
        public Ogrenci Ogrenci { get; set; }
        public List<Ders> MevcutDersler { get; set; }
    }
}
