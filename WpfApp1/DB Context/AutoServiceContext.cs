using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace WpfApp1
{
    public partial class AutoServiceContext : DbContext
    {
        public AutoServiceContext()
        {
        }

        public AutoServiceContext(DbContextOptions<AutoServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AutoConcern> AutoConcerns { get; set; }
        public virtual DbSet<AutoPart> AutoParts { get; set; }
        public virtual DbSet<AutoPartPrice> AutoPartPrices { get; set; }
        public virtual DbSet<AutoService> AutoServices { get; set; }
        public virtual DbSet<AutoServiceAutoPart> AutoServiceAutoParts { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarBrand> CarBrands { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Compatibility> Compatibilities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = Configurations.Configuration.GetConfiguration();
                optionsBuilder.UseSqlServer(config.Config.GetConnectionString("AutoServiceConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Idworker);

                entity.ToTable("Account");

                entity.Property(e => e.Idworker)
                    .ValueGeneratedNever()
                    .HasColumnName("IDWorker");

                entity.Property(e => e.LoginAccount)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordAccount)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdworkerNavigation)
                    .WithOne(p => p.Account)
                    .HasForeignKey<Account>(d => d.Idworker)
                    .HasConstraintName("FK_WorkerAccount");
            });

            modelBuilder.Entity<AutoConcern>(entity =>
            {
                entity.HasKey(e => e.IdautoConcern)
                    .HasName("PK_IDAutoConcern");

                entity.ToTable("AutoConcern");

                entity.Property(e => e.IdautoConcern).HasColumnName("IDAutoConcern");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.NameAutoConcern)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdcountryNavigation)
                    .WithMany(p => p.AutoConcerns)
                    .HasForeignKey(d => d.Idcountry)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDCountryAutoConcern");
            });

            modelBuilder.Entity<AutoPart>(entity =>
            {
                entity.HasKey(e => e.IdautoPart);

                entity.ToTable("AutoPart");

                entity.Property(e => e.IdautoPart).HasColumnName("IDAutoPart");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.NameAutoPart)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdcountryNavigation)
                    .WithMany(p => p.AutoParts)
                    .HasForeignKey(d => d.Idcountry)
                    .HasConstraintName("FK_IDCountryAutoPart");
            });

            modelBuilder.Entity<AutoPartPrice>(entity =>
            {
                entity.HasKey(e => new { e.IdautoPart, e.DateChange });

                entity.ToTable("AutoPartPrice");

                entity.Property(e => e.IdautoPart).HasColumnName("IDAutoPart");

                entity.Property(e => e.DateChange).HasColumnType("date");

                entity.Property(e => e.PriceWithoutRepair).HasColumnType("money");

                entity.HasOne(d => d.IdautoPartNavigation)
                    .WithMany(p => p.AutoPartPrices)
                    .HasForeignKey(d => d.IdautoPart)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AutoPartPrice");
            });

            modelBuilder.Entity<AutoService>(entity =>
            {
                entity.HasKey(e => e.IdautoService)
                    .HasName("PK_IDAutoService");

                entity.ToTable("AutoService");

                entity.Property(e => e.IdautoService).HasColumnName("IDAutoService");

                entity.Property(e => e.DateAutoService).HasColumnType("date");

                entity.Property(e => e.IdserviceType).HasColumnName("IDServiceType");

                entity.Property(e => e.Idworker).HasColumnName("IDWorker");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.StateNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdserviceTypeNavigation)
                    .WithMany(p => p.AutoServices)
                    .HasForeignKey(d => d.IdserviceType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceTypeAutoService");

                entity.HasOne(d => d.IdworkerNavigation)
                    .WithMany(p => p.AutoServices)
                    .HasForeignKey(d => d.Idworker)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IDWorker");

                entity.HasOne(d => d.StateNumberNavigation)
                    .WithMany(p => p.AutoServices)
                    .HasForeignKey(d => d.StateNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StateNumber");
            });

            modelBuilder.Entity<AutoServiceAutoPart>(entity =>
            {
                entity.HasKey(e => new { e.IdautoService, e.IdautoPart })
                    .HasName("PK_AutoServiceAutoPartIDAutoServiceIDAutoPart");

                entity.ToTable("AutoServiceAutoPart");

                entity.Property(e => e.IdautoService).HasColumnName("IDAutoService");

                entity.Property(e => e.IdautoPart).HasColumnName("IDAutoPart");

                entity.HasOne(d => d.IdautoPartNavigation)
                    .WithMany(p => p.AutoServiceAutoParts)
                    .HasForeignKey(d => d.IdautoPart)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDAutoPartAutoServiceAutoPart");

                entity.HasOne(d => d.IdautoServiceNavigation)
                    .WithMany(p => p.AutoServiceAutoParts)
                    .HasForeignKey(d => d.IdautoService)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AutoServiceAutoPartIDAutoService");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(e => e.StateNumber);

                entity.ToTable("Car");

                entity.Property(e => e.StateNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DataOfRelease).HasColumnType("date");

                entity.Property(e => e.Idclient).HasColumnName("IDClient");

                entity.Property(e => e.Idmodel).HasColumnName("IDModel");

                entity.HasOne(d => d.IdclientNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Idclient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDClientCar");

                entity.HasOne(d => d.IdmodelNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Idmodel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModelCar");
            });

            modelBuilder.Entity<CarBrand>(entity =>
            {
                entity.HasKey(e => e.IdcarBrand)
                    .HasName("PK_IDCarBrand");

                entity.ToTable("CarBrand");

                entity.Property(e => e.IdcarBrand).HasColumnName("IDCarBrand");

                entity.Property(e => e.IdautoConcern).HasColumnName("IDAutoConcern");

                entity.Property(e => e.NameCarBrand)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdautoConcernNavigation)
                    .WithMany(p => p.CarBrands)
                    .HasForeignKey(d => d.IdautoConcern)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IDAutoConcernCarBrand");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Idclient)
                    .HasName("PK_IDClient");

                entity.ToTable("Client");

                entity.Property(e => e.Idclient).HasColumnName("IDClient");

                entity.Property(e => e.NameClient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Compatibility>(entity =>
            {
                entity.HasKey(e => new { e.Idmodel, e.IdautoPart });

                entity.ToTable("Compatibility");

                entity.Property(e => e.Idmodel).HasColumnName("IDModel");

                entity.Property(e => e.IdautoPart).HasColumnName("IDAutoPart");

                entity.HasOne(d => d.IdautoPartNavigation)
                    .WithMany(p => p.Compatibilities)
                    .HasForeignKey(d => d.IdautoPart)
                    .HasConstraintName("FK_IDAutoPartCompatibility");

                entity.HasOne(d => d.IdmodelNavigation)
                    .WithMany(p => p.Compatibilities)
                    .HasForeignKey(d => d.Idmodel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDModelCompatibility");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Idcountry)
                    .HasName("PK_IDCountry");

                entity.ToTable("Country");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.NameCountry)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.HasKey(e => e.Idmodel);

                entity.ToTable("Model");

                entity.Property(e => e.Idmodel).HasColumnName("IDModel");

                entity.Property(e => e.IdcarBrand).HasColumnName("IDCarBrand");

                entity.Property(e => e.NameModel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdcarBrandNavigation)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.IdcarBrand)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CarBrandModel");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.Idposition)
                    .HasName("PK_IDPosition");

                entity.ToTable("Position");

                entity.Property(e => e.Idposition).HasColumnName("IDPosition");

                entity.Property(e => e.NamePosition)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.HasKey(e => e.IdserviceType);

                entity.ToTable("ServiceType");

                entity.Property(e => e.IdserviceType).HasColumnName("IDServiceType");

                entity.Property(e => e.NameServiceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PriceServiceType).HasColumnType("money");
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasKey(e => e.Idworker);

                entity.ToTable("Worker");

                entity.Property(e => e.Idworker).HasColumnName("IDWorker");

                entity.Property(e => e.Idposition).HasColumnName("IDPosition");

                entity.Property(e => e.NameWorker)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdpositionNavigation)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.Idposition)
                    .HasConstraintName("FK_IDPositionWorker");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
