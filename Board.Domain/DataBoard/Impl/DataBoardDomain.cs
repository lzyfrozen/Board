using Board.Domain.Entities;
using Board.Infrastructure;
using Board.Infrastructure.DBHelpers;
using Board.ToolKits;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Board.Domain.DataBoard.Impl
{
    public class DataBoardDomain : IDataBoardDomain
    {
        private readonly IOracleRepository _sqlRepository;
        private static string warehouseId = AppSettings.app(new[] { "WareHouse", "WareHouseId" });
        private static string BeforeDay = AppSettings.app(new[] { "WareHouse", "BeforeDay" });
        public DataBoardDomain(IOracleRepository sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }

        public Location GetLocationInfo()
        {
            var sql = $@"begin
                    open :rslt1 for select count(distinct locationid) as totallocationid from bas_location where organizationid='WMSJM' and warehouseid =:warehouseid and zonegroup not like 'STAGING_%';
                    open :rslt2 for select count(distinct a.locationid) as uselocationid from inv_lot_loc_id a 
                        left join bas_location b on a.organizationid=b.organizationid and a.warehouseid=b.warehouseid and a.locationid=b.locationid 
                        where a.organizationid = 'WMSJM' and a.warehouseid =:warehouseid and a.qty>0 and b.zonegroup not like 'STAGING_%';
                    open :rslt3 for select count(1) as ExpireAlert
	                    from INV_LOT_LOC_ID a
	                    left join INV_LOT_ATT b on a.ORGANIZATIONID=b.ORGANIZATIONID and a.CUSTOMERID=b.CUSTOMERID and a.LOTNUM=b.LOTNUM
	                    left join BAS_SKU c on a.ORGANIZATIONID=c.ORGANIZATIONID and a.CUSTOMERID=c.CUSTOMERID and a.SKU=c.SKU
	                    left join  BAS_SKU_MULTIWAREHOUSE bsm on    
	                    c.organizationId = bsm.organizationId    
	                    and c.customerId = bsm.customerId    
	                    and c.sku = bsm.sku    
	                    and bsm.warehouseId = ''
	                    where a.organizationId = 'WMSJM'
	                    and a.warehouseid =:warehouseid
	                    and b.LOTATT07 in ('N')--只取良品库存
	                    and a.qty - a.qtyAllocated -a.qtyOnHold - a.qtyRpOut - a.qtyMvOut >0
	                    and a.qty = a.qty - a.qtyAllocated -a.qtyOnHold - a.qtyRpOut - a.qtyMvOut
	                    and nvl(to_date(b.LOTATT02, 'yyyy-MM-dd'), 
	                    case when c.shelfLifeFlag = 'Y' and c.shelfLifeUnit='DAY' and c.shelfLife is not null then to_date(b.LOTATT01, 'yyyy-MM-dd') + c.shelfLife
	                         when c.shelfLifeFlag = 'Y' and c.shelfLifeUnit='MONTH' and c.shelfLife is not null then add_months(to_date(b.LOTATT01, 'yyyy-MM-dd'), c.shelfLife)
	                         when c.shelfLifeFlag = 'Y' and c.shelfLifeUnit='YEAR' and c.shelfLife is not null then add_months(to_date(b.LOTATT01, 'yyyy-MM-dd'), c.shelfLife*12)
	                    end)-trunc(sysdate) <= case when c.shelfLifeFlag = 'Y' and c.shelfLifeUOM='DAY' and c.shelfLifeAlertDays is not null then c.shelfLifeAlertDays
	 							                    when c.shelfLifeFlag = 'Y' and c.shelfLifeUOM='MONTH' and c.shelfLifeAlertDays is not null then c.shelfLifeAlertDays*30
	 							                    when c.shelfLifeFlag = 'Y' and c.shelfLifeUOM='YEAR' and c.shelfLifeAlertDays is not null then c.shelfLifeAlertDays*365
								                    else 0 end;
                    end;";

            OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();
            dynamicParameters.Add(":rslt1", OracleDbType.RefCursor, ParameterDirection.Output);
            dynamicParameters.Add(":rslt2", OracleDbType.RefCursor, ParameterDirection.Output);
            dynamicParameters.Add(":rslt3", OracleDbType.RefCursor, ParameterDirection.Output);
            dynamicParameters.Add(":warehouseid", OracleDbType.Varchar2, ParameterDirection.Input, warehouseId);
            var reader = _sqlRepository.QueryMultiple(sql, dynamicParameters);
            var result = new Location();
            result.TotalBin = reader.Read<long>().FirstOrDefault();
            result.UseBin = reader.Read<long>().FirstOrDefault();
            result.FreelBin = result.TotalBin - result.UseBin;
            result.ExprieAlert = reader.Read<long>().FirstOrDefault();

            return result;
        }
        public long GetLocationId()
        {
            throw new NotImplementedException();
        }

        public List<InTask> GetInTask()
        {
            //--当天收货未关闭
            var sql = $@"select t.docno as asnno,t.transactiontype,max(t.addtime) as addtime
                        from act_transaction_log t
                        where t.organizationid ='WMSJM' and t.warehouseid =:warehouseid and t.doctype='ASN' and t.transactiontype in ('IN','99') and t.status='99'
                        and t.docno in
                        (
                        select distinct a.docno  
                        from act_transaction_log a
                        where a.organizationid ='WMSJM' and a.warehouseid =:warehouseid and a.doctype='ASN' and a.transactiontype in ('IN') and a.status='99'
                        and trunc(a.addtime) >= trunc(sysdate-{BeforeDay})
                        and not exists (
                        select 1
                        from act_transaction_log b
                        where b.organizationid ='WMSJM' and b.warehouseid =:warehouseid and b.doctype='ASN' and b.transactiontype='90' 
                        and trunc(b.addtime) >= trunc(sysdate-{BeforeDay})
                        and b.docno=a.docno) --order by a.docno
                        )
                        group by t.docno,t.transactiontype
                        order by t.docno";
            var data = _sqlRepository.Query<InTask>(sql, new { warehouseid = warehouseId });
            return data.ToList();
        }

        public List<OutTask> GetOutTask()
        {
            //--当天拣货未关闭
            var sql = $@"select t.docno as orderno,t.transactiontype,max(t.addtime) as addtime
                        from act_transaction_log t
                        where t.organizationid ='WMSJM' and t.warehouseid =:warehouseid and t.doctype='SO' and t.transactiontype in ('PK','99') and t.status='99'
                        and t.docno in
                        (
                        select distinct a.docno  
                        from act_transaction_log a
                        where a.organizationid ='WMSJM' and a.warehouseid =:warehouseid and a.doctype='SO' and a.transactiontype in ('PK') and a.status='99'
                        and trunc(a.addtime) >= trunc(sysdate-{BeforeDay})
                        and not exists (
                        select 1
                        from act_transaction_log b
                        where b.organizationid ='WMSJM' and b.warehouseid =:warehouseid and b.doctype='SO' and b.transactiontype='90' 
                        and trunc(b.addtime) >= trunc(sysdate-{BeforeDay})
                        and b.docno=a.docno) --order by a.docno
                        )
                        group by t.docno,t.transactiontype
                        order by t.docno";
            var data = _sqlRepository.Query<OutTask>(sql, new { warehouseid = warehouseId });
            return data.ToList();
        }

        public List<FGOutTask> GetFGOutTask()
        {
            //--最近7天类型为ZLF1,ZLF2的发运单,状态未关闭的
            var sql = $@"select 
                        a.OrderNo,a.expectedShipmentTime1 as ShipmentTime,a.ConsigneeId,d.customerdescr1 as ConsigneeName,
                        b.Sku,c.skudescr1 as SkuName,c.alternate_sku4 as Model,sum(b.qtyOrdered) as QtyOrdered,sum(b.qtyShipped) as QtyShipped
                        from doc_order_header a 
                        left join doc_order_details b on a.organizationid=b.organizationid and a.warehouseid =b.warehouseid  and a.orderno =b.orderno 
                        left join bas_sku c on b.organizationid=c.organizationid and b.customerid=c.customerid  and b.sku =c.sku
                        left join bas_customer d on a.organizationid=d.organizationid and a.consigneeid=d.customerid and d.customertype='CO' 
                        where a.organizationid ='WMSJM' and a.warehouseid =:warehouseid
                        and a.ordertype in ('ZLF1','ZLF2') and a.sostatus not in ('90','99') and b.linestatus<>'90'
                        and a.addtime >= trunc(sysdate-{BeforeDay})
                        group by a.OrderNo,a.expectedShipmentTime1,a.ConsigneeId,d.customerdescr1,b.Sku,c.skudescr1,c.alternate_sku4
                        order by a.OrderNo desc";
            var data = _sqlRepository.Query<FGOutTask>(sql, new { warehouseid = warehouseId });
            return data.ToList();
        }

        public List<FGInPlan> GetFGInPlan()
        {
            var sql = $@"select rownum,b.* from (select a.sku,a.productmodel,a.planqty,a.completeqty 
                        from WMSHZ_CUS_FG_IN_PLAN_RATE a
                        where a.organizationid = 'WMSJM' and a.warehouseid = :warehouseid
                        order by a.planqty desc) b
                        where rownum<11";
            var data = _sqlRepository.Query<FGInPlan>(sql, new { warehouseid = warehouseId });
            return data.ToList();
        }

        public List<InOutQty> GetInOutQtyWeek()
        {
            //最近一周出入库数量
            var sql = $@"select 'IN' as InOutType,to_char(a.AddTime,'yyyy-mm-dd') as Dates,to_char(a.AddTime,'dd') as Days,sum(a.TOQTY) as Qty 
                        from act_transaction_log a
                        where a.organizationid ='WMSJM' and a.warehouseid =:warehouseid and a.doctype='ASN' and a.transactiontype in ('IN') and a.status='99'
                        and a.addtime >= trunc(sysdate-6)
                        and not exists (
                        select 1
                        from act_transaction_log b
                        where b.organizationid ='WMSJM' and b.warehouseid =:warehouseid and b.doctype='ASN' and b.transactiontype='90' 
                        and b.addtime >= trunc(sysdate-6)
                        and b.docno=a.docno) --order by a.docno
                        group by to_char(a.AddTime,'yyyy-mm-dd'),to_char(a.AddTime,'dd')
                        --order by Dates,Days
                        union all
                        select 'OUT' as InOutType,to_char(a.AddTime,'yyyy-mm-dd') as Dates,to_char(a.AddTime,'dd') as Days,sum(a.TOQTY) as Qty 
                        from act_transaction_log a
                        where a.organizationid ='WMSJM' and a.warehouseid =:warehouseid and a.doctype='SO' and a.transactiontype in ('SO') and a.status='99'
                        and a.addtime >= trunc(sysdate-6)
                        and not exists (
                        select 1
                        from act_transaction_log b
                        where b.organizationid ='WMSJM' and b.warehouseid =:warehouseid and b.doctype='SO' and b.transactiontype='90' 
                        and b.addtime >= trunc(sysdate-6)
                        and b.docno=a.docno) --order by a.docno
                        group by to_char(a.AddTime,'yyyy-mm-dd'),to_char(a.AddTime,'dd')
                        --order by Dates,Days";
            var data = _sqlRepository.Query<InOutQty>(sql, new { warehouseid = warehouseId });
            return data.ToList();
        }

        public List<Humiture> GetHumiture()
        {
            //设备采集最新的一条数据
            var sql = $@"select m.*
                        from (select row_number() over(partition by a.deviceid order by a.collectiontime desc) as seqno,a.organizationid,a.warehouseid,a.deviceid,a.devicename,a.collectiontime,a.humidity,0 as humidity_ll,a.humidity_ul,a.temperature,a.temperature_ll,a.temperature_ul 
                        from wmshz_cus_device_collection a
                        where a.organizationid ='WMSJM' and a.warehouseid=:warehouseid 
                        and a.deviceid in ('T34A-01','T34A-02','T34B-01','T34B-02','T34C-01','T34C-02','T34C-03') and to_date(a.collectiontime,'yyyy-mm-dd hh24:mi:ss')>=trunc(sysdate-6)
                        ) m
                        where m.seqno=1";
            var data = _sqlRepository.Query<Humiture>(sql, new { warehouseid = warehouseId });
            return data.ToList();
        }
    }
}
