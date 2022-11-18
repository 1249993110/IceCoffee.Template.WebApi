USE [IceCoffee.Template]
GO

--用户表
CREATE TABLE  T_User(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),			--用户Id
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),			--创建日期
	Name VARCHAR(64) NOT NULL,									--用户名称, 不允许使用全数字或邮箱格式
	DisplayName NVARCHAR(64) NOT NULL,							--显示名称
	Email VARCHAR(256),											--电子邮件
	PasswordHash VARCHAR(512) NOT NULL,							--密码哈希值
	PasswordSalt VARCHAR(512) NOT NULL,							--密码盐值
	PhoneNumber VARCHAR(16),									--电话号码
	Address NVARCHAR(512),										--地址
	Description NVARCHAR(512),									--说明
	LastLoginTime DATETIME,										--上次登录时间
	LastLoginIp VARCHAR(64),									--上次登录Ip
	IsEnabled BIT NOT NULL,										--是否启用
	LockoutEndDate DATETIME,									--锁定结束日期
    AccessFailedCount INTEGER NOT NULL DEFAULT 0				--访问失败次数
);

--创建索引
CREATE UNIQUE INDEX Index_Name ON T_User(Name);
CREATE UNIQUE INDEX Index_PhoneNumber ON T_User(PhoneNumber);
CREATE INDEX Index_DisplayName ON T_User(DisplayName);
CREATE INDEX Index_IsEnabled ON T_User(IsEnabled);
GO
USE [IceCoffee.Template]
GO

--角色表
CREATE TABLE T_Role(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),			--角色Id
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),			--创建日期
	CreatorId UNIQUEIDENTIFIER,									--创建者Id
	ModifierId UNIQUEIDENTIFIER,								--修改者Id
	ModifiedDate DATETIME,										--修改日期
	Name NVARCHAR(64) NOT NULL,									--角色名称
	IsEnabled BIT NOT NULL,										--是否启用
	Description NVARCHAR(512)									--说明
);

--创建索引
CREATE UNIQUE INDEX Index_Name ON T_Role(Name);
CREATE INDEX Index_IsEnabled ON T_Role(IsEnabled);
GO
USE [IceCoffee.Template]
GO

--用户角色表
CREATE TABLE T_UserRole(
	Fk_UserId UNIQUEIDENTIFIER NOT NULL,				--用户Id
	Fk_RoleId UNIQUEIDENTIFIER NOT NULL,				--角色Id
	PRIMARY KEY(Fk_UserId,Fk_RoleId),
	FOREIGN KEY (Fk_UserId) REFERENCES T_User(Id) ON DELETE CASCADE,
	FOREIGN KEY (Fk_RoleId) REFERENCES T_Role(Id) ON DELETE CASCADE
);
GO
USE [IceCoffee.Template]
GO

--菜单表
CREATE TABLE T_Menu(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),			--角色Id
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),			--创建日期
	CreatorId UNIQUEIDENTIFIER,									--创建者Id
	ModifierId UNIQUEIDENTIFIER,								--修改者Id
	ModifiedDate DATETIME,										--修改日期
	ParentId UNIQUEIDENTIFIER,									--父菜单Id
	Name NVARCHAR(512) NOT NULL,									--菜单名称
	Icon VARCHAR(32),											--菜单图标
	Sort INTEGER NOT NULL,										--排序
	Url VARCHAR(512),											--菜单Url
	IsEnabled BIT NOT NULL,										--是否启用
	IsExternalLink BIT NOT NULL DEFAULT 0,						--是否为外链
	Description NVARCHAR(512)									--说明
);

--创建索引
CREATE INDEX Index_IsEnabled ON T_Menu(IsEnabled);
GO
USE [IceCoffee.Template]
GO

