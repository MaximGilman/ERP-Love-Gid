
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/21/2018 09:34:33
-- Generated from EDMX file: C:\Users\Maxim Gilman\source\repos\ERP Love-Gid\ERP Love-Gid\ERP model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ERP_LG];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'EmployeeSet'
CREATE TABLE [dbo].[EmployeeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FIO] nvarchar(max)  NOT NULL,
    [IsAdmin] bit  NOT NULL,
    [Salary] int  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EventSet'
CREATE TABLE [dbo].[EventSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [PercentToEmpl] int  NOT NULL
);
GO

-- Creating table 'ContractSet'
CREATE TABLE [dbo].[ContractSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Sum_only_contract] int  NOT NULL,
    [Sum_plus] int  NOT NULL,
    [Received] int  NOT NULL,
    [Date_of_event] datetime  NOT NULL,
    [Date_of_sign] datetime  NOT NULL,
    [Payment1] datetime  NOT NULL,
    [Payment2] datetime  NOT NULL,
    [Payment3] datetime  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [IdEvent] int  NOT NULL,
    [IdEmployee] int  NOT NULL,
    [Employee_Id] int  NOT NULL,
    [Event_Id] int  NOT NULL,
    [Client_Id] int  NOT NULL,
    [Payments_Id] int  NOT NULL
);
GO

-- Creating table 'ClientSet'
CREATE TABLE [dbo].[ClientSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FIO] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AccountSet'
CREATE TABLE [dbo].[AccountSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Sum] int  NOT NULL
);
GO

-- Creating table 'PaymentsSet'
CREATE TABLE [dbo].[PaymentsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Receipt] int  NOT NULL,
    [IdToEmployee] int  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Sum] int  NOT NULL,
    [IdAccount] int  NOT NULL,
    [IdContract] int  NOT NULL,
    [Account_Id] int  NOT NULL,
    [Employee_Id] int  NOT NULL
);
GO

-- Creating table 'PaymentToPeersSet'
CREATE TABLE [dbo].[PaymentToPeersSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Sum] int  NOT NULL,
    [IdToEmployee] int  NOT NULL,
    [IdContract] int  NOT NULL,
    [IdEvent] int  NOT NULL,
    [Month] datetime  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [Event_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

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

-- Creating primary key on [Id] in table 'ContractSet'
ALTER TABLE [dbo].[ContractSet]
ADD CONSTRAINT [PK_ContractSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClientSet'
ALTER TABLE [dbo].[ClientSet]
ADD CONSTRAINT [PK_ClientSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccountSet'
ALTER TABLE [dbo].[AccountSet]
ADD CONSTRAINT [PK_AccountSet]
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

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Employee_Id] in table 'ContractSet'
ALTER TABLE [dbo].[ContractSet]
ADD CONSTRAINT [FK_ContractEmployee]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[EmployeeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractEmployee'
CREATE INDEX [IX_FK_ContractEmployee]
ON [dbo].[ContractSet]
    ([Employee_Id]);
GO

-- Creating foreign key on [Event_Id] in table 'ContractSet'
ALTER TABLE [dbo].[ContractSet]
ADD CONSTRAINT [FK_ContractEvent]
    FOREIGN KEY ([Event_Id])
    REFERENCES [dbo].[EventSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractEvent'
CREATE INDEX [IX_FK_ContractEvent]
ON [dbo].[ContractSet]
    ([Event_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'ContractSet'
ALTER TABLE [dbo].[ContractSet]
ADD CONSTRAINT [FK_ClientContract]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[ClientSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientContract'
CREATE INDEX [IX_FK_ClientContract]
ON [dbo].[ContractSet]
    ([Client_Id]);
GO

-- Creating foreign key on [Account_Id] in table 'PaymentsSet'
ALTER TABLE [dbo].[PaymentsSet]
ADD CONSTRAINT [FK_PaymentsAccount]
    FOREIGN KEY ([Account_Id])
    REFERENCES [dbo].[AccountSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentsAccount'
CREATE INDEX [IX_FK_PaymentsAccount]
ON [dbo].[PaymentsSet]
    ([Account_Id]);
GO

-- Creating foreign key on [Event_Id] in table 'PaymentToPeersSet'
ALTER TABLE [dbo].[PaymentToPeersSet]
ADD CONSTRAINT [FK_PaymentToPeersEvent]
    FOREIGN KEY ([Event_Id])
    REFERENCES [dbo].[EventSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentToPeersEvent'
CREATE INDEX [IX_FK_PaymentToPeersEvent]
ON [dbo].[PaymentToPeersSet]
    ([Event_Id]);
GO

-- Creating foreign key on [Payments_Id] in table 'ContractSet'
ALTER TABLE [dbo].[ContractSet]
ADD CONSTRAINT [FK_PaymentsContract]
    FOREIGN KEY ([Payments_Id])
    REFERENCES [dbo].[PaymentsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentsContract'
CREATE INDEX [IX_FK_PaymentsContract]
ON [dbo].[ContractSet]
    ([Payments_Id]);
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------