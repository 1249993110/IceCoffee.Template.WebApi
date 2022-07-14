--用户表
CREATE TABLE t_user(
	id UUID NOT NULL DEFAULT GEN_RANDOM_UUID(),					--用户Id
	created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,	--创建日期
	name VARCHAR(64) NOT NULL,									--用户名称
	display_name VARCHAR(64) NOT NULL,							--显示名称
	email VARCHAR(256),											--电子邮件
	password_hash VARCHAR(512) NOT NULL,						--密码哈希值
	password_salt VARCHAR(512) NOT NULL,						--密码盐值
	phone_number VARCHAR(16),									--电话号码
	address VARCHAR(512),										--地址
	description VARCHAR(512),									--说明
	last_login_time TIMESTAMP,									--上次登录时间
	last_login_ip VARCHAR(64),									--上次登录Ip
	login_enabled BOOLEAN NOT NULL,								--是否允许登录
	lockout_end_date TIMESTAMP,									--锁定结束日期
    access_failed_count INTEGER NOT NULL DEFAULT 0,				--访问失败次数
	PRIMARY KEY(id)
);

--创建索引
CREATE UNIQUE INDEX index_name ON t_user(name);
CREATE UNIQUE INDEX index_phone_number ON t_user(phone_number);
CREATE INDEX index_display_name ON t_user(display_name);
