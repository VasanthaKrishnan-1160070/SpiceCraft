
-- Check if the database exists and drop it if it does
--if db_id('SpiceCraft') is not null
--begin
--    alter database SpiceCraft set single_user with rollback immediate;
--    drop database SpiceCraft;
--end



-- Create the database
--create database SpiceCraft;

-- Use the newly created database
use SpiceCraft;

create table Roles (
    RoleId int primary key identity(1,1),
    RoleName varchar(50) not null unique
);

create table ShippingOptions (
    ShippingOptionId int primary key identity(1,1),
    ShippingOptionName varchar(100) not null unique,
    Description varchar(100) not null
);


create table Users (
    UserId int primary key identity(1,1),
    Title varchar(5) check (Title in ('Mr.', 'Ms.', 'Dr.', 'Mr.')),
    FirstName varchar(100),
    LastName varchar(100),
    Email varchar(100) unique,
    Phone varchar(12),
    ProfileImg varchar(200),
    RoleId int not null,
    IsActive bit default 1,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (RoleId) references Roles (RoleId)
);

create table UsersCredential (
    UserCredentialId int primary key identity(1,1),
    Username varchar(100) not null unique,
    Password varchar(8000) default 'scrypt:32768:8:1$7VfJfm$00cf53e4d541c768689a76261af7a8be0adebdcdb6ecfee9e4d6918f2516ef477f42ecbde163079414d1d09041f8fb4198095e77afba9dd4d6a2a172b009ddd1',
    UserId int unique not null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (UserId) references Users (UserId)
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
    foreign key (UserId) references Users(UserId),
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
    foreign key (UserId) references Users(UserId)
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
    foreign key (UserId) references Users (UserId)
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
    foreign key (SenderUserId) references Users(UserId),
    foreign key (ReceiverUserId) references Users(UserId),
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
    foreign key (UserId) references Users(UserId)
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
    foreign key (UserId) references Users (UserId),
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
    foreign key (UserId) references Users(UserId)    
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
    foreign key (UserId) references Users(UserId)
);

