
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Allan Albacete
-- Create date: 03/27/2013
-- Description:	Get Plans for specific account (EMEDICARD)
-- =============================================
ALTER PROCEDURE emed_account_plans(@accountCode as varchar(25))
AS
BEGIN

	SELECT B.PLAN_DESC + ' | ' +  A.RNB_FOR as [plan], A.RSPROOMRATE_ID,  a.PLAN_CODE FROM SYS_ROOMRATE_MTBL A 
		INNER JOIN SYS_PLAN_LTBL B ON B.PLAN_CODE = A.PLAN_CODE 
		WHERE A.ACCOUNT_CODE=@accountCode 
		ORDER BY A.RSPROOMRATE_ID ASC		
END
GO
