USE [IceCoffee.Template]
GO

--创建视图
CREATE VIEW V_UserAggregate AS
SELECT 
      UserId AS Id
      ,UserName AS Name
      ,CreatedDate
      ,DisplayName
      ,PhoneNumber
      ,Email
      ,LastLoginTime
      ,LastLoginIp
      ,Address
      ,Description
      ,UserEnabled AS IsEnabled
      ,STRING_AGG(CAST(RoleId AS VARCHAR(36)),',') AS Roles
FROM V_User GROUP BY 
      UserId
      ,UserName
      ,CreatedDate
      ,DisplayName
      ,PhoneNumber
      ,Email
      ,LastLoginTime
      ,LastLoginIp
      ,Address
      ,Description
      ,UserEnabled
GO
