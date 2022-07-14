--菜单表
CREATE TABLE t_menu(
	id UUID NOT NULL DEFAULT GEN_RANDOM_UUID(),					--菜单Id
	created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,	--创建日期
	creator_id UUID,											--创建者Id
	modifier_id UUID,											--修改者Id
	modified_date TIMESTAMP,									--修改日期
	parent_id UUID,												--父菜单Id
	name VARCHAR(512) NOT NULL,									--菜单名称
	icon VARCHAR(32),											--菜单图标
	sort INTEGER NOT NULL,										--排序
	url VARCHAR(512),											--菜单Url
	is_enabled BOOLEAN NOT NULL,								--是否启用
	is_external_link BOOLEAN NOT NULL DEFAULT FALSE,			--是否为外链
	description VARCHAR(512),									--说明
	PRIMARY KEY(id)
);
