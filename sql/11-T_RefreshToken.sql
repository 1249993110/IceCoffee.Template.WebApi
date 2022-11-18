USE [IceCoffee.Template]
GO

--Jwt RefreshToken表
CREATE TABLE T_RefreshToken(
	Id CHAR(64) NOT NULL PRIMARY KEY,							--Refresh Token
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),			--创建日期
	FK_UserId UNIQUEIDENTIFIER NOT NULL,						--用户Id
	JwtId UNIQUEIDENTIFIER NOT NULL,							--使用 JwtId 映射到对应的 token
	IsRevorked BIT NOT NULL,									--是否出于安全原因已将其撤销
	ExpiryDate DATETIME NOT NULL,								--Refresh Token 的生命周期很长，可以长达数月。注意一个Refresh Token只能被用来刷新一次
	FOREIGN KEY (FK_UserId) REFERENCES T_User(Id) ON DELETE CASCADE
);

--创建索引
CREATE INDEX Index_FK_UserId ON T_RefreshToken(FK_UserId);
CREATE UNIQUE INDEX Index_JwtId ON T_RefreshToken(JwtId);
GO
