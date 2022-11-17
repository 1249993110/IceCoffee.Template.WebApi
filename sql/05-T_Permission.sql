USE [IceCoffee.Template]
GO

--许可表
CREATE TABLE T_Permission(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),			--角色Id
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),			--创建日期
	CreatorId UNIQUEIDENTIFIER,									--创建者Id
	ModifierId UNIQUEIDENTIFIER,								--修改者Id
	ModifiedDate DATETIME,										--修改日期
	Area VARCHAR(512) NOT NULL,									--授权区域
	IsEnabled BIT NOT NULL,										--是否启用
	Description NVARCHAR(512)									--说明
);

--创建索引
CREATE UNIQUE INDEX Index_Area ON T_Permission(Area);
CREATE INDEX Index_IsEnabled ON T_Permission(IsEnabled);
GO
