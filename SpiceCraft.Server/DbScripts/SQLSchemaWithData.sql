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
    Title varchar(5) check (Title in ('Mr.', 'Ms.', 'Dr.', 'Mr.')) default('Mr.'),
    FirstName varchar(100) not null,
    LastName varchar(100) not null,
    Email varchar(100) unique  not null,
    Phone varchar(12),
    ProfileImg varchar(200),
	DateofBirth date,
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
    OrderStatus varchar(50) check (OrderStatus in ('Prepared', 'Ready To Ship', 'Shipped', 'Ready For Pickup', 'Cancelled', 'Returned')) not null default('Prepared'),
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

/*
   Example onions, green chili, capsigum, cauliflower, rice, Brocolli, spinach, curry leaves,
   chicken, lamb, flour, ladies finger, beetroot, beans, curry powder, ginger, garlic,
   oil and many more.
   For drinks like wine it is just the wine as Ingredient
*/
create table Ingredients (
  IngredientId int primary key identity(1,1),
  IngredientName varchar(200) not null,
  Unit varchar(100) check (Ingredients.Unit in ('Bag', 'Basket', 'Bunch', 'Individual', 'Box', 'Bottle')) not null default 'Individual',
  ItemsPerUnit int not null default 1, -- for example for a unit of bag will have 30 onions or 1 item if it is individual
  CreatedAt datetime default getdate(),
  UpdatedAt datetime default getdate(),
)

/*
   Relates the Ingredients needed to make that food item.
   It also specifies Quantity needed for each plate.   
*/
create table ItemIngredients (
  ItemIngredientId int primary key identity(1,1),
  ItemId int not null,
  IngredientId int not null,
  Size varchar(100) check (ItemIngredients.Size in ('Small', 'Medium', 'Large')) default 'Medium', -- QuantityNeeded varies based on size as well
  QuantityNeeded int not null default 1, -- quantity needed to make the item ( example 2 onions needed for that item )
  CreatedAt datetime default getdate(),
  UpdatedAt datetime default getdate(),
  foreign key (ItemId) references Items(ItemId),
  foreign key (IngredientId) references Ingredients(IngredientId)   
)

