--菜单表
CREATE TABLE IF NOT EXISTS T_Menu(
	Id TEXT NOT NULL PRIMARY KEY,								--角色Id
	CreatedDate TIMESTAMP NOT NULL DEFAULT (DATETIME(CURRENT_TIMESTAMP,'LOCALTIME')),--创建日期
	CreatorId TEXT,												--创建者Id
	ModifierId TEXT,											--修改者Id
	ModifiedDate TIMESTAMP,										--修改日期
	ParentId TEXT,												--父菜单Id
	Name VARCHAR(512) NOT NULL,									--菜单名称
	Icon VARCHAR(32),											--菜单图标
	Sort INTEGER NOT NULL,										--排序
	Url VARCHAR(512),											--菜单Url
	IsEnabled BOOLEAN NOT NULL,									--是否启用
	IsExternalLink BOOLEAN NOT NULL DEFAULT FALSE,				--是否为外链
	Description VARCHAR(512)									--说明
);
