
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Allan Albacete
-- Create date: 04/03/2012
-- Description:	Get Active Member 
-- =============================================
CREATE PROCEDURE emed_check_active_member 
	-- Add the parameters for the stored procedure here
	@MemberCode varchar(20), 
	@AccountCode varchar(25)
AS
BEGIN
	SELECT MEM_LNAME, MEM_FNAME, MEM_MI, MEM_BDAY, PRIN_CODE, PRIN_APPNUM FROM SYS_UWPRINCIPAL_ACTIVE_MTBL WHERE PRIN_CODE = @MemberCode AND ACCOUNT_CODE=@AccountCode 
END
GO
