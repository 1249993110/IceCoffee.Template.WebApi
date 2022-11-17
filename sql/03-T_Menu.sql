USE [IceCoffee.Template]
GO

--菜单表
CREATE TABLE T_Menu(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),			--角色Id
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),			--创建日期
	CreatorId UNIQUEIDENTIFIER,									--创建者Id
	ModifierId UNIQUEIDENTIFIER,								--修改者Id
	ModifiedDate DATETIME,										--修改日期
	ParentId UNIQUEIDENTIFIER,									--父菜单Id
	Name NVARCHAR(512) NOT NULL,									--菜单名称
	Icon VARCHAR(32),											--菜单图标
	Sort INTEGER NOT NULL,										--排序
	Url VARCHAR(512),											--菜单Url
	IsEnabled BIT NOT NULL,										--是否启用
	IsExternalLink BIT NOT NULL DEFAULT 0,						--是否为外链
	Description NVARCHAR(512)									--说明
);

--创建索引
CREATE INDEX Index_IsEnabled ON T_Menu(IsEnabled);
GO
