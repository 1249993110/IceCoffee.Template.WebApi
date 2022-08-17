PRAGMA FOREIGN_KEYS = ON;						--启用外键

--角色许可表
CREATE TABLE IF NOT EXISTS T_RolePermission(	
	Fk_RoleId TEXT NOT NULL,					--角色Id
	Fk_PermissionId TEXT NOT NULL,				--许可Id
	PRIMARY KEY(Fk_RoleId,Fk_PermissionId),
	FOREIGN KEY (Fk_RoleId) REFERENCES T_Role(Id) ON DELETE CASCADE,
	FOREIGN KEY (Fk_PermissionId) REFERENCES T_Permission(Id) ON DELETE CASCADE
);
