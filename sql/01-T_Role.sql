USE [IceCoffee.Template]
GO

--角色表
CREATE TABLE T_Role(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),			--角色Id
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),			--创建日期
	CreatorId UNIQUEIDENTIFIER,									--创建者Id
	ModifierId UNIQUEIDENTIFIER,								--修改者Id
	ModifiedDate DATETIME,										--修改日期
	Name VARCHAR(64) NOT NULL,									--角色名称
	Description VARCHAR(512)									--说明
);
GO
