--用户表
CREATE TABLE IF NOT EXISTS T_User(
	Id TEXT NOT NULL PRIMARY KEY,								--用户Id
	CreatedDate TIMESTAMP NOT NULL DEFAULT (DATETIME(CURRENT_TIMESTAMP,'LOCALTIME')),--创建日期
	Name VARCHAR(64) NOT NULL,									--用户名称
	DisplayName VARCHAR(64) NOT NULL,							--显示名称
	Email VARCHAR(256),											--电子邮件
	PasswordHash VARCHAR(512) NOT NULL,							--密码哈希值
	PasswordSalt VARCHAR(512) NOT NULL,							--密码盐值
	PhoneNumber VARCHAR(16),									--电话号码
	Address VARCHAR(512),										--地址
	Description VARCHAR(512),									--说明
	LastLoginTime TIMESTAMP,									--上次登录时间
	LastLoginIp VARCHAR(64),									--上次登录Ip
	LoginEnabled BOOLEAN NOT NULL,								--是否允许登录
	LockoutEndDate TIMESTAMP,									--锁定结束日期
    AccessFailedCount INTEGER NOT NULL DEFAULT 0				--访问失败次数
);

--创建索引
CREATE UNIQUE INDEX IF NOT EXISTS Index_Name ON T_User(Name);
CREATE UNIQUE INDEX IF NOT EXISTS Index_PhoneNumber ON T_User(PhoneNumber);
CREATE INDEX IF NOT EXISTS Index_DisplayName ON T_User(DisplayName);
