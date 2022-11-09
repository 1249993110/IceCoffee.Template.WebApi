USE [IceCoffee.Template]
GO

--创建视图
CREATE VIEW V_UserRole AS
SELECT
	u.Id AS UserId,
	u.Name AS UserName,
	u.CreatedDate,
	u.DisplayName,
	u.PhoneNumber,
	u.Email,
	u.LastLoginTime,
	u.LastLoginIp,
	u.Address,
	u.Description,
	u.IsEnabled AS UserEnabled,
	r.Id AS RoleId,
	r.Name AS RoleName,
	r.IsEnabled AS RoleEnabled
FROM
	T_User AS u
LEFT JOIN T_UserRole AS ur ON
	u.Id = ur.Fk_UserId 
LEFT JOIN T_Role AS r ON
	ur.Fk_RoleId = r.Id;
GO
