USE master;

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SpiceCraft')
BEGIN
    -- Set the database to single-user mode
    ALTER DATABASE SpiceCraft SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    
    -- Drop the database
    DROP DATABASE SpiceCraft;
END
ELSE
BEGIN
    PRINT 'Database SpiceCraft does not exist.';
END;

create database SpiceCraft;

use SpiceCraft;

-- Create Tables
create table Roles (
    RoleId int primary key identity(1,1),
    RoleName varchar(50) not null unique
);

create table ShippingOptions (
    ShippingOptionId int primary key identity(1,1),
    ShippingOptionName varchar(100) not null unique,
    Description varchar(100) not null,
    Cost decimal(10,2) default 5.00 not null,
    FreeShippingThreshold decimal(10,2) not null
);

create table Users (
    UserId int primary key identity(1,1),
    Title varchar(5) check (Title in ('Mr.', 'Ms.', 'Dr.', 'Mr.')),
    FirstName varchar(100) not null,
    LastName varchar(100) not null,
    Email varchar(100) unique  not null,
    Phone varchar(12),
    ProfileImg varchar(200),
    RoleId int not null,
    IsActive bit default 1 not null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (RoleId) references Roles (RoleId)
);

create table UsersCredential (
    UserCredentialId int primary key identity(1,1),
    UserName varchar(100) not null unique,
    Password varchar(8000),
    UserId int unique not null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (UserId) references Users (UserId)
);

