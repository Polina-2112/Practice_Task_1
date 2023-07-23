using Microsoft.EntityFrameworkCore;

namespace RestfullWeb.Model;

public partial class Context : DbContext
{
    public Context() { }

    public Context(DbContextOptions<Context> options) : base(options) { }

    public virtual DbSet<Address_in_Location> Address_in_Locations { get; set; }

    public virtual DbSet<Catalog> Catalog { get; set; }

    public virtual DbSet<Location> Location { get; set; }

    public virtual DbSet<Company> Company { get; set; }

    public virtual DbSet<Price_by_Location> Price_by_Location { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address_in_Location>(entity =>
        {
            entity.HasNoKey().ToTable("address_in_location");

            entity.Property(e => e.FiasHouseCode).HasMaxLength(32).HasColumnName("fias_house_code");
            entity.Property(e => e.LocationId).HasColumnName("location_id");

            entity
                .HasOne(d => d.Location)
                .WithMany()
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("address_in_location_location_id_fkey");
        });

        modelBuilder.Entity<Catalog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("catalog_position_pkey");

            entity.ToTable("catalog_position");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.MaxOrder).HasColumnName("max_order");
            entity.Property(e => e.MinOrder).HasColumnName("min_order");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
            entity
                .Property(e => e.measurement)
                .HasMaxLength(255)
                .HasColumnName("measurement");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("location_pkey");

            entity.ToTable("location");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("management_company_pkey");

            entity.ToTable("company");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
        });

        modelBuilder.Entity<Price_by_Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("price_by_location_pkey");

            entity.ToTable("price_by_location");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CatalogId).HasColumnName("catalog_id");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Price_on_request).HasColumnName("price_on_request");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
