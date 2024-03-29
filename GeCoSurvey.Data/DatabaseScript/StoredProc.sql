﻿USE [GeCo.Auth]
GO
/****** Object:  StoredProcedure [dbo].[GetUsersInArea]    Script Date: 07/16/2011 20:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUsersInArea]
	-- Add the parameters for the stored procedure here
	@Area nvarchar,
	@Ruolo nvarchar
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     dbo.aspnet_Users.LoweredUserName
FROM         dbo.aspnet_ProfileCustom INNER JOIN
                      dbo.aspnet_UsersInRoles ON dbo.aspnet_ProfileCustom.UserID = dbo.aspnet_UsersInRoles.UserId INNER JOIN
                      dbo.aspnet_Users ON dbo.aspnet_ProfileCustom.UserID = dbo.aspnet_Users.UserId CROSS JOIN
                      dbo.aspnet_Roles
WHERE     (dbo.aspnet_ProfileCustom.Area = @Area) AND (dbo.aspnet_Roles.RoleName = @Ruolo)
END
