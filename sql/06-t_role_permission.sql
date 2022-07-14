--角色许可表
CREATE TABLE t_role_permission(	
	fk_role_id UUID REFERENCES t_role(id),				--角色Id
	fk_permission_id UUID REFERENCES t_permission(Id),	--许可Id
	PRIMARY KEY(fk_role_id,fk_permission_id)
);
