USE [master]
GO

/****** Object:  Database [Bank]    Script Date: 9/27/2018 11:27:11 AM ******/
DROP DATABASE [Bank]
GO

/****** Object:  Database [Bank]    Script Date: 9/27/2018 11:27:11 AM ******/
CREATE DATABASE [Bank]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Bank', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Bank.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Bank_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Bank_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

ALTER DATABASE [Bank] SET COMPATIBILITY_LEVEL = 140
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Bank].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Bank] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Bank] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Bank] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Bank] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Bank] SET ARITHABORT OFF 
GO

ALTER DATABASE [Bank] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Bank] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Bank] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Bank] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Bank] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Bank] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Bank] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Bank] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Bank] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Bank] SET  DISABLE_BROKER 
GO

ALTER DATABASE [Bank] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Bank] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Bank] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Bank] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Bank] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Bank] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Bank] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Bank] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [Bank] SET  MULTI_USER 
GO

ALTER DATABASE [Bank] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Bank] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Bank] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Bank] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [Bank] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Bank] SET QUERY_STORE = OFF
GO

USE [Bank]
GO

ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO

ALTER DATABASE [Bank] SET  READ_WRITE 
GO


CREATE TABLE [dbo].[Address] (
    [ID]      INT           IDENTITY (1, 1) NOT NULL,
    [Address] NVARCHAR (50) NOT NULL,
    [City]    NVARCHAR (25) NOT NULL,
    [State]   NVARCHAR (25) NOT NULL,
    [ZipCode] NVARCHAR (5)  NOT NULL
);

GO
CREATE TABLE [dbo].[AuditTrail] (
    [ID]        INT      IDENTITY (1, 1) NOT NULL,
    [ActionID]  INT      NOT NULL,
    [RequestID] INT      NOT NULL,
    [UserID]    INT      NOT NULL,
    [TimeStamp] DATETIME NULL
);

GO
CREATE TABLE [dbo].[CreditScore] (
    [ID]              INT  IDENTITY (1, 1) NOT NULL,
    [UserID]          INT  NOT NULL,
    [TransUnion]      INT  NULL,
    [Equifax]         INT  NULL,
    [LastUpdatedDate] DATE NULL
);

GO
CREATE TABLE [dbo].[EmailConfirmation] (
    [ID]     INT           IDENTITY (1, 1) NOT NULL,
    [UserID] INT           NOT NULL,
    [Token]  NVARCHAR (50) NOT NULL
);

GO
CREATE TABLE [dbo].[LoanApprovalRules] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [MinimumScore]   INT           NOT NULL,
    [MaximumScore]   INT           NOT NULL,
    [DownPaymentReq] FLOAT (53)    NULL,
    [DownPaymentPct] FLOAT (53)    NULL,
    [APR]            FLOAT (53)    NOT NULL,
    [LoanType]       NVARCHAR (10) NOT NULL
);

GO
CREATE TABLE [dbo].[LoanRequest] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [UserID]         INT           NOT NULL,
    [SocialSecurity] NVARCHAR (11) NULL,
    [Income]         FLOAT (53)    NULL,
    [Employer]       NVARCHAR (25) NULL,
    [JobTitle]       NVARCHAR (25) NULL,
    [EmploymentType] NVARCHAR (10) NULL,
    [Status]         NVARCHAR (10) NULL,
    [Amount]         FLOAT (53)    NULL,
    [Terms]          INT           NULL,
    [DownPayment]    FLOAT (53)    NULL,
    [LoanType]       NVARCHAR (10) NULL
);

GO
CREATE TABLE [dbo].[LoanResults] (
    [ID]                   INT            IDENTITY (1, 1) NOT NULL,
    [RequestID]            INT            NOT NULL,
    [Amount]               FLOAT (53)     NULL,
    [APR]                  FLOAT (53)     NOT NULL,
    [MonthlyPayment]       FLOAT (53)     NULL,
    [CreditScore]          INT            NULL,
    [Term]                 INT            NULL,
    [SuggestedDownPayment] FLOAT (53)     NULL,
    [Decision]             NVARCHAR (10)  NULL,
    [ResolutionDate]       DATE           NULL,
    [Comments]             NVARCHAR (250) NULL
);

GO
CREATE TABLE [dbo].[User] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [Email]       NVARCHAR (50)  NOT NULL,
    [FirstName]   NVARCHAR (25) NOT NULL,
    [LastName]    NVARCHAR (16) NOT NULL,
    [MiddleName]  NVARCHAR (25) NULL,
    [Telephone]   NVARCHAR (12) NULL,
    [AddressID]   INT           NULL,
    [Password]    NVARCHAR (32) NOT NULL,
    [UserStatus]  INT           NULL,
    [CreatedDate] DATETIME      NULL
);

GO
ALTER TABLE [dbo].[Address]
    ADD CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([ID] ASC);

GO
ALTER TABLE [dbo].[AuditTrail]
    ADD CONSTRAINT [PK_AuditTrail] PRIMARY KEY CLUSTERED ([ID] ASC);

GO
ALTER TABLE [dbo].[CreditScore]
    ADD CONSTRAINT [PK_CreditScore] PRIMARY KEY CLUSTERED ([ID] ASC);

GO
ALTER TABLE [dbo].[EmailConfirmation]
    ADD CONSTRAINT [PK_EmailConfirmation] PRIMARY KEY CLUSTERED ([ID] ASC);