-- Create CorporateClients table
CREATE TABLE CorporateClients (
      CorporateId INT IDENTITY(1,1) PRIMARY KEY,
      UserId INT NOT NULL UNIQUE,
      Approved BIT DEFAULT 0,
      CompanyName varchar(100) UNIQUE NOT NULL,
      CompanyDescription varchar(MAX),
      DiscountRate DECIMAL(5,2) DEFAULT 0,
      CreditLimit DECIMAL(10,2) DEFAULT 0,
      CreditUsed DECIMAL(10,2) DEFAULT 0,
      PaymentOption varchar(10) NOT NULL CHECK (PaymentOption IN ('pay_now', 'pay_later')) DEFAULT 'pay_now',
      CreatedAt DATETIME DEFAULT GETDATE(),
      UpdatedAt DATETIME DEFAULT GETDATE(),
      CONSTRAINT FK_CorporateClients_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- Create PaymentSchedules table
CREATE TABLE PaymentSchedules (
      ScheduleId INT IDENTITY(1,1) PRIMARY KEY,
      CorporateId INT NOT NULL,
      ScheduleType varchar(10) NOT NULL CHECK (ScheduleType IN ('immediate', 'monthly')),
      LastPaymentDate DATE,
      NextPaymentDue DATE,
      CONSTRAINT FK_PaymentSchedules_CorporateClients FOREIGN KEY (CorporateId) REFERENCES CorporateClients(CorporateId) ON DELETE CASCADE
);

create table ItemCategories (
    CategoryId int primary key identity(1,1),
    CategoryName varchar(300) not null unique,
    ParentCategoryId int,
    foreign key (ParentCategoryId) references ItemCategories(CategoryId)
);

create table Items (
    ItemId int primary key identity(1,1),
    CategoryId int not null,
    ItemName varchar(300) not null,
    OwnProduct bit default 1 not null,
    Discount decimal(10,2) default 0 not null,
    Description text,
    Price decimal(10,2) not null,
    IsRemoved bit default 0 not null,
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
    IsFreeShipping bit default 0 not null,
	Preference varchar(100),
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
    ActualPrice decimal(10,2) not null,
    [Description] varchar(100),
    DiscountRate decimal(10,2) default 0 not null,
    Quantity int not null,
    PurchasePrice decimal(10,2) not null,
    SpiceLevel varchar(100) check (SpiceLevel in ('Mild', 'Medium', 'Hot', 'Extra Hot')) default 'Medium',
    size varchar(100) check (size in ('Small', 'Medium', 'Large', 'Family Pack')) default 'Medium',
    foreign key (OrderId) references Orders(OrderId),
    foreign key (ItemId) references Items(ItemId),
    unique(OrderId, ItemId)
);

create table ShoppingCarts (
    CartId int primary key identity(1,1),
    UserId int not null unique,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    IsOrdered bit default 0 not null,
    foreign key (UserId) references Users(UserId)
);

create table CartItems (
    CartItemId int primary key identity(1,1),
    CartId int not null,
    ItemId int not null,
    Quantity int not null,
    [Description] varchar(100),
    PriceAtAdd decimal(10, 2) not null,
    foreign key (CartId) references ShoppingCarts(CartId),
    foreign key (ItemId) references Items(ItemId),
    unique(CartId, ItemId)
);

create table Inventory (
    InventoryId int primary key identity(1,1),
    ItemId int not null unique,
    CurrentStock int not null,
    LowStockThreshold int not null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (ItemId) references Items(ItemId)
);

create table PromotionItems (
    PromotionItemId int primary key identity(1,1),
    ItemId int not null,
    DiscountRate decimal(5,2) default 0 not null,
    foreign key (ItemId) references Items(ItemId),
    unique(ItemId)
);

create table PromotionCategories (
    PromotionCategoryId int primary key identity(1,1),
    CategoryId int not null,
    DiscountRate decimal(5,2) default 0 not null,
    foreign key (CategoryId) references ItemCategories(CategoryId),
    unique(CategoryId)
);

create table PromotionComboItems (
    PromotionComboItemsId int primary key identity(1,1),
    ItemId int not null,
    ComboName varchar(100) not null,
    BuyQuantity int not null default 2,
    GetQuantity int not null default 1,
    foreign key (ItemId) references Items(ItemId),
    unique(ItemId)
);

create table PromotionBulkItems (
    PromotionBulkItemId int primary key identity(1,1),
    ItemId int not null,
    RequiredQuantity int not null default 0,
    DiscountRate decimal(5,2) default 0  not null,
    foreign key (ItemId) references Items(ItemId),
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
    IsUsed bit default 0 not null,
    foreign key (RewardId) references Rewards (RewardId),
    foreign key (UserId) references Users (UserId)
);

create table Invoices (
    InvoiceId int primary key identity(1,1),   
    OrderId int not null unique,
    IssueDate date not null,
    DueDate date not null,
    TotalAmount decimal(10,2) not null,
    Paid bit default 0 not null,
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
    IsActive bit default 1 not null,
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
    foreign key (ItemId) references Items(ItemId)
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

create table UserActivityLog (
  UserActivityLogId int primary key identity(1,1),
  UserId int not null, 
  NavigationItem varchar(100) not null,
  Routing varchar(100) not null,
  TimeSpent float null,
  SessionId varchar(200) null,
  ClickCount int default(1),
  foreign key (UserId) references Users(UserId)
)

-- Insert Data into Tables
-- Roles
INSERT INTO Roles (RoleName) VALUES 
('Admin'),
('Manager'),
('Staff'),
('Customer'),
('CorporateClient');

-- ShippingOptions
INSERT INTO ShippingOptions (ShippingOptionName, Description, Cost, FreeShippingThreshold) VALUES
('Standard Delivery', 'Delivers in 3 hours', 5.00, 15.00),
('Express Delivery', 'Immediate Delivery', 10.00, 25.00),
('Pickup in Store', 'Pick up the order from store', 0.00, 0.00);

-- Users
INSERT INTO Users (Title, FirstName, LastName, Email, Phone, ProfileImg, RoleId, IsActive) VALUES 
('Mr.', 'John', 'Doe', 'john.doe@example.com', '1234567890', 'img1.jpg', 1, 1),
('Ms.', 'Jane', 'Smith', 'jane.smith@example.com', '0987654321', 'img2.jpg', 2, 1),
('Dr.', 'Alan', 'Walker', 'alan.walker@example.com', '1112223333', 'img3.jpg', 3, 1),
('Mr.', 'Mark', 'Johnson', 'mark.johnson@example.com', '4445556666', 'img4.jpg', 4, 1),
('Ms.', 'Sarah', 'Connor', 'sarah.connor@example.com', '7778889999', 'img5.jpg', 4, 1),
('Dr.', 'Emma', 'Davis', 'emma.davis@example.com', '2223334444', 'img6.jpg', 2, 1),
('Mr.', 'Luke', 'Skywalker', 'luke.skywalker@example.com', '5556667777', 'img7.jpg', 3, 1),
('Ms.', 'Leia', 'Organa', 'leia.organa@example.com', '8889990000', 'img8.jpg', 4, 1),
('Mr.', 'Han', 'Solo', 'han.solo@example.com', '1110002222', 'img9.jpg', 4, 1),
('Ms.', 'Rey', 'Palpatine', 'rey.palpatine@example.com', '3334445555', 'img10.jpg', 1, 1),
('Mr.', 'Kylo', 'Ren', 'kylo.ren@example.com', '6667778888', 'img11.jpg', 3, 1),
('Ms.', 'Padme', 'Amidala', 'padme.amidala@example.com', '9990001111', 'img12.jpg', 2, 1),
('Dr.', 'Yoda', 'Unknown', 'yoda.unknown@example.com', '2221113333', 'img13.jpg', 2, 1),
('Mr.', 'Obi-Wan', 'Kenobi', 'obiwan.kenobi@example.com', '4445556666', 'img14.jpg', 4, 1),
('Ms.', 'Ahsoka', 'Tano', 'ahsoka.tano@example.com', '7778889999', 'img15.jpg', 4, 1),
('Mr.', 'Anakin', 'Skywalker', 'anakin.skywalker@example.com', '1234567890', 'img16.jpg', 3, 1),
('Ms.', 'Mace', 'Windu', 'mace.windu@example.com', '0987654321', 'img17.jpg', 1, 1),
('Dr.', 'Qui-Gon', 'Jinn', 'quigon.jinn@example.com', '1112223333', 'img18.jpg', 4, 1),
('Mr.', 'Jar Jar', 'Binks', 'jarjar.binks@example.com', '4445556666', 'img19.jpg', 4, 1),
('Ms.', 'Darth', 'Maul', 'darth.maul@example.com', '7778889999', 'img20.jpg', 3, 1);

-- UsersCredential
--INSERT INTO UsersCredential (UserName, Password, UserId) VALUES 
--('john_doe', 'password123', 1),
--('jane_smith', 'password123', 2),
--('alan_walker', 'password123', 3),
--('mark_johnson', 'password123', 4),
--('sarah_connor', 'password123', 5),
--('emma_davis', 'password123', 6),
--('luke_skywalker', 'password123', 7),
--('leia_organa', 'password123', 8),
--('han_solo', 'password123', 9),
--('rey_palpatine', 'password123', 10),
--('kylo_ren', 'password123', 11),
--('padme_amidala', 'password123', 12),
--('yoda_unknown', 'password123', 13),
--('obiwan_kenobi', 'password123', 14),
--('ahsoka_tano', 'password123', 15),
--('anakin_skywalker', 'password123', 16),
--('mace_windu', 'password123', 17),
--('quigon_jinn', 'password123', 18),
--('jarjar_binks', 'password123', 19),
--('darth_maul', 'password123', 20);

-- ItemCategories
-- ItemCategories
INSERT INTO ItemCategories (CategoryName, ParentCategoryId) VALUES 
('Indian Appetizers', NULL),
('Indian Main Course', NULL),
('Indian Desserts', NULL),
('Indian Drinks', NULL),
('Mediterranean Appetizers', NULL),
('Mediterranean Main Course', NULL),
('Mediterranean Desserts', NULL),
('Mediterranean Drinks', NULL),
('Mexican Appetizers', NULL),
('Mexican Main Course', NULL),
('Mexican Desserts', NULL),
('Mexican Drinks', NULL),
('Vegetarian Dishes', NULL),
('Non-Vegetarian Dishes', NULL),
('Vegan Dishes', NULL),
('Gluten-Free Dishes', NULL),
('Spicy Dishes', NULL),
('Indian Bread', 1),
('Mediterranean Bread', 5),
('Mexican Sides', 9);


-- Items (30 rows)
INSERT INTO Items (CategoryId, ItemName, OwnProduct, Discount, Description, Price) VALUES 
(1, 'Butter Chicken', 1, 10.00, 'Delicious Indian dish made with butter and spices', 15.99),
(1, 'Chicken Tikka Masala', 1, 15.00, 'Popular Indian dish with a spicy tomato-based sauce', 17.99),
(1, 'Biryani', 1, 5.00, 'Aromatic Indian rice dish with spices and meat', 13.99),
(1, 'Naan Bread', 1, 0, 'Indian flatbread perfect for dipping', 3.99),
(1, 'Samosas', 1, 0, 'Crispy Indian pastry filled with spiced potatoes and peas', 4.99),
(2, 'Falafel', 1, 0, 'Deep-fried ball made from ground chickpeas, a Mediterranean staple', 6.99),
(2, 'Shawarma', 1, 10.00, 'Middle Eastern dish of thinly sliced roasted meat', 12.99),
(2, 'Hummus', 1, 5.00, 'Creamy blend of chickpeas, tahini, and spices', 7.99),
(2, 'Tabbouleh', 1, 0, 'Mediterranean salad made with parsley, tomatoes, and bulgur', 8.99),
(2, 'Baba Ganoush', 1, 0, 'Smoky roasted eggplant dip', 8.99),
(3, 'Tacos', 1, 0, 'Mexican dish with various fillings wrapped in a soft tortilla', 10.99),
(3, 'Burritos', 1, 5.00, 'Mexican dish with a flour tortilla wrapped around fillings', 11.99),
(3, 'Quesadillas', 1, 0, 'Grilled tortilla filled with cheese and other ingredients', 9.99),
(3, 'Guacamole', 1, 0, 'Avocado dip with lime, onion, and cilantro', 5.99),
(3, 'Churros', 1, 10.00, 'Deep-fried dough pastry sprinkled with sugar', 4.99),
(1, 'Rogan Josh', 1, 15.00, 'Indian lamb dish cooked with yogurt and spices', 18.99),
(2, 'Baklava', 1, 5.00, 'Rich, sweet pastry made of layers of filo filled with chopped nuts', 9.99),
(3, 'Enchiladas', 1, 0, 'Rolled tortilla with a savory filling and chili sauce', 14.99),
(1, 'Palak Paneer', 1, 10.00, 'Indian vegetarian dish made with spinach and paneer', 13.99),
(1, 'Vindaloo', 1, 20.00, 'Indian curry dish known for its intense heat', 16.99),
(2, 'Moussaka', 1, 10.00, 'Mediterranean dish with layers of eggplant and meat', 15.99),
(3, 'Tostadas', 1, 5.00, 'Crispy fried tortilla topped with various ingredients', 12.99),
(2, 'Gyro', 1, 0, 'Greek dish made with meat, tomato, onion, and tzatziki sauce', 11.99),
(3, 'Nachos', 1, 0, 'Tortilla chips topped with cheese and other ingredients', 8.99),
(1, 'Pani Puri', 1, 0, 'Indian street food, hollow puri filled with flavored water', 5.99),
(2, 'Spanakopita', 1, 0, 'Greek pastry filled with spinach and feta cheese', 7.99),
(3, 'Tamales', 1, 5.00, 'Mexican dish made of masa filled with meat or beans and steamed', 9.99),
(1, 'Chole Bhature', 1, 10.00, 'Indian dish made of spicy chickpeas served with fried bread', 11.99),
(2, 'Dolma', 1, 15.00, 'Stuffed grape leaves, a Mediterranean delicacy', 8.99),
(3, 'Empanadas', 1, 5.00, 'Mexican pastry filled with sweet or savory ingredients', 6.99);

-- Orders
DECLARE @CurrentDate DATE = GETDATE();

-- Insert Orders with dates ranging from 20 days back from the current date
INSERT INTO Orders (UserId, OrderDate, ShippingOptionId, TotalCost, OrderStatus) VALUES 
(1, DATEADD(DAY, -20, @CurrentDate), 1, 50.00, 'Prepared'),
(2, DATEADD(DAY, -19, @CurrentDate), 2, 75.00, 'Shipped'),
(3, DATEADD(DAY, -18, @CurrentDate), 3, 65.00, 'Ready To Ship'),
(4, DATEADD(DAY, -17, @CurrentDate), 1, 45.00, 'Ready For Pickup'),
(5, DATEADD(DAY, -16, @CurrentDate), 2, 95.00, 'Cancelled'),
(6, DATEADD(DAY, -15, @CurrentDate), 3, 85.00, 'Returned'),
(7, DATEADD(DAY, -14, @CurrentDate), 1, 35.00, 'Prepared'),
(8, DATEADD(DAY, -13, @CurrentDate), 2, 25.00, 'Shipped'),
(9, DATEADD(DAY, -12, @CurrentDate), 3, 15.00, 'Ready To Ship'),
(10, DATEADD(DAY, -11, @CurrentDate), 1, 55.00, 'Ready For Pickup'),
(11, DATEADD(DAY, -10, @CurrentDate), 2, 65.00, 'Cancelled'),
(12, DATEADD(DAY, -9, @CurrentDate), 3, 75.00, 'Returned'),
(13, DATEADD(DAY, -8, @CurrentDate), 1, 85.00, 'Prepared'),
(14, DATEADD(DAY, -7, @CurrentDate), 2, 95.00, 'Shipped'),
(15, DATEADD(DAY, -6, @CurrentDate), 3, 105.00, 'Ready To Ship'),
(16, DATEADD(DAY, -5, @CurrentDate), 1, 115.00, 'Ready For Pickup'),
(17, DATEADD(DAY, -4, @CurrentDate), 2, 125.00, 'Cancelled'),
(18, DATEADD(DAY, -3, @CurrentDate), 3, 135.00, 'Returned'),
(19, DATEADD(DAY, -2, @CurrentDate), 1, 145.00, 'Prepared'),
(20, DATEADD(DAY, -1, @CurrentDate), 2, 155.00, 'Shipped');


-- OrderDetails
INSERT INTO OrderDetails (OrderId, ItemId, ActualPrice, DiscountRate, Quantity, PurchasePrice) VALUES 
(1, 1, 15.99, 10.00, 1, 14.39),
(2, 2, 17.99, 15.00, 2, 30.58),
(3, 3, 13.99, 5.00, 1, 13.29),
(4, 4, 3.99, 0, 3, 11.97),
(5, 5, 4.99, 0, 2, 9.98),
(6, 6, 6.99, 0, 4, 27.96),
(7, 7, 12.99, 10.00, 1, 11.69),
(8, 8, 7.99, 5.00, 2, 15.18),
(9, 9, 8.99, 0, 1, 8.99),
(10, 10, 8.99, 0, 3, 26.97),
(11, 11, 10.99, 0, 2, 21.98),
(12, 12, 11.99, 5.00, 1, 11.39),
(13, 13, 9.99, 0, 2, 19.98),
(14, 14, 5.99, 0, 3, 17.97),
(15, 15, 4.99, 10.00, 4, 17.96),
(16, 16, 18.99, 15.00, 1, 16.14),
(17, 17, 9.99, 5.00, 2, 18.98),
(18, 18, 14.99, 0, 1, 14.99),
(19, 19, 13.99, 10.00, 1, 12.59),
(20, 20, 15.99, 0, 2, 31.98);


-- ShoppingCarts
INSERT INTO ShoppingCarts (UserId) VALUES 
(1),
(2),
(3),
(4),
(5),
(6),
(7),
(8),
(9),
(10),
(11),
(12),
(13),
(14),
(15),
(16),
(17),
(18),
(19),
(20);

-- CartItems
INSERT INTO CartItems (CartId, ItemId, Quantity, PriceAtAdd) VALUES 
(1, 1, 1, 15.99),
(2, 2, 2, 17.99),
(3, 3, 1, 13.99),
(4, 4, 3, 3.99),
(5, 5, 2, 4.99),
(6, 6, 4, 6.99),
(7, 7, 1, 12.99),
(8, 8, 2, 7.99),
(9, 9, 1, 8.99),
(10, 10, 3, 8.99),
(11, 11, 2, 10.99),
(12, 12, 1, 11.99),
(13, 13, 2, 9.99),
(14, 14, 3, 5.99),
(15, 15, 4, 4.99),
(16, 16, 1, 18.99),
(17, 17, 2, 9.99),
(18, 18, 1, 14.99),
(19, 19, 1, 13.99),
(20, 20, 2, 15.99);


-- Inventory
INSERT INTO Inventory (ItemId, CurrentStock, LowStockThreshold) VALUES 
(1, 100, 10),
(2, 150, 15),
(3, 200, 20),
(4, 120, 12),
(5, 140, 14),
(6, 130, 13),
(7, 110, 11),
(8, 160, 16),
(9, 170, 17),
(10, 180, 18),
(11, 190, 19),
(12, 210, 21),
(13, 220, 22),
(14, 230, 23),
(15, 240, 24),
(16, 250, 25),
(17, 260, 26),
(18, 270, 27),
(19, 280, 28),
(20, 290, 29),
(21, 300, 30),
(22, 310, 31),
(23, 320, 32),
(24, 330, 33),
(25, 340, 34),
(26, 350, 35),
(27, 360, 36),
(28, 370, 37),
(29, 380, 38),
(30, 390, 39);

-- PromotionItems
INSERT INTO PromotionItems (ItemId, DiscountRate) VALUES 
(1, 10.00),
(2, 15.00),
(3, 5.00),
(4, 0),
(5, 0),
(6, 0),
(7, 10.00),
(8, 5.00),
(9, 0),
(10, 0),
(11, 0),
(12, 5.00),
(13, 0),
(14, 0),
(15, 10.00),
(16, 15.00),
(17, 5.00),
(18, 0),
(19, 10.00),
(20, 0);

-- PromotionCategories
INSERT INTO PromotionCategories (CategoryId, DiscountRate) VALUES 
(1, 10.00),
(2, 15.00),
(3, 5.00),
(4, 0),
(5, 0),
(6, 0),
(7, 10.00),
(8, 5.00),
(9, 0),
(10, 0),
(11, 0),
(12, 5.00),
(13, 0),
(14, 0),
(15, 10.00),
(16, 15.00),
(17, 5.00),
(18, 0),
(19, 10.00),
(20, 0);

-- PromotionComboItems
INSERT INTO PromotionComboItems (ItemId, ComboName, BuyQuantity, GetQuantity) VALUES 
(1, 'Combo 1', 2, 1),
(2, 'Combo 2', 3, 1),
(3, 'Combo 3', 4, 1),
(4, 'Combo 4', 2, 1),
(5, 'Combo 5', 3, 1),
(6, 'Combo 6', 4, 1),
(7, 'Combo 7', 2, 1),
(8, 'Combo 8', 3, 1),
(9, 'Combo 9', 4, 1),
(10, 'Combo 10', 2, 1),
(11, 'Combo 11', 3, 1),
(12, 'Combo 12', 4, 1),
(13, 'Combo 13', 2, 1),
(14, 'Combo 14', 3, 1),
(15, 'Combo 15', 4, 1),
(16, 'Combo 16', 2, 1),
(17, 'Combo 17', 3, 1),
(18, 'Combo 18', 4, 1),
(19, 'Combo 19', 2, 1),
(20, 'Combo 20', 3, 1);

-- PromotionBulkItems
INSERT INTO PromotionBulkItems (ItemId, RequiredQuantity, DiscountRate) VALUES 
(1, 5, 10.00),
(2, 10, 15.00),
(3, 15, 5.00),
(4, 20, 0),
(5, 25, 0),
(6, 30, 0),
(7, 35, 10.00),
(8, 40, 5.00),
(9, 45, 0),
(10, 50, 0),
(11, 55, 0),
(12, 60, 5.00),
(13, 65, 0),
(14, 70, 0),
(15, 75, 10.00),
(16, 80, 15.00),
(17, 85, 5.00),
(18, 90, 0),
(19, 95, 10.00),
(20, 100, 0);

-- Rewards
INSERT INTO Rewards (PointsThreshold, RewardDescription) VALUES 
(100, '10% off next purchase'),
(200, '15% off next purchase'),
(300, '20% off next purchase'),
(400, '25% off next purchase'),
(500, '30% off next purchase'),
(600, 'Free shipping on next order'),
(700, 'Free gift with next purchase'),
(800, 'Double reward points on next purchase'),
(900, 'Early access to new products'),
(1000, 'VIP customer service'),
(1100, '10% off next purchase'),
(1200, '15% off next purchase'),
(1300, '20% off next purchase'),
(1400, '25% off next purchase'),
(1500, '30% off next purchase'),
(1600, 'Free shipping on next order'),
(1700, 'Free gift with next purchase'),
(1800, 'Double reward points on next purchase'),
(1900, 'Early access to new products'),
(2000, 'VIP customer service');

-- CustomerRewards
INSERT INTO CustomerRewards (RewardId, UserId, IsUsed) VALUES 
(1, 1, 0),
(2, 2, 0),
(3, 3, 1),
(4, 4, 0),
(5, 5, 1),
(6, 6, 0),
(7, 7, 1),
(8, 8, 0),
(9, 9, 1),
(10, 10, 0),
(11, 11, 0),
(12, 12, 1),
(13, 13, 0),
(14, 14, 1),
(15, 15, 0),
(16, 16, 1),
(17, 17, 0),
(18, 18, 1),
(19, 19, 0),
(20, 20, 1);

-- Invoices
INSERT INTO Invoices (OrderId, IssueDate, DueDate, TotalAmount, Paid) VALUES 
(1, '2024-08-01', '2024-08-10', 50.00, 1),
(2, '2024-08-02', '2024-08-11', 75.00, 1),
(3, '2024-08-03', '2024-08-12', 65.00, 1),
(4, '2024-08-04', '2024-08-13', 45.00, 0),
(5, '2024-08-05', '2024-08-14', 95.00, 1),
(6, '2024-08-06', '2024-08-15', 85.00, 0),
(7, '2024-08-07', '2024-08-16', 35.00, 1),
(8, '2024-08-08', '2024-08-17', 25.00, 1),
(9, '2024-08-09', '2024-08-18', 15.00, 0),
(10, '2024-08-10', '2024-08-19', 55.00, 1),
(11, '2024-08-11', '2024-08-20', 65.00, 0),
(12, '2024-08-12', '2024-08-21', 75.00, 1),
(13, '2024-08-13', '2024-08-22', 85.00, 0),
(14, '2024-08-14', '2024-08-23', 95.00, 1),
(15, '2024-08-15', '2024-08-24', 105.00, 0),
(16, '2024-08-16', '2024-08-25', 115.00, 1),
(17, '2024-08-17', '2024-08-26', 125.00, 1),
(18, '2024-08-18', '2024-08-27', 135.00, 1),
(19, '2024-08-19', '2024-08-28', 145.00, 0),
(20, '2024-08-20', '2024-08-29', 155.00, 1);

-- EnquiryType
INSERT INTO EnquiryType (EnquiryName) VALUES 
('Order Status'),
('Items Information'),
('Shipping Information'),
('Return and Refund'),
('Technical Support'),
('Payment Issues'),
('Promotions and Discounts'),
('Account Management'),
('General Inquiry'),
('Feedback and Complaints'),
('Order Status'),
('Items Information'),
('Shipping Information'),
('Return and Refund'),
('Technical Support'),
('Payment Issues'),
('Promotions and Discounts'),
('Account Management'),
('General Inquiry'),
('Feedback and Complaints');

-- Enquiry
INSERT INTO Enquiry (EnquiryTypeId) VALUES 
(1),
(2),
(3),
(4),
(5),
(6),
(7),
(8),
(9),
(10),
(11),
(12),
(13),
(14),
(15),
(16),
(17),
(18),
(19),
(20);

-- Messages
INSERT INTO Messages (SenderUserId, ReceiverUserId, EnquiryId, Subject, MessageContent) VALUES 
(1, 2, 1, 'Order Status', 'What is the status of my order?'),
(3, 4, 2, 'Items Information', 'Can you provide more details about this Items?'),
(5, 6, 3, 'Shipping Information', 'When will my order be shipped?'),
(7, 8, 4, 'Return and Refund', 'How can I return an item?'),
(9, 10, 5, 'Technical Support', 'I am facing an issue with the website.'),
(11, 12, 6, 'Payment Issues', 'My payment did not go through.'),
(13, 14, 7, 'Promotions and Discounts', 'Are there any ongoing promotions?'),
(15, 16, 8, 'Account Management', 'How can I update my account details?'),
(17, 18, 9, 'General Inquiry', 'I have a general question.'),
(19, 20, 10, 'Feedback and Complaints', 'I would like to provide feedback.'),
(2, 1, 11, 'Order Status', 'Has my order been shipped?'),
(4, 3, 12, 'Items Information', 'Is this Items available in other colors?'),
(6, 5, 13, 'Shipping Information', 'What are the available shipping options?'),
(8, 7, 14, 'Return and Refund', 'Can I get a refund for this item?'),
(10, 9, 15, 'Technical Support', 'I am unable to log in to my account.'),
(12, 11, 16, 'Payment Issues', 'I was charged twice for my order.'),
(14, 13, 17, 'Promotions and Discounts', 'Can I combine multiple discounts?'),
(16, 15, 18, 'Account Management', 'How do I change my password?'),
(18, 17, 19, 'General Inquiry', 'What are your business hours?'),
(20, 19, 20, 'Feedback and Complaints', 'I have a complaint about my order.');

-- Payments
DECLARE @CurrentDate2 DATE = GETDATE();

-- Insert Payments with dates ranging from 20 days back from the current date
INSERT INTO Payments (UserId, OrderId, PaymentMethod, Amount, PaymentStatus, PaymentDate) VALUES 
(1, 1, 'credit card', 50.00, 'Completed', DATEADD(DAY, -20, @CurrentDate2)),
(2, 2, 'gift card', 75.00, 'Completed', DATEADD(DAY, -19, @CurrentDate2)),
(3, 3, 'credit card', 65.00, 'Pending', DATEADD(DAY, -18, @CurrentDate2)),
(4, 4, 'client credit', 45.00, 'Failed', DATEADD(DAY, -17, @CurrentDate2)),
(5, 5, 'credit card', 95.00, 'Completed', DATEADD(DAY, -16, @CurrentDate2)),
(6, 6, 'credit card and gift card', 85.00, 'Completed', DATEADD(DAY, -15, @CurrentDate)),
(7, 7, 'gift card', 35.00, 'Completed', DATEADD(DAY, -14, @CurrentDate2)),
(8, 8, 'credit card', 25.00, 'Pending', DATEADD(DAY, -13, @CurrentDate2)),
(9, 9, 'client credit', 15.00, 'Completed', DATEADD(DAY, -12, @CurrentDate2)),
(10, 10, 'credit card', 55.00, 'Completed', DATEADD(DAY, -11, @CurrentDate2)),
(11, 11, 'credit card', 65.00, 'Completed', DATEADD(DAY, -10, @CurrentDate2)),
(12, 12, 'credit card', 75.00, 'Completed', DATEADD(DAY, -9, @CurrentDate2)),
(13, 13, 'gift card', 85.00, 'Completed', DATEADD(DAY, -8, @CurrentDate2)),
(14, 14, 'credit card', 95.00, 'Completed', DATEADD(DAY, -7, @CurrentDate2)),
(15, 15, 'credit card', 105.00, 'Completed', DATEADD(DAY, -6, @CurrentDate2)),
(16, 16, 'client credit', 115.00, 'Completed', DATEADD(DAY, -5, @CurrentDate2)),
(17, 17, 'credit card and gift card', 125.00, 'Completed', DATEADD(DAY, -4, @CurrentDate2)),
(18, 18, 'credit card', 135.00, 'Pending', DATEADD(DAY, -3, @CurrentDate2)),
(19, 19, 'client credit', 145.00, 'Failed', DATEADD(DAY, -2, @CurrentDate2)),
(20, 20, 'credit card', 155.00, 'Completed', DATEADD(DAY, -1, @CurrentDate2));


-- GiftCards
INSERT INTO GiftCards (Code, Balance, ExpirationDate, IsActive) VALUES 
('GIFT1001', 50.00, '2025-12-31', 1),
('GIFT1002', 75.00, '2025-12-31', 1),
('GIFT1003', 100.00, '2025-12-31', 1),
('GIFT1004', 25.00, '2025-12-31', 1),
('GIFT1005', 50.00, '2025-12-31', 1),
('GIFT1006', 75.00, '2025-12-31', 1),
('GIFT1007', 100.00, '2025-12-31', 1),
('GIFT1008', 25.00, '2025-12-31', 1),
('GIFT1009', 50.00, '2025-12-31', 1),
('GIFT1010', 75.00, '2025-12-31', 1),
('GIFT1011', 100.00, '2025-12-31', 1),
('GIFT1012', 25.00, '2025-12-31', 1),
('GIFT1013', 50.00, '2025-12-31', 1),
('GIFT1014', 75.00, '2025-12-31', 1),
('GIFT1015', 100.00, '2025-12-31', 1),
('GIFT1016', 25.00, '2025-12-31', 1),
('GIFT1017', 50.00, '2025-12-31', 1),
('GIFT1018', 75.00, '2025-12-31', 1),
('GIFT1019', 100.00, '2025-12-31', 1),
('GIFT1020', 25.00, '2025-12-31', 1);

-- CustomerGiftCard
INSERT INTO CustomerGiftCard (UserId, GiftCardId) VALUES 
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10),
(11, 11),
(12, 12),
(13, 13),
(14, 14),
(15, 15),
(16, 16),
(17, 17),
(18, 18),
(19, 19),
(20, 20);

-- ItemImages
INSERT INTO ItemImages (ItemId, ImageCode, ImageName, ImageIndex, IsMain) VALUES 
(1, 'IMG1001', 'butter_chicken.jpg', 1, 1),
(2, 'IMG1002', 'chicken_tikka_masala.jpg', 1, 1),
(3, 'IMG1003', 'biryani.jpg', 1, 1),
(4, 'IMG1004', 'naan_bread.jpg', 1, 1),
(5, 'IMG1005', 'samosas.jpg', 1, 1),
(6, 'IMG1006', 'falafel.jpg', 1, 1),
(7, 'IMG1007', 'shawarma.jpg', 1, 1),
(8, 'IMG1008', 'hummus.jpg', 1, 1),
(9, 'IMG1009', 'tabbouleh.jpg', 1, 1),
(10, 'IMG1010', 'baba_ganoush.jpg', 1, 1),
(11, 'IMG1011', 'tacos.jpg', 1, 1),
(12, 'IMG1012', 'burritos.jpg', 1, 1),
(13, 'IMG1013', 'quesadillas.jpg', 1, 1),
(14, 'IMG1014', 'guacamole.jpg', 1, 1),
(15, 'IMG1015', 'churros.jpg', 1, 1),
(16, 'IMG1016', 'rogan_josh.jpg', 1, 1),
(17, 'IMG1017', 'baklava.jpg', 1, 1),
(18, 'IMG1018', 'enchiladas.jpg', 1, 1),
(19, 'IMG1019', 'palak_paneer.jpg', 1, 1),
(20, 'IMG1020', 'vindaloo.jpg', 1, 1),
(21, 'IMG1021', 'moussaka.jpg', 1, 1),
(22, 'IMG1022', 'tostadas.jpg', 1, 1),
(23, 'IMG1023', 'gyro.jpg', 1, 1),
(24, 'IMG1024', 'nachos.jpg', 1, 1),
(25, 'IMG1025', 'pani_puri.jpg', 1, 1),
(26, 'IMG1026', 'spanakopita.jpg', 1, 1),
(27, 'IMG1027', 'tamales.jpg', 1, 1),
(28, 'IMG1028', 'chole_bhature.jpg', 1, 1),
(29, 'IMG1029', 'dolma.jpg', 1, 1),
(30, 'IMG1030', 'empanadas.jpg', 1, 1);

-- UserAddresses
INSERT INTO UserAddresses (UserId, AddressType, StreetAddress1, StreetAddress2, City, StateOrProvince, PostalCode) VALUES 
(1, 'billing', '123 Main St', 'Apt 1', 'Springfield', 'IL', '6201'),
(2, 'shipping', '456 Elm St', 'Apt 2', 'Springfield', 'IL', '6202'),
(3, 'billing', '789 Oak St', 'Apt 3', 'Springfield', 'IL', '6203'),
(4, 'shipping', '101 Maple St', 'Apt 4', 'Springfield', 'IL', '6204'),
(5, 'billing', '102 Pine St', 'Apt 5', 'Springfield', 'IL', '6205'),
(6, 'shipping', '103 Cedar St', 'Apt 6', 'Springfield', 'IL', '6206'),
(7, 'billing', '104 Birch St', 'Apt 7', 'Springfield', 'IL', '6207'),
(8, 'shipping', '105 Walnut St', 'Apt 8', 'Springfield', 'IL', '6208'),
(9, 'billing', '106 Hickory St', 'Apt 9', 'Springfield', 'IL', '62709'),
(10, 'shipping', '107 Chestnut St', 'Apt 10', 'Springfield', 'IL', '6210'),
(11, 'billing', '108 Sycamore St', 'Apt 11', 'Springfield', 'IL', '6211'),
(12, 'shipping', '109 Redwood St', 'Apt 12', 'Springfield', 'IL', '6212'),
(13, 'billing', '110 Spruce St', 'Apt 13', 'Springfield', 'IL', '6213'),
(14, 'shipping', '111 Ash St', 'Apt 14', 'Springfield', 'IL', '6214'),
(15, 'billing', '112 Poplar St', 'Apt 15', 'Springfield', 'IL', '6215'),
(16, 'shipping', '113 Fir St', 'Apt 16', 'Springfield', 'IL', '6216'),
(17, 'billing', '114 Larch St', 'Apt 17', 'Springfield', 'IL', '6217'),
(18, 'shipping', '115 Aspen St', 'Apt 18', 'Springfield', 'IL', '6218'),
(19, 'billing', '116 Palm St', 'Apt 19', 'Springfield', 'IL', '6219'),
(20, 'shipping', '117 Cypress St', 'Apt 20', 'Springfield', 'IL', '6220');

-- Notification
INSERT INTO Notification (NotificationType, EntityId, IsRead, Title, Message, UserId) VALUES 
('Order', 1, 0, 'Order Shipped', 'Your order has been shipped.', 1),
('Message', 2, 1, 'New Message', 'You have received a new message.', 2),
('Order', 3, 0, 'Order Delivered', 'Your order has been delivered.', 3),
('Other', 4, 1, 'Promotion', 'Check out our latest promotions.', 4),
('Order', 5, 0, 'Order Returned', 'Your return request has been processed.', 5),
('Message', 6, 1, 'New Message', 'You have received a new message.', 6),
('Order', 7, 0, 'Order Shipped', 'Your order has been shipped.', 7),
('Message', 8, 1, 'New Message', 'You have received a new message.', 8),
('Order', 9, 0, 'Order Delivered', 'Your order has been delivered.', 9),
('Other', 10, 1, 'Promotion', 'Check out our latest promotions.', 10),
('Order', 11, 0, 'Order Shipped', 'Your order has been shipped.', 11),
('Message', 12, 1, 'New Message', 'You have received a new message.', 12),
('Order', 13, 0, 'Order Delivered', 'Your order has been delivered.', 13),
('Other', 14, 1, 'Promotion', 'Check out our latest promotions.', 14),
('Order', 15, 0, 'Order Returned', 'Your return request has been processed.', 15),
('Message', 16, 1, 'New Message', 'You have received a new message.', 16),
('Order', 17, 0, 'Order Shipped', 'Your order has been shipped.', 17),
('Message', 18, 1, 'New Message', 'You have received a new message.', 18),
('Order', 19, 0, 'Order Delivered', 'Your order has been delivered.', 19),
('Other', 20, 1, 'Promotion', 'Check out our latest promotions.', 20);
