--许可表
CREATE TABLE t_permission(
	id UUID NOT NULL DEFAULT GEN_RANDOM_UUID(),					--许可Id
	created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,	--创建日期
	creator_id UUID,											--创建者Id
	modifier_id UUID,											--修改者Id
	modified_date TIMESTAMP,									--修改日期
	area VARCHAR(512) NOT NULL,									--授权区域
	http_methods VARCHAR(64),									--允许的HTTP方法, 逗号分割
	description VARCHAR(512),									--说明
	PRIMARY KEY(id)
);
