-- Channels
CREATE TABLE Channels(
	Id uniqueidentifier NOT NULL,
	Name nvarchar(50) NOT NULL,
	ClientPort uniqueidentifier NOT NULL,
	CreateAt datetime NOT NULL,
	CONSTRAINT PK_Channels PRIMARY KEY CLUSTERED (Id ASC)
)
ALTER TABLE Channels ADD  CONSTRAINT DF_Channels_Id  DEFAULT (newid()) FOR Id
ALTER TABLE Channels ADD  CONSTRAINT DF_Channels_Name  DEFAULT ('') FOR Name
ALTER TABLE Channels ADD  CONSTRAINT DF_Channels_ClientPort  DEFAULT (newid()) FOR ClientPort
ALTER TABLE Channels ADD  CONSTRAINT DF_Channels_CreateAt  DEFAULT (GETDATE()) FOR CreateAt
GO

-- Accounts
CREATE TABLE Accounts(
	Id uniqueidentifier NOT NULL,
	ProviderName nvarchar(50) NOT NULL,
	UniqueIdInProvider nvarchar(200) NOT NULL,
	AccountName nvarchar(200) NOT NULL,
	CONSTRAINT PK_Accounts PRIMARY KEY CLUSTERED (Id ASC)
)
ALTER TABLE Accounts ADD  CONSTRAINT DF_Accounts_Id  DEFAULT (newid()) FOR Id
ALTER TABLE Accounts ADD  CONSTRAINT DF_Accounts_ProviderName  DEFAULT ('') FOR ProviderName
ALTER TABLE Accounts ADD  CONSTRAINT DF_Accounts_UniqueIdInProvider  DEFAULT ('') FOR UniqueIdInProvider
ALTER TABLE Accounts ADD  CONSTRAINT DF_Accounts_AccountName  DEFAULT ('') FOR AccountName
GO

-- ChannelMembers
CREATE TABLE ChannelMembers(
	Id uniqueidentifier NOT NULL,
	ChannelId uniqueidentifier NOT NULL,
	AccountId uniqueidentifier NOT NULL,
	IsOwner bit NOT NULL,
	CreateAt datetime NOT NULL,
	CONSTRAINT PK_ChannelMembers PRIMARY KEY CLUSTERED (Id ASC)
)
ALTER TABLE ChannelMembers ADD  CONSTRAINT DF_ChannelMembers_IsOwner  DEFAULT ((0)) FOR IsOwner
ALTER TABLE ChannelMembers ADD  CONSTRAINT DF_ChannelMembers_CreateAt DEFAULT (GETDATE()) FOR CreateAt
GO
