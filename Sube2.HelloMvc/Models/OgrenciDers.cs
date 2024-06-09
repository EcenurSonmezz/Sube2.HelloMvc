namespace Sube2.HelloMvc.Models
{
    public class OgrenciDers
    {
        public int OgrenciId { get; set; }
        public virtual Ogrenci Ogrenci { get; set; }

        public int DersId { get; set; }
        public virtual Ders Ders { get; set; }
    }
}
