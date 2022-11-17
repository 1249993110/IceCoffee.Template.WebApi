USE [IceCoffee.Template]
GO

--角色表
CREATE TABLE T_Role(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),			--角色Id
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),			--创建日期
	CreatorId UNIQUEIDENTIFIER,									--创建者Id
	ModifierId UNIQUEIDENTIFIER,								--修改者Id
	ModifiedDate DATETIME,										--修改日期
	Name NVARCHAR(64) NOT NULL,									--角色名称
	IsEnabled BIT NOT NULL,										--是否启用
	Description NVARCHAR(512)									--说明
);

--创建索引
CREATE UNIQUE INDEX Index_Name ON T_Role(Name);
CREATE INDEX Index_IsEnabled ON T_Role(IsEnabled);
GO
