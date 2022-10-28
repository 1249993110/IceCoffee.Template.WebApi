USE [IceCoffee.Template]
GO

--角色许可表
CREATE TABLE T_RolePermission(	
	Fk_RoleId UNIQUEIDENTIFIER NOT NULL,					--角色Id
	Fk_PermissionId UNIQUEIDENTIFIER NOT NULL,				--许可Id
	PRIMARY KEY(Fk_RoleId,Fk_PermissionId),
	FOREIGN KEY (Fk_RoleId) REFERENCES T_Role(Id) ON DELETE CASCADE,
	FOREIGN KEY (Fk_PermissionId) REFERENCES T_Permission(Id) ON DELETE CASCADE
);
GO
