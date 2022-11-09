USE [IceCoffee.Template]
GO

--创建视图
CREATE VIEW V_RoleMenu AS
SELECT
	r.Id AS RoleId,
	r.Name AS RoleName,
	r.IsEnabled AS RoleEnabled,
	m.Id AS MenuId,
	m.ParentId AS MenuParentId,
	m.Name AS MenuName,
	m.Icon,
	m.Sort,
	m.Url,
	m.IsEnabled AS MenuEnabled,
	m.IsExternalLink,
	m.Description AS MenuDescription,
FROM
	T_Role AS r
LEFT JOIN T_RoleMenu AS rm ON
	r.Id = rm.Fk_RoleId 
LEFT JOIN T_Menu AS m ON
	rm.Fk_MenuId = m.Id;
GO
