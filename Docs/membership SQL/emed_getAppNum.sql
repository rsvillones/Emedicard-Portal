USE [Membership]
GO
/****** Object:  StoredProcedure [dbo].[usp_RxER_get_Last_ID]    Script Date: 04/01/2013 09:32:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Allan Albacete
-- Create date: 04/01/2013
-- Description:	GetAppnum
-- =============================================
CREATE PROCEDURE [dbo].[emed_GetAPPNUM]	
	@Description as  varchar(50)
as 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   
	SET NOCOUNT ON;
	
	declare @CURRENT_DATE nvarchar(max)
	set @CURRENT_DATE = cast( year( getdate()) as nvarchar(MAX)) +  cast( month( getdate()) as nvarchar(MAX))	

	begin tran
	
	UPDATE MEMBERSHIP.DBO.SYS_LASTID_MTBL
	WITH (ROWLOCK)
	SET LastID=LastID+1,LASTDATE=getdate()			
	output deleted.LastID as last_id
	--into #TempTable
	WHERE FIELDNAME = @Description
	
	COMMIT TRAN	 
	
	
END


