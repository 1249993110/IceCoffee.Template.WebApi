USE [IceCoffee.Template]
GO

--创建视图
CREATE VIEW V_User AS
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
      ,(STUFF((SELECT ',' + CAST(RoleId AS VARCHAR(36)) FROM V_UserRole WHERE UserId = vur.UserId FOR XML PATH('')),1,1,'')) AS Roles
FROM V_UserRole AS vur GROUP BY 
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
