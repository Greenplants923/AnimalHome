
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/10/2020 16:38:17
-- Generated from EDMX file: E:\STUDY\课程\研一下\4_应用系统开发实践（覃飙）\MyWeiboProject202005061120\MyWeiboProject202005061120\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [myWeiboDB202005061120];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UsersAnimals]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UsersSet] DROP CONSTRAINT [FK_UsersAnimals];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersPosts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostsSet] DROP CONSTRAINT [FK_UsersPosts];
GO
IF OBJECT_ID(N'[dbo].[FK_LikesPosts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LikesSet] DROP CONSTRAINT [FK_LikesPosts];
GO
IF OBJECT_ID(N'[dbo].[FK_LikesUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LikesSet] DROP CONSTRAINT [FK_LikesUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_CommentsUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommentsSet] DROP CONSTRAINT [FK_CommentsUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_CommentsPosts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommentsSet] DROP CONSTRAINT [FK_CommentsPosts];
GO
IF OBJECT_ID(N'[dbo].[FK_FollowerUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FollowerSet] DROP CONSTRAINT [FK_FollowerUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_FollowedUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FollowedSet] DROP CONSTRAINT [FK_FollowedUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_FollowedFollowers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FollowedSet] DROP CONSTRAINT [FK_FollowedFollowers];
GO
IF OBJECT_ID(N'[dbo].[FK_FollowerFollowers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FollowerSet] DROP CONSTRAINT [FK_FollowerFollowers];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersSpecialFollower]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SpecialFollowerSet] DROP CONSTRAINT [FK_UsersSpecialFollower];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersSpecialFollowed]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SpecialFollowedSet] DROP CONSTRAINT [FK_UsersSpecialFollowed];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UsersSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsersSet];
GO
IF OBJECT_ID(N'[dbo].[AnimalsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AnimalsSet];
GO
IF OBJECT_ID(N'[dbo].[PostsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostsSet];
GO
IF OBJECT_ID(N'[dbo].[LikesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LikesSet];
GO
IF OBJECT_ID(N'[dbo].[CommentsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommentsSet];
GO
IF OBJECT_ID(N'[dbo].[FollowerSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FollowerSet];
GO
IF OBJECT_ID(N'[dbo].[FollowedSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FollowedSet];
GO
IF OBJECT_ID(N'[dbo].[SpecialFollowerSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpecialFollowerSet];
GO
IF OBJECT_ID(N'[dbo].[SpecialFollowedSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpecialFollowedSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UsersSet'
CREATE TABLE [dbo].[UsersSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [Sex] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [Location] nvarchar(max)  NULL,
    [AboutMe] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [AnimalsId] int  NOT NULL,
    [TimeStamp] datetime  NOT NULL,
    [Vip] bit  NULL
);
GO

-- Creating table 'AnimalsSet'
CREATE TABLE [dbo].[AnimalsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DefaultPhoto] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PostsSet'
CREATE TABLE [dbo].[PostsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Body] nvarchar(max)  NOT NULL,
    [Photo] nvarchar(max)  NULL,
    [TimeStamp] datetime  NOT NULL,
    [LikesNumber] bigint  NOT NULL,
    [CommentsNumber] bigint  NOT NULL,
    [Score] float  NOT NULL,
    [UsersId] int  NOT NULL
);
GO

-- Creating table 'LikesSet'
CREATE TABLE [dbo].[LikesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TimeStamp] datetime  NOT NULL,
    [PostsId] int  NOT NULL,
    [UsersId] int  NOT NULL
);
GO

-- Creating table 'CommentsSet'
CREATE TABLE [dbo].[CommentsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TimeStamp] datetime  NOT NULL,
    [UsersId] int  NOT NULL,
    [PostsId] int  NOT NULL,
    [Body] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FollowerSet'
CREATE TABLE [dbo].[FollowerSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TimeStamp] datetime  NULL,
    [UsersId] int  NOT NULL,
    [FollowersId] int  NULL
);
GO

-- Creating table 'FollowedSet'
CREATE TABLE [dbo].[FollowedSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TimeStamp] datetime  NULL,
    [UsersId] int  NOT NULL,
    [FollowedsId] int  NULL
);
GO

-- Creating table 'SpecialFollowerSet'
CREATE TABLE [dbo].[SpecialFollowerSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TimeStamp] datetime  NULL,
    [UsersId] int  NOT NULL,
    [SPFollowersId] int  NULL
);
GO

-- Creating table 'SpecialFollowedSet'
CREATE TABLE [dbo].[SpecialFollowedSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TimeStamp] datetime  NULL,
    [UsersId] int  NOT NULL,
    [SPFollowedsId] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UsersSet'
ALTER TABLE [dbo].[UsersSet]
ADD CONSTRAINT [PK_UsersSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AnimalsSet'
ALTER TABLE [dbo].[AnimalsSet]
ADD CONSTRAINT [PK_AnimalsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PostsSet'
ALTER TABLE [dbo].[PostsSet]
ADD CONSTRAINT [PK_PostsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LikesSet'
ALTER TABLE [dbo].[LikesSet]
ADD CONSTRAINT [PK_LikesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CommentsSet'
ALTER TABLE [dbo].[CommentsSet]
ADD CONSTRAINT [PK_CommentsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FollowerSet'
ALTER TABLE [dbo].[FollowerSet]
ADD CONSTRAINT [PK_FollowerSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FollowedSet'
ALTER TABLE [dbo].[FollowedSet]
ADD CONSTRAINT [PK_FollowedSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SpecialFollowerSet'
ALTER TABLE [dbo].[SpecialFollowerSet]
ADD CONSTRAINT [PK_SpecialFollowerSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SpecialFollowedSet'
ALTER TABLE [dbo].[SpecialFollowedSet]
ADD CONSTRAINT [PK_SpecialFollowedSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AnimalsId] in table 'UsersSet'
ALTER TABLE [dbo].[UsersSet]
ADD CONSTRAINT [FK_UsersAnimals]
    FOREIGN KEY ([AnimalsId])
    REFERENCES [dbo].[AnimalsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersAnimals'
CREATE INDEX [IX_FK_UsersAnimals]
ON [dbo].[UsersSet]
    ([AnimalsId]);
GO

-- Creating foreign key on [UsersId] in table 'PostsSet'
ALTER TABLE [dbo].[PostsSet]
ADD CONSTRAINT [FK_UsersPosts]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersPosts'
CREATE INDEX [IX_FK_UsersPosts]
ON [dbo].[PostsSet]
    ([UsersId]);
GO

-- Creating foreign key on [PostsId] in table 'LikesSet'
ALTER TABLE [dbo].[LikesSet]
ADD CONSTRAINT [FK_LikesPosts]
    FOREIGN KEY ([PostsId])
    REFERENCES [dbo].[PostsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LikesPosts'
CREATE INDEX [IX_FK_LikesPosts]
ON [dbo].[LikesSet]
    ([PostsId]);
GO

-- Creating foreign key on [UsersId] in table 'LikesSet'
ALTER TABLE [dbo].[LikesSet]
ADD CONSTRAINT [FK_LikesUsers]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LikesUsers'
CREATE INDEX [IX_FK_LikesUsers]
ON [dbo].[LikesSet]
    ([UsersId]);
GO

-- Creating foreign key on [UsersId] in table 'CommentsSet'
ALTER TABLE [dbo].[CommentsSet]
ADD CONSTRAINT [FK_CommentsUsers]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommentsUsers'
CREATE INDEX [IX_FK_CommentsUsers]
ON [dbo].[CommentsSet]
    ([UsersId]);
GO

-- Creating foreign key on [PostsId] in table 'CommentsSet'
ALTER TABLE [dbo].[CommentsSet]
ADD CONSTRAINT [FK_CommentsPosts]
    FOREIGN KEY ([PostsId])
    REFERENCES [dbo].[PostsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommentsPosts'
CREATE INDEX [IX_FK_CommentsPosts]
ON [dbo].[CommentsSet]
    ([PostsId]);
GO

-- Creating foreign key on [UsersId] in table 'FollowerSet'
ALTER TABLE [dbo].[FollowerSet]
ADD CONSTRAINT [FK_FollowerUsers]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FollowerUsers'
CREATE INDEX [IX_FK_FollowerUsers]
ON [dbo].[FollowerSet]
    ([UsersId]);
GO

-- Creating foreign key on [UsersId] in table 'FollowedSet'
ALTER TABLE [dbo].[FollowedSet]
ADD CONSTRAINT [FK_FollowedUsers]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FollowedUsers'
CREATE INDEX [IX_FK_FollowedUsers]
ON [dbo].[FollowedSet]
    ([UsersId]);
GO

-- Creating foreign key on [FollowedsId] in table 'FollowedSet'
ALTER TABLE [dbo].[FollowedSet]
ADD CONSTRAINT [FK_FollowedFollowers]
    FOREIGN KEY ([FollowedsId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FollowedFollowers'
CREATE INDEX [IX_FK_FollowedFollowers]
ON [dbo].[FollowedSet]
    ([FollowedsId]);
GO

-- Creating foreign key on [FollowersId] in table 'FollowerSet'
ALTER TABLE [dbo].[FollowerSet]
ADD CONSTRAINT [FK_FollowerFollowers]
    FOREIGN KEY ([FollowersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FollowerFollowers'
CREATE INDEX [IX_FK_FollowerFollowers]
ON [dbo].[FollowerSet]
    ([FollowersId]);
GO

-- Creating foreign key on [UsersId] in table 'SpecialFollowerSet'
ALTER TABLE [dbo].[SpecialFollowerSet]
ADD CONSTRAINT [FK_UsersSpecialFollower]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersSpecialFollower'
CREATE INDEX [IX_FK_UsersSpecialFollower]
ON [dbo].[SpecialFollowerSet]
    ([UsersId]);
GO

-- Creating foreign key on [UsersId] in table 'SpecialFollowedSet'
ALTER TABLE [dbo].[SpecialFollowedSet]
ADD CONSTRAINT [FK_UsersSpecialFollowed]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersSpecialFollowed'
CREATE INDEX [IX_FK_UsersSpecialFollowed]
ON [dbo].[SpecialFollowedSet]
    ([UsersId]);
GO

-- Creating foreign key on [SPFollowersId] in table 'SpecialFollowerSet'
ALTER TABLE [dbo].[SpecialFollowerSet]
ADD CONSTRAINT [FK_UsersSpecialFollower1]
    FOREIGN KEY ([SPFollowersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersSpecialFollower1'
CREATE INDEX [IX_FK_UsersSpecialFollower1]
ON [dbo].[SpecialFollowerSet]
    ([SPFollowersId]);
GO

-- Creating foreign key on [SPFollowedsId] in table 'SpecialFollowedSet'
ALTER TABLE [dbo].[SpecialFollowedSet]
ADD CONSTRAINT [FK_SpecialFollowedUsers]
    FOREIGN KEY ([SPFollowedsId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SpecialFollowedUsers'
CREATE INDEX [IX_FK_SpecialFollowedUsers]
ON [dbo].[SpecialFollowedSet]
    ([SPFollowedsId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------