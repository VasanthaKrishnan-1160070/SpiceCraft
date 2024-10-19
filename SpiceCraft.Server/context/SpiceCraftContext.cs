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
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B0A345EA609");

            entity.HasIndex(e => new { e.CartId, e.ItemId }, "UQ__CartItem__F69B3F8E541C56E6").IsUnique();

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PriceAtAdd).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__CartI__69C6B1F5");

            entity.HasOne(d => d.Item).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__ItemI__6ABAD62E");
        });

        modelBuilder.Entity<CorporateClient>(entity =>
        {
            entity.HasKey(e => e.CorporateId).HasName("PK__Corporat__87E40386866A20F9");

            entity.HasIndex(e => e.UserId, "UQ__Corporat__1788CC4DF34B8C1E").IsUnique();

            entity.HasIndex(e => e.CompanyName, "UQ__Corporat__9BCE05DC67181DDE").IsUnique();

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
            entity.HasKey(e => e.CustomerGiftCardId).HasName("PK__Customer__F382824D2F831AD4");

            entity.ToTable("CustomerGiftCard");

            entity.HasOne(d => d.GiftCard).WithMany(p => p.CustomerGiftCards)
                .HasForeignKey(d => d.GiftCardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerG__GiftC__408F9238");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerGiftCards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerG__UserI__3F9B6DFF");
        });

        modelBuilder.Entity<CustomerReward>(entity =>
        {
            entity.HasKey(e => e.CustomerRewardsId).HasName("PK__Customer__2E79ECCE56CCE80B");

            entity.HasOne(d => d.Reward).WithMany(p => p.CustomerRewards)
                .HasForeignKey(d => d.RewardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerR__Rewar__1A69E950");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerRewards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerR__UserI__1B5E0D89");
        });

        modelBuilder.Entity<Enquiry>(entity =>
        {
            entity.HasKey(e => e.EnquiryId).HasName("PK__Enquiry__0A019B7DC8DBEBDB");

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
                .HasConstraintName("FK__Enquiry__Enquiry__26CFC035");
        });

        modelBuilder.Entity<EnquiryType>(entity =>
        {
            entity.HasKey(e => e.EnquiryTypeId).HasName("PK__EnquiryT__BE8CFEFCE5895974");

            entity.ToTable("EnquiryType");

            entity.Property(e => e.EnquiryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GiftCard>(entity =>
        {
            entity.HasKey(e => e.GiftCardId).HasName("PK__GiftCard__9FBB0CC1BF23605F");

            entity.HasIndex(e => e.Code, "UQ__GiftCard__A25C5AA756864278").IsUnique();

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
            entity.HasKey(e => e.IngredientId).HasName("PK__Ingredie__BEAEB25A9211530F");

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
            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6B3BAF004B0");

            entity.ToTable("Inventory");

            entity.HasIndex(e => e.IngredientId, "UQ__Inventor__BEAEB25B9B3E1EA5").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Ingredient).WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Ingre__7FB5F314");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__D796AAB5F9FD0056");

            entity.HasIndex(e => e.OrderId, "UQ__Invoices__C3905BCE02E92496").IsUnique();

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__OrderI__2022C2A6");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Items__727E838B63463431");

            entity.HasIndex(e => new { e.CategoryId, e.ItemName }, "UQ__Items__6DED0D35A6781C8F").IsUnique();

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
                .HasConstraintName("FK__Items__CategoryI__4C364F0E");
        });

        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ItemCate__19093A0B3EFC4C64");

            entity.HasIndex(e => e.CategoryName, "UQ__ItemCate__8517B2E04C0A42C4").IsUnique();

            entity.Property(e => e.CategoryName)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK__ItemCateg__Paren__42ACE4D4");
        });

        modelBuilder.Entity<ItemImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ItemImag__7516F70C1950E19F");

            entity.HasIndex(e => e.ImageCode, "UQ__ItemImag__A7875E722ADE5D48").IsUnique();

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
                .HasConstraintName("FK__ItemImage__ItemI__45544755");
        });

        modelBuilder.Entity<ItemIngredient>(entity =>
        {
            entity.HasKey(e => e.ItemIngredientId).HasName("PK__ItemIngr__D8B0DB39239E64C0");

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
                .HasConstraintName("FK__ItemIngre__Ingre__79FD19BE");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemIngredients)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemIngre__ItemI__7908F585");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9CE1D560F7");

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
                .HasConstraintName("FK__Messages__Enquir__2E70E1FD");

            entity.HasOne(d => d.ReceiverUser).WithMany(p => p.MessageReceiverUsers)
                .HasForeignKey(d => d.ReceiverUserId)
                .HasConstraintName("FK__Messages__Receiv__2D7CBDC4");

            entity.HasOne(d => d.SenderUser).WithMany(p => p.MessageSenderUsers)
                .HasForeignKey(d => d.SenderUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Sender__2C88998B");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12694432B5");

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
                .HasConstraintName("FK__Notificat__UserI__51BA1E3A");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF0A5DE977");

            entity.HasIndex(e => new { e.OrderDate, e.UserId }, "UQ__Orders__A7F88E8F7E562384").IsUnique();

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
                .HasConstraintName("FK__Orders__Shipping__55BFB948");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__UserId__54CB950F");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailsId).HasName("PK__OrderDet__9DD74DBD061FEFFE");

            entity.HasIndex(e => new { e.OrderId, e.ItemId }, "UQ__OrderDet__64B7B3F6F993F7C0").IsUnique();

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
                .HasConstraintName("FK__OrderDeta__ItemI__5F492382");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__5E54FF49");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Payments__55433A6B495F7C0C");

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
                .HasConstraintName("FK__Payments__OrderI__361203C5");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__UserId__370627FE");
        });

        modelBuilder.Entity<PaymentSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__PaymentS__9C8A5B49876D8DB8");

            entity.Property(e => e.ScheduleType)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Corporate).WithMany(p => p.PaymentSchedules)
                .HasForeignKey(d => d.CorporateId)
                .HasConstraintName("FK_PaymentSchedules_CorporateClients");
        });

        modelBuilder.Entity<PromotionBulkItem>(entity =>
        {
            entity.HasKey(e => e.PromotionBulkItemId).HasName("PK__Promotio__430AF23522467BFB");

            entity.HasIndex(e => e.ItemId, "UQ__Promotio__727E838A6E06E219").IsUnique();

            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Item).WithOne(p => p.PromotionBulkItem)
                .HasForeignKey<PromotionBulkItem>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__ItemI__14B10FFA");
        });

        modelBuilder.Entity<PromotionCategory>(entity =>
        {
            entity.HasKey(e => e.PromotionCategoryId).HasName("PK__Promotio__0D44A8A956D10738");

            entity.HasIndex(e => e.CategoryId, "UQ__Promotio__19093A0AD928F3FA").IsUnique();

            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Category).WithOne(p => p.PromotionCategory)
                .HasForeignKey<PromotionCategory>(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__Categ__093F5D4E");
        });

        modelBuilder.Entity<PromotionComboItem>(entity =>
        {
            entity.HasKey(e => e.PromotionComboItemsId).HasName("PK__Promotio__DD97A31E4B2794E0");

            entity.HasIndex(e => e.ItemId, "UQ__Promotio__727E838A6C0AFAAE").IsUnique();

            entity.Property(e => e.BuyQuantity).HasDefaultValue(2);
            entity.Property(e => e.ComboName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GetQuantity).HasDefaultValue(1);

            entity.HasOne(d => d.Item).WithOne(p => p.PromotionComboItem)
                .HasForeignKey<PromotionComboItem>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__ItemI__0EF836A4");
        });

        modelBuilder.Entity<PromotionItem>(entity =>
        {
            entity.HasKey(e => e.PromotionItemId).HasName("PK__Promotio__2B1778CCBE20BE20");

            entity.HasIndex(e => e.ItemId, "UQ__Promotio__727E838A57602652").IsUnique();

            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Item).WithOne(p => p.PromotionItem)
                .HasForeignKey<PromotionItem>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__ItemI__047AA831");
        });

        modelBuilder.Entity<RecentlyViewed>(entity =>
        {
            entity.HasKey(e => e.RecentlyViewedId).HasName("PK__Recently__EDEACF377E4D2558");

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
                .HasConstraintName("FK__RecentlyV__ItemI__7132C993");

            entity.HasOne(d => d.User).WithMany(p => p.RecentlyVieweds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecentlyV__Updat__703EA55A");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.RewardId).HasName("PK__Rewards__825015B935D05440");

            entity.Property(e => e.RewardDescription).HasColumnType("text");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A39F200D2");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61606B2BF6CC").IsUnique();

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShippingOption>(entity =>
        {
            entity.HasKey(e => e.ShippingOptionId).HasName("PK__Shipping__642EC60D3FC564BA");

            entity.HasIndex(e => e.ShippingOptionName, "UQ__Shipping__6AB401BA6121BC4C").IsUnique();

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
            entity.HasKey(e => e.CartId).HasName("PK__Shopping__51BCD7B7BE5B692D");

            entity.HasIndex(e => e.UserId, "UQ__Shopping__1788CC4D52204500").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.ShoppingCart)
                .HasForeignKey<ShoppingCart>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShoppingC__UserI__65F62111");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B249D8DCA76B8");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__UserI__567ED357");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C3F6ACB0F");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105348081F677").IsUnique();

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
                .HasConstraintName("FK__Users__RoleId__27F8EE98");
        });

        modelBuilder.Entity<UserActivityLog>(entity =>
        {
            entity.HasKey(e => e.UserActivityLogId).HasName("PK__UserActi__488B24531E8295F6");

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
                .HasConstraintName("FK__UserActiv__UserI__5C37ACAD");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__UserAddr__091C2AFBC3891BAE");

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
                .HasConstraintName("FK__UserAddre__UserI__4A18FC72");
        });

        modelBuilder.Entity<UserItemInteraction>(entity =>
        {
            entity.HasKey(e => e.UserItemInteractionId).HasName("PK__UserItem__3B423F2CAB7B1A99");

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
                .HasConstraintName("FK__UserItemI__UserI__62E4AA3C");
        });

        modelBuilder.Entity<UserItemRating>(entity =>
        {
            entity.HasKey(e => e.UserItemRating1).HasName("PK__UserItem__72C7947594B46BF6");

            entity.ToTable("UserItemRating");

            entity.Property(e => e.UserItemRating1).HasColumnName("UserItemRating");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
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
                .HasConstraintName("FK__UserItemR__ItemI__6A85CC04");

            entity.HasOne(d => d.User).WithMany(p => p.UserItemRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserItemR__UserI__6991A7CB");
        });

        modelBuilder.Entity<UsersCredential>(entity =>
        {
            entity.HasKey(e => e.UserCredentialId).HasName("PK__UsersCre__17C49DA76FA521BB");

            entity.ToTable("UsersCredential");

            entity.HasIndex(e => e.UserId, "UQ__UsersCre__1788CC4DE9CD81BF").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__UsersCre__C9F28456B3E1E4E9").IsUnique();

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
                .HasConstraintName("FK__UsersCred__UserI__2EA5EC27");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
