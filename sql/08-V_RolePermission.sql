USE [IceCoffee.Template]
GO

--创建视图
CREATE VIEW V_RolePermission AS
SELECT
	r.Id AS RoleId,
	r.Name AS RoleName,
	p.Id AS PermissionId,
	p.Area
FROM
	T_Role AS r
LEFT JOIN T_RolePermission AS rp ON
	r.Id = rp.Fk_RoleId 
LEFT JOIN T_Permission AS p ON
	rp.Fk_PermissionId = p.Id;
GO
