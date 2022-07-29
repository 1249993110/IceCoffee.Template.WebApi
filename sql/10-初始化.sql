INSERT INTO t_user(name,display_name,password_hash,password_salt,login_enabled) VALUES('admin','系统管理员','Xg9+QTHDb5Mw9vaEe9q8PqvlZqE=','NCPuMnV9WlfswrYk42cENwKP2mU/K9IJ',true);

INSERT INTO t_role(name,description,is_enabled) VALUES('Administrator','管理员',true);
INSERT INTO t_user_role(fk_user_id,fk_role_id) VALUES((SELECT id FROM t_user WHERE name='admin'),(SELECT id FROM t_role WHERE name='Administrator'));


INSERT INTO t_permission(area,http_methods) VALUES('SystemManagement','*');
INSERT INTO t_role_permission(fk_role_id,fk_permission_id) VALUES((SELECT id FROM t_role WHERE name='Administrator'),(SELECT id FROM t_permission WHERE area='SystemManagement'));

INSERT INTO t_menu(parent_id,name,sort,url,is_enabled) VALUES(null,'主页',0,'/home',true);
INSERT INTO t_menu(parent_id,name,sort,url,is_enabled) VALUES(null,'系统管理',99,null,true);
INSERT INTO t_menu(parent_id,name,sort,url,is_enabled) VALUES((SELECT id FROM t_menu WHERE name='系统管理'),'用户管理',1,'/system-management/users',true);
INSERT INTO t_menu(parent_id,name,sort,url,is_enabled) VALUES((SELECT id FROM t_menu WHERE name='系统管理'),'角色管理',2,'/system-management/roles',true);
INSERT INTO t_menu(parent_id,name,sort,url,is_enabled) VALUES((SELECT id FROM t_menu WHERE name='系统管理'),'菜单管理',3,'/system-management/menus',true);
INSERT INTO t_menu(parent_id,name,sort,url,is_enabled) VALUES((SELECT id FROM t_menu WHERE name='系统管理'),'权限管理',4,'/system-management/permissions',true);
INSERT INTO t_menu(parent_id,name,sort,url,is_enabled,is_external_link) VALUES(null,'接口文档',99,'/api/swagger/index.html',true,true);

INSERT INTO t_role_menu(fk_role_id,fk_menu_id) VALUES((SELECT id FROM t_role WHERE name='Administrator'),UNNEST(ARRAY(SELECT id FROM t_menu)));