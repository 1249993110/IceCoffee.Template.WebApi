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
