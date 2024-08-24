
-- Check if the database exists and drop it if it does
--if db_id('SpiceCraft') is not null
--begin
--    alter database SpiceCraft set single_user with rollback immediate;
--    drop database SpiceCraft;
--end



-- Create the database
-- create database SpiceCraft;

--go;

-- Use the newly created database
use SpiceCraft;

GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 24/08/2024 9:04:17 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 24/08/2024 9:04:17 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 24/08/2024 9:04:17 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 24/08/2024 9:04:17 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 24/08/2024 9:04:17 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 24/08/2024 9:04:17 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] int identity(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 24/08/2024 9:04:17 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO

create table ShippingOptions (
    ShippingOptionId int primary key identity(1,1),
    ShippingOptionName varchar(100) not null unique,
    Description varchar(100) not null
);

create table ItemCategories (
    CategoryId int primary key identity(1,1),
    CategoryName varchar(300) not null unique,
    ParentCategoryId int,
    foreign key (ParentCategoryId) references ItemCategories(CategoryId)
);

create table Products (
    ItemId int primary key identity(1,1),
    CategoryId int not null,
    ItemName varchar(300) not null,
    OwnProduct bit,
    Discount decimal(10,2) default 0,
    Description text,
    Price decimal(10,2) not null,
    IsRemoved bit default 0,
    CreatedDate date default null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (CategoryId) references ItemCategories(CategoryId),
    unique(CategoryId, ItemName)
);

create table Orders (
    OrderId int primary key identity(1,1),
    UserId int not null,
    OrderDate datetime not null,
    ShippingOptionId int null,
    TotalCost decimal(10,2) not null,
    IsFreeShipping bit default 0,
    OrderStatus varchar(50) check (OrderStatus in ('Prepared', 'Ready To Ship', 'Shipped', 'Ready For Pickup', 'Cancelled', 'Returned')) not null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (UserId) references AspNetUsers(Id),
    foreign key (ShippingOptionId) references ShippingOptions(ShippingOptionId),
    unique(OrderDate, UserId)
);

create table OrderDetails (
    OrderDetailsId int primary key identity(1,1),
    OrderId int not null,
    ItemId int not null,
    ActualPrice decimal(10,2),
    Size varchar(5) check (Size in ('XS', 'S', 'M', 'L', 'XL', 'XXL')) default 'L',
    Color varchar(50) check (Color in ('Black', 'Navy Blue', 'White', 'Gray', 'Beige and Tan', 'Red', 'Burgundy and Maroon', 'Pastels', 'Earthy Tones')) default 'Black',
    DiscountRate decimal(10,2) default 0,
    Quantity int not null,
    PurchasePrice decimal(10,2) not null,
    foreign key (OrderId) references Orders(OrderId),
    foreign key (ItemId) references Products(ItemId),
    unique(OrderId, ItemId)
);

create table ShoppingCarts (
    CartId int primary key identity(1,1),
    UserId int not null unique,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    IsOrdered bit default 0,
    foreign key (UserId) references AspNetUsers(Id)
);

create table CartItems (
    CartItemId int primary key identity(1,1),
    CartId int not null,
    ItemId int not null,
    Quantity int not null,
    Size varchar(5) check (Size in ('XS', 'S', 'M', 'L', 'XL', 'XXL')) default 'L',
    Color varchar(50) check (Color in ('Black', 'Navy Blue', 'White', 'Gray', 'Beige and Tan', 'Red', 'Burgundy and Maroon', 'Pastels', 'Earthy Tones')) default 'Black',
    PriceAtAdd decimal(10, 2),
    foreign key (CartId) references ShoppingCarts(CartId),
    foreign key (ItemId) references Products(ItemId),
    unique(CartId, ItemId)
);

create table Inventory (
    InventoryId int primary key identity(1,1),
    ItemId int not null unique,
    CurrentStock int not null,
    LowStockThreshold int not null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (ItemId) references Products(ItemId)
);

create table PromotionItems (
    PromotionItemId int primary key identity(1,1),
    ItemId int not null,
    DiscountRate decimal(5,2) default 0,
    foreign key (ItemId) references Products(ItemId),
    unique(ItemId)
);

create table PromotionCategories (
    PromotionCategoryId int primary key identity(1,1),
    CategoryId int not null,
    DiscountRate decimal(5,2) default 0,
    foreign key (CategoryId) references ItemCategories(CategoryId),
    unique(CategoryId)
);

create table PromotionComboItems (
    PromotionComboItemsId int primary key identity(1,1),
    ItemId int not null,
    ComboName varchar(100),
    BuyQuantity int not null default 2,
    GetQuantity int not null default 1,
    foreign key (ItemId) references Products(ItemId),
    unique(ItemId)
);

create table PromotionBulkItems (
    PromotionBulkItemId int primary key identity(1,1),
    ItemId int not null,
    RequiredQuantity int not null default 0,
    DiscountRate decimal(5,2) default 0,
    foreign key (ItemId) references Products(ItemId),
    unique(ItemId)
);

create table Rewards (
    RewardId int primary key identity(1,1),
    PointsThreshold int not null,
    RewardDescription text
);

create table CustomerRewards (
    CustomerRewardsId int primary key identity(1,1),
    RewardId int not null,
    UserId int not null,
    IsUsed bit,
    foreign key (RewardId) references Rewards (RewardId),
    foreign key (UserId) references AspNetUsers(Id)
);

create table Invoices (
    InvoiceId int primary key identity(1,1),   
    OrderId int not null unique,
    IssueDate date not null,
    DueDate date not null,
    TotalAmount decimal(10,2) not null,
    Paid bit default 0,    
    foreign key (OrderId) references Orders(OrderId)
);

create table EnquiryType (
    EnquiryTypeId int primary key identity(1,1),
    EnquiryName varchar(100) not null
);

create table Enquiry (
    EnquiryId int primary key identity(1,1),
    EnquiryTypeId int not null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (EnquiryTypeId) references EnquiryType(EnquiryTypeId)
);

create table Messages (
    MessageId int primary key identity(1,1),
    SenderUserId int not null,
    ReceiverUserId int,
    EnquiryId int not null,
    Subject varchar(1000) default null,
    MessageContent text,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (SenderUserId) references AspNetUsers(Id),
    foreign key (ReceiverUserId) references AspNetUsers(Id),
    foreign key (EnquiryId) references Enquiry(EnquiryId)
);

create table Payments (
    TransactionId int primary key identity(1,1),
    UserId int not null,
    OrderId int not null,
    PaymentMethod varchar(50) check (PaymentMethod in ('credit card', 'gift card', 'credit card and gift card', 'client credit', 'client credit and credit card')) default 'credit card',
    Amount decimal(10,2) not null,
    PaymentStatus varchar(50) check (PaymentStatus in ('Pending', 'Completed', 'Failed', 'Not Paid', 'Paid')),
    PaymentDate datetime not null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (OrderId) references Orders(OrderId),
    foreign key (UserId) references AspNetUsers(Id)
);

create table GiftCards (
    GiftCardId int primary key identity(1,1),
    Code varchar(50) unique not null,
    Balance decimal(10,2) not null,
    ExpirationDate date,
    IsActive bit default 1,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate()
);

create table CustomerGiftCard (
    CustomerGiftCardId int primary key identity(1,1),
    UserId int not null,
    GiftCardId int not null,
    foreign key (UserId) references AspNetUsers(Id),
    foreign key (GiftCardId) references GiftCards (GiftCardId)
);

create table ItemImages (
    ImageId int primary key identity(1,1),
    ItemId int not null,
    ImageCode varchar(300) not null unique,
    ImageName varchar(300),
    ImageIndex int,
    IsMain bit default 0,
    foreign key (ItemId) references Products(ItemId)
);

create table UserAddresses (
    AddressId int primary key identity(1,1),
    UserId int not null,
    AddressType varchar(20) check (AddressType in ('billing', 'shipping')) default 'shipping',
    StreetAddress1 varchar(100) not null,
    StreetAddress2 varchar(100),
    City varchar(100) not null,
    StateOrProvince varchar(100),
    PostalCode varchar(100),
	foreign key (UserId) references AspNetUsers(Id)
);

create table Notification (
    NotificationId int primary key identity(1,1),
    NotificationType varchar(50) check (NotificationType in ('Order', 'Message', 'Other')),
    EntityId int not null, -- can be order id, message id or other id
    IsRead bit default 0, -- whether the user read the notification
    Title varchar(300) not null,
    Message varchar(1000) default '',
    UserId int not null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (UserId) references AspNetUsers(Id)
);

