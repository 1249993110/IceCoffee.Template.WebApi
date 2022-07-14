--Jwt RefreshToken表
CREATE TABLE t_refresh_token(
	id CHAR(64) NOT NULL,										--Refresh Token
	created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,	--创建日期
	fk_user_id UUID NOT NULL REFERENCES t_user(id),				--用户Id
	jwt_id UUID NOT NULL,										--使用 JwtId 映射到对应的 token
	is_revorked BOOLEAN NOT NULL,								--是否出于安全原因已将其撤销
	expiry_date TIMESTAMP NOT NULL,								--Refresh Token 的生命周期很长，可以长达数月。注意一个Refresh Token只能被用来刷新一次
	PRIMARY KEY(id)
);

--创建索引
CREATE INDEX index_fk_user_id ON t_refresh_token(fk_user_id);
CREATE UNIQUE INDEX index_jwt_id ON t_refresh_token(jwt_id);
