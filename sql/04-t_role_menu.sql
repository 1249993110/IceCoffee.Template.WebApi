--角色微信菜单表
CREATE TABLE t_role_menu(	
	fk_role_id UUID REFERENCES t_role(id),	--角色Id
	fk_menu_id UUID REFERENCES t_menu(id),	--菜单Id
	PRIMARY KEY(fk_role_id,fk_menu_id)
);
