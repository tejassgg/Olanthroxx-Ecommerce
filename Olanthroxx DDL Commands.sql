
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/19/2026 23:20:40
-- Generated from EDMX file: C:\Users\Tejas\OneDrive\Desktop\Desktop Files\Olanthroxxdotnet-master\WebAPI\Olanthroxx.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Olanthroxxx];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_tblBudgetDetails_tblUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBudgetDetails] DROP CONSTRAINT [FK_tblBudgetDetails_tblUser];
GO
IF OBJECT_ID(N'[dbo].[FK_tblMasCity_tblMasState]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblMasCity] DROP CONSTRAINT [FK_tblMasCity_tblMasState];
GO
IF OBJECT_ID(N'[dbo].[FK_tblMovieBookingHistory_tblMovieScreenTimingConfig]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblMovieBookingHistory] DROP CONSTRAINT [FK_tblMovieBookingHistory_tblMovieScreenTimingConfig];
GO
IF OBJECT_ID(N'[dbo].[FK_tblMovieScreenTimingConfig_tblMovieDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblMovieScreenTimingConfig] DROP CONSTRAINT [FK_tblMovieScreenTimingConfig_tblMovieDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_tblMovieScreenTimingConfig_tblScreenDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblMovieScreenTimingConfig] DROP CONSTRAINT [FK_tblMovieScreenTimingConfig_tblScreenDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_tblOrderDetails_tblProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblOrderDetails] DROP CONSTRAINT [FK_tblOrderDetails_tblProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_tblProduct_tblCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblProduct] DROP CONSTRAINT [FK_tblProduct_tblCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_tblProductReview_tblProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblProductReview] DROP CONSTRAINT [FK_tblProductReview_tblProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_tblScreenDetails_tblTheaterDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblScreenDetails] DROP CONSTRAINT [FK_tblScreenDetails_tblTheaterDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_tblTempCartDetails_tblUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblTempCartDetails] DROP CONSTRAINT [FK_tblTempCartDetails_tblUser];
GO
IF OBJECT_ID(N'[dbo].[FK_tblUserDetails_tblUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblUserDetails] DROP CONSTRAINT [FK_tblUserDetails_tblUser];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[tblAddress]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblAddress];
GO
IF OBJECT_ID(N'[dbo].[tblApplicationSetting]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblApplicationSetting];
GO
IF OBJECT_ID(N'[dbo].[tblBudgetDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBudgetDetails];
GO
IF OBJECT_ID(N'[dbo].[tblCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCategory];
GO
IF OBJECT_ID(N'[dbo].[tblExpense]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblExpense];
GO
IF OBJECT_ID(N'[dbo].[tblForgotPasswordHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblForgotPasswordHistory];
GO
IF OBJECT_ID(N'[dbo].[tblLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblLogs];
GO
IF OBJECT_ID(N'[dbo].[tblMasCity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblMasCity];
GO
IF OBJECT_ID(N'[dbo].[tblMasCommonType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblMasCommonType];
GO
IF OBJECT_ID(N'[dbo].[tblMasState]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblMasState];
GO
IF OBJECT_ID(N'[dbo].[tblMovieBookingHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblMovieBookingHistory];
GO
IF OBJECT_ID(N'[dbo].[tblMovieDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblMovieDetails];
GO
IF OBJECT_ID(N'[dbo].[tblMovieScreenTimingConfig]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblMovieScreenTimingConfig];
GO
IF OBJECT_ID(N'[dbo].[tblOrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblOrderDetails];
GO
IF OBJECT_ID(N'[dbo].[tblPayment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPayment];
GO
IF OBJECT_ID(N'[dbo].[tblProduct]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblProduct];
GO
IF OBJECT_ID(N'[dbo].[tblProductReview]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblProductReview];
GO
IF OBJECT_ID(N'[dbo].[tblScreenDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblScreenDetails];
GO
IF OBJECT_ID(N'[dbo].[tblTempCartDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTempCartDetails];
GO
IF OBJECT_ID(N'[dbo].[tblTheaterDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTheaterDetails];
GO
IF OBJECT_ID(N'[dbo].[tblUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblUser];
GO
IF OBJECT_ID(N'[dbo].[tblUserDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblUserDetails];
GO
IF OBJECT_ID(N'[dbo].[tblUserSessionDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblUserSessionDetails];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'tblApplicationSettings'
CREATE TABLE [dbo].[tblApplicationSettings] (
    [KeyID] int  NOT NULL,
    [KeyName] nvarchar(20)  NULL,
    [KeyValue] nvarchar(500)  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'tblLogs'
CREATE TABLE [dbo].[tblLogs] (
    [LogID] int IDENTITY(1,1) NOT NULL,
    [IdentificationNo] nvarchar(20)  NOT NULL,
    [Data] nvarchar(max)  NOT NULL,
    [CreatedBy] nvarchar(20)  NOT NULL,
    [CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'tblMasCommonTypes'
CREATE TABLE [dbo].[tblMasCommonTypes] (
    [CommonID] int IDENTITY(1,1) NOT NULL,
    [Code] int  NULL,
    [MasterType] nvarchar(50)  NULL,
    [Description] nvarchar(150)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'tblUsers'
CREATE TABLE [dbo].[tblUsers] (
    [AccountID] int IDENTITY(1,1) NOT NULL,
    [UserID] uniqueidentifier  NOT NULL,
    [UserName] nvarchar(20)  NOT NULL,
    [PasswordSalt] nchar(20)  NOT NULL,
    [PasswordHistory] nvarchar(max)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [LastPasswordChange] datetime  NOT NULL,
    [LoginType] nvarchar(50)  NULL,
    [Password] nvarchar(500)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'tblUserSessionDetails'
CREATE TABLE [dbo].[tblUserSessionDetails] (
    [SessionID] int  NOT NULL,
    [AccountID] int  NULL,
    [SessionDateTime] datetime  NULL,
    [Port] nvarchar(50)  NULL,
    [UserType] nvarchar(20)  NULL,
    [Username] nvarchar(20)  NULL,
    [SessionComments] nvarchar(150)  NULL
);
GO

-- Creating table 'tblCategories'
CREATE TABLE [dbo].[tblCategories] (
    [CategoryID] int IDENTITY(1,1) NOT NULL,
    [CategoryType] nvarchar(50)  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'tblPayments'
CREATE TABLE [dbo].[tblPayments] (
    [PaymentID] int IDENTITY(1,1) NOT NULL,
    [OrderID] uniqueidentifier  NOT NULL,
    [UserName] nvarchar(20)  NOT NULL,
    [TotalPayableAmount] int  NOT NULL,
    [PaymentMode] nvarchar(50)  NOT NULL,
    [PaymentStatus] int  NOT NULL,
    [PaymentDetails] nvarchar(max)  NOT NULL,
    [TransactionNo] nvarchar(50)  NOT NULL,
    [CreatedDate] datetime  NULL,
    [ChequeNo] nvarchar(50)  NULL,
    [ChequeDate] datetime  NULL,
    [AccountNo] nvarchar(50)  NULL,
    [UPI_ID] nvarchar(50)  NULL,
    [IFSC_Code] nvarchar(50)  NULL,
    [PaymentOf] nvarchar(30)  NULL
);
GO

-- Creating table 'tblUserDetails'
CREATE TABLE [dbo].[tblUserDetails] (
    [UserDetailsID] int IDENTITY(1,1) NOT NULL,
    [AccountID_FK] int  NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [MidName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [MobileNo] nvarchar(10)  NOT NULL,
    [EmailID] nvarchar(50)  NOT NULL,
    [AadharNumber] nchar(14)  NULL,
    [State] nvarchar(50)  NOT NULL,
    [City] nvarchar(50)  NOT NULL,
    [PinCode] int  NOT NULL,
    [FullAddress] nvarchar(250)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserImgPath] nvarchar(120)  NULL
);
GO

-- Creating table 'tblMasStates'
CREATE TABLE [dbo].[tblMasStates] (
    [StateID] int IDENTITY(1,1) NOT NULL,
    [State] nvarchar(50)  NOT NULL,
    [CountryID] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'tblForgotPasswordHistories'
CREATE TABLE [dbo].[tblForgotPasswordHistories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(50)  NOT NULL,
    [AttemptNo] int  NOT NULL,
    [MaxNoOfAttempts] int  NOT NULL,
    [ExpiryTime] datetime  NOT NULL,
    [Link] nvarchar(150)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [Key] uniqueidentifier  NOT NULL,
    [PasswordChangedDate] datetime  NULL,
    [IsValid] bit  NULL
);
GO

-- Creating table 'tblExpenses'
CREATE TABLE [dbo].[tblExpenses] (
    [ExpID] int IDENTITY(1,1) NOT NULL,
    [BudgetID_FK] int  NOT NULL,
    [ExpName] nvarchar(50)  NOT NULL,
    [Amount] int  NOT NULL,
    [UserID] nvarchar(20)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL
);
GO

-- Creating table 'tblBudgetDetails'
CREATE TABLE [dbo].[tblBudgetDetails] (
    [BudgetID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Amount] int  NOT NULL,
    [Balance] int  NOT NULL,
    [AccountID_FK] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL
);
GO

-- Creating table 'tblProducts'
CREATE TABLE [dbo].[tblProducts] (
    [ProductID] int IDENTITY(1,1) NOT NULL,
    [PName] nvarchar(50)  NOT NULL,
    [PTitle] nvarchar(200)  NOT NULL,
    [PDescription] nvarchar(max)  NOT NULL,
    [CategoryID_FK] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [Price] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL,
    [SellerName] nvarchar(50)  NULL,
    [ImgPath] nvarchar(max)  NULL,
    [DiscountPer] int  NULL,
    [ItemsSold] int  NULL
);
GO

-- Creating table 'tblProductReviews'
CREATE TABLE [dbo].[tblProductReviews] (
    [ReviewID] int IDENTITY(1,1) NOT NULL,
    [ProductID_FK] int  NOT NULL,
    [OrderID_FK] uniqueidentifier  NOT NULL,
    [Rating] int  NOT NULL,
    [Review] nvarchar(250)  NOT NULL,
    [ImgPath] nvarchar(100)  NULL,
    [UserID] nvarchar(20)  NOT NULL,
    [CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'tblMasCities'
CREATE TABLE [dbo].[tblMasCities] (
    [CityID] int IDENTITY(1,1) NOT NULL,
    [StateID_FK] int  NOT NULL,
    [City] nvarchar(30)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [CreatedDate] nvarchar(30)  NULL
);
GO

-- Creating table 'tblAddresses'
CREATE TABLE [dbo].[tblAddresses] (
    [AddressID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(25)  NOT NULL,
    [Type] varchar(10)  NULL,
    [UserID] nvarchar(20)  NOT NULL,
    [CityID] int  NOT NULL,
    [StateID] int  NOT NULL,
    [Pincode] int  NOT NULL,
    [FullAddress] nvarchar(150)  NOT NULL,
    [Address1] nvarchar(50)  NULL,
    [Address2] nvarchar(50)  NULL,
    [Landmark] nvarchar(50)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL
);
GO

-- Creating table 'tblOrderDetails'
CREATE TABLE [dbo].[tblOrderDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [OrderID] uniqueidentifier  NOT NULL,
    [ProductID] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [Amount] int  NOT NULL,
    [UserID] nvarchar(50)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [OrderStatus] int  NULL,
    [ModifiedDate] datetime  NULL,
    [ShippingAddressID_FK] int  NULL,
    [BillingAddressID_FK] int  NULL,
    [OTPToBeShown] bit  NULL,
    [OTP] int  NULL
);
GO

-- Creating table 'tblTheaterDetails'
CREATE TABLE [dbo].[tblTheaterDetails] (
    [TheaterID] int IDENTITY(1,1) NOT NULL,
    [UserName_FK] nvarchar(20)  NULL,
    [Name] nvarchar(50)  NOT NULL,
    [NoOfScreens] int  NOT NULL,
    [Pincode] int  NOT NULL,
    [Location] nvarchar(50)  NOT NULL,
    [Landmark] nvarchar(50)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL,
    [ImgPath] nvarchar(200)  NULL
);
GO

-- Creating table 'tblMovieDetails'
CREATE TABLE [dbo].[tblMovieDetails] (
    [MovieID] int IDENTITY(1,1) NOT NULL,
    [MovieName] nvarchar(50)  NOT NULL,
    [Duration] int  NOT NULL,
    [StarCast] nvarchar(200)  NOT NULL,
    [Description] nvarchar(500)  NOT NULL,
    [ImgPath] nvarchar(150)  NULL,
    [IsActive] bit  NOT NULL,
    [IsOutOfTheater] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL,
    [ReleaseDate] datetime  NULL,
    [BGImgPath] nvarchar(150)  NULL,
    [Language] nvarchar(100)  NULL,
    [MovieType] nvarchar(100)  NULL,
    [IsAdult] bit  NULL,
    [TrailerLink] nvarchar(50)  NULL
);
GO

-- Creating table 'tblMovieBookingHistories'
CREATE TABLE [dbo].[tblMovieBookingHistories] (
    [BookingID] uniqueidentifier  NOT NULL,
    [ScreenTimingConfigID_FK] int  NOT NULL,
    [MovieName] nvarchar(50)  NULL,
    [NoOfSeats] int  NOT NULL,
    [SeatNo] nvarchar(50)  NOT NULL,
    [SeatCategory] int  NULL,
    [Amount] int  NOT NULL,
    [UserName] nvarchar(20)  NOT NULL,
    [IsUsed] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [QRCodeImageData] nvarchar(max)  NULL
);
GO

-- Creating table 'tblMovieScreenTimingConfigs'
CREATE TABLE [dbo].[tblMovieScreenTimingConfigs] (
    [ScreenTimingConfigID] int IDENTITY(1,1) NOT NULL,
    [MovieID_FK] int  NOT NULL,
    [ScreenID_FK] int  NOT NULL,
    [TimingID] int  NOT NULL,
    [IsActive] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL,
    [MovieDate] datetime  NULL
);
GO

-- Creating table 'tblScreenDetails'
CREATE TABLE [dbo].[tblScreenDetails] (
    [ScreenID] int IDENTITY(1,1) NOT NULL,
    [ScreenNo] int  NOT NULL,
    [TheaterID_FK] int  NOT NULL,
    [NoOfSeatsSilver] int  NOT NULL,
    [PriceSilver] int  NOT NULL,
    [NoOfSeatsGold] int  NOT NULL,
    [PriceGold] int  NOT NULL,
    [NoOfSeatsPlatinum] int  NOT NULL,
    [PricePlatinum] int  NOT NULL,
    [NoOfSeatsRecliner] int  NOT NULL,
    [PriceRecliner] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'tblTempCartDetails'
CREATE TABLE [dbo].[tblTempCartDetails] (
    [CartID] int IDENTITY(1,1) NOT NULL,
    [AccountID_FK] int  NOT NULL,
    [TempCartDetails] nvarchar(max)  NOT NULL,
    [SessionID] nvarchar(100)  NOT NULL,
    [IsUsed] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UsedDate] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [KeyID] in table 'tblApplicationSettings'
ALTER TABLE [dbo].[tblApplicationSettings]
ADD CONSTRAINT [PK_tblApplicationSettings]
    PRIMARY KEY CLUSTERED ([KeyID] ASC);
GO

-- Creating primary key on [LogID] in table 'tblLogs'
ALTER TABLE [dbo].[tblLogs]
ADD CONSTRAINT [PK_tblLogs]
    PRIMARY KEY CLUSTERED ([LogID] ASC);
GO

-- Creating primary key on [CommonID] in table 'tblMasCommonTypes'
ALTER TABLE [dbo].[tblMasCommonTypes]
ADD CONSTRAINT [PK_tblMasCommonTypes]
    PRIMARY KEY CLUSTERED ([CommonID] ASC);
GO

-- Creating primary key on [AccountID] in table 'tblUsers'
ALTER TABLE [dbo].[tblUsers]
ADD CONSTRAINT [PK_tblUsers]
    PRIMARY KEY CLUSTERED ([AccountID] ASC);
GO

-- Creating primary key on [SessionID] in table 'tblUserSessionDetails'
ALTER TABLE [dbo].[tblUserSessionDetails]
ADD CONSTRAINT [PK_tblUserSessionDetails]
    PRIMARY KEY CLUSTERED ([SessionID] ASC);
GO

-- Creating primary key on [CategoryID] in table 'tblCategories'
ALTER TABLE [dbo].[tblCategories]
ADD CONSTRAINT [PK_tblCategories]
    PRIMARY KEY CLUSTERED ([CategoryID] ASC);
GO

-- Creating primary key on [PaymentID] in table 'tblPayments'
ALTER TABLE [dbo].[tblPayments]
ADD CONSTRAINT [PK_tblPayments]
    PRIMARY KEY CLUSTERED ([PaymentID] ASC);
GO

-- Creating primary key on [UserDetailsID] in table 'tblUserDetails'
ALTER TABLE [dbo].[tblUserDetails]
ADD CONSTRAINT [PK_tblUserDetails]
    PRIMARY KEY CLUSTERED ([UserDetailsID] ASC);
GO

-- Creating primary key on [StateID] in table 'tblMasStates'
ALTER TABLE [dbo].[tblMasStates]
ADD CONSTRAINT [PK_tblMasStates]
    PRIMARY KEY CLUSTERED ([StateID] ASC);
GO

-- Creating primary key on [ID] in table 'tblForgotPasswordHistories'
ALTER TABLE [dbo].[tblForgotPasswordHistories]
ADD CONSTRAINT [PK_tblForgotPasswordHistories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ExpID] in table 'tblExpenses'
ALTER TABLE [dbo].[tblExpenses]
ADD CONSTRAINT [PK_tblExpenses]
    PRIMARY KEY CLUSTERED ([ExpID] ASC);
GO

-- Creating primary key on [BudgetID] in table 'tblBudgetDetails'
ALTER TABLE [dbo].[tblBudgetDetails]
ADD CONSTRAINT [PK_tblBudgetDetails]
    PRIMARY KEY CLUSTERED ([BudgetID] ASC);
GO

-- Creating primary key on [ProductID] in table 'tblProducts'
ALTER TABLE [dbo].[tblProducts]
ADD CONSTRAINT [PK_tblProducts]
    PRIMARY KEY CLUSTERED ([ProductID] ASC);
GO

-- Creating primary key on [ReviewID] in table 'tblProductReviews'
ALTER TABLE [dbo].[tblProductReviews]
ADD CONSTRAINT [PK_tblProductReviews]
    PRIMARY KEY CLUSTERED ([ReviewID] ASC);
GO

-- Creating primary key on [CityID] in table 'tblMasCities'
ALTER TABLE [dbo].[tblMasCities]
ADD CONSTRAINT [PK_tblMasCities]
    PRIMARY KEY CLUSTERED ([CityID] ASC);
GO

-- Creating primary key on [AddressID] in table 'tblAddresses'
ALTER TABLE [dbo].[tblAddresses]
ADD CONSTRAINT [PK_tblAddresses]
    PRIMARY KEY CLUSTERED ([AddressID] ASC);
GO

-- Creating primary key on [ID] in table 'tblOrderDetails'
ALTER TABLE [dbo].[tblOrderDetails]
ADD CONSTRAINT [PK_tblOrderDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [TheaterID] in table 'tblTheaterDetails'
ALTER TABLE [dbo].[tblTheaterDetails]
ADD CONSTRAINT [PK_tblTheaterDetails]
    PRIMARY KEY CLUSTERED ([TheaterID] ASC);
GO

-- Creating primary key on [MovieID] in table 'tblMovieDetails'
ALTER TABLE [dbo].[tblMovieDetails]
ADD CONSTRAINT [PK_tblMovieDetails]
    PRIMARY KEY CLUSTERED ([MovieID] ASC);
GO

-- Creating primary key on [BookingID] in table 'tblMovieBookingHistories'
ALTER TABLE [dbo].[tblMovieBookingHistories]
ADD CONSTRAINT [PK_tblMovieBookingHistories]
    PRIMARY KEY CLUSTERED ([BookingID] ASC);
GO

-- Creating primary key on [ScreenTimingConfigID] in table 'tblMovieScreenTimingConfigs'
ALTER TABLE [dbo].[tblMovieScreenTimingConfigs]
ADD CONSTRAINT [PK_tblMovieScreenTimingConfigs]
    PRIMARY KEY CLUSTERED ([ScreenTimingConfigID] ASC);
GO

-- Creating primary key on [ScreenID] in table 'tblScreenDetails'
ALTER TABLE [dbo].[tblScreenDetails]
ADD CONSTRAINT [PK_tblScreenDetails]
    PRIMARY KEY CLUSTERED ([ScreenID] ASC);
GO

-- Creating primary key on [CartID] in table 'tblTempCartDetails'
ALTER TABLE [dbo].[tblTempCartDetails]
ADD CONSTRAINT [PK_tblTempCartDetails]
    PRIMARY KEY CLUSTERED ([CartID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AccountID_FK] in table 'tblUserDetails'
ALTER TABLE [dbo].[tblUserDetails]
ADD CONSTRAINT [FK_tblUserDetails_tblUser]
    FOREIGN KEY ([AccountID_FK])
    REFERENCES [dbo].[tblUsers]
        ([AccountID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblUserDetails_tblUser'
CREATE INDEX [IX_FK_tblUserDetails_tblUser]
ON [dbo].[tblUserDetails]
    ([AccountID_FK]);
GO

-- Creating foreign key on [AccountID_FK] in table 'tblBudgetDetails'
ALTER TABLE [dbo].[tblBudgetDetails]
ADD CONSTRAINT [FK_tblBudgetDetails_tblUser]
    FOREIGN KEY ([AccountID_FK])
    REFERENCES [dbo].[tblUsers]
        ([AccountID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBudgetDetails_tblUser'
CREATE INDEX [IX_FK_tblBudgetDetails_tblUser]
ON [dbo].[tblBudgetDetails]
    ([AccountID_FK]);
GO

-- Creating foreign key on [CategoryID_FK] in table 'tblProducts'
ALTER TABLE [dbo].[tblProducts]
ADD CONSTRAINT [FK_tblProduct_tblCategory]
    FOREIGN KEY ([CategoryID_FK])
    REFERENCES [dbo].[tblCategories]
        ([CategoryID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblProduct_tblCategory'
CREATE INDEX [IX_FK_tblProduct_tblCategory]
ON [dbo].[tblProducts]
    ([CategoryID_FK]);
GO

-- Creating foreign key on [ProductID_FK] in table 'tblProductReviews'
ALTER TABLE [dbo].[tblProductReviews]
ADD CONSTRAINT [FK_tblProductReview_tblProduct]
    FOREIGN KEY ([ProductID_FK])
    REFERENCES [dbo].[tblProducts]
        ([ProductID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblProductReview_tblProduct'
CREATE INDEX [IX_FK_tblProductReview_tblProduct]
ON [dbo].[tblProductReviews]
    ([ProductID_FK]);
GO

-- Creating foreign key on [StateID_FK] in table 'tblMasCities'
ALTER TABLE [dbo].[tblMasCities]
ADD CONSTRAINT [FK_tblMasCity_tblMasState]
    FOREIGN KEY ([StateID_FK])
    REFERENCES [dbo].[tblMasStates]
        ([StateID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblMasCity_tblMasState'
CREATE INDEX [IX_FK_tblMasCity_tblMasState]
ON [dbo].[tblMasCities]
    ([StateID_FK]);
GO

-- Creating foreign key on [ProductID] in table 'tblOrderDetails'
ALTER TABLE [dbo].[tblOrderDetails]
ADD CONSTRAINT [FK_tblOrderDetails_tblProduct]
    FOREIGN KEY ([ProductID])
    REFERENCES [dbo].[tblProducts]
        ([ProductID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblOrderDetails_tblProduct'
CREATE INDEX [IX_FK_tblOrderDetails_tblProduct]
ON [dbo].[tblOrderDetails]
    ([ProductID]);
GO

-- Creating foreign key on [ScreenTimingConfigID_FK] in table 'tblMovieBookingHistories'
ALTER TABLE [dbo].[tblMovieBookingHistories]
ADD CONSTRAINT [FK_tblMovieBookingHistory_tblMovieScreenTimingConfig]
    FOREIGN KEY ([ScreenTimingConfigID_FK])
    REFERENCES [dbo].[tblMovieScreenTimingConfigs]
        ([ScreenTimingConfigID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblMovieBookingHistory_tblMovieScreenTimingConfig'
CREATE INDEX [IX_FK_tblMovieBookingHistory_tblMovieScreenTimingConfig]
ON [dbo].[tblMovieBookingHistories]
    ([ScreenTimingConfigID_FK]);
GO

-- Creating foreign key on [MovieID_FK] in table 'tblMovieScreenTimingConfigs'
ALTER TABLE [dbo].[tblMovieScreenTimingConfigs]
ADD CONSTRAINT [FK_tblMovieScreenTimingConfig_tblMovieDetails]
    FOREIGN KEY ([MovieID_FK])
    REFERENCES [dbo].[tblMovieDetails]
        ([MovieID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblMovieScreenTimingConfig_tblMovieDetails'
CREATE INDEX [IX_FK_tblMovieScreenTimingConfig_tblMovieDetails]
ON [dbo].[tblMovieScreenTimingConfigs]
    ([MovieID_FK]);
GO

-- Creating foreign key on [ScreenID_FK] in table 'tblMovieScreenTimingConfigs'
ALTER TABLE [dbo].[tblMovieScreenTimingConfigs]
ADD CONSTRAINT [FK_tblMovieScreenTimingConfig_tblScreenDetails]
    FOREIGN KEY ([ScreenID_FK])
    REFERENCES [dbo].[tblScreenDetails]
        ([ScreenID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblMovieScreenTimingConfig_tblScreenDetails'
CREATE INDEX [IX_FK_tblMovieScreenTimingConfig_tblScreenDetails]
ON [dbo].[tblMovieScreenTimingConfigs]
    ([ScreenID_FK]);
GO

-- Creating foreign key on [TheaterID_FK] in table 'tblScreenDetails'
ALTER TABLE [dbo].[tblScreenDetails]
ADD CONSTRAINT [FK_tblScreenDetails_tblTheaterDetails]
    FOREIGN KEY ([TheaterID_FK])
    REFERENCES [dbo].[tblTheaterDetails]
        ([TheaterID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblScreenDetails_tblTheaterDetails'
CREATE INDEX [IX_FK_tblScreenDetails_tblTheaterDetails]
ON [dbo].[tblScreenDetails]
    ([TheaterID_FK]);
GO

-- Creating foreign key on [AccountID_FK] in table 'tblTempCartDetails'
ALTER TABLE [dbo].[tblTempCartDetails]
ADD CONSTRAINT [FK_tblTempCartDetails_tblUser]
    FOREIGN KEY ([AccountID_FK])
    REFERENCES [dbo].[tblUsers]
        ([AccountID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblTempCartDetails_tblUser'
CREATE INDEX [IX_FK_tblTempCartDetails_tblUser]
ON [dbo].[tblTempCartDetails]
    ([AccountID_FK]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------