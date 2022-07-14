--角色表
CREATE TABLE t_role(
	id UUID NOT NULL DEFAULT GEN_RANDOM_UUID(),					--角色Id
	created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,	--创建日期
	creator_id UUID,											--创建者Id
	modifier_id UUID,											--修改者Id
	modified_date TIMESTAMP,									--修改日期
	name VARCHAR(64) NOT NULL,									--角色名称
	description VARCHAR(512),									--说明
	is_enabled BOOLEAN NOT NULL,								--是否启用
	PRIMARY KEY(id)
);
