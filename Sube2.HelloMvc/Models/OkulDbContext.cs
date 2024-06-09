using Microsoft.EntityFrameworkCore;

namespace Sube2.HelloMvc.Models
{
    public class OkulDbContext : DbContext
    {
        public DbSet<Ogrenci> Ogrenciler { get; set; }
        public DbSet<Ders> Dersler { get; set; }
        public DbSet<OgrenciDers> OgrenciDersler { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=ECENURSONMEZ;Integrated Security=true;Initial Catalog=OkulDBEF2;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ogrenci>().ToTable("tblOgrenciler");
            modelBuilder.Entity<Ogrenci>().Property(o => o.Ad).HasColumnType("varchar").HasMaxLength(40).IsRequired();
            modelBuilder.Entity<Ogrenci>().Property(o => o.Soyad).HasColumnType("varchar").HasMaxLength(40).IsRequired();

            modelBuilder.Entity<Ders>().ToTable("tblDersler");
            modelBuilder.Entity<Ders>().Property(d => d.DersAdi).HasColumnType("varchar").HasMaxLength(100).IsRequired();

            modelBuilder.Entity<OgrenciDers>().ToTable("OgrenciDersler");
            modelBuilder.Entity<OgrenciDers>()
                .HasKey(od => new { od.OgrenciId, od.DersId });

            modelBuilder.Entity<OgrenciDers>()
                .HasOne(od => od.Ogrenci)
                .WithMany(o => o.OgrenciDersler)
                .HasForeignKey(od => od.OgrenciId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OgrenciDers>()
                .HasOne(od => od.Ders)
                .WithMany(d => d.OgrenciDersler)
                .HasForeignKey(od => od.DersId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}