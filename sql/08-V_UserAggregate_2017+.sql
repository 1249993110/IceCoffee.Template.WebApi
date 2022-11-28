USE [IceCoffee.Template]
GO

--创建视图
CREATE VIEW V_UserAggregate AS
SELECT 
      Id
      ,Name
      ,CreatedDate
      ,DisplayName
      ,PhoneNumber
      ,Email
      ,LastLoginTime
      ,LastLoginIp
      ,Address
      ,Description
      ,IsEnabled
      ,(SELECT STRING_AGG(CAST(FK_RoleId AS VARCHAR(36)),',') FROM T_UserRole WHERE FK_UserId = T_User.Id) AS Roles
From T_User
GO
