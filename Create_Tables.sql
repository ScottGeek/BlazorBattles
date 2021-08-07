Use [BlazorBattleDB]
GO

-- One can remove the _EFMigrationHistory for production if one wants.
-- Just keep in mind that on DB changes one might loose data if one does not do DB migrations...
-- This is up to you!.

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Units] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [Attack] int NOT NULL,
    [Defense] int NOT NULL,
    [HitPoints] int NOT NULL,
    [BananaCost] int NOT NULL,
    CONSTRAINT [PK_Units] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210802190404_Initial', N'5.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Email] nvarchar(max) NULL,
    [Username] nvarchar(max) NULL,
    [PasswordHash] varbinary(max) NULL,
    [PasswordSalt] varbinary(max) NULL,
    [Bananas] int NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [IsConfirmed] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210802200931_UserModel', N'5.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [UserUnits] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [UnitId] int NOT NULL,
    [HitPoints] int NOT NULL,
    CONSTRAINT [PK_UserUnits] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserUnits_Units_UnitId] FOREIGN KEY ([UnitId]) REFERENCES [Units] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserUnits_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_UserUnits_UnitId] ON [UserUnits] ([UnitId]);
GO

CREATE INDEX [IX_UserUnits_UserId] ON [UserUnits] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210803195937_UserUnits', N'5.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Users] ADD [Battles] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Users] ADD [Defeats] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Users] ADD [Victories] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210804201458_UserBattleStats', N'5.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Battles] (
    [Id] int NOT NULL IDENTITY,
    [AttackerId] int NOT NULL,
    [OpponentId] int NOT NULL,
    [WinnerId] int NOT NULL,
    [WinnerDamage] int NOT NULL,
    [RoundsFought] int NOT NULL,
    [BattleDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Battles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Battles_Users_AttackerId] FOREIGN KEY ([AttackerId]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_Battles_Users_OpponentId] FOREIGN KEY ([OpponentId]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_Battles_Users_WinnerId] FOREIGN KEY ([WinnerId]) REFERENCES [Users] ([Id])
);
GO

CREATE INDEX [IX_Battles_AttackerId] ON [Battles] ([AttackerId]);
GO

CREATE INDEX [IX_Battles_OpponentId] ON [Battles] ([OpponentId]);
GO

CREATE INDEX [IX_Battles_WinnerId] ON [Battles] ([WinnerId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210806152447_Battles', N'5.0.8');
GO

COMMIT;
GO