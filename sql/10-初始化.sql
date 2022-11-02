USE [IceCoffee.Template]
GO

INSERT INTO T_User(Id,Name,DisplayName,PasswordHash,PasswordSalt,LoginEnabled) VALUES('6474EFE2-7C58-9C1B-8A89-88C898CB543A','admin','系统管理员','Xg9+QTHDb5Mw9vaEe9q8PqvlZqE=','NCPuMnV9WlfswrYk42cENwKP2mU/K9IJ',1);

INSERT INTO T_Role(Id,Name,Description) VALUES('929A7D9F-AEE3-8634-A009-4C12259E09AD','Administrator','管理员');
INSERT INTO T_UserRole(Fk_UserId,Fk_RoleId) VALUES((SELECT Id FROM T_User WHERE Name='admin'),(SELECT Id FROM T_Role WHERE Name='Administrator'));


INSERT INTO T_Permission(Id,Area) VALUES('F3502BF4-0AFB-93B3-0E1D-43786DF94AB1','SystemManagement');
INSERT INTO T_RolePermission(Fk_RoleId,Fk_PermissionId) VALUES((SELECT Id FROM T_Role WHERE Name='Administrator'),(SELECT Id FROM T_Permission WHERE Area='SystemManagement'));

INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('F1FB0526-CB1E-FCA8-2754-5868EFF0194B',null,'主页','home',0,'/home',1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('BB106982-0F6E-BFD8-F668-FE622FA6195A',null,'系统管理','s-management',99,null,1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('ABC2607E-3EA6-D730-BB2C-7C8B3C213E88',(SELECT Id FROM T_Menu WHERE Name='系统管理'),'用户管理','user',1,'/system-management/users',1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('32B78DAF-CCAF-0AAE-2AF8-143ECD878D58',(SELECT Id FROM T_Menu WHERE Name='系统管理'),'角色管理','role',2,'/system-management/roles',1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('2F2C047E-716B-417E-6AE8-188E1F3AA684',(SELECT Id FROM T_Menu WHERE Name='系统管理'),'菜单管理','menu',3,'/system-management/menus',1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('4AF9C52E-DA74-116C-4EA3-95DBE373B0D5',(SELECT Id FROM T_Menu WHERE Name='系统管理'),'权限管理','permission',4,'/system-management/permissions',1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled,IsExternalLink) VALUES('3001E9D1-17EB-2B7C-FAE2-7724B74FD7CA',null,'接口文档','document',100,'/swagger/index.html',1,1);

INSERT INTO T_RoleMenu(Fk_RoleId,Fk_MenuId) SELECT r.Id AS Fk_RoleId, m.Id AS Fk_MenuId FROM T_Role AS r LEFT JOIN T_Menu AS m ON r.Name='Administrator';
