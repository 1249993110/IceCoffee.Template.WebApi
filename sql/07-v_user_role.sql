--创建视图
CREATE VIEW v_user_role AS
SELECT
	u.id AS user_id,
	u.name AS user_name,
	u.display_name,
	u.email,
	u.phone_number,
	r.id AS role_id,
	r.name AS role_name,
	r.is_enabled AS role_enabled
FROM
	t_user AS u
LEFT JOIN t_user_role AS ur ON
	u.id = ur.fk_user_id 
LEFT JOIN T_Role AS r ON
	ur.fk_role_id = r.id;
