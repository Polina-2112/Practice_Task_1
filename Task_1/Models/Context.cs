using Microsoft.EntityFrameworkCore;

namespace RestfullWeb.Model;

public partial class Context : DbContext
{
    public Context() { }

    public Context(DbContextOptions<Context> options) : base(options) { }

    public virtual DbSet<Address_in_Location> address_in_locations { get; set; }

    public virtual DbSet<Catalog> catalog { get; set; }

    public virtual DbSet<Location> cocation { get; set; }

    public virtual DbSet<Company> company { get; set; }

    public virtual DbSet<Price_by_Location> price_by_location { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address_in_Location>(entity =>
        {
            entity.HasNoKey().ToTable("address_in_location");

            entity.Property(e => e.fias_house_code).HasMaxLength(32).HasColumnName("fias_house_code");
            entity.Property(e => e.location_id).HasColumnName("location_id");

            entity
                .HasOne(d => d.location)
                .WithMany()
                .HasForeignKey(d => d.location_id)
                .HasConstraintName("address_in_location_location_id_fkey");
        });

        modelBuilder.Entity<Catalog>(entity =>
        {
            entity.HasKey(e => e.id).HasName("catalog_position_pkey");

            entity.ToTable("catalog");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.company_id).HasColumnName("company_id");
            entity.Property(e => e.max_order).HasColumnName("max_order");
            entity.Property(e => e.min_order).HasColumnName("min_order");
            entity.Property(e => e.name).HasMaxLength(255).HasColumnName("name");
            entity
                .Property(e => e.measurement)
                .HasMaxLength(255)
                .HasColumnName("measurement");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.id).HasName("location_pkey");

            entity.ToTable("location");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.company_id).HasColumnName("company_id");
            entity.Property(e => e.name).HasMaxLength(255).HasColumnName("name");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.id).HasName("management_company_pkey");

            entity.ToTable("company");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.name).HasMaxLength(255).HasColumnName("name");
        });

        modelBuilder.Entity<Price_by_Location>(entity =>
        {
            entity.HasKey(e => e.id).HasName("price_by_location_pkey");

            entity.ToTable("price_by_location");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.catalog_id).HasColumnName("catalog_id");
            entity.Property(e => e.location_id).HasColumnName("location_id");
            entity.Property(e => e.price).HasColumnName("price");
            entity.Property(e => e.price_on_request).HasColumnName("price_on_request");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