--角色菜单表
CREATE TABLE T_RoleMenu(	
	Fk_RoleId UNIQUEIDENTIFIER NOT NULL,				--角色Id
	Fk_MenuId UNIQUEIDENTIFIER NOT NULL,				--菜单Id
	PRIMARY KEY(Fk_RoleId,Fk_MenuId),
	FOREIGN KEY (Fk_RoleId) REFERENCES T_Role(Id) ON DELETE CASCADE,
	FOREIGN KEY (Fk_MenuId) REFERENCES T_Menu(Id) ON DELETE CASCADE
);
GO
USE [IceCoffee.Template]
GO

--许可表
CREATE TABLE T_Permission(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),			--角色Id
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),			--创建日期
	CreatorId UNIQUEIDENTIFIER,									--创建者Id
	ModifierId UNIQUEIDENTIFIER,								--修改者Id
	ModifiedDate DATETIME,										--修改日期
	Area VARCHAR(512) NOT NULL,									--授权区域
	IsEnabled BIT NOT NULL,										--是否启用
	Description NVARCHAR(512)									--说明
);

--创建索引
CREATE UNIQUE INDEX Index_Area ON T_Permission(Area);
CREATE INDEX Index_IsEnabled ON T_Permission(IsEnabled);
GO
USE [IceCoffee.Template]
GO

--角色许可表
CREATE TABLE T_RolePermission(	
	Fk_RoleId UNIQUEIDENTIFIER NOT NULL,					--角色Id
	Fk_PermissionId UNIQUEIDENTIFIER NOT NULL,				--许可Id
	PRIMARY KEY(Fk_RoleId,Fk_PermissionId),
	FOREIGN KEY (Fk_RoleId) REFERENCES T_Role(Id) ON DELETE CASCADE,
	FOREIGN KEY (Fk_PermissionId) REFERENCES T_Permission(Id) ON DELETE CASCADE
);
GO
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
USE [IceCoffee.Template]
GO

--创建视图
CREATE VIEW V_User AS
SELECT
      UserId AS Id
      ,UserName AS Name
      ,CreatedDate
      ,DisplayName
      ,PhoneNumber
      ,Email
      ,LastLoginTime
      ,LastLoginIp
      ,Address
      ,Description
      ,UserEnabled AS IsEnabled
      ,(STUFF((SELECT ',' + CAST(RoleId AS VARCHAR(36)) FROM V_UserRole WHERE UserId = vur.UserId FOR XML PATH('')),1,1,'')) AS Roles
FROM V_UserRole AS vur GROUP BY 
      UserId
      ,UserName
      ,CreatedDate
      ,DisplayName
      ,PhoneNumber
      ,Email
      ,LastLoginTime
      ,LastLoginIp
      ,Address
      ,Description
      ,UserEnabled
GO
USE [IceCoffee.Template]
GO

--创建视图
CREATE VIEW V_User AS
SELECT 
      UserId AS Id
      ,UserName AS Name
      ,CreatedDate
      ,DisplayName
      ,PhoneNumber
      ,Email
      ,LastLoginTime
      ,LastLoginIp
      ,Address
      ,Description
      ,UserEnabled AS IsEnabled
      ,STRING_AGG(CAST(RoleId AS VARCHAR(36)),',') AS Roles
FROM V_UserRole GROUP BY 
      UserId
      ,UserName
      ,CreatedDate
      ,DisplayName
      ,PhoneNumber
      ,Email
      ,LastLoginTime
      ,LastLoginIp
      ,Address
      ,Description
      ,UserEnabled
GO
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
	m.Description AS MenuDescription
FROM
	T_Role AS r
LEFT JOIN T_RoleMenu AS rm ON
	r.Id = rm.Fk_RoleId 
LEFT JOIN T_Menu AS m ON
	rm.Fk_MenuId = m.Id;
GO
USE [IceCoffee.Template]
GO

--创建视图
CREATE VIEW V_RolePermission AS
SELECT
	r.Id AS RoleId,
	r.Name AS RoleName,
	r.IsEnabled AS RoleEnabled,
	p.Id AS PermissionId,
	p.Area,
	p.IsEnabled AS PermissionEnabled
FROM
	T_Role AS r
LEFT JOIN T_RolePermission AS rp ON
	r.Id = rp.Fk_RoleId 