/*
  Whenever the order is made for the item the inventory is reduced based on the ItemIngredients
  QuantityNeeded
*/
create table Inventory (
    InventoryId int primary key identity(1,1),
    IngredientId int not null unique,
    CurrentStock int not null, -- current stock is the unit of Ingredients multiplied by ItemsPerUnit mostly individual
    LowStockThreshold int not null, -- low stock threshold stock is the unit of Ingredients multiplied by ItemsPerUnit mostly individual
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate(),
    foreign key (IngredientId) references Ingredients(IngredientId)
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

create table Subscriptions (
   SubscriptionId int primary key  identity(1,1),
   UserId int not null,
   CreatedAt datetime default getdate(),
   UpdatedAt datetime default getdate(),
   foreign key (UserId) references Users(UserId)
)

create table UserActivityLog (
  UserActivityLogId int primary key identity(1,1),
  UserId int not null, 
  NavigationItem varchar(100) not null,
  Routing varchar(100) not null,
  TimeSpent float null,
  SessionId varchar(200) null,
  ClickCount int default(1),
  CreatedAt datetime default getdate(),
  UpdatedAt datetime default getdate(),
  foreign key (UserId) references Users(UserId)
)

create table UserItemInteraction (
 UserItemInteractionId int primary key identity(1,1),
 UserId int not null,
 Interaction int default(0),
 CreatedAt datetime default getdate(),
 UpdatedAt datetime default getdate(),
 foreign key (UserId) references Users(UserId)
)

create table UserItemRating (
  UserItemRating int primary key identity(1,1),
  UserId int not null,
  Rating int default 1,
  ItemId int not null,
  RatingDescription varchar(250) null,
  CreatedAt datetime default getdate(),
  UpdatedAt datetime default getdate(),
  foreign key (UserId) references Users(UserId),
  foreign key (ItemId) references Items(ItemId)
)

create table RecentlyViewed (
    RecentlyViewedId int primary key identity(1,1),
    UserId int not null,
    ItemId int not null,
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate()
    foreign key (UserId) references Users(UserId),
    foreign key (ItemId) references Items(ItemId)
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
INSERT INTO Users (Title, FirstName, LastName, Email, Phone, ProfileImg, RoleId, IsActive, DateOfBirth) VALUES 
('Mr.', 'John', 'Doe', 'john.doe@example.com', '1234567890', 'img1.jpg', 1, 1, '1980-05-15'),
('Ms.', 'Jane', 'Smith', 'jane.smith@example.com', '0987654321', 'img2.jpg', 2, 1, '1985-09-23'),
('Dr.', 'Alan', 'Walker', 'alan.walker@example.com', '1112223333', 'img3.jpg', 3, 1, '1978-07-30'),
('Mr.', 'Mark', 'Johnson', 'mark.johnson@example.com', '4445556666', 'img4.jpg', 4, 1, '1982-11-11'),
('Ms.', 'Sarah', 'Connor', 'sarah.connor@example.com', '7778889999', 'img5.jpg', 4, 1, '1987-02-20'),
('Dr.', 'Emma', 'Davis', 'emma.davis@example.com', '2223334444', 'img6.jpg', 2, 1, '1975-04-18'),
('Mr.', 'Luke', 'Skywalker', 'luke.skywalker@example.com', '5556667777', 'img7.jpg', 3, 1, '1983-06-10'),
('Ms.', 'Leia', 'Organa', 'leia.organa@example.com', '8889990000', 'img8.jpg', 4, 1, '1983-06-10'),
('Mr.', 'Han', 'Solo', 'han.solo@example.com', '1110002222', 'img9.jpg', 4, 1, '1975-07-13'),
('Ms.', 'Rey', 'Palpatine', 'rey.palpatine@example.com', '3334445555', 'img10.jpg', 1, 1, '1993-12-12'),
('Mr.', 'Kylo', 'Ren', 'kylo.ren@example.com', '6667778888', 'img11.jpg', 3, 1, '1988-01-01'),
('Ms.', 'Padme', 'Amidala', 'padme.amidala@example.com', '9990001111', 'img12.jpg', 2, 1, '1981-03-17'),
('Dr.', 'Yoda', 'Unknown', 'yoda.unknown@example.com', '2221113333', 'img13.jpg', 2, 1, '1900-01-01'),
('Mr.', 'Obi-Wan', 'Kenobi', 'obiwan.kenobi@example.com', '4445556666', 'img14.jpg', 4, 1, '1964-03-25'),
('Ms.', 'Ahsoka', 'Tano', 'ahsoka.tano@example.com', '7778889999', 'img15.jpg', 4, 1, '1984-10-14'),
('Mr.', 'Anakin', 'Skywalker', 'anakin.skywalker@example.com', '1234567890', 'img16.jpg', 3, 1, '1981-05-22'),
('Ms.', 'Mace', 'Windu', 'mace.windu@example.com', '0987654321', 'img17.jpg', 1, 1, '1972-11-06'),
('Dr.', 'Qui-Gon', 'Jinn', 'quigon.jinn@example.com', '1112223333', 'img18.jpg', 4, 1, '1960-08-19'),
('Mr.', 'Jar Jar', 'Binks', 'jarjar.binks@example.com', '4445556666', 'img19.jpg', 4, 1, '1973-05-04'),
('Ms.', 'Darth', 'Maul', 'darth.maul@example.com', '7778889999', 'img20.jpg', 3, 1, '1977-09-16');


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

-- Insert primary categories
INSERT INTO ItemCategories (CategoryName, ParentCategoryId) VALUES 
('Appetizers', NULL),
('Main Course', NULL),
('Desserts', NULL),
('Drinks', NULL),
('Dishes', NULL),
('Bread', NULL),
('Other', NULL);

-- Insert subcategories for Appetizers, Main Course, Desserts, Drinks
INSERT INTO ItemCategories (CategoryName, ParentCategoryId) VALUES 
('Indian Appetizers', 1),  -- 1 is the ParentCategoryId for Appetizers
('Mediterranean Appetizers', 1), 
('Mexican Appetizers', 1),

('Indian Main Course', 2), -- 2 is the ParentCategoryId for Main Course
('Mediterranean Main Course', 2),
('Mexican Main Course', 2),

('Indian Desserts', 3),    -- 3 is the ParentCategoryId for Desserts
('Mediterranean Desserts', 3),
('Mexican Desserts', 3),

('Indian Drinks', 4),      -- 4 is the ParentCategoryId for Drinks
('Mediterranean Drinks', 4),
('Mexican Drinks', 4),

-- Other categories like Vegetarian, Vegan, etc., are not under primary categories, so they remain with NULL ParentCategoryId
('Vegetarian Dishes', 5),
('Non-Vegetarian Dishes', 5),
('Vegan Dishes', 5),
('Gluten-Free Dishes', 5),
('Spicy Dishes', 5),

-- Insert bread and sides as subcategories under their respective regions
('Indian Bread', 6),       -- Indian Bread goes under Indian Main Course (ParentCategoryId = 2)
('Mediterranean Bread', 6),-- Mediterranean Bread goes under Mediterranean Main Course
('Mexican Sides', 7);     -- Mexican Sides goes under Mexican Main Course


-- Items (30 rows)
-- Insert Items into subcategories
-- Insert Items into subcategories (with auto-increment IDs starting from 8)
INSERT INTO Items (CategoryId, ItemName, OwnProduct, Discount, Description, Price) VALUES 
-- Indian Appetizers (CategoryId = 8)
(8, 'Samosas', 1, 0, 'Crispy Indian pastry filled with spiced potatoes and peas', 4.99),
(8, 'Pani Puri', 1, 0, 'Indian street food, hollow puri filled with flavored water', 5.99),

-- Mediterranean Appetizers (CategoryId = 9)
(9, 'Falafel', 1, 0, 'Deep-fried ball made from ground chickpeas, a Mediterranean staple', 6.99),
(9, 'Hummus', 1, 5.00, 'Creamy blend of chickpeas, tahini, and spices', 7.99),
(9, 'Tabbouleh', 1, 0, 'Mediterranean salad made with parsley, tomatoes, and bulgur', 8.99),
(9, 'Baba Ganoush', 1, 0, 'Smoky roasted eggplant dip', 8.99),

-- Mexican Appetizers (CategoryId = 10)
(10, 'Guacamole', 1, 0, 'Avocado dip with lime, onion, and cilantro', 5.99),
(10, 'Nachos', 1, 0, 'Tortilla chips topped with cheese and other ingredients', 8.99),

-- Indian Main Course (CategoryId = 11)
(11, 'Butter Chicken', 1, 10.00, 'Delicious Indian dish made with butter and spices', 15.99),
(11, 'Chicken Tikka Masala', 1, 15.00, 'Popular Indian dish with a spicy tomato-based sauce', 17.99),
(11, 'Biryani', 1, 5.00, 'Aromatic Indian rice dish with spices and meat', 13.99),
(11, 'Rogan Josh', 1, 15.00, 'Indian lamb dish cooked with yogurt and spices', 18.99),
(11, 'Palak Paneer', 1, 10.00, 'Indian vegetarian dish made with spinach and paneer', 13.99),
(11, 'Vindaloo', 1, 20.00, 'Indian curry dish known for its intense heat', 16.99),
(11, 'Chole Bhature', 1, 10.00, 'Indian dish made of spicy chickpeas served with fried bread', 11.99),

-- Mediterranean Main Course (CategoryId = 12)
(12, 'Shawarma', 1, 10.00, 'Middle Eastern dish of thinly sliced roasted meat', 12.99),
(12, 'Moussaka', 1, 10.00, 'Mediterranean dish with layers of eggplant and meat', 15.99),
(12, 'Gyro', 1, 0, 'Greek dish made with meat, tomato, onion, and tzatziki sauce', 11.99),

-- Mexican Main Course (CategoryId = 13)
(13, 'Burritos', 1, 5.00, 'Mexican dish with a flour tortilla wrapped around fillings', 11.99),
(13, 'Tacos', 1, 0, 'Mexican dish with various fillings wrapped in a soft tortilla', 10.99),
(13, 'Quesadillas', 1, 0, 'Grilled tortilla filled with cheese and other ingredients', 9.99),
(13, 'Enchiladas', 1, 0, 'Rolled tortilla with a savory filling and chili sauce', 14.99),
(13, 'Tostadas', 1, 5.00, 'Crispy fried tortilla topped with various ingredients', 12.99),

-- Indian Desserts (CategoryId = 14)
(14, 'Gulab Jamun', 1, 5.00, 'Sweet Indian dessert made of deep-fried dough balls soaked in syrup', 6.99),

-- Mediterranean Desserts (CategoryId = 15)
(15, 'Baklava', 1, 5.00, 'Rich, sweet pastry made of layers of filo filled with chopped nuts', 9.99),

-- Mexican Desserts (CategoryId = 16)
(16, 'Churros', 1, 10.00, 'Deep-fried dough pastry sprinkled with sugar', 4.99),

-- Indian Drinks (CategoryId = 17)
(17, 'Mango Lassi', 1, 0, 'Refreshing mango-flavored yogurt drink', 4.99),

-- Mediterranean Drinks (CategoryId = 18)
(18, 'Turkish Coffee', 1, 0, 'Strong, unfiltered coffee', 3.99),

-- Mexican Drinks (CategoryId = 19)
(19, 'Horchata', 1, 0, 'Traditional Mexican drink made from rice and cinnamon', 3.99),

-- Indian Bread (CategoryId = 20)
(20, 'Naan Bread', 1, 0, 'Indian flatbread perfect for dipping', 3.99),

-- Mediterranean Bread (CategoryId = 21)
(21, 'Pita Bread', 1, 0, 'Mediterranean flatbread', 2.99),

-- Mexican Sides (CategoryId = 22)
(22, 'Mexican Rice', 1, 0, 'Spiced rice with tomatoes and onions', 3.99),

-- Other Mediterranean Items (CategoryId = 23)
(23, 'Spanakopita', 1, 0, 'Greek pastry filled with spinach and feta cheese', 7.99),
(23, 'Dolma', 1, 15.00, 'Stuffed grape leaves, a Mediterranean delicacy', 8.99),

-- Other Mexican Items (CategoryId = 24)
(24, 'Tamales', 1, 5.00, 'Mexican dish made of masa filled with meat or beans and steamed', 9.99),
(24, 'Empanadas', 1, 5.00, 'Mexican pastry filled with sweet or savory ingredients', 6.99),


-- additional items 

-- CategoryId 8 (Indian Appetizers)
(8, 'Aloo Tikki', 1, 0, 'Spiced potato patties, popular in Indian street food', 5.99),

-- CategoryId 9 (Mediterranean Appetizers)
(9, 'Dolma', 1, 5.00, 'Stuffed grape leaves, a Mediterranean delicacy', 6.99),

-- CategoryId 10 (Mexican Appetizers)
(10, 'Elote', 1, 0, 'Mexican street corn with cotija cheese and chili powder', 4.99),

-- CategoryId 11 (Indian Main Course)
(11, 'Paneer Butter Masala', 1, 10.00, 'Cottage cheese cooked in a rich and creamy tomato gravy', 13.99),

-- CategoryId 12 (Mediterranean Main Course)
(12, 'Lamb Kebab', 1, 0, 'Grilled lamb skewers marinated in Mediterranean spices', 16.99),

-- CategoryId 13 (Mexican Main Course)
(13, 'Chicken Fajitas', 1, 5.00, 'Grilled chicken served with tortillas, onions, and peppers', 14.99),

-- CategoryId 14 (Indian Desserts)
(14, 'Rasmalai', 1, 5.00, 'Soft cheese patties soaked in flavored milk', 6.99),

-- CategoryId 15 (Mediterranean Desserts)
(15, 'Knafeh', 1, 0, 'Cheese pastry soaked in sweet sugar syrup, a Mediterranean favorite', 7.99),

-- CategoryId 16 (Mexican Desserts)
(16, 'Tres Leches Cake', 1, 0, 'Light sponge cake soaked in three types of milk', 6.99),

-- CategoryId 17 (Indian Drinks)
(17, 'Masala Chai', 1, 0, 'Indian spiced tea brewed with cardamom, cinnamon, and cloves', 2.99);



-- Orders
DECLARE @CurrentDate4 DATE = GETDATE();

-- Insert Orders with dates ranging from 20 days back from the current date
INSERT INTO Orders (UserId, OrderDate, ShippingOptionId, TotalCost, OrderStatus) VALUES 
(1, DATEADD(DAY, -365, @CurrentDate4), 1, 60.00, 'Shipped'),
(2, DATEADD(DAY, -360, @CurrentDate4), 2, 90.00, 'Prepared'),
(3, DATEADD(DAY, -355, @CurrentDate4), 3, 55.00, 'Cancelled'),
(4, DATEADD(DAY, -350, @CurrentDate4), 1, 80.00, 'Shipped'),
(5, DATEADD(DAY, -345, @CurrentDate4), 2, 75.00, 'Prepared'),
(6, DATEADD(DAY, -340, @CurrentDate4), 3, 95.00, 'Shipped'),
(7, DATEADD(DAY, -335, @CurrentDate4), 1, 85.00, 'Cancelled'),
(8, DATEADD(DAY, -330, @CurrentDate4), 2, 65.00, 'Prepared'),
(9, DATEADD(DAY, -325, @CurrentDate4), 3, 70.00, 'Shipped'),
(10, DATEADD(DAY, -320, @CurrentDate4), 1, 60.00, 'Cancelled'),
(11, DATEADD(DAY, -315, @CurrentDate4), 2, 90.00, 'Prepared'),
(12, DATEADD(DAY, -310, @CurrentDate4), 3, 55.00, 'Shipped'),
(13, DATEADD(DAY, -305, @CurrentDate4), 1, 75.00, 'Cancelled'),
(14, DATEADD(DAY, -300, @CurrentDate4), 2, 65.00, 'Shipped'),
(15, DATEADD(DAY, -295, @CurrentDate4), 3, 80.00, 'Cancelled'),
(16, DATEADD(DAY, -290, @CurrentDate4), 1, 60.00, 'Prepared'),
(17, DATEADD(DAY, -285, @CurrentDate4), 2, 95.00, 'Shipped'),
(18, DATEADD(DAY, -280, @CurrentDate4), 3, 70.00, 'Cancelled'),
(19, DATEADD(DAY, -275, @CurrentDate4), 1, 85.00, 'Prepared'),
(20, DATEADD(DAY, -270, @CurrentDate4), 2, 90.00, 'Shipped'),
(1, DATEADD(DAY, -265, @CurrentDate4), 1, 55.00, 'Cancelled'),
(2, DATEADD(DAY, -260, @CurrentDate4), 2, 75.00, 'Shipped'),
(3, DATEADD(DAY, -255, @CurrentDate4), 3, 60.00, 'Prepared'),
(4, DATEADD(DAY, -250, @CurrentDate4), 1, 90.00, 'Cancelled'),
(5, DATEADD(DAY, -245, @CurrentDate4), 2, 65.00, 'Shipped'),
(6, DATEADD(DAY, -240, @CurrentDate4), 3, 80.00, 'Prepared'),
(7, DATEADD(DAY, -235, @CurrentDate4), 1, 85.00, 'Shipped'),
(8, DATEADD(DAY, -230, @CurrentDate4), 2, 60.00, 'Prepared'),
(9, DATEADD(DAY, -225, @CurrentDate4), 3, 95.00, 'Shipped'),
(10, DATEADD(DAY, -220, @CurrentDate4), 1, 75.00, 'Cancelled'),
(11, DATEADD(DAY, -215, @CurrentDate4), 2, 70.00, 'Shipped'),
(12, DATEADD(DAY, -210, @CurrentDate4), 3, 85.00, 'Prepared'),
(13, DATEADD(DAY, -205, @CurrentDate4), 1, 90.00, 'Shipped'),
(14, DATEADD(DAY, -200, @CurrentDate4), 2, 60.00, 'Cancelled'),
(15, DATEADD(DAY, -195, @CurrentDate4), 3, 95.00, 'Shipped'),
(16, DATEADD(DAY, -190, @CurrentDate4), 1, 70.00, 'Prepared'),
(17, DATEADD(DAY, -185, @CurrentDate4), 2, 85.00, 'Shipped'),
(18, DATEADD(DAY, -180, @CurrentDate4), 3, 75.00, 'Cancelled'),
(19, DATEADD(DAY, -175, @CurrentDate4), 1, 80.00, 'Shipped'),
(20, DATEADD(DAY, -170, @CurrentDate4), 2, 60.00, 'Prepared'),
(1, DATEADD(DAY, -165, @CurrentDate4), 1, 95.00, 'Shipped'),
(2, DATEADD(DAY, -160, @CurrentDate4), 2, 85.00, 'Cancelled'),
(3, DATEADD(DAY, -155, @CurrentDate4), 3, 75.00, 'Prepared'),
(4, DATEADD(DAY, -150, @CurrentDate4), 1, 90.00, 'Shipped'),
(5, DATEADD(DAY, -145, @CurrentDate4), 2, 60.00, 'Cancelled'),
(6, DATEADD(DAY, -140, @CurrentDate4), 3, 80.00, 'Shipped'),
(7, DATEADD(DAY, -135, @CurrentDate4), 1, 95.00, 'Prepared'),
(8, DATEADD(DAY, -130, @CurrentDate4), 2, 70.00, 'Shipped'),
(9, DATEADD(DAY, -125, @CurrentDate4), 3, 85.00, 'Cancelled'),
(10, DATEADD(DAY, -120, @CurrentDate4), 1, 60.00, 'Shipped'),
(11, DATEADD(DAY, -115, @CurrentDate4), 2, 90.00, 'Prepared'),
(12, DATEADD(DAY, -110, @CurrentDate4), 3, 95.00, 'Cancelled'),
(13, DATEADD(DAY, -105, @CurrentDate4), 1, 75.00, 'Shipped'),
(14, DATEADD(DAY, -100, @CurrentDate4), 2, 65.00, 'Prepared'),
(15, DATEADD(DAY, -95, @CurrentDate4), 3, 80.00, 'Shipped'),
(1, '2023-10-12', 1, 60.00, 'Shipped'),
(2, '2023-11-15', 2, 80.00, 'Cancelled'),
(3, '2023-12-20', 3, 90.00, 'Prepared'),
(4, '2024-01-22', 1, 110.00, 'Ready For Pickup'),
(5, '2024-02-25', 2, 100.00, 'Returned'),
(6, '2024-03-03', 3, 70.00, 'Shipped'),
(7, '2024-04-18', 1, 55.00, 'Cancelled'),
(8, '2024-05-12', 2, 120.00, 'Prepared'),
(9, '2024-06-14', 3, 130.00, 'Shipped'),
(10, '2024-07-20', 1, 90.00, 'Returned'),
(11, '2024-08-10', 2, 95.00, 'Prepared'),
(12, '2024-09-05', 3, 85.00, 'Shipped'),
(13, '2024-09-29', 1, 115.00, 'Ready For Pickup'),
(14, '2024-10-01', 2, 105.00, 'Returned'),
(15, '2024-10-02', 3, 125.00, 'Cancelled');



-- OrderDetails
-- Insert OrderDetails for each of the 70 orders (random 1, 2, or 3 items per order)
INSERT INTO OrderDetails (OrderId, ItemId, ActualPrice, DiscountRate, Quantity, PurchasePrice, SpiceLevel, Size) VALUES
-- Order 1 (1 item)
(1, 1, 12.99, 0, 1, 12.99, 'Medium', 'Medium'),

-- Order 2 (2 items)
(2, 2, 15.99, 5.00, 2, 30.38, 'Hot', 'Large'),
(2, 3, 10.99, 0, 1, 10.99, 'Mild', 'Small'),

-- Order 3 (3 items)
(3, 4, 9.99, 10.00, 1, 8.99, 'Medium', 'Family Pack'),
(3, 5, 12.99, 0, 2, 25.98, 'Hot', 'Medium'),
(3, 6, 7.99, 5.00, 1, 7.59, 'Extra Hot', 'Small'),

-- Order 4 (1 item)
(4, 7, 16.99, 0, 1, 16.99, 'Medium', 'Medium'),

-- Order 5 (2 items)
(5, 8, 14.99, 5.00, 1, 14.24, 'Mild', 'Large'),
(5, 9, 11.99, 0, 2, 23.98, 'Hot', 'Medium'),

-- Order 6 (3 items)
(6, 10, 13.99, 0, 2, 27.98, 'Medium', 'Small'),
(6, 11, 9.99, 10.00, 1, 8.99, 'Mild', 'Medium'),
(6, 12, 18.99, 5.00, 1, 18.04, 'Hot', 'Family Pack'),

-- Order 7 (1 item)
(7, 13, 17.99, 0, 1, 17.99, 'Medium', 'Medium'),

-- Order 8 (2 items)
(8, 14, 14.99, 5.00, 1, 14.24, 'Medium', 'Large'),
(8, 15, 11.99, 0, 2, 23.98, 'Mild', 'Small'),

-- Order 9 (3 items)
(9, 16, 9.99, 10.00, 1, 8.99, 'Extra Hot', 'Medium'),
(9, 17, 19.99, 0, 2, 39.98, 'Medium', 'Family Pack'),
(9, 18, 7.99, 5.00, 1, 7.59, 'Hot', 'Small'),

-- Order 10 (1 item)
(10, 19, 16.99, 0, 1, 16.99, 'Medium', 'Large'),

-- Order 11 (2 items)
(11, 20, 14.99, 5.00, 1, 14.24, 'Mild', 'Large'),
(11, 21, 12.99, 0, 2, 25.98, 'Medium', 'Medium'),

-- Order 12 (3 items)
(12, 22, 8.99, 10.00, 1, 8.09, 'Medium', 'Family Pack'),
(12, 23, 9.99, 0, 1, 9.99, 'Mild', 'Medium'),
(12, 24, 7.99, 5.00, 2, 15.18, 'Hot', 'Small'),

-- Order 13 (1 item)
(13, 25, 15.99, 0, 1, 15.99, 'Extra Hot', 'Medium'),

-- Order 14 (2 items)
(14, 26, 14.99, 5.00, 1, 14.24, 'Mild', 'Medium'),
(14, 27, 12.99, 0, 2, 25.98, 'Medium', 'Large'),

-- Order 15 (3 items)
(15, 28, 10.99, 10.00, 1, 9.89, 'Hot', 'Small'),
(15, 29, 13.99, 0, 2, 27.98, 'Medium', 'Large'),
(15, 30, 8.99, 5.00, 1, 8.54, 'Mild', 'Medium'),

-- Order 16 (1 item)
(16, 3, 11.99, 0, 1, 11.99, 'Medium', 'Small'),

-- Order 17 (2 items)
(17, 2, 19.99, 5.00, 1, 18.99, 'Medium', 'Family Pack'),
(17, 3, 17.99, 0, 2, 35.98, 'Hot', 'Large'),

-- Order 18 (3 items)
(18, 4, 14.99, 0, 1, 14.99, 'Mild', 'Medium'),
(18, 5, 9.99, 10.00, 2, 17.98, 'Medium', 'Small'),
(18, 6, 18.99, 5.00, 1, 18.04, 'Extra Hot', 'Family Pack'),

-- Order 19 (1 item)
(19, 7, 12.99, 0, 1, 12.99, 'Hot', 'Medium'),

(20, 1, 14.99, 0, 2, 29.98, 'Hot', 'Medium'),
(20, 2, 9.99, 5.00, 1, 9.49, 'Mild', 'Large'),

-- Order 21 (1 item)
(21, 3, 12.99, 10.00, 1, 11.69, 'Medium', 'Small'),

-- Order 22 (3 items)
(22, 4, 7.99, 0, 2, 15.98, 'Hot', 'Medium'),
(22, 5, 9.99, 0, 1, 9.99, 'Mild', 'Large'),
(22, 6, 10.99, 5.00, 1, 10.44, 'Medium', 'Small'),

-- Order 23 (2 items)
(23, 7, 8.99, 0, 2, 17.98, 'Extra Hot', 'Medium'),
(23, 8, 15.99, 0, 1, 15.99, 'Mild', 'Family Pack'),

-- Order 24 (3 items)
(24, 9, 6.99, 5.00, 1, 6.64, 'Medium', 'Small'),
(24, 10, 11.99, 0, 2, 23.98, 'Hot', 'Medium'),
(24, 11, 13.99, 0, 1, 13.99, 'Mild', 'Large'),

-- Continue the pattern for Orders 25 through 70

-- Order 25 (1 item)
(25, 12, 10.99, 0, 1, 10.99, 'Medium', 'Medium'),

-- Order 26 (2 items)
(26, 13, 14.99, 5.00, 1, 14.24, 'Hot', 'Large'),
(26, 14, 12.99, 0, 2, 25.98, 'Medium', 'Medium'),

-- Order 27 (3 items)
(27, 15, 9.99, 10.00, 1, 8.99, 'Mild', 'Small'),
(27, 16, 11.99, 0, 2, 23.98, 'Medium', 'Large'),
(27, 17, 15.99, 0, 1, 15.99, 'Hot', 'Family Pack'),

-- Order 28 (2 items)
(28, 1, 10.99, 5.00, 1, 10.44, 'Medium', 'Medium'),
(28, 2, 12.99, 0, 2, 25.98, 'Hot', 'Large'),

-- Order 29 (1 item)
(29, 3, 9.99, 10.00, 1, 8.99, 'Mild', 'Small'),

-- Order 30 (3 items)
(30, 4, 14.99, 0, 2, 29.98, 'Medium', 'Medium'),
(30, 5, 8.99, 5.00, 1, 8.54, 'Mild', 'Large'),
(30, 6, 13.99, 0, 1, 13.99, 'Extra Hot', 'Small'),

-- Order 31 (2 items)
(31, 7, 11.99, 0, 1, 11.99, 'Medium', 'Large'),
(31, 8, 14.99, 5.00, 1, 14.24, 'Hot', 'Family Pack'),

-- Order 32 (1 item)
(32, 9, 9.99, 10.00, 1, 8.99, 'Mild', 'Medium'),

-- Order 33 (3 items)
(33, 10, 12.99, 0, 2, 25.98, 'Hot', 'Large'),
(33, 11, 10.99, 5.00, 1, 10.44, 'Mild', 'Medium'),
(33, 12, 7.99, 0, 1, 7.99, 'Medium', 'Small'),

-- Order 34 (2 items)
(34, 13, 13.99, 0, 1, 13.99, 'Hot', 'Family Pack'),
(34, 14, 11.99, 5.00, 2, 22.78, 'Medium', 'Medium'),

-- Order 35 (1 item)
(35, 15, 8.99, 10.00, 1, 8.09, 'Mild', 'Large'),

-- Order 36 (3 items)
(36, 16, 14.99, 0, 2, 29.98, 'Medium', 'Medium'),
(36, 17, 12.99, 5.00, 1, 12.34, 'Hot', 'Family Pack'),
(36, 18, 9.99, 0, 1, 9.99, 'Extra Hot', 'Small'),

-- Order 37 (2 items)
(37, 19, 11.99, 0, 2, 23.98, 'Mild', 'Medium'),
(37, 20, 7.99, 5.00, 1, 7.59, 'Medium', 'Small'),

-- Order 38 (1 item)
(38, 21, 8.99, 0, 1, 8.99, 'Hot', 'Large'),

-- Order 39 (3 items)
(39, 22, 9.99, 0, 1, 9.99, 'Mild', 'Small'),
(39, 23, 14.99, 5.00, 1, 14.24, 'Medium', 'Medium'),
(39, 24, 11.99, 0, 2, 23.98, 'Hot', 'Family Pack'),

-- Order 40 (2 items)
(40, 25, 12.99, 0, 1, 12.99, 'Medium', 'Medium'),
(40, 26, 8.99, 5.00, 2, 17.08, 'Mild', 'Large'),

-- Order 41 (1 item)
(41, 27, 13.99, 0, 1, 13.99, 'Hot', 'Small'),

-- Order 42 (3 items)
(42, 28, 10.99, 0, 1, 10.99, 'Medium', 'Family Pack'),
(42, 29, 11.99, 5.00, 2, 22.78, 'Hot', 'Medium'),
(42, 30, 7.99, 0, 1, 7.99, 'Extra Hot', 'Small'),

-- Order 43 (2 items)
(43, 11, 8.99, 5.00, 1, 8.54, 'Mild', 'Large'),
(43, 12, 12.99, 0, 1, 12.99, 'Medium', 'Medium'),

-- Order 44 (1 item)
(44, 3, 11.99, 0, 1, 11.99, 'Hot', 'Medium'),

-- Order 45 (3 items)
(45, 14, 12.99, 5.00, 1, 12.34, 'Mild', 'Small'),
(45, 15, 8.99, 0, 2, 17.98, 'Medium', 'Large'),
(45, 16, 9.99, 0, 1, 9.99, 'Extra Hot', 'Medium'),

-- Order 46 (2 items)
(46, 7, 14.99, 0, 2, 29.98, 'Hot', 'Family Pack'),
(46, 8, 7.99, 5.00, 1, 7.59, 'Mild', 'Medium'),

-- Order 47 (1 item)
(47, 39, 12.99, 0, 1, 12.99, 'Medium', 'Small'),

-- Order 48 (3 items)
(48, 1, 8.99, 5.00, 1, 8.54, 'Hot', 'Medium'),
(48, 2, 14.99, 0, 2, 29.98, 'Mild', 'Large'),
(48, 3, 10.99, 5.00, 1, 10.44, 'Medium', 'Family Pack'),

-- Order 49 (2 items)
(49, 3, 12.99, 0, 1, 12.99, 'Extra Hot', 'Medium'),
(49, 4, 9.99, 5.00, 1, 9.49, 'Mild', 'Large'),

-- Order 50 (1 item)
(50, 5, 11.99, 0, 1, 11.99, 'Medium', 'Small'),

-- Order 51 (3 items)
(51, 6, 12.99, 0, 2, 25.98, 'Hot', 'Medium'),
(51, 7, 10.99, 5.00, 1, 10.44, 'Mild', 'Large'),
(51, 8, 8.99, 0, 1, 8.99, 'Medium', 'Family Pack'),

-- Order 52 (1 item)
(52, 1, 10.99, 5.00, 1, 10.44, 'Mild', 'Small'),

-- Order 53 (2 items)
(53, 2, 13.99, 0, 2, 27.98, 'Hot', 'Medium'),
(53, 3, 11.99, 0, 1, 11.99, 'Medium', 'Large'),

-- Order 54 (3 items)
(54, 4, 14.99, 5.00, 1, 14.24, 'Extra Hot', 'Family Pack'),
(54, 5, 9.99, 10.00, 1, 8.99, 'Medium', 'Medium'),
(54, 6, 12.99, 0, 2, 25.98, 'Hot', 'Small'),

-- Order 55 (2 items)
(55, 7, 8.99, 5.00, 1, 8.54, 'Mild', 'Medium'),
(55, 8, 15.99, 0, 2, 31.98, 'Medium', 'Large'),

-- Order 56 (1 item)
(56, 9, 11.99, 0, 1, 11.99, 'Hot', 'Small'),

-- Order 57 (3 items)
(57, 10, 12.99, 5.00, 1, 12.34, 'Mild', 'Medium'),
(57, 11, 13.99, 0, 1, 13.99, 'Medium', 'Family Pack'),
(57, 12, 9.99, 0, 2, 19.98, 'Hot', 'Large'),

-- Order 58 (2 items)
(58, 13, 14.99, 0, 2, 29.98, 'Medium', 'Large'),
(58, 14, 7.99, 5.00, 1, 7.59, 'Mild', 'Small'),

-- Order 59 (1 item)
(59, 15, 9.99, 10.00, 1, 8.99, 'Extra Hot', 'Medium'),

-- Order 60 (3 items)
(60, 16, 8.99, 0, 2, 17.98, 'Hot', 'Small'),
(60, 17, 14.99, 0, 1, 14.99, 'Medium', 'Family Pack'),
(60, 18, 11.99, 5.00, 1, 11.39, 'Mild', 'Large'),

-- Order 61 (2 items)
(61, 19, 10.99, 0, 1, 10.99, 'Hot', 'Medium'),
(61, 20, 9.99, 5.00, 2, 18.98, 'Mild', 'Small'),

-- Order 62 (1 item)
(62, 21, 12.99, 0, 1, 12.99, 'Medium', 'Large'),

-- Order 63 (3 items)
(63, 22, 8.99, 5.00, 1, 8.54, 'Mild', 'Medium'),
(63, 23, 12.99, 0, 2, 25.98, 'Medium', 'Family Pack'),
(63, 24, 7.99, 0, 1, 7.99, 'Hot', 'Small'),

-- Order 64 (2 items)
(64, 25, 10.99, 0, 1, 10.99, 'Medium', 'Medium'),
(64, 26, 9.99, 5.00, 2, 18.98, 'Mild', 'Large'),

-- Order 65 (1 item)
(65, 27, 14.99, 0, 1, 14.99, 'Extra Hot', 'Family Pack'),

-- Order 66 (3 items)
(66, 28, 13.99, 0, 2, 27.98, 'Hot', 'Medium'),
(66, 29, 11.99, 5.00, 1, 11.39, 'Mild', 'Small'),
(66, 30, 9.99, 0, 1, 9.99, 'Medium', 'Large'),

-- Order 67 (2 items)
(67, 1, 12.99, 0, 1, 12.99, 'Mild', 'Family Pack'),
(67, 2, 8.99, 5.00, 2, 17.98, 'Medium', 'Medium'),

-- Order 68 (1 item)
(68, 33, 14.99, 0, 1, 14.99, 'Hot', 'Large'),

-- Order 69 (3 items)
(69, 4, 7.99, 5.00, 1, 7.59, 'Mild', 'Medium'),
(69, 5, 12.99, 0, 2, 25.98, 'Medium', 'Large'),
(69, 6, 8.99, 0, 1, 8.99, 'Hot', 'Small'),

-- Order 70 (2 items)
(70, 7, 11.99, 0, 2, 23.98, 'Mild', 'Medium'),
(70, 8, 9.99, 5.00, 1, 9.49, 'Medium', 'Large');


-- Payments
DECLARE @CurrentDate2 DATE = GETDATE();

-- Insert Payments with dates ranging from 20 days back from the current date
INSERT INTO Payments (UserId, OrderId, PaymentMethod, Amount, PaymentStatus, PaymentDate) VALUES
(1, 1, 'credit card', 60.00, 'Completed', DATEADD(DAY, -365, GETDATE())),
(2, 2, 'gift card', 90.00, 'Completed', DATEADD(DAY, -360, GETDATE())),
(3, 3, 'credit card', 55.00, 'Pending', DATEADD(DAY, -355, GETDATE())),
(4, 4, 'client credit', 80.00, 'Failed', DATEADD(DAY, -350, GETDATE())),
(5, 5, 'credit card', 75.00, 'Completed', DATEADD(DAY, -345, GETDATE())),
(6, 6, 'credit card', 95.00, 'Completed', DATEADD(DAY, -340, GETDATE())),
(7, 7, 'credit card and gift card', 85.00, 'Completed', DATEADD(DAY, -335, GETDATE())),
(8, 8, 'gift card', 65.00, 'Pending', DATEADD(DAY, -330, GETDATE())),
(9, 9, 'credit card', 70.00, 'Completed', DATEADD(DAY, -325, GETDATE())),
(10, 10, 'client credit', 60.00, 'Failed', DATEADD(DAY, -320, GETDATE())),
(11, 11, 'credit card', 90.00, 'Completed', DATEADD(DAY, -315, GETDATE())),
(12, 12, 'credit card', 55.00, 'Completed', DATEADD(DAY, -310, GETDATE())),
(13, 13, 'gift card', 75.00, 'Completed', DATEADD(DAY, -305, GETDATE())),
(14, 14, 'credit card', 65.00, 'Completed', DATEADD(DAY, -300, GETDATE())),
(15, 15, 'credit card', 80.00, 'Completed', DATEADD(DAY, -295, GETDATE())),
(16, 16, 'credit card', 70.00, 'Completed', DATEADD(DAY, -290, GETDATE())),
(17, 17, 'credit card', 85.00, 'Completed', DATEADD(DAY, -285, GETDATE())),
(18, 18, 'credit card', 75.00, 'Pending', DATEADD(DAY, -280, GETDATE())),
(19, 19, 'client credit', 80.00, 'Completed', DATEADD(DAY, -275, GETDATE())),
(20, 20, 'credit card', 60.00, 'Completed', DATEADD(DAY, -270, GETDATE())),
(1, 21, 'credit card', 90.00, 'Completed', DATEADD(DAY, -265, GETDATE())),
(2, 22, 'gift card', 75.00, 'Completed', DATEADD(DAY, -260, GETDATE())),
(3, 23, 'credit card', 65.00, 'Pending', DATEADD(DAY, -255, GETDATE())),
(4, 24, 'credit card', 95.00, 'Completed', DATEADD(DAY, -250, GETDATE())),
(5, 25, 'credit card', 85.00, 'Completed', DATEADD(DAY, -245, GETDATE())),
(6, 26, 'client credit', 55.00, 'Failed', DATEADD(DAY, -240, GETDATE())),
(7, 27, 'credit card', 65.00, 'Completed', DATEADD(DAY, -235, GETDATE())),
(8, 28, 'gift card', 50.00, 'Completed', DATEADD(DAY, -230, GETDATE())),
(9, 29, 'credit card', 90.00, 'Completed', DATEADD(DAY, -225, GETDATE())),
(10, 30, 'credit card', 120.00, 'Completed', DATEADD(DAY, -220, GETDATE())),
(11, 31, 'credit card', 75.00, 'Completed', DATEADD(DAY, -215, GETDATE())),
(12, 32, 'credit card', 100.00, 'Completed', DATEADD(DAY, -210, GETDATE())),
(13, 33, 'credit card', 90.00, 'Completed', DATEADD(DAY, -205, GETDATE())),
(14, 34, 'gift card', 95.00, 'Completed', DATEADD(DAY, -200, GETDATE())),
(15, 35, 'credit card', 70.00, 'Pending', DATEADD(DAY, -195, GETDATE())),
(16, 36, 'credit card', 85.00, 'Completed', DATEADD(DAY, -190, GETDATE())),
(17, 37, 'client credit', 65.00, 'Completed', DATEADD(DAY, -185, GETDATE())),
(18, 38, 'credit card', 55.00, 'Completed', DATEADD(DAY, -180, GETDATE())),
(19, 39, 'credit card', 90.00, 'Completed', DATEADD(DAY, -175, GETDATE())),
(20, 40, 'gift card', 80.00, 'Completed', DATEADD(DAY, -170, GETDATE())),
(1, 41, 'credit card', 60.00, 'Pending', DATEADD(DAY, -165, GETDATE())),
(2, 42, 'credit card', 75.00, 'Completed', DATEADD(DAY, -160, GETDATE())),
(3, 43, 'credit card', 65.00, 'Completed', DATEADD(DAY, -155, GETDATE())),
(4, 44, 'client credit', 70.00, 'Failed', DATEADD(DAY, -150, GETDATE())),
(5, 45, 'credit card', 85.00, 'Completed', DATEADD(DAY, -145, GETDATE())),
(6, 46, 'credit card', 55.00, 'Completed', DATEADD(DAY, -140, GETDATE())),
(7, 47, 'credit card', 100.00, 'Completed', DATEADD(DAY, -135, GETDATE())),
(8, 48, 'client credit', 65.00, 'Completed', DATEADD(DAY, -130, GETDATE())),
(9, 49, 'gift card', 55.00, 'Completed', DATEADD(DAY, -125, GETDATE())),
(10, 50, 'credit card', 70.00, 'Completed', DATEADD(DAY, -120, GETDATE())),
(11, 51, 'credit card', 95.00, 'Completed', DATEADD(DAY, -115, GETDATE())),
(12, 52, 'credit card', 75.00, 'Completed', DATEADD(DAY, -110, GETDATE())),
(13, 53, 'gift card', 85.00, 'Completed', DATEADD(DAY, -105, GETDATE())),
(14, 54, 'credit card', 65.00, 'Completed', DATEADD(DAY, -100, GETDATE())),
(15, 55, 'client credit', 60.00, 'Completed', DATEADD(DAY, -95, GETDATE())),
(16, 56, 'credit card', 70.00, 'Completed', DATEADD(DAY, -90, GETDATE())),
(17, 57, 'gift card', 75.00, 'Completed', DATEADD(DAY, -85, GETDATE())),
(18, 58, 'credit card', 95.00, 'Completed', DATEADD(DAY, -80, GETDATE())),
(19, 59, 'client credit', 80.00, 'Completed', DATEADD(DAY, -75, GETDATE())),
(20, 60, 'credit card', 85.00, 'Completed', DATEADD(DAY, -70, GETDATE())),
(1, 61, 'credit card', 70.00, 'Completed', DATEADD(DAY, -65, GETDATE())),
(2, 62, 'client credit', 55.00, 'Completed', DATEADD(DAY, -60, GETDATE())),
(3, 63, 'credit card', 75.00, 'Completed', DATEADD(DAY, -55, GETDATE())),
(4, 64, 'gift card', 65.00, 'Completed', DATEADD(DAY, -50, GETDATE())),
(5, 65, 'credit card', 85.00, 'Completed', DATEADD(DAY, -45, GETDATE())),
(6, 66, 'client credit', 70.00, 'Completed', DATEADD(DAY, -40, GETDATE())),
(7, 67, 'credit card', 75.00, 'Completed', DATEADD(DAY, -35, GETDATE())),
(8, 68, 'credit card', 60.00, 'Completed', DATEADD(DAY, -30, GETDATE())),
(9, 69, 'client credit', 85.00, 'Completed', DATEADD(DAY, -25, GETDATE())),
(10, 70, 'credit card', 90.00, 'Completed', DATEADD(DAY, -20, GETDATE()));

---- Invoices
--INSERT INTO Invoices (OrderId, IssueDate, DueDate, TotalAmount, Paid) VALUES 
--(1, '2024-08-01', '2024-08-10', 50.00, 1),
--(2, '2024-08-02', '2024-08-11', 75.00, 1),
--(3, '2024-08-03', '2024-08-12', 65.00, 1),
--(4, '2024-08-04', '2024-08-13', 45.00, 0),
--(5, '2024-08-05', '2024-08-14', 95.00, 1),
--(6, '2024-08-06', '2024-08-15', 85.00, 0),
--(7, '2024-08-07', '2024-08-16', 35.00, 1),
--(8, '2024-08-08', '2024-08-17', 25.00, 1),
--(9, '2024-08-09', '2024-08-18', 15.00, 0),
--(10, '2024-08-10', '2024-08-19', 55.00, 1),
--(11, '2024-08-11', '2024-08-20', 65.00, 0),
--(12, '2024-08-12', '2024-08-21', 75.00, 1),
--(13, '2024-08-13', '2024-08-22', 85.00, 0),
--(14, '2024-08-14', '2024-08-23', 95.00, 1),
--(15, '2024-08-15', '2024-08-24', 105.00, 0),
--(16, '2024-08-16', '2024-08-25', 115.00, 1),
--(17, '2024-08-17', '2024-08-26', 125.00, 1),
--(18, '2024-08-18', '2024-08-27', 135.00, 1),
--(19, '2024-08-19', '2024-08-28', 145.00, 0),
--(20, '2024-08-20', '2024-08-29', 155.00, 1);



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

-- Insert Ingredients (40 different ingredients)
INSERT INTO Ingredients (IngredientName, Unit, ItemsPerUnit, CreatedAt, UpdatedAt) VALUES
('Onions', 'Bag', 30, GETDATE(), GETDATE()),
('Green Chili', 'Bag', 50, GETDATE(), GETDATE()),
('Capsicum', 'Basket', 10, GETDATE(), GETDATE()),
('Cauliflower', 'Individual', 1, GETDATE(), GETDATE()),
('Rice', 'Bag', 25, GETDATE(), GETDATE()),
('Broccoli', 'Basket', 5, GETDATE(), GETDATE()),
('Spinach', 'Bunch', 10, GETDATE(), GETDATE()),
('Curry Leaves', 'Bunch', 15, GETDATE(), GETDATE()),
('Chicken', 'Box', 5, GETDATE(), GETDATE()),
('Lamb', 'Box', 5, GETDATE(), GETDATE()),
('Flour', 'Bag', 20, GETDATE(), GETDATE()),
('Oil', 'Bottle', 1, GETDATE(), GETDATE()),
('Ginger', 'Bunch', 15, GETDATE(), GETDATE()),
('Garlic', 'Bunch', 20, GETDATE(), GETDATE()),
('Wine', 'Bottle', 1, GETDATE(), GETDATE()),
('Tomatoes', 'Bag', 20, GETDATE(), GETDATE()),
('Cilantro', 'Bunch', 10, GETDATE(), GETDATE()),
('Mint Leaves', 'Bunch', 10, GETDATE(), GETDATE()),
('Carrots', 'Bag', 20, GETDATE(), GETDATE()),
('Potatoes', 'Bag', 15, GETDATE(), GETDATE()),
('Peas', 'Bag', 10, GETDATE(), GETDATE()),
('Yogurt', 'Box', 6, GETDATE(), GETDATE()),
('Butter', 'Box', 10, GETDATE(), GETDATE()),
('Paneer', 'Box', 5, GETDATE(), GETDATE()),
('Cumin Seeds', 'Bag', 50, GETDATE(), GETDATE()),
('Mustard Seeds', 'Bag', 50, GETDATE(), GETDATE()),
('Bay Leaves', 'Bunch', 25, GETDATE(), GETDATE()),
('Turmeric Powder', 'Bag', 30, GETDATE(), GETDATE()),
('Red Chili Powder', 'Bag', 30, GETDATE(), GETDATE()),
('Coriander Powder', 'Bag', 30, GETDATE(), GETDATE()),
('Cardamom', 'Bag', 20, GETDATE(), GETDATE()),
('Cloves', 'Bag', 15, GETDATE(), GETDATE()),
('Black Pepper', 'Bag', 50, GETDATE(), GETDATE()),
('Sugar', 'Bag', 25, GETDATE(), GETDATE()),
('Salt', 'Bag', 50, GETDATE(), GETDATE()),
('Honey', 'Bottle', 1, GETDATE(), GETDATE()),
('Milk', 'Bottle', 1, GETDATE(), GETDATE()),
('Vanilla Extract', 'Bottle', 1, GETDATE(), GETDATE()),
('Almonds', 'Bag', 20, GETDATE(), GETDATE()),
('Coconut Milk', 'Box', 5, GETDATE(), GETDATE());

-- Insert ItemIngredients (Link Items to Ingredients with quantity needed per size)
-- Total of 60 rows for at least 2 ingredients per item

-- Item 1: Samosas (CategoryId 8)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(1, 1, 'Medium', 2, GETDATE(), GETDATE()),  -- Onions
(1, 11, 'Medium', 1, GETDATE(), GETDATE()); -- Flour

-- Item 2: Pani Puri (CategoryId 8)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(2, 2, 'Medium', 5, GETDATE(), GETDATE()),  -- Green Chili
(2, 11, 'Medium', 1, GETDATE(), GETDATE()); -- Flour

-- Item 3: Falafel (CategoryId 9)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(3, 3, 'Medium', 1, GETDATE(), GETDATE()),  -- Capsicum
(3, 13, 'Medium', 1, GETDATE(), GETDATE()); -- Ginger

-- Item 4: Hummus (CategoryId 9)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(4, 7, 'Medium', 2, GETDATE(), GETDATE()),  -- Spinach
(4, 12, 'Medium', 1, GETDATE(), GETDATE()); -- Oil

-- Item 5: Butter Chicken (CategoryId 11)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(5, 9, 'Medium', 2, GETDATE(), GETDATE()),  -- Chicken
(5, 17, 'Medium', 2, GETDATE(), GETDATE()); -- Tomatoes

-- Item 6: Chicken Tikka Masala (CategoryId 11)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(6, 9, 'Medium', 3, GETDATE(), GETDATE()),  -- Chicken
(6, 18, 'Medium', 1, GETDATE(), GETDATE()); -- Mint Leaves

-- Item 7: Biryani (CategoryId 11)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(7, 5, 'Medium', 1, GETDATE(), GETDATE()),  -- Rice
(7, 8, 'Medium', 2, GETDATE(), GETDATE());  -- Curry Leaves

-- Item 8: Rogan Josh (CategoryId 11)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(8, 10, 'Medium', 2, GETDATE(), GETDATE()), -- Lamb
(8, 19, 'Medium', 1, GETDATE(), GETDATE()); -- Carrots

-- Item 9: Palak Paneer (CategoryId 11)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(9, 7, 'Medium', 3, GETDATE(), GETDATE()),  -- Spinach
(9, 24, 'Medium', 2, GETDATE(), GETDATE()); -- Paneer

-- Item 10: Vindaloo (CategoryId 11)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(10, 1, 'Medium', 2, GETDATE(), GETDATE()), -- Onions
(10, 28, 'Medium', 1, GETDATE(), GETDATE()); -- Turmeric Powder

-- Item 11: Gyro (CategoryId 12)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(11, 10, 'Medium', 2, GETDATE(), GETDATE()), -- Lamb
(11, 3, 'Medium', 1, GETDATE(), GETDATE());  -- Capsicum

-- Item 12: Shawarma (CategoryId 12)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(12, 9, 'Medium', 2, GETDATE(), GETDATE()),  -- Chicken
(12, 17, 'Medium', 2, GETDATE(), GETDATE()); -- Tomatoes

-- Item 13: Burritos (CategoryId 13)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(13, 20, 'Medium', 3, GETDATE(), GETDATE()), -- Potatoes
(13, 2, 'Medium', 4, GETDATE(), GETDATE());  -- Green Chili

-- Item 14: Tacos (CategoryId 13)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(14, 3, 'Medium', 1, GETDATE(), GETDATE()),  -- Capsicum
(14, 21, 'Medium', 2, GETDATE(), GETDATE()); -- Peas


-- Item 15: Quesadillas (CategoryId 13)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(15, 11, 'Medium', 1, GETDATE(), GETDATE()),  -- Flour
(15, 23, 'Medium', 2, GETDATE(), GETDATE());  -- Butter

-- Item 16: Enchiladas (CategoryId 13)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(16, 17, 'Medium', 2, GETDATE(), GETDATE()),  -- Tomatoes
(16, 20, 'Medium', 3, GETDATE(), GETDATE());  -- Potatoes

-- Item 17: Tostadas (CategoryId 13)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(17, 3, 'Medium', 1, GETDATE(), GETDATE()),   -- Capsicum
(17, 28, 'Medium', 1, GETDATE(), GETDATE());  -- Turmeric Powder

-- Item 18: Gulab Jamun (CategoryId 14)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(18, 34, 'Medium', 2, GETDATE(), GETDATE()),  -- Sugar
(18, 23, 'Medium', 1, GETDATE(), GETDATE());  -- Butter

-- Item 19: Baklava (CategoryId 15)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(19, 39, 'Medium', 1, GETDATE(), GETDATE()),  -- Almonds
(19, 36, 'Medium', 1, GETDATE(), GETDATE());  -- Honey

-- Item 20: Churros (CategoryId 16)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(20, 11, 'Medium', 1, GETDATE(), GETDATE()),  -- Flour
(20, 34, 'Medium', 2, GETDATE(), GETDATE());  -- Sugar

-- Item 21: Mango Lassi (CategoryId 17)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(21, 22, 'Medium', 1, GETDATE(), GETDATE()),  -- Yogurt
(21, 37, 'Medium', 1, GETDATE(), GETDATE());  -- Milk

-- Item 22: Turkish Coffee (CategoryId 18)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(22, 33, 'Medium', 1, GETDATE(), GETDATE()),  -- Black Pepper (as spice)
(22, 37, 'Medium', 1, GETDATE(), GETDATE());  -- Milk

-- Item 23: Horchata (CategoryId 19)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(23, 5, 'Medium', 1, GETDATE(), GETDATE()),   -- Rice
(23, 34, 'Medium', 2, GETDATE(), GETDATE());  -- Sugar

-- Item 24: Naan Bread (CategoryId 20)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(24, 11, 'Medium', 1, GETDATE(), GETDATE()),  -- Flour
(24, 23, 'Medium', 1, GETDATE(), GETDATE());  -- Butter

-- Item 25: Pita Bread (CategoryId 21)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(25, 11, 'Medium', 1, GETDATE(), GETDATE()),  -- Flour
(25, 12, 'Medium', 1, GETDATE(), GETDATE());  -- Oil

-- Item 26: Mexican Rice (CategoryId 22)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(26, 5, 'Medium', 1, GETDATE(), GETDATE()),   -- Rice
(26, 2, 'Medium', 3, GETDATE(), GETDATE());   -- Green Chili

-- Item 27: Spanakopita (CategoryId 23)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(27, 7, 'Medium', 2, GETDATE(), GETDATE()),   -- Spinach
(27, 23, 'Medium', 1, GETDATE(), GETDATE());  -- Butter

-- Item 28: Dolma (CategoryId 23)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(28, 8, 'Medium', 1, GETDATE(), GETDATE()),   -- Curry Leaves
(28, 16, 'Medium', 1, GETDATE(), GETDATE());  -- Tomatoes

-- Item 29: Tamales (CategoryId 24)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(29, 11, 'Medium', 1, GETDATE(), GETDATE()),  -- Flour
(29, 20, 'Medium', 2, GETDATE(), GETDATE());  -- Potatoes

-- Item 30: Empanadas (CategoryId 24)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(30, 11, 'Medium', 1, GETDATE(), GETDATE()),  -- Flour
(30, 34, 'Medium', 2, GETDATE(), GETDATE());  -- Sugar

-- Item 31: Aloo Tikki (CategoryId 8)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(31, 1, 'Medium', 2, GETDATE(), GETDATE()),  -- Onions
(31, 20, 'Medium', 2, GETDATE(), GETDATE());  -- Potatoes

-- Item 32: Dolma (CategoryId 9)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(32, 8, 'Medium', 1, GETDATE(), GETDATE()),  -- Curry Leaves
(32, 16, 'Medium', 2, GETDATE(), GETDATE());  -- Tomatoes

-- Item 33: Elote (CategoryId 10)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(33, 3, 'Medium', 2, GETDATE(), GETDATE()),  -- Capsicum
(33, 17, 'Medium', 1, GETDATE(), GETDATE());  -- Tomatoes

-- Item 34: Paneer Butter Masala (CategoryId 11)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(34, 24, 'Medium', 2, GETDATE(), GETDATE()),  -- Paneer
(34, 23, 'Medium', 1, GETDATE(), GETDATE());  -- Butter

-- Item 35: Lamb Kebab (CategoryId 12)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(35, 10, 'Medium', 2, GETDATE(), GETDATE()),  -- Lamb
(35, 3, 'Medium', 1, GETDATE(), GETDATE());  -- Capsicum

-- Item 36: Chicken Fajitas (CategoryId 13)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(36, 9, 'Medium', 2, GETDATE(), GETDATE()),  -- Chicken
(36, 3, 'Medium', 1, GETDATE(), GETDATE());  -- Capsicum

-- Item 37: Rasmalai (CategoryId 14)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(37, 22, 'Medium', 1, GETDATE(), GETDATE()),  -- Yogurt
(37, 34, 'Medium', 2, GETDATE(), GETDATE());  -- Sugar

-- Item 38: Knafeh (CategoryId 15)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(38, 39, 'Medium', 2, GETDATE(), GETDATE()),  -- Almonds
(38, 36, 'Medium', 1, GETDATE(), GETDATE());  -- Honey

-- Item 39: Tres Leches Cake (CategoryId 16)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(39, 37, 'Medium', 1, GETDATE(), GETDATE()),  -- Milk
(39, 34, 'Medium', 2, GETDATE(), GETDATE());  -- Sugar

-- Item 40: Masala Chai (CategoryId 17)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(40, 13, 'Medium', 1, GETDATE(), GETDATE()),  -- Ginger
(40, 14, 'Medium', 2, GETDATE(), GETDATE());  -- Garlic

-- Item 41: Samosas (CategoryId 8)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(41, 1, 'Medium', 2, GETDATE(), GETDATE()),  -- Onions
(41, 20, 'Medium', 3, GETDATE(), GETDATE());  -- Potatoes

-- Item 42: Pani Puri (CategoryId 8)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(42, 2, 'Medium', 5, GETDATE(), GETDATE()),  -- Green Chili
(42, 11, 'Medium', 1, GETDATE(), GETDATE());  -- Flour

-- Item 43: Falafel (CategoryId 9)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(43, 3, 'Medium', 1, GETDATE(), GETDATE()),  -- Capsicum
(43, 13, 'Medium', 1, GETDATE(), GETDATE());  -- Ginger

-- Item 44: Hummus (CategoryId 9)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(44, 7, 'Medium', 2, GETDATE(), GETDATE()),  -- Spinach
(44, 12, 'Medium', 1, GETDATE(), GETDATE());  -- Oil

-- Item 45: Tabbouleh (CategoryId 9)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(45, 7, 'Medium', 1, GETDATE(), GETDATE()),  -- Spinach
(45, 17, 'Medium', 1, GETDATE(), GETDATE());  -- Tomatoes

-- Item 46: Baba Ganoush (CategoryId 9)
INSERT INTO ItemIngredients (ItemId, IngredientId, Size, QuantityNeeded, CreatedAt, UpdatedAt) VALUES
(46, 3, 'Medium', 1, GETDATE(), GETDATE()),  -- Capsicum
(46, 17, 'Medium', 2, GETDATE(), GETDATE());  -- Tomatoes


-- Insert Inventory Data (for 40 Ingredients)
INSERT INTO Inventory (IngredientId, CurrentStock, LowStockThreshold, CreatedAt, UpdatedAt) VALUES
(1, 200, 30, GETDATE(), GETDATE()),   -- Onions
(2, 300, 50, GETDATE(), GETDATE()),   -- Green Chili
(3, 100, 20, GETDATE(), GETDATE()),   -- Capsicum
(4, 50, 10, GETDATE(), GETDATE()),    -- Cauliflower
(5, 150, 40, GETDATE(), GETDATE()),   -- Rice
(6, 75, 15, GETDATE(), GETDATE()),    -- Broccoli
(7, 60, 15, GETDATE(), GETDATE()),    -- Spinach
(8, 90, 25, GETDATE(), GETDATE()),    -- Curry Leaves
(9, 40, 10, GETDATE(), GETDATE()),    -- Chicken
(10, 30, 10, GETDATE(), GETDATE()),   -- Lamb
(11, 100, 25, GETDATE(), GETDATE()),  -- Flour
(12, 20, 5, GETDATE(), GETDATE()),    -- Oil
(13, 50, 10, GETDATE(), GETDATE()),   -- Ginger
(14, 40, 10, GETDATE(), GETDATE()),   -- Garlic
(15, 10, 2, GETDATE(), GETDATE()),    -- Wine
(16, 70, 15, GETDATE(), GETDATE()),   -- Tomatoes
(17, 50, 10, GETDATE(), GETDATE()),   -- Cilantro
(18, 60, 15, GETDATE(), GETDATE()),   -- Mint Leaves
(19, 40, 10, GETDATE(), GETDATE()),   -- Carrots
(20, 50, 15, GETDATE(), GETDATE()),   -- Potatoes
(21, 30, 10, GETDATE(), GETDATE()),   -- Peas
(22, 25, 5, GETDATE(), GETDATE()),    -- Yogurt
(23, 40, 10, GETDATE(), GETDATE()),   -- Butter
(24, 20, 5, GETDATE(), GETDATE()),    -- Paneer
(25, 90, 25, GETDATE(), GETDATE()),   -- Cumin Seeds
(26, 70, 20, GETDATE(), GETDATE()),   -- Mustard Seeds
(27, 60, 20, GETDATE(), GETDATE()),   -- Bay Leaves
(28, 80, 25, GETDATE(), GETDATE()),   -- Turmeric Powder
(29, 60, 20, GETDATE(), GETDATE()),   -- Red Chili Powder
(30, 70, 20, GETDATE(), GETDATE()),   -- Coriander Powder
(31, 30, 10, GETDATE(), GETDATE()),   -- Cardamom
(32, 25, 8, GETDATE(), GETDATE()),    -- Cloves
(33, 100, 30, GETDATE(), GETDATE()),  -- Black Pepper
(34, 150, 50, GETDATE(), GETDATE()),  -- Sugar
(35, 100, 30, GETDATE(), GETDATE()),  -- Salt
(36, 15, 5, GETDATE(), GETDATE()),    -- Honey
(37, 25, 10, GETDATE(), GETDATE()),   -- Milk
(38, 10, 3, GETDATE(), GETDATE()),    -- Vanilla Extract
(39, 40, 15, GETDATE(), GETDATE()),   -- Almonds
(40, 20, 5, GETDATE(), GETDATE());    -- Coconut Milk


update Items
 set Description = 'Indulge in our mouthwatering creation, carefully crafted to offer the perfect blend of bold flavors and wholesome goodness. Tender pieces of protein are marinated in a special blend of spices, wrapped in a warm, fluffy tortilla or bun. Each bite bursts with vibrant flavors, complemented by a crisp mix of fresh vegetables.

A drizzle of our signature sauce adds a touch of creamy heat, perfectly balanced by the crunch of the veggies. To enhance the richness, a sprinkle of shredded cheese completes the experience, making every bite satisfying and full of texture.

Whether you''re craving something with a kick or simply looking for a hearty, fulfilling meal, this dish is made with the freshest ingredients to deliver quality you can taste. It''s perfect for lunch, dinner, or whenever you''re in the mood for something comforting and flavorful.'
where itemId > 0;