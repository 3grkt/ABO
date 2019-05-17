IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_tblDistributor_GetByDistNumbers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_tblDistributor_GetByDistNumbers]
GO

-- =============================================
-- Author:		Tri Nguyen
-- Description:	Get distributor data by list of DistributorNo
-- Log:
--		03/21/2015: created
-- =============================================
CREATE PROCEDURE [dbo].[usp_tblDistributor_GetByDistNumbers] 
(
	@DistNumbers varchar(max)
)
AS
SELECT
	[DistributorNo],
	[Name1],
	[Sex1],
	[Name2],
	[Sex2],
	[Address1],
	[Address2],
	[Address3],
	[Address4],
	[City],
	[Mobile],
	[HPHONE],
	[BPHONE],
	[Status],
	[Class],
	[Award],
	[JDATE],
	[EJDATE],
	[SPONS],
	[DD],
	[DIAMOND],
	[BDATE1],
	[BPLACE1],
	[BDATE2],
	[BPLACE2],
	[ID1],
	[IDDATE1],
	[IDPLACE1],
	[ID2],
	[IDDATE2],
	[IDPLACE2]
FROM 
	tblDistributor  
WHERE 
	[DistributorNo] IN (SELECT number FROM ufn_bigintlist_to_table(@DistNumbers))


RETURN

GO