LEFT JOIN T_Permission AS p ON
	rp.Fk_PermissionId = p.Id;
GO
USE [IceCoffee.Template]
GO

--Jwt RefreshToken表
CREATE TABLE T_RefreshToken(
	Id CHAR(64) NOT NULL PRIMARY KEY,							--Refresh Token
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),			--创建日期
	Fk_UserId UNIQUEIDENTIFIER NOT NULL,						--用户Id
	JwtId UNIQUEIDENTIFIER NOT NULL,							--使用 JwtId 映射到对应的 token
	IsRevorked BIT NOT NULL,									--是否出于安全原因已将其撤销
	ExpiryDate DATETIME NOT NULL,								--Refresh Token 的生命周期很长，可以长达数月。注意一个Refresh Token只能被用来刷新一次
	FOREIGN KEY (Fk_UserId) REFERENCES T_User(Id) ON DELETE CASCADE
);

--创建索引
CREATE INDEX Index_Fk_UserId ON T_RefreshToken(Fk_UserId);
CREATE UNIQUE INDEX Index_JwtId ON T_RefreshToken(JwtId);
GO
USE [IceCoffee.Template]
GO

INSERT INTO T_User(Id,Name,DisplayName,PasswordHash,PasswordSalt,IsEnabled) VALUES('6474EFE2-7C58-9C1B-8A89-88C898CB543A','admin','系统管理员','Xg9+QTHDb5Mw9vaEe9q8PqvlZqE=','NCPuMnV9WlfswrYk42cENwKP2mU/K9IJ',1);

INSERT INTO T_Role(Id,Name,IsEnabled,Description) VALUES('929A7D9F-AEE3-8634-A009-4C12259E09AD','Administrator',1,'管理员');
INSERT INTO T_UserRole(Fk_UserId,Fk_RoleId) VALUES((SELECT Id FROM T_User WHERE Name='admin'),(SELECT Id FROM T_Role WHERE Name='Administrator'));


INSERT INTO T_Permission(Id,Area,IsEnabled) VALUES('F3502BF4-0AFB-93B3-0E1D-43786DF94AB1','SystemManagement',1);
INSERT INTO T_RolePermission(Fk_RoleId,Fk_PermissionId) VALUES((SELECT Id FROM T_Role WHERE Name='Administrator'),(SELECT Id FROM T_Permission WHERE Area='SystemManagement'));

INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('F1FB0526-CB1E-FCA8-2754-5868EFF0194B',null,'主页','home',0,'/home',1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('BB106982-0F6E-BFD8-F668-FE622FA6195A',null,'系统管理','s-management',99,null,1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('ABC2607E-3EA6-D730-BB2C-7C8B3C213E88',(SELECT Id FROM T_Menu WHERE Name='系统管理'),'用户管理','user',1,'/system-management/users',1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('32B78DAF-CCAF-0AAE-2AF8-143ECD878D58',(SELECT Id FROM T_Menu WHERE Name='系统管理'),'角色管理','role',2,'/system-management/roles',1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('2F2C047E-716B-417E-6AE8-188E1F3AA684',(SELECT Id FROM T_Menu WHERE Name='系统管理'),'菜单管理','menu',3,'/system-management/menus',1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled) VALUES('4AF9C52E-DA74-116C-4EA3-95DBE373B0D5',(SELECT Id FROM T_Menu WHERE Name='系统管理'),'权限管理','permission',4,'/system-management/permissions',1);
INSERT INTO T_Menu(Id,ParentId,Name,Icon,Sort,Url,IsEnabled,IsExternalLink) VALUES('3001E9D1-17EB-2B7C-FAE2-7724B74FD7CA',null,'接口文档','document',100,'/swagger/index.html',1,1);

INSERT INTO T_RoleMenu(Fk_RoleId,Fk_MenuId) SELECT r.Id AS Fk_RoleId, m.Id AS Fk_MenuId FROM T_Role AS r LEFT JOIN T_Menu AS m ON r.Name='Administrator';
