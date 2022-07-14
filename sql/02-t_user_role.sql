--用户角色表
CREATE TABLE t_user_role(
	fk_user_id UUID REFERENCES t_user(id),	--用户Id
	fk_role_id UUID REFERENCES t_role(id),	--角色Id
	PRIMARY KEY(fk_user_id,fk_role_id)
);
