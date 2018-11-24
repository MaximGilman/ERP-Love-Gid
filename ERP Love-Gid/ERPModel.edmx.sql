
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/24/2018 18:51:49
-- Generated from EDMX file: C:\Users\ГильманМаксимМихайло\Desktop\Бекап флешки\ERP Love-Gid\ERP Love-Gid\ERPModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ERPLoveGid];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ClientContract]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractSet] DROP CONSTRAINT [FK_ClientContract];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractSet] DROP CONSTRAINT [FK_ContractEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractSet] DROP CONSTRAINT [FK_ContractEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentToPeersEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentToPeersSet] DROP CONSTRAINT [FK_PaymentToPeersEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentToPeersContract]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentToPeersSet] DROP CONSTRAINT [FK_PaymentToPeersContract];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountPayments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentsSet] DROP CONSTRAINT [FK_AccountPayments];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractPayments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentsSet] DROP CONSTRAINT [FK_ContractPayments];
GO
IF OBJECT_ID(N'[dbo].[FK_Pay_minPayments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentsSet] DROP CONSTRAINT [FK_Pay_minPayments];
GO
IF OBJECT_ID(N'[dbo].[FK_Pay_minAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pay_minSet] DROP CONSTRAINT [FK_Pay_minAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeePay_min]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pay_minSet] DROP CONSTRAINT [FK_EmployeePay_min];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentsEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentsSet] DROP CONSTRAINT [FK_PaymentsEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentsEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentsSet] DROP CONSTRAINT [FK_PaymentsEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalarySet] DROP CONSTRAINT [FK_EmployeeSalary];
GO
IF OBJECT_ID(N'[dbo].[FK_EventSalary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalarySet] DROP CONSTRAINT [FK_EventSalary];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentsEmployee1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentsSet] DROP CONSTRAINT [FK_PaymentsEmployee1];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalaryPerMonth]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalaryPerMonthSet] DROP CONSTRAINT [FK_EmployeeSalaryPerMonth];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AccountSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountSet];
GO
IF OBJECT_ID(N'[dbo].[ClientSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientSet];
GO
IF OBJECT_ID(N'[dbo].[ContractSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContractSet];
GO
IF OBJECT_ID(N'[dbo].[EmployeeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeSet];
GO
IF OBJECT_ID(N'[dbo].[EventSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventSet];
GO
IF OBJECT_ID(N'[dbo].[PaymentsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentsSet];
GO
IF OBJECT_ID(N'[dbo].[PaymentToPeersSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentToPeersSet];
GO
IF OBJECT_ID(N'[dbo].[Pay_minSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pay_minSet];
GO
IF OBJECT_ID(N'[dbo].[SalarySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalarySet];
GO
IF OBJECT_ID(N'[dbo].[SalaryTypesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalaryTypesSet];
GO
IF OBJECT_ID(N'[dbo].[SalaryPerMonthSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalaryPerMonthSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AccountSet'
CREATE TABLE [dbo].[AccountSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Sum] int  NOT NULL
);
GO

-- Creating table 'ClientSet'
CREATE TABLE [dbo].[ClientSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FIO] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ContractSet'
CREATE TABLE [dbo].[ContractSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Sum_only_contract] int  NOT NULL,
    [Sum_plus] int  NOT NULL,
    [Received] int  NOT NULL,
    [Date_of_event] datetime  NULL,
    [Date_of_sign] datetime  NOT NULL,
    [Payment1Date] datetime  NULL,
    [Payment2Date] datetime  NULL,
    [Payment3Date] datetime  NULL,
    [Comment] nvarchar(max)  NULL,
    [Status] nvarchar(max)  NOT NULL,
    [Payment1Sum] int  NULL,
    [Payment2Sum] int  NULL,
    [Payment3Sum] int  NULL,
    [ClientSet_Id] int  NOT NULL,
    [EmployeeSet_Id] int  NOT NULL,
    [EventSet_Id] int  NOT NULL
);
GO

-- Creating table 'EmployeeSet'
CREATE TABLE [dbo].[EmployeeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsAdmin] bit  NOT NULL,
    [Scale] int  NOT NULL,
    [Notes] nvarchar(max)  NULL,
    [Login] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Surname] nvarchar(max)  NOT NULL,
    [Patronymic] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EventSet'
CREATE TABLE [dbo].[EventSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Percent] nvarchar(max)  NULL
);
GO

-- Creating table 'PaymentsSet'
CREATE TABLE [dbo].[PaymentsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Receipt] int  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [SumToOptyonallyUsing] int  NULL,
    [EmployeeId] int  NOT NULL,
    [ContractId] int  NOT NULL,
    [StatusForPeers] bit  NULL,
    [StatusForSalary] bit  NULL,
    [DateForPayment] datetime  NULL,
    [Secured] bit  NOT NULL,
    [Account_Id] int  NULL,
    [Contract_Id] int  NULL,
    [Pay_min_Id] int  NULL,
    [Employee_Id] int  NOT NULL,
    [Event_Id] int  NOT NULL,
    [EmployeeTo_Id] int  NULL
);
GO

-- Creating table 'PaymentToPeersSet'
CREATE TABLE [dbo].[PaymentToPeersSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Sum] int  NOT NULL,
    [IdToEmployee] int  NOT NULL,
    [IdContract] int  NOT NULL,
    [Month] datetime  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [EventSet_Id] int  NOT NULL,
    [ContractSet_Id] int  NOT NULL
);
GO

-- Creating table 'Pay_minSet'
CREATE TABLE [dbo].[Pay_minSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Sum] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Finished] bit  NOT NULL,
    [Secured] bit  NOT NULL,
    [Account_Id] int  NOT NULL,
    [Employee_Id] int  NOT NULL
);
GO

-- Creating table 'SalarySet'
CREATE TABLE [dbo].[SalarySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PercentOfSalary] nvarchar(max)  NOT NULL,
    [Value] int  NOT NULL,
    [ValueOlga] int  NULL,
    [ValueSergey] int  NULL,
    [Employee_Id] int  NOT NULL,
    [Event_Id] int  NOT NULL
);
GO

-- Creating table 'SalaryTypesSet'
CREATE TABLE [dbo].[SalaryTypesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SalaryPerMonthSet'
CREATE TABLE [dbo].[SalaryPerMonthSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CurMonthSal] int  NULL,
    [CurMonthSalFact] int  NULL,
    [DateMonth] smallint  NULL,
    [DateYear] int  NULL,
    [IncomeToCompany] int  NOT NULL,
    [IncomeToCompanyFact] int  NOT NULL,
    [Employee_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AccountSet'
ALTER TABLE [dbo].[AccountSet]
ADD CONSTRAINT [PK_AccountSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClientSet'
ALTER TABLE [dbo].[ClientSet]
ADD CONSTRAINT [PK_ClientSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ContractSet'
ALTER TABLE [dbo].[ContractSet]
ADD CONSTRAINT [PK_ContractSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeSet'
ALTER TABLE [dbo].[EmployeeSet]
ADD CONSTRAINT [PK_EmployeeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EventSet'
ALTER TABLE [dbo].[EventSet]
ADD CONSTRAINT [PK_EventSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PaymentsSet'
ALTER TABLE [dbo].[PaymentsSet]
ADD CONSTRAINT [PK_PaymentsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PaymentToPeersSet'
ALTER TABLE [dbo].[PaymentToPeersSet]
ADD CONSTRAINT [PK_PaymentToPeersSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pay_minSet'
ALTER TABLE [dbo].[Pay_minSet]
ADD CONSTRAINT [PK_Pay_minSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalarySet'
ALTER TABLE [dbo].[SalarySet]
ADD CONSTRAINT [PK_SalarySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalaryTypesSet'
ALTER TABLE [dbo].[SalaryTypesSet]
ADD CONSTRAINT [PK_SalaryTypesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalaryPerMonthSet'
ALTER TABLE [dbo].[SalaryPerMonthSet]
ADD CONSTRAINT [PK_SalaryPerMonthSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ClientSet_Id] in table 'ContractSet'
ALTER TABLE [dbo].[ContractSet]
ADD CONSTRAINT [FK_ClientContract]
    FOREIGN KEY ([ClientSet_Id])
    REFERENCES [dbo].[ClientSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientContract'
CREATE INDEX [IX_FK_ClientContract]
ON [dbo].[ContractSet]
    ([ClientSet_Id]);
GO

-- Creating foreign key on [EmployeeSet_Id] in table 'ContractSet'
ALTER TABLE [dbo].[ContractSet]
ADD CONSTRAINT [FK_ContractEmployee]
    FOREIGN KEY ([EmployeeSet_Id])
    REFERENCES [dbo].[EmployeeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractEmployee'
CREATE INDEX [IX_FK_ContractEmployee]
ON [dbo].[ContractSet]
    ([EmployeeSet_Id]);
GO

-- Creating foreign key on [EventSet_Id] in table 'ContractSet'
ALTER TABLE [dbo].[ContractSet]
ADD CONSTRAINT [FK_ContractEvent]
    FOREIGN KEY ([EventSet_Id])
    REFERENCES [dbo].[EventSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractEvent'
CREATE INDEX [IX_FK_ContractEvent]
ON [dbo].[ContractSet]
    ([EventSet_Id]);
GO

-- Creating foreign key on [EventSet_Id] in table 'PaymentToPeersSet'
ALTER TABLE [dbo].[PaymentToPeersSet]
ADD CONSTRAINT [FK_PaymentToPeersEvent]
    FOREIGN KEY ([EventSet_Id])
    REFERENCES [dbo].[EventSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentToPeersEvent'
CREATE INDEX [IX_FK_PaymentToPeersEvent]
ON [dbo].[PaymentToPeersSet]
    ([EventSet_Id]);
GO

-- Creating foreign key on [ContractSet_Id] in table 'PaymentToPeersSet'
ALTER TABLE [dbo].[PaymentToPeersSet]
ADD CONSTRAINT [FK_PaymentToPeersContract]
    FOREIGN KEY ([ContractSet_Id])
    REFERENCES [dbo].[ContractSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentToPeersContract'
CREATE INDEX [IX_FK_PaymentToPeersContract]
ON [dbo].[PaymentToPeersSet]
    ([ContractSet_Id]);
GO

-- Creating foreign key on [Account_Id] in table 'PaymentsSet'
ALTER TABLE [dbo].[PaymentsSet]
ADD CONSTRAINT [FK_AccountPayments]
    FOREIGN KEY ([Account_Id])
    REFERENCES [dbo].[AccountSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountPayments'
CREATE INDEX [IX_FK_AccountPayments]
ON [dbo].[PaymentsSet]
    ([Account_Id]);
GO

-- Creating foreign key on [Contract_Id] in table 'PaymentsSet'
ALTER TABLE [dbo].[PaymentsSet]
ADD CONSTRAINT [FK_ContractPayments]
    FOREIGN KEY ([Contract_Id])
    REFERENCES [dbo].[ContractSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractPayments'
CREATE INDEX [IX_FK_ContractPayments]
ON [dbo].[PaymentsSet]
    ([Contract_Id]);
GO

-- Creating foreign key on [Pay_min_Id] in table 'PaymentsSet'
ALTER TABLE [dbo].[PaymentsSet]
ADD CONSTRAINT [FK_Pay_minPayments]
    FOREIGN KEY ([Pay_min_Id])
    REFERENCES [dbo].[Pay_minSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Pay_minPayments'
CREATE INDEX [IX_FK_Pay_minPayments]
ON [dbo].[PaymentsSet]
    ([Pay_min_Id]);
GO

-- Creating foreign key on [Account_Id] in table 'Pay_minSet'
ALTER TABLE [dbo].[Pay_minSet]
ADD CONSTRAINT [FK_Pay_minAccount]
    FOREIGN KEY ([Account_Id])
    REFERENCES [dbo].[AccountSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Pay_minAccount'
CREATE INDEX [IX_FK_Pay_minAccount]
ON [dbo].[Pay_minSet]
    ([Account_Id]);
GO

-- Creating foreign key on [Employee_Id] in table 'Pay_minSet'
ALTER TABLE [dbo].[Pay_minSet]
ADD CONSTRAINT [FK_EmployeePay_min]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[EmployeeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeePay_min'
CREATE INDEX [IX_FK_EmployeePay_min]
ON [dbo].[Pay_minSet]
    ([Employee_Id]);
GO

-- Creating foreign key on [Employee_Id] in table 'PaymentsSet'
ALTER TABLE [dbo].[PaymentsSet]
ADD CONSTRAINT [FK_PaymentsEmployee]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[EmployeeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentsEmployee'
CREATE INDEX [IX_FK_PaymentsEmployee]
ON [dbo].[PaymentsSet]
    ([Employee_Id]);
GO

-- Creating foreign key on [Event_Id] in table 'PaymentsSet'
ALTER TABLE [dbo].[PaymentsSet]
ADD CONSTRAINT [FK_PaymentsEvent]
    FOREIGN KEY ([Event_Id])
    REFERENCES [dbo].[EventSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentsEvent'
CREATE INDEX [IX_FK_PaymentsEvent]
ON [dbo].[PaymentsSet]
    ([Event_Id]);
GO

-- Creating foreign key on [Employee_Id] in table 'SalarySet'
ALTER TABLE [dbo].[SalarySet]
ADD CONSTRAINT [FK_EmployeeSalary]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[EmployeeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalary'
CREATE INDEX [IX_FK_EmployeeSalary]
ON [dbo].[SalarySet]
    ([Employee_Id]);
GO

-- Creating foreign key on [Event_Id] in table 'SalarySet'
ALTER TABLE [dbo].[SalarySet]
ADD CONSTRAINT [FK_EventSalary]
    FOREIGN KEY ([Event_Id])
    REFERENCES [dbo].[EventSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventSalary'
CREATE INDEX [IX_FK_EventSalary]
ON [dbo].[SalarySet]
    ([Event_Id]);
GO

-- Creating foreign key on [EmployeeTo_Id] in table 'PaymentsSet'
ALTER TABLE [dbo].[PaymentsSet]
ADD CONSTRAINT [FK_PaymentsEmployee1]
    FOREIGN KEY ([EmployeeTo_Id])
    REFERENCES [dbo].[EmployeeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentsEmployee1'
CREATE INDEX [IX_FK_PaymentsEmployee1]
ON [dbo].[PaymentsSet]
    ([EmployeeTo_Id]);
GO

-- Creating foreign key on [Employee_Id] in table 'SalaryPerMonthSet'
ALTER TABLE [dbo].[SalaryPerMonthSet]
ADD CONSTRAINT [FK_EmployeeSalaryPerMonth]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[EmployeeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalaryPerMonth'
CREATE INDEX [IX_FK_EmployeeSalaryPerMonth]
ON [dbo].[SalaryPerMonthSet]
    ([Employee_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------