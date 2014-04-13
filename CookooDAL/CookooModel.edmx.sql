
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/18/2014 22:19:19
-- Generated from EDMX file: C:\ziki\Cookoo\CookooDAL\CookooModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CookooDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Rcp_Category_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rcp_Category] DROP CONSTRAINT [FK_Rcp_Category_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_CooKooRecipe_Image]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CooKooRecipes] DROP CONSTRAINT [FK_CooKooRecipe_Image];
GO
IF OBJECT_ID(N'[dbo].[FK_CooKooRecipe_RecipeDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CooKooRecipes] DROP CONSTRAINT [FK_CooKooRecipe_RecipeDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_ImageRecipe_ImageRecipe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ImageRecipes] DROP CONSTRAINT [FK_ImageRecipe_ImageRecipe];
GO
IF OBJECT_ID(N'[dbo].[FK_ImageRecipe_RecipeDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ImageRecipes] DROP CONSTRAINT [FK_ImageRecipe_RecipeDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_Message_UserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_Message_UserProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_Message_UserProfile1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_Message_UserProfile1];
GO
IF OBJECT_ID(N'[dbo].[FK_Rcp_Category_RecipeDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rcp_Category] DROP CONSTRAINT [FK_Rcp_Category_RecipeDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_RecipeDetails_RecipeSrcType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecipeDetails] DROP CONSTRAINT [FK_RecipeDetails_RecipeSrcType];
GO
IF OBJECT_ID(N'[dbo].[FK_T_RecipeBase_UserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecipeDetails] DROP CONSTRAINT [FK_T_RecipeBase_UserProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_UrlRecipe_RecipeDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UrlRecipes] DROP CONSTRAINT [FK_UrlRecipe_RecipeDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_User_Recipe_RecipeBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[User_Recipe] DROP CONSTRAINT [FK_User_Recipe_RecipeBase];
GO
IF OBJECT_ID(N'[dbo].[FK_Users_Recipes_T_UsersProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[User_Recipe] DROP CONSTRAINT [FK_Users_Recipes_T_UsersProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_webpages_UsersInRoles_webpages_Roles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [FK_webpages_UsersInRoles_webpages_Roles];
GO
IF OBJECT_ID(N'[dbo].[FK_webpages_UsersInRoles_UserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [FK_webpages_UsersInRoles_UserProfile];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[CooKooRecipes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CooKooRecipes];
GO
IF OBJECT_ID(N'[dbo].[Images]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Images];
GO
IF OBJECT_ID(N'[dbo].[ImageRecipes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ImageRecipes];
GO
IF OBJECT_ID(N'[dbo].[Languages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Languages];
GO
IF OBJECT_ID(N'[dbo].[Messages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Messages];
GO
IF OBJECT_ID(N'[dbo].[Rcp_Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rcp_Category];
GO
IF OBJECT_ID(N'[dbo].[RecipeDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecipeDetails];
GO
IF OBJECT_ID(N'[dbo].[RecipeSrcTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecipeSrcTypes];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[UrlRecipes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UrlRecipes];
GO
IF OBJECT_ID(N'[dbo].[User_Recipe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User_Recipe];
GO
IF OBJECT_ID(N'[dbo].[UserProfiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserProfiles];
GO
IF OBJECT_ID(N'[dbo].[webpages_Membership]', 'U') IS NOT NULL
    DROP TABLE [dbo].[webpages_Membership];
GO
IF OBJECT_ID(N'[dbo].[webpages_OAuthMembership]', 'U') IS NOT NULL
    DROP TABLE [dbo].[webpages_OAuthMembership];
GO
IF OBJECT_ID(N'[dbo].[webpages_Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[webpages_Roles];
GO
IF OBJECT_ID(N'[dbo].[webpages_UsersInRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[webpages_UsersInRoles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [CtgryID] int IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CooKooRecipes'
CREATE TABLE [dbo].[CooKooRecipes] (
    [CKRcpID] bigint  NOT NULL,
    [DoesCount] int  NULL,
    [PreparationTimeMin] int  NULL,
    [Ingredients] nvarchar(max)  NOT NULL,
    [Instructions] nvarchar(max)  NOT NULL,
    [RegularImageID] bigint  NULL
);
GO

-- Creating table 'Images'
CREATE TABLE [dbo].[Images] (
    [ImageID] bigint IDENTITY(1,1) NOT NULL,
    [ImageName] nchar(50)  NOT NULL,
    [ImageData] varbinary(max)  NOT NULL,
    [ContentType] nchar(50)  NOT NULL
);
GO

-- Creating table 'ImageRecipes'
CREATE TABLE [dbo].[ImageRecipes] (
    [ImgRcp] bigint  NOT NULL,
    [ImgID] bigint  NOT NULL,
    [FakeColumn] bit  NULL
);
GO

-- Creating table 'Languages'
CREATE TABLE [dbo].[Languages] (
    [LanguageID] int  NOT NULL,
    [Language1] nvarchar(50)  NOT NULL,
    [IsRtf] bit  NOT NULL
);
GO

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [MsgID] bigint IDENTITY(1,1) NOT NULL,
    [FromUserID] int  NOT NULL,
    [ToUserID] int  NOT NULL,
    [MsgTitle] nvarchar(max)  NOT NULL,
    [MsgContent] nvarchar(max)  NULL,
    [TimeCreated] datetime  NOT NULL,
    [IsRead] bit  NOT NULL
);
GO

-- Creating table 'Rcp_Category'
CREATE TABLE [dbo].[Rcp_Category] (
    [RcpDetailsID] bigint  NOT NULL,
    [CtgryID] int  NOT NULL,
    [FakeColumn] bit  NULL
);
GO

-- Creating table 'RecipeDetails'
CREATE TABLE [dbo].[RecipeDetails] (
    [RcpDetailsID] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [OwnerID] int  NOT NULL,
    [ImageUrl] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [RcpSrcTypeID] int  NOT NULL
);
GO

-- Creating table 'RecipeSrcTypes'
CREATE TABLE [dbo].[RecipeSrcTypes] (
    [RcpSrcTypeID] int IDENTITY(1,1) NOT NULL,
    [RecipeTableName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'UrlRecipes'
CREATE TABLE [dbo].[UrlRecipes] (
    [UrlRcpID] bigint  NOT NULL,
    [Url] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'User_Recipe'
CREATE TABLE [dbo].[User_Recipe] (
    [UserID] int  NOT NULL,
    [RecipeID] bigint  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'UserProfiles'
CREATE TABLE [dbo].[UserProfiles] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(56)  NOT NULL,
    [Language] nvarchar(max)  NOT NULL,
    [Created] datetime  NULL
);
GO

-- Creating table 'webpages_Membership'
CREATE TABLE [dbo].[webpages_Membership] (
    [UserId] int  NOT NULL,
    [CreateDate] datetime  NULL,
    [ConfirmationToken] nvarchar(128)  NULL,
    [IsConfirmed] bit  NULL,
    [LastPasswordFailureDate] datetime  NULL,
    [PasswordFailuresSinceLastSuccess] int  NOT NULL,
    [Password] nvarchar(128)  NOT NULL,
    [PasswordChangedDate] datetime  NULL,
    [PasswordSalt] nvarchar(128)  NOT NULL,
    [PasswordVerificationToken] nvarchar(128)  NULL,
    [PasswordVerificationTokenExpirationDate] datetime  NULL
);
GO

-- Creating table 'webpages_OAuthMembership'
CREATE TABLE [dbo].[webpages_OAuthMembership] (
    [Provider] nvarchar(30)  NOT NULL,
    [ProviderUserId] nvarchar(100)  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'webpages_Roles'
CREATE TABLE [dbo].[webpages_Roles] (
    [RoleId] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'webpages_UsersInRoles'
CREATE TABLE [dbo].[webpages_UsersInRoles] (
    [webpages_Roles_RoleId] int  NOT NULL,
    [UserProfiles_UserId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CtgryID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([CtgryID] ASC);
GO

-- Creating primary key on [CKRcpID] in table 'CooKooRecipes'
ALTER TABLE [dbo].[CooKooRecipes]
ADD CONSTRAINT [PK_CooKooRecipes]
    PRIMARY KEY CLUSTERED ([CKRcpID] ASC);
GO

-- Creating primary key on [ImageID] in table 'Images'
ALTER TABLE [dbo].[Images]
ADD CONSTRAINT [PK_Images]
    PRIMARY KEY CLUSTERED ([ImageID] ASC);
GO

-- Creating primary key on [ImgRcp], [ImgID] in table 'ImageRecipes'
ALTER TABLE [dbo].[ImageRecipes]
ADD CONSTRAINT [PK_ImageRecipes]
    PRIMARY KEY CLUSTERED ([ImgRcp], [ImgID] ASC);
GO

-- Creating primary key on [LanguageID] in table 'Languages'
ALTER TABLE [dbo].[Languages]
ADD CONSTRAINT [PK_Languages]
    PRIMARY KEY CLUSTERED ([LanguageID] ASC);
GO

-- Creating primary key on [MsgID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([MsgID] ASC);
GO

-- Creating primary key on [RcpDetailsID], [CtgryID] in table 'Rcp_Category'
ALTER TABLE [dbo].[Rcp_Category]
ADD CONSTRAINT [PK_Rcp_Category]
    PRIMARY KEY CLUSTERED ([RcpDetailsID], [CtgryID] ASC);
GO

-- Creating primary key on [RcpDetailsID] in table 'RecipeDetails'
ALTER TABLE [dbo].[RecipeDetails]
ADD CONSTRAINT [PK_RecipeDetails]
    PRIMARY KEY CLUSTERED ([RcpDetailsID] ASC);
GO

-- Creating primary key on [RcpSrcTypeID] in table 'RecipeSrcTypes'
ALTER TABLE [dbo].[RecipeSrcTypes]
ADD CONSTRAINT [PK_RecipeSrcTypes]
    PRIMARY KEY CLUSTERED ([RcpSrcTypeID] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [UrlRcpID] in table 'UrlRecipes'
ALTER TABLE [dbo].[UrlRecipes]
ADD CONSTRAINT [PK_UrlRecipes]
    PRIMARY KEY CLUSTERED ([UrlRcpID] ASC);
GO

-- Creating primary key on [UserID], [RecipeID] in table 'User_Recipe'
ALTER TABLE [dbo].[User_Recipe]
ADD CONSTRAINT [PK_User_Recipe]
    PRIMARY KEY CLUSTERED ([UserID], [RecipeID] ASC);
GO

-- Creating primary key on [UserId] in table 'UserProfiles'
ALTER TABLE [dbo].[UserProfiles]
ADD CONSTRAINT [PK_UserProfiles]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [UserId] in table 'webpages_Membership'
ALTER TABLE [dbo].[webpages_Membership]
ADD CONSTRAINT [PK_webpages_Membership]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [Provider], [ProviderUserId] in table 'webpages_OAuthMembership'
ALTER TABLE [dbo].[webpages_OAuthMembership]
ADD CONSTRAINT [PK_webpages_OAuthMembership]
    PRIMARY KEY CLUSTERED ([Provider], [ProviderUserId] ASC);
GO

-- Creating primary key on [RoleId] in table 'webpages_Roles'
ALTER TABLE [dbo].[webpages_Roles]
ADD CONSTRAINT [PK_webpages_Roles]
    PRIMARY KEY CLUSTERED ([RoleId] ASC);
GO

-- Creating primary key on [webpages_Roles_RoleId], [UserProfiles_UserId] in table 'webpages_UsersInRoles'
ALTER TABLE [dbo].[webpages_UsersInRoles]
ADD CONSTRAINT [PK_webpages_UsersInRoles]
    PRIMARY KEY NONCLUSTERED ([webpages_Roles_RoleId], [UserProfiles_UserId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CtgryID] in table 'Rcp_Category'
ALTER TABLE [dbo].[Rcp_Category]
ADD CONSTRAINT [FK_Rcp_Category_Category]
    FOREIGN KEY ([CtgryID])
    REFERENCES [dbo].[Categories]
        ([CtgryID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Rcp_Category_Category'
CREATE INDEX [IX_FK_Rcp_Category_Category]
ON [dbo].[Rcp_Category]
    ([CtgryID]);
GO

-- Creating foreign key on [RegularImageID] in table 'CooKooRecipes'
ALTER TABLE [dbo].[CooKooRecipes]
ADD CONSTRAINT [FK_CooKooRecipe_Image]
    FOREIGN KEY ([RegularImageID])
    REFERENCES [dbo].[Images]
        ([ImageID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CooKooRecipe_Image'
CREATE INDEX [IX_FK_CooKooRecipe_Image]
ON [dbo].[CooKooRecipes]
    ([RegularImageID]);
GO

-- Creating foreign key on [CKRcpID] in table 'CooKooRecipes'
ALTER TABLE [dbo].[CooKooRecipes]
ADD CONSTRAINT [FK_CooKooRecipe_RecipeDetails]
    FOREIGN KEY ([CKRcpID])
    REFERENCES [dbo].[RecipeDetails]
        ([RcpDetailsID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ImgID] in table 'ImageRecipes'
ALTER TABLE [dbo].[ImageRecipes]
ADD CONSTRAINT [FK_ImageRecipe_ImageRecipe]
    FOREIGN KEY ([ImgID])
    REFERENCES [dbo].[Images]
        ([ImageID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ImageRecipe_ImageRecipe'
CREATE INDEX [IX_FK_ImageRecipe_ImageRecipe]
ON [dbo].[ImageRecipes]
    ([ImgID]);
GO

-- Creating foreign key on [ImgRcp] in table 'ImageRecipes'
ALTER TABLE [dbo].[ImageRecipes]
ADD CONSTRAINT [FK_ImageRecipe_RecipeDetails]
    FOREIGN KEY ([ImgRcp])
    REFERENCES [dbo].[RecipeDetails]
        ([RcpDetailsID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ToUserID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_Message_UserProfile]
    FOREIGN KEY ([ToUserID])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Message_UserProfile'
CREATE INDEX [IX_FK_Message_UserProfile]
ON [dbo].[Messages]
    ([ToUserID]);
GO

-- Creating foreign key on [FromUserID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_Message_UserProfile1]
    FOREIGN KEY ([FromUserID])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Message_UserProfile1'
CREATE INDEX [IX_FK_Message_UserProfile1]
ON [dbo].[Messages]
    ([FromUserID]);
GO

-- Creating foreign key on [RcpDetailsID] in table 'Rcp_Category'
ALTER TABLE [dbo].[Rcp_Category]
ADD CONSTRAINT [FK_Rcp_Category_RecipeDetails]
    FOREIGN KEY ([RcpDetailsID])
    REFERENCES [dbo].[RecipeDetails]
        ([RcpDetailsID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RcpSrcTypeID] in table 'RecipeDetails'
ALTER TABLE [dbo].[RecipeDetails]
ADD CONSTRAINT [FK_RecipeDetails_RecipeSrcType]
    FOREIGN KEY ([RcpSrcTypeID])
    REFERENCES [dbo].[RecipeSrcTypes]
        ([RcpSrcTypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RecipeDetails_RecipeSrcType'
CREATE INDEX [IX_FK_RecipeDetails_RecipeSrcType]
ON [dbo].[RecipeDetails]
    ([RcpSrcTypeID]);
GO

-- Creating foreign key on [OwnerID] in table 'RecipeDetails'
ALTER TABLE [dbo].[RecipeDetails]
ADD CONSTRAINT [FK_T_RecipeBase_UserProfile]
    FOREIGN KEY ([OwnerID])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_T_RecipeBase_UserProfile'
CREATE INDEX [IX_FK_T_RecipeBase_UserProfile]
ON [dbo].[RecipeDetails]
    ([OwnerID]);
GO

-- Creating foreign key on [UrlRcpID] in table 'UrlRecipes'
ALTER TABLE [dbo].[UrlRecipes]
ADD CONSTRAINT [FK_UrlRecipe_RecipeDetails]
    FOREIGN KEY ([UrlRcpID])
    REFERENCES [dbo].[RecipeDetails]
        ([RcpDetailsID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RecipeID] in table 'User_Recipe'
ALTER TABLE [dbo].[User_Recipe]
ADD CONSTRAINT [FK_User_Recipe_RecipeBase]
    FOREIGN KEY ([RecipeID])
    REFERENCES [dbo].[RecipeDetails]
        ([RcpDetailsID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_User_Recipe_RecipeBase'
CREATE INDEX [IX_FK_User_Recipe_RecipeBase]
ON [dbo].[User_Recipe]
    ([RecipeID]);
GO

-- Creating foreign key on [UserID] in table 'User_Recipe'
ALTER TABLE [dbo].[User_Recipe]
ADD CONSTRAINT [FK_Users_Recipes_T_UsersProfile]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [webpages_Roles_RoleId] in table 'webpages_UsersInRoles'
ALTER TABLE [dbo].[webpages_UsersInRoles]
ADD CONSTRAINT [FK_webpages_UsersInRoles_webpages_Roles]
    FOREIGN KEY ([webpages_Roles_RoleId])
    REFERENCES [dbo].[webpages_Roles]
        ([RoleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserProfiles_UserId] in table 'webpages_UsersInRoles'
ALTER TABLE [dbo].[webpages_UsersInRoles]
ADD CONSTRAINT [FK_webpages_UsersInRoles_UserProfile]
    FOREIGN KEY ([UserProfiles_UserId])
    REFERENCES [dbo].[UserProfiles]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_webpages_UsersInRoles_UserProfile'
CREATE INDEX [IX_FK_webpages_UsersInRoles_UserProfile]
ON [dbo].[webpages_UsersInRoles]
    ([UserProfiles_UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------