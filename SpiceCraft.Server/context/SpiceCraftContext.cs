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

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemCategory> ItemCategories { get; set; }

    public virtual DbSet<ItemImage> ItemImages { get; set; }

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

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ShippingOption> ShippingOptions { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserActivityLog> UserActivityLogs { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UsersCredential> UsersCredentials { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B0A7C011442");

            entity.HasIndex(e => new { e.CartId, e.ItemId }, "UQ__CartItem__F69B3F8EEE1A8CA9").IsUnique();

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PriceAtAdd).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__CartI__05D8E0BE");

            entity.HasOne(d => d.Item).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__ItemI__06CD04F7");
        });

        modelBuilder.Entity<CorporateClient>(entity =>
        {
            entity.HasKey(e => e.CorporateId).HasName("PK__Corporat__87E40386E0BCDBAD");

            entity.HasIndex(e => e.UserId, "UQ__Corporat__1788CC4D82339A83").IsUnique();

            entity.HasIndex(e => e.CompanyName, "UQ__Corporat__9BCE05DC0897CF9B").IsUnique();

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
            entity.HasKey(e => e.CustomerGiftCardId).HasName("PK__Customer__F382824D474D9586");

            entity.ToTable("CustomerGiftCard");

            entity.HasOne(d => d.GiftCard).WithMany(p => p.CustomerGiftCards)
                .HasForeignKey(d => d.GiftCardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerG__GiftC__4D5F7D71");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerGiftCards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerG__UserI__4C6B5938");
        });

        modelBuilder.Entity<CustomerReward>(entity =>
        {
            entity.HasKey(e => e.CustomerRewardsId).HasName("PK__Customer__2E79ECCEE9DB0CF0");

            entity.HasOne(d => d.Reward).WithMany(p => p.CustomerRewards)
                .HasForeignKey(d => d.RewardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerR__Rewar__2739D489");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerRewards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerR__UserI__282DF8C2");
        });

        modelBuilder.Entity<Enquiry>(entity =>
        {
            entity.HasKey(e => e.EnquiryId).HasName("PK__Enquiry__0A019B7D3C2E50E1");

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
                .HasConstraintName("FK__Enquiry__Enquiry__339FAB6E");
        });

        modelBuilder.Entity<EnquiryType>(entity =>
        {
            entity.HasKey(e => e.EnquiryTypeId).HasName("PK__EnquiryT__BE8CFEFCC779702D");

            entity.ToTable("EnquiryType");

            entity.Property(e => e.EnquiryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GiftCard>(entity =>
        {
            entity.HasKey(e => e.GiftCardId).HasName("PK__GiftCard__9FBB0CC1D2568D35");

            entity.HasIndex(e => e.Code, "UQ__GiftCard__A25C5AA7AA139D8C").IsUnique();

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

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6B36C92ED3A");

            entity.ToTable("Inventory");

            entity.HasIndex(e => e.ItemId, "UQ__Inventor__727E838AD03E45C0").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__ItemI__0C85DE4D");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__D796AAB54E5C13D0");

            entity.HasIndex(e => e.OrderId, "UQ__Invoices__C3905BCE7459F945").IsUnique();

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__OrderI__2CF2ADDF");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Items__727E838BC43FEDB6");

            entity.HasIndex(e => new { e.CategoryId, e.ItemName }, "UQ__Items__6DED0D351F214A12").IsUnique();

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
                .HasConstraintName("FK__Items__CategoryI__68487DD7");
        });

        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ItemCate__19093A0BC8EECC57");

            entity.HasIndex(e => e.CategoryName, "UQ__ItemCate__8517B2E028A0246C").IsUnique();

            entity.Property(e => e.CategoryName)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK__ItemCateg__Paren__5EBF139D");
        });

        modelBuilder.Entity<ItemImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ItemImag__7516F70CE8CDE5E2");

            entity.HasIndex(e => e.ImageCode, "UQ__ItemImag__A7875E7203F6D8BC").IsUnique();

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
                .HasConstraintName("FK__ItemImage__ItemI__5224328E");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9C6123ADFA");

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
                .HasConstraintName("FK__Messages__Enquir__3B40CD36");

            entity.HasOne(d => d.ReceiverUser).WithMany(p => p.MessageReceiverUsers)
                .HasForeignKey(d => d.ReceiverUserId)
                .HasConstraintName("FK__Messages__Receiv__3A4CA8FD");

            entity.HasOne(d => d.SenderUser).WithMany(p => p.MessageSenderUsers)
                .HasForeignKey(d => d.SenderUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Sender__395884C4");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E1291FD50E1");

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
                .HasConstraintName("FK__Notificat__UserI__5E8A0973");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF9C1531F9");

            entity.HasIndex(e => new { e.OrderDate, e.UserId }, "UQ__Orders__A7F88E8F30A778F0").IsUnique();

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
                .HasConstraintName("FK__Orders__Shipping__71D1E811");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__UserId__70DDC3D8");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailsId).HasName("PK__OrderDet__9DD74DBDBAAD2B5A");

            entity.HasIndex(e => new { e.OrderId, e.ItemId }, "UQ__OrderDet__64B7B3F6D8454502").IsUnique();

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
                .HasConstraintName("FK__OrderDeta__ItemI__7B5B524B");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__7A672E12");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Payments__55433A6BC2DF27B0");

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
                .HasConstraintName("FK__Payments__OrderI__42E1EEFE");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__UserId__43D61337");
        });

        modelBuilder.Entity<PaymentSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__PaymentS__9C8A5B4910851794");

            entity.Property(e => e.ScheduleType)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Corporate).WithMany(p => p.PaymentSchedules)
                .HasForeignKey(d => d.CorporateId)
                .HasConstraintName("FK_PaymentSchedules_CorporateClients");
        });

        modelBuilder.Entity<PromotionBulkItem>(entity =>
        {
            entity.HasKey(e => e.PromotionBulkItemId).HasName("PK__Promotio__430AF235288229C3");

            entity.HasIndex(e => e.ItemId, "UQ__Promotio__727E838ABB42AD78").IsUnique();

            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Item).WithOne(p => p.PromotionBulkItem)
                .HasForeignKey<PromotionBulkItem>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__ItemI__2180FB33");
        });

        modelBuilder.Entity<PromotionCategory>(entity =>
        {
            entity.HasKey(e => e.PromotionCategoryId).HasName("PK__Promotio__0D44A8A9AA9A670F");

            entity.HasIndex(e => e.CategoryId, "UQ__Promotio__19093A0AE8A7F3A8").IsUnique();

            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Category).WithOne(p => p.PromotionCategory)
                .HasForeignKey<PromotionCategory>(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__Categ__160F4887");
        });

        modelBuilder.Entity<PromotionComboItem>(entity =>
        {
            entity.HasKey(e => e.PromotionComboItemsId).HasName("PK__Promotio__DD97A31EE4936857");

            entity.HasIndex(e => e.ItemId, "UQ__Promotio__727E838A9A04D35B").IsUnique();

            entity.Property(e => e.BuyQuantity).HasDefaultValue(2);
            entity.Property(e => e.ComboName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GetQuantity).HasDefaultValue(1);

            entity.HasOne(d => d.Item).WithOne(p => p.PromotionComboItem)
                .HasForeignKey<PromotionComboItem>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__ItemI__1BC821DD");
        });

        modelBuilder.Entity<PromotionItem>(entity =>
        {
            entity.HasKey(e => e.PromotionItemId).HasName("PK__Promotio__2B1778CCA87817BF");

            entity.HasIndex(e => e.ItemId, "UQ__Promotio__727E838A3A592F3F").IsUnique();

            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Item).WithOne(p => p.PromotionItem)
                .HasForeignKey<PromotionItem>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__ItemI__114A936A");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.RewardId).HasName("PK__Rewards__825015B90CC3E4DB");

            entity.Property(e => e.RewardDescription).HasColumnType("text");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A6E710884");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B616098A82820").IsUnique();

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShippingOption>(entity =>
        {
            entity.HasKey(e => e.ShippingOptionId).HasName("PK__Shipping__642EC60D0CA4125C");

            entity.HasIndex(e => e.ShippingOptionName, "UQ__Shipping__6AB401BAE1B112E7").IsUnique();

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
            entity.HasKey(e => e.CartId).HasName("PK__Shopping__51BCD7B7A5A741C4");

            entity.HasIndex(e => e.UserId, "UQ__Shopping__1788CC4DA4198F3F").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.ShoppingCart)
                .HasForeignKey<ShoppingCart>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShoppingC__UserI__02084FDA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CBC834BDF");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534F1F9BE1E").IsUnique();

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
                .HasConstraintName("FK__Users__RoleId__440B1D61");
        });

        modelBuilder.Entity<UserActivityLog>(entity =>
        {
            entity.HasKey(e => e.UserActivityLogId).HasName("PK__UserActi__488B2453461E2C33");

            entity.ToTable("UserActivityLog");

            entity.Property(e => e.ClickCount).HasDefaultValue(1);
            entity.Property(e => e.NavigationItem)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Routing)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SessionId)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.UserActivityLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserActiv__UserI__625A9A57");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__UserAddr__091C2AFB069676FD");

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
                .HasConstraintName("FK__UserAddre__UserI__56E8E7AB");
        });

        modelBuilder.Entity<UsersCredential>(entity =>
        {
            entity.HasKey(e => e.UserCredentialId).HasName("PK__UsersCre__17C49DA72D11A051");

            entity.ToTable("UsersCredential");

            entity.HasIndex(e => e.UserId, "UQ__UsersCre__1788CC4D28D1A64E").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__UsersCre__C9F2845666E56652").IsUnique();

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
                .HasConstraintName("FK__UsersCred__UserI__4AB81AF0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
