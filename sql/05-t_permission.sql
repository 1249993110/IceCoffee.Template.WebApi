--许可表
CREATE TABLE IF NOT EXISTS T_Permission(
	Id TEXT NOT NULL PRIMARY KEY,								--角色Id
	CreatedDate TIMESTAMP NOT NULL DEFAULT (DATETIME(CURRENT_TIMESTAMP,'LOCALTIME')),--创建日期
	CreatorId TEXT,												--创建者Id
	ModifierId TEXT,											--修改者Id
	ModifiedDate TIMESTAMP,										--修改日期
	Area VARCHAR(512) NOT NULL,									--授权区域
	Description VARCHAR(512)									--说明
);
