USE [IceCoffee.Template]
GO

--创建视图
CREATE VIEW V_RolePermission AS
SELECT
	r.Id AS RoleId,
	r.Name AS RoleName,
	r.IsEnabled AS RoleEnabled,
	p.Id AS PermissionId,
	p.Area,
	p.IsEnabled AS PermissionEnabled
FROM
	T_Role AS r
LEFT JOIN T_RolePermission AS rp ON
	r.Id = rp.FK_RoleId 
LEFT JOIN T_Permission AS p ON
	rp.FK_PermissionId = p.Id;
GO
