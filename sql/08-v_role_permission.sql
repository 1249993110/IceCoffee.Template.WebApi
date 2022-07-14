--创建视图
CREATE VIEW v_role_permission AS
SELECT
	r.id AS role_id,
	r.name AS role_name,
	p.id AS permission_id,
	p.area,
	p.http_methods
FROM
	t_role AS r
LEFT JOIN t_role_permission AS rp ON
	r.id = rp.fk_role_id 
LEFT JOIN t_permission AS p ON
	rp.fk_permission_id = p.id;
