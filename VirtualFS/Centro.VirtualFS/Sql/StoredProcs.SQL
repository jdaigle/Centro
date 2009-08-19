SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Joseph Daigle
-- Create date: 12/21/2007
-- Description:	This will create a new Root Directory with the given directory name
-- =============================================
CREATE PROCEDURE [dbo].sp_CreateNewRootDirectory 
	@directoryname varchar(255)
AS
BEGIN
	INSERT INTO [dbo].Directories (directoryname, parentdirectoryid, directorydeleted, rootdirectory)
		   VALUES (@directoryname, 0, 0, 1);
END
GO



-- =============================================
-- Author:		Joseph Daigle
-- Create date: 12/21/2007
-- Description:	This will setup the default ACL for a specific directory given it's ID
-- =============================================
CREATE PROCEDURE [dbo].sp_CreateDefaultAccessControlList 
	@directoryid int
AS
BEGIN
	IF NOT EXISTS (SELECT aclid FROM [dbo].AccessControlLists WHERE directoryid=@directoryid)
	BEGIN
		INSERT INTO [dbo].AccessControlLists (directoryid) VALUES (@directoryid)
	END
END
GO



-- =============================================
-- Author:		Joseph Daigle
-- Create date: 12/21/2007
-- Description:	This will give a specific user admin privilages on a specific directory given it's ID
-- =============================================
CREATE PROCEDURE [dbo].sp_GrantAdminPermission
	@membername varchar(100),
	@directoryid int
AS
BEGIN
	DECLARE @aclid INT	
	IF NOT EXISTS (SELECT aclid FROM [dbo].AccessControlLists WHERE directoryid=@directoryid)
		EXEC [dbo].sp_CreateDefaultAccessControlList @directoryid;
	SELECT @aclid= aclid FROM [dbo].AccessControlLists WHERE directoryid=@directoryid
	IF NOT EXISTS (SELECT aclmemberid FROM [dbo].ACLMembers WHERE aclid=@aclid AND membername=@membername)
		INSERT INTO [dbo].ACLMembers (aclid, membername, aclread, aclchange, acladmin)
			   VALUES (@aclid, @membername, 0, 0, 1)
	UPDATE [dbo].ACLMembers SET aclread=0, aclchange=0, acladmin=1 WHERE membername=@membername AND aclid=@aclid;
END
GO
