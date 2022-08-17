--创建视图
CREATE VIEW IF NOT EXISTS V_UserRole AS
SELECT
	u.Id AS UserId,
	u.Name AS UserName,
	u.DisplayName,
	u.Email,
	u.PhoneNumber,
	r.Id AS RoleId,
	r.Name AS RoleName
FROM
	T_User AS u
LEFT JOIN T_UserRole AS ur ON
	u.Id = ur.Fk_UserId 
LEFT JOIN T_Role AS r ON
	ur.Fk_RoleId = r.Id;
