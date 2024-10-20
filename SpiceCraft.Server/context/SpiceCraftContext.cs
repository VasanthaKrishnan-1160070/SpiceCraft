using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.Context;

public partial class SpiceCraftContext : DbContext
{
    public SpiceCraftContext()
    {
    }

    public SpiceCraftContext(DbContextOptions<SpiceCraftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<CorporateClient> CorporateClients { get; set; }

    public virtual DbSet<CustomerGiftCard> CustomerGiftCards { get; set; }

    public virtual DbSet<CustomerReward> CustomerRewards { get; set; }

    public virtual DbSet<Enquiry> Enquiries { get; set; }

    public virtual DbSet<EnquiryType> EnquiryTypes { get; set; }

    public virtual DbSet<GiftCard> GiftCards { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemCategory> ItemCategories { get; set; }

    public virtual DbSet<ItemImage> ItemImages { get; set; }

    public virtual DbSet<ItemIngredient> ItemIngredients { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentSchedule> PaymentSchedules { get; set; }

    public virtual DbSet<PromotionBulkItem> PromotionBulkItems { get; set; }

    public virtual DbSet<PromotionCategory> PromotionCategories { get; set; }

    public virtual DbSet<PromotionComboItem> PromotionComboItems { get; set; }

    public virtual DbSet<PromotionItem> PromotionItems { get; set; }

    public virtual DbSet<RecentlyViewed> RecentlyVieweds { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ShippingOption> ShippingOptions { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserActivityLog> UserActivityLogs { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UserItemInteraction> UserItemInteractions { get; set; }

    public virtual DbSet<UserItemRating> UserItemRatings { get; set; }

    public virtual DbSet<UsersCredential> UsersCredentials { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B0A7390D549");

            entity.HasIndex(e => new { e.CartId, e.ItemId }, "UQ__CartItem__F69B3F8E47E83C3C").IsUnique();

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PriceAtAdd).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__CartI__24334AAC");

            entity.HasOne(d => d.Item).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__ItemI__25276EE5");
        });

        modelBuilder.Entity<CorporateClient>(entity =>
        {
            entity.HasKey(e => e.CorporateId).HasName("PK__Corporat__87E40386378A205F");

            entity.HasIndex(e => e.UserId, "UQ__Corporat__1788CC4D91426301").IsUnique();

            entity.HasIndex(e => e.CompanyName, "UQ__Corporat__9BCE05DC432A65C0").IsUnique();

            entity.Property(e => e.Approved).HasDefaultValue(false);
            entity.Property(e => e.CompanyDescription).IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreditLimit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreditUsed)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DiscountRate)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PaymentOption)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("pay_now");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.CorporateClient)
                .HasForeignKey<CorporateClient>(d => d.UserId)
                .HasConstraintName("FK_CorporateClients_Users");
        });

        modelBuilder.Entity<CustomerGiftCard>(entity =>
        {
            entity.HasKey(e => e.CustomerGiftCardId).HasName("PK__Customer__F382824DEEC22F5B");

            entity.ToTable("CustomerGiftCard");

            entity.HasOne(d => d.GiftCard).WithMany(p => p.CustomerGiftCards)
                .HasForeignKey(d => d.GiftCardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerG__GiftC__7AFC2AEF");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerGiftCards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerG__UserI__7A0806B6");
        });

        modelBuilder.Entity<CustomerReward>(entity =>
        {
            entity.HasKey(e => e.CustomerRewardsId).HasName("PK__Customer__2E79ECCE7C7438AB");

            entity.HasOne(d => d.Reward).WithMany(p => p.CustomerRewards)
                .HasForeignKey(d => d.RewardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerR__Rewar__54D68207");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerRewards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerR__UserI__55CAA640");
        });

        modelBuilder.Entity<Enquiry>(entity =>
        {
            entity.HasKey(e => e.EnquiryId).HasName("PK__Enquiry__0A019B7DC0A5528D");

            entity.ToTable("Enquiry");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.EnquiryType).WithMany(p => p.Enquiries)
                .HasForeignKey(d => d.EnquiryTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Enquiry__Enquiry__613C58EC");
        });

