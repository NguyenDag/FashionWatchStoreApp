using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectPRN.Models;

public partial class ProjectPrnContext : DbContext
{
    public static ProjectPrnContext Ins = new ProjectPrnContext();
    public ProjectPrnContext()
    {
        if (Ins == null) Ins = this;
    }



    public ProjectPrnContext(DbContextOptions<ProjectPrnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountRank> AccountRanks { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DBDefault");
        optionsBuilder.UseSqlServer(connectionString);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__F267253E8F40E60A");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.AccountRankId).HasColumnName("accountRankID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.AccountRank).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.AccountRankId)
                .HasConstraintName("FK__Account__account__3D5E1FD2");
        });

        modelBuilder.Entity<AccountRank>(entity =>
        {
            entity.HasKey(e => e.AccountRankId).HasName("PK__AccountR__010901409DE26627");

            entity.ToTable("AccountRank");

            entity.Property(e => e.AccountRankId).HasColumnName("accountRankID");
            entity.Property(e => e.AccountRankName)
                .HasMaxLength(100)
                .HasColumnName("accountRankName");
            entity.Property(e => e.MinAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("minAmount");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__06B772B9D0451D4A");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandId).HasColumnName("brandID");
            entity.Property(e => e.BrandAddress)
                .HasMaxLength(255)
                .HasColumnName("brandAddress");
            entity.Property(e => e.BrandEmail)
                .HasMaxLength(255)
                .HasColumnName("brandEmail");
            entity.Property(e => e.BrandName)
                .HasMaxLength(255)
                .HasColumnName("brandName");
            entity.Property(e => e.BrandPhone)
                .HasMaxLength(20)
                .HasColumnName("brandPhone");
            entity.Property(e => e.BrandStatus)
                .HasDefaultValue(true)
                .HasColumnName("brandStatus");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__415B03D897FCF804");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("cartID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Account).WithMany(p => p.Carts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__accountID__46E78A0C");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__productID__47DBAE45");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1F868C8466C");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CategoryDescription).HasColumnName("categoryDescription");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__Discount__D2130A06259B73F3");

            entity.ToTable("Discount");

            entity.Property(e => e.DiscountId).HasColumnName("discountID");
            entity.Property(e => e.AccountRankId).HasColumnName("accountRankID");
            entity.Property(e => e.DiscountName)
                .HasMaxLength(100)
                .HasColumnName("discountName");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.MinCost)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("minCost");
            entity.Property(e => e.Percent).HasColumnName("percent");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__0809337D3EB956AE");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.ActualCost)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("actualCost");
            entity.Property(e => e.DiscountId).HasColumnName("discountID");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("orderDate");
            entity.Property(e => e.PaymentType).HasColumnName("paymentType");
            entity.Property(e => e.ReceiveAddress).HasColumnName("receiveAddress");
            entity.Property(e => e.ReceiveDate)
                .HasColumnType("datetime")
                .HasColumnName("receiveDate");
            entity.Property(e => e.ReceiveName)
                .HasMaxLength(100)
                .HasColumnName("receiveName");
            entity.Property(e => e.ReceivePhone)
                .HasMaxLength(20)
                .HasColumnName("receivePhone");
            entity.Property(e => e.ShipDate)
                .HasColumnType("datetime")
                .HasColumnName("shipDate");
            entity.Property(e => e.ShipId).HasColumnName("shipID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalCost)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("totalCost");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Order__accountID__4F7CD00D");

            entity.HasOne(d => d.Discount).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK__Order__discountI__5165187F");

            entity.HasOne(d => d.Ship).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipId)
                .HasConstraintName("FK__Order__shipID__5070F446");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE2A04DA1D94");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__5441852A");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__produ__5535A963");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__2D10D14AB1DE611D");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.BrandId).HasColumnName("brandID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.PriceBuy)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("priceBuy");
            entity.Property(e => e.PriceSell)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("priceSell");
            entity.Property(e => e.ProductDescription).HasColumnName("productDescription");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("productName");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasDefaultValue(1)
                .HasColumnName("status");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__brandID__440B1D61");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__categor__4316F928");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ProductI__336E9B755CCCA3BE");

            entity.Property(e => e.ImageId).HasColumnName("imageID");
            entity.Property(e => e.ImageUrl).HasColumnName("imageURL");
            entity.Property(e => e.IsMain).HasColumnName("isMain");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductIm__produ__59063A47");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__2ECD6E24E8A7A5FC");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("reviewID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("reviewDate");
            entity.Property(e => e.ReviewRating).HasColumnName("reviewRating");
            entity.Property(e => e.ReviewText).HasColumnName("reviewText");

            entity.HasOne(d => d.Account).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Review__accountI__5DCAEF64");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Review__productI__5CD6CB2B");
        });

        modelBuilder.Entity<Shipping>(entity =>
        {
            entity.HasKey(e => e.ShipId).HasName("PK__Shipping__CC873B251108B3AC");

            entity.ToTable("Shipping");

            entity.Property(e => e.ShipId).HasColumnName("shipID");
            entity.Property(e => e.ShipCost)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("shipCost");
            entity.Property(e => e.ShipMethod)
                .HasMaxLength(255)
                .HasColumnName("shipMethod");
            entity.Property(e => e.ShipTime).HasColumnName("shipTime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
