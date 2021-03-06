-- select dss dist not exist in abo
  select dss_dist.*
  from dms.dbo.tblDistributor dss_dist
  left join abo.dbo.tblDistributor abo_dist ON abo_dist.DistNumber = dss_dist.DistributorNo
  where abo_dist.DistNumber is null --and DistNumber = 2071056
  
-- select dist exist in both dbs

  select abo_dist.*
  from dms.dbo.tblDistributor dss_dist
  join abo.dbo.tblDistributor abo_dist ON abo_dist.DistNumber = dss_dist.DistributorNo
  
-- select in dss db
select *
from dms.dbo.tblDistributor
where LEN(ltrim(rtrim(HPHONE))) > 0
--where DistributorNo = 2008941

-- select in abo db
select * from abo.dbo.tblDistributor where DistNumber in (2756806,2008941,2071056)

---- insert to ABO tblDistributor
--insert abo.dbo.tblDistributorLog
--	(DISTNO, UPDATEDTE, UpdateType, Warehouse)
--values
--	(2009093, '20150707', 'A', '01')