GO
ALTER TABLE [dbo].[LoanApprovalRules]
    ADD CONSTRAINT [PK_LoanApprovalRules] PRIMARY KEY CLUSTERED ([ID] ASC);

GO
ALTER TABLE [dbo].[LoanRequest]
    ADD CONSTRAINT [PK_LoanRequest] PRIMARY KEY CLUSTERED ([ID] ASC);

GO
ALTER TABLE [dbo].[LoanResults]
    ADD CONSTRAINT [PK_LoanResults] PRIMARY KEY CLUSTERED ([ID] ASC);

GO
ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID] ASC);

GO
ALTER TABLE [dbo].[AuditTrail]
    ADD CONSTRAINT [FK_AuditTrail_LoanRequest] FOREIGN KEY ([RequestID]) REFERENCES [dbo].[LoanRequest] ([ID]);

GO
ALTER TABLE [dbo].[CreditScore]
    ADD CONSTRAINT [FK_CreditScore_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]);

GO
ALTER TABLE [dbo].[EmailConfirmation]
    ADD CONSTRAINT [FK_EmailConfirmation_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]);

GO
ALTER TABLE [dbo].[LoanRequest]
    ADD CONSTRAINT [FK_LoanRequest_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]);

GO
ALTER TABLE [dbo].[LoanResults]
    ADD CONSTRAINT [FK_LoanResults_LoanRequest] FOREIGN KEY ([RequestID]) REFERENCES [dbo].[LoanRequest] ([ID]);

GO
ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [FK_User_Address] FOREIGN KEY ([AddressID]) REFERENCES [dbo].[Address] ([ID]);

GO
ALTER TABLE [dbo].[AuditTrail]
    ADD CONSTRAINT [DF_AuditTrail_TimeStamp] DEFAULT (getdate()) FOR [TimeStamp];

GO
ALTER TABLE [dbo].[LoanRequest]
    ADD CONSTRAINT [DF_LoanRequest_Income] DEFAULT ((0)) FOR [Income];

GO
ALTER TABLE [dbo].[LoanResults]
    ADD CONSTRAINT [DF_LoanResults_APR] DEFAULT ((0)) FOR [APR];

GO
ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [DF_User_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

GO
GRANT VIEW ANY COLUMN ENCRYPTION KEY DEFINITION TO PUBLIC;

GO
GRANT VIEW ANY COLUMN MASTER KEY DEFINITION TO PUBLIC;

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK to Bank.LoanRequest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AuditTrail', @level2type = N'COLUMN', @level2name = N'RequestID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK to Bank.User', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AuditTrail', @level2type = N'COLUMN', @level2name = N'UserID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Equifax credit score', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CreditScore', @level2type = N'COLUMN', @level2name = N'Equifax';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Stamp for last time a credit scored was pulled', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CreditScore', @level2type = N'COLUMN', @level2name = N'LastUpdatedDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Trans Union credit score', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CreditScore', @level2type = N'COLUMN', @level2name = N'TransUnion';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK for Bank.User', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CreditScore', @level2type = N'COLUMN', @level2name = N'UserID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK to Bank.User', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmailConfirmation', @level2type = N'COLUMN', @level2name = N'UserID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Amount of down payment requierd for spesific loan type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanApprovalRules', @level2type = N'COLUMN', @level2name = N'DownPaymentReq';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanApprovalRules', @level2type = N'COLUMN', @level2name = N'LoanType';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Max credit score requierd to quilify for loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanApprovalRules', @level2type = N'COLUMN', @level2name = N'MaximumScore';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Min credit score requierd to quilify for loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanApprovalRules', @level2type = N'COLUMN', @level2name = N'MinimumScore';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Amount requested for loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanRequest', @level2type = N'COLUMN', @level2name = N'Amount';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User job information', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanRequest', @level2type = N'COLUMN', @level2name = N'Employer';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User job information', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanRequest', @level2type = N'COLUMN', @level2name = N'EmploymentType';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User anual income', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanRequest', @level2type = N'COLUMN', @level2name = N'Income';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User job information', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanRequest', @level2type = N'COLUMN', @level2name = N'JobTitle';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type of loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanRequest', @level2type = N'COLUMN', @level2name = N'LoanType';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'period in with the loan will be repaid', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanRequest', @level2type = N'COLUMN', @level2name = N'Terms';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fk to Bank.User', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanRequest', @level2type = N'COLUMN', @level2name = N'UserID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Amount of the loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanResults', @level2type = N'COLUMN', @level2name = N'Amount';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'interest rate for the loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanResults', @level2type = N'COLUMN', @level2name = N'APR';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Credit Score of the user requesting the loan, these come from the credit reporting agencys (a api will delive it to the system)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanResults', @level2type = N'COLUMN', @level2name = N'CreditScore';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Monthly payment for the loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanResults', @level2type = N'COLUMN', @level2name = N'MonthlyPayment';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK to .LoanRequest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanResults', @level2type = N'COLUMN', @level2name = N'RequestID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date when the bank determined an outcome for the requested loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanResults', @level2type = N'COLUMN', @level2name = N'ResolutionDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Period of time in witch the user in paying the loan back to the financial institution', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanResults', @level2type = N'COLUMN', @level2name = N'Term';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK to Bank.Address', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'AddressID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Stamp for user creation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'CreatedDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Password hash', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'User', @level2type = N'COLUMN', @level2name = N'Password';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contains the loan request for each user', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanRequest';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contains the loan results from the Bank.LoanRequest table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LoanResults';

GO