        modelBuilder.Entity<EnquiryType>(entity =>
        {
            entity.HasKey(e => e.EnquiryTypeId).HasName("PK__EnquiryT__BE8CFEFC0A8E05C8");

            entity.ToTable("EnquiryType");

            entity.Property(e => e.EnquiryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GiftCard>(entity =>
        {
            entity.HasKey(e => e.GiftCardId).HasName("PK__GiftCard__9FBB0CC1C09C9839");

            entity.HasIndex(e => e.Code, "UQ__GiftCard__A25C5AA759BBA392").IsUnique();

            entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("PK__Ingredie__BEAEB25AC6627E1D");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IngredientName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ItemsPerUnit).HasDefaultValue(1);
            entity.Property(e => e.Unit)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("Individual");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6B3D0724B79");

            entity.ToTable("Inventory");

            entity.HasIndex(e => e.IngredientId, "UQ__Inventor__BEAEB25BABC10ACC").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Ingredient).WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Ingre__3A228BCB");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__D796AAB527009680");

            entity.HasIndex(e => e.OrderId, "UQ__Invoices__C3905BCEE162F1BD").IsUnique();

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__OrderI__5A8F5B5D");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Items__727E838B0436D35E");

            entity.HasIndex(e => new { e.CategoryId, e.ItemName }, "UQ__Items__6DED0D357C4EE3A7").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Discount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.OwnProduct).HasDefaultValue(true);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Items)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Items__CategoryI__06A2E7C5");
        });

        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ItemCate__19093A0B8C04EDB5");

            entity.HasIndex(e => e.CategoryName, "UQ__ItemCate__8517B2E0B86BA3D6").IsUnique();

            entity.Property(e => e.CategoryName)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK__ItemCateg__Paren__7D197D8B");
        });

        modelBuilder.Entity<ItemImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ItemImag__7516F70CB104D621");

            entity.HasIndex(e => e.ImageCode, "UQ__ItemImag__A7875E728684AD3F").IsUnique();

            entity.Property(e => e.ImageCode)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ImageName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.IsMain).HasDefaultValue(false);

            entity.HasOne(d => d.Item).WithMany(p => p.ItemImages)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemImage__ItemI__7FC0E00C");
        });

        modelBuilder.Entity<ItemIngredient>(entity =>
        {
            entity.HasKey(e => e.ItemIngredientId).HasName("PK__ItemIngr__D8B0DB39C155D2C0");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.QuantityNeeded).HasDefaultValue(1);
            entity.Property(e => e.Size)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("Medium");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.ItemIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemIngre__Ingre__3469B275");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemIngredients)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemIngre__ItemI__33758E3C");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9C91CF3C72");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MessageContent).HasColumnType("text");
            entity.Property(e => e.Subject)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Enquiry).WithMany(p => p.Messages)
                .HasForeignKey(d => d.EnquiryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Enquir__68DD7AB4");

            entity.HasOne(d => d.ReceiverUser).WithMany(p => p.MessageReceiverUsers)
                .HasForeignKey(d => d.ReceiverUserId)
                .HasConstraintName("FK__Messages__Receiv__67E9567B");

            entity.HasOne(d => d.SenderUser).WithMany(p => p.MessageSenderUsers)
                .HasForeignKey(d => d.SenderUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Sender__66F53242");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E120914B3C6");

            entity.ToTable("Notification");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.NotificationType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__0C26B6F1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF5C92A480");

            entity.HasIndex(e => new { e.OrderDate, e.UserId }, "UQ__Orders__A7F88E8F16B82C6B").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Prepared");
            entity.Property(e => e.Preference)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ShippingOption).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShippingOptionId)
                .HasConstraintName("FK__Orders__Shipping__102C51FF");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__UserId__0F382DC6");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailsId).HasName("PK__OrderDet__9DD74DBD460DD73C");

            entity.HasIndex(e => new { e.OrderId, e.ItemId }, "UQ__OrderDet__64B7B3F626BC0763").IsUnique();

            entity.Property(e => e.ActualPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DiscountRate).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Size)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("Medium")
                .HasColumnName("size");
            entity.Property(e => e.SpiceLevel)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("Medium");

            entity.HasOne(d => d.Item).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__ItemI__19B5BC39");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__18C19800");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Payments__55433A6B1C242A64");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("credit card");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__OrderI__707E9C7C");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__UserId__7172C0B5");
        });

        modelBuilder.Entity<PaymentSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__PaymentS__9C8A5B49BF5669A6");

            entity.Property(e => e.ScheduleType)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Corporate).WithMany(p => p.PaymentSchedules)
                .HasForeignKey(d => d.CorporateId)
                .HasConstraintName("FK_PaymentSchedules_CorporateClients");
        });

        modelBuilder.Entity<PromotionBulkItem>(entity =>
        {
            entity.HasKey(e => e.PromotionBulkItemId).HasName("PK__Promotio__430AF235625A2AC9");

            entity.HasIndex(e => e.ItemId, "UQ__Promotio__727E838ABFAA44F4").IsUnique();

            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Item).WithOne(p => p.PromotionBulkItem)
                .HasForeignKey<PromotionBulkItem>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__ItemI__4F1DA8B1");
        });

        modelBuilder.Entity<PromotionCategory>(entity =>
        {
            entity.HasKey(e => e.PromotionCategoryId).HasName("PK__Promotio__0D44A8A9704E068B");

            entity.HasIndex(e => e.CategoryId, "UQ__Promotio__19093A0A8A535E12").IsUnique();

            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Category).WithOne(p => p.PromotionCategory)
                .HasForeignKey<PromotionCategory>(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__Categ__43ABF605");
        });

        modelBuilder.Entity<PromotionComboItem>(entity =>
        {
            entity.HasKey(e => e.PromotionComboItemsId).HasName("PK__Promotio__DD97A31E3C8B494B");

            entity.HasIndex(e => e.ItemId, "UQ__Promotio__727E838A31F97753").IsUnique();

            entity.Property(e => e.BuyQuantity).HasDefaultValue(2);
            entity.Property(e => e.ComboName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GetQuantity).HasDefaultValue(1);

            entity.HasOne(d => d.Item).WithOne(p => p.PromotionComboItem)
                .HasForeignKey<PromotionComboItem>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__ItemI__4964CF5B");
        });

        modelBuilder.Entity<PromotionItem>(entity =>
        {
            entity.HasKey(e => e.PromotionItemId).HasName("PK__Promotio__2B1778CCC106C1F9");

            entity.HasIndex(e => e.ItemId, "UQ__Promotio__727E838A566E2751").IsUnique();

            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Item).WithOne(p => p.PromotionItem)
                .HasForeignKey<PromotionItem>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__ItemI__3EE740E8");
        });

        modelBuilder.Entity<RecentlyViewed>(entity =>
        {
            entity.HasKey(e => e.RecentlyViewedId).HasName("PK__Recently__EDEACF37586021C1");

            entity.ToTable("RecentlyViewed");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ViewCount).HasDefaultValue(1);

            entity.HasOne(d => d.Item).WithMany(p => p.RecentlyVieweds)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecentlyV__ItemI__2C938683");

            entity.HasOne(d => d.User).WithMany(p => p.RecentlyVieweds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecentlyV__Updat__2B9F624A");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.RewardId).HasName("PK__Rewards__825015B934A59104");

            entity.Property(e => e.RewardDescription).HasColumnType("text");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A6379D65F");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160F848571B").IsUnique();

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShippingOption>(entity =>
        {
            entity.HasKey(e => e.ShippingOptionId).HasName("PK__Shipping__642EC60D33276F99");

            entity.HasIndex(e => e.ShippingOptionName, "UQ__Shipping__6AB401BAC2FDDC82").IsUnique();

            entity.Property(e => e.Cost)
                .HasDefaultValue(5.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FreeShippingThreshold).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ShippingOptionName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Shopping__51BCD7B7348C5420");

            entity.HasIndex(e => e.UserId, "UQ__Shopping__1788CC4DF9413F84").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.ShoppingCart)
                .HasForeignKey<ShoppingCart>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShoppingC__UserI__2062B9C8");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B249D8F4EDA06");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__UserI__10EB6C0E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CF9098379");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534F927E87C").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ProfileImg)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasDefaultValue("Mr.");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__6265874F");
        });

        modelBuilder.Entity<UserActivityLog>(entity =>
        {
            entity.HasKey(e => e.UserActivityLogId).HasName("PK__UserActi__488B245347E68D53");

            entity.ToTable("UserActivityLog");

            entity.Property(e => e.ClickCount).HasDefaultValue(1);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NavigationItem)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Routing)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SessionId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserActivityLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserActiv__UserI__16A44564");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__UserAddr__091C2AFB59887F65");

            entity.Property(e => e.AddressType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("shipping");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StateOrProvince)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StreetAddress1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StreetAddress2)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAddre__UserI__04859529");
        });

        modelBuilder.Entity<UserItemInteraction>(entity =>
        {
            entity.HasKey(e => e.UserItemInteractionId).HasName("PK__UserItem__3B423F2CD5EFA5E7");

            entity.ToTable("UserItemInteraction");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Interaction).HasDefaultValue(0);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserItemInteractions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserItemI__UserI__1D5142F3");
        });

        modelBuilder.Entity<UserItemRating>(entity =>
        {
            entity.HasKey(e => e.UserItemRating1).HasName("PK__UserItem__72C794756332B2A5");

            entity.ToTable("UserItemRating");

            entity.Property(e => e.UserItemRating1).HasColumnName("UserItemRating");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImprovementDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsNegativeReview).HasDefaultValue(false);
            entity.Property(e => e.Rating).HasDefaultValue(1);
            entity.Property(e => e.RatingDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.UserItemRatings)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserItemR__ItemI__25E688F4");

            entity.HasOne(d => d.User).WithMany(p => p.UserItemRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserItemR__UserI__24F264BB");
        });

        modelBuilder.Entity<UsersCredential>(entity =>
        {
            entity.HasKey(e => e.UserCredentialId).HasName("PK__UsersCre__17C49DA7ACF5D805");

            entity.ToTable("UsersCredential");

            entity.HasIndex(e => e.UserId, "UQ__UsersCre__1788CC4D49DC17B8").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__UsersCre__C9F2845696670D40").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithOne(p => p.UsersCredential)
                .HasForeignKey<UsersCredential>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsersCred__UserI__691284DE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
