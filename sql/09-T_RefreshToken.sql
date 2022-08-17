--Jwt RefreshToken表
CREATE TABLE IF NOT EXISTS T_RefreshToken(
	Id CHAR(64) NOT NULL PRIMARY KEY,							--Refresh Token
	CreatedDate TIMESTAMP NOT NULL DEFAULT (DATETIME(CURRENT_TIMESTAMP,'LOCALTIME')),--创建日期
	Fk_UserId TEXT,												--用户Id
	JwtId TEXT NOT NULL,										--使用 JwtId 映射到对应的 token
	IsRevorked BOOLEAN NOT NULL,								--是否出于安全原因已将其撤销
	ExpiryDate TIMESTAMP NOT NULL,								--Refresh Token 的生命周期很长，可以长达数月。注意一个Refresh Token只能被用来刷新一次
	FOREIGN KEY (Fk_UserId) REFERENCES T_User(Id) ON DELETE CASCADE
);

--创建索引
CREATE INDEX IF NOT EXISTS Index_Fk_UserId ON T_RefreshToken(Fk_UserId);
CREATE UNIQUE INDEX IF NOT EXISTS Index_JwtId ON T_RefreshToken(JwtId);
