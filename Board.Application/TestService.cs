using Board.Infrastructure;
using Board.Infrastructure.DBHelpers;
using Board.Infrastructure.Models;
using Dapper;

namespace Board.Application
{
    public class TestService : ITestService
    {
        private readonly IOracleRepository _sqlRepository;
        public TestService(IOracleRepository sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }
        [Transactional]
        public string Hello()
        {
            return "Hello-Test";
        }

        public List<dynamic> GetLocaltionList()
        {
            var list_res = new List<string>();
            using (var conn = DBHelper.Connection)
            {

                var list = conn.Query<dynamic>("select LOCATIONID,ZONEID,ZONEGROUP,LOCATIONUSAGE from BAS_LOCATION where ORGANIZATIONID='WMSJM' and WAREHOUSEID='JM01'");

                return list.ToList();
            }
        }

        public async Task<List<string>> TestDB()
        {
            var list_res = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                var list = await _sqlRepository.QueryAsync<string>("select asnno from doc_asn_header where warehouseid=:warehouseid and asnno=:asnno", new
                {
                    warehouseid = "JM01",
                    asnno = "JM01A2303060002"
                });
                //var res = await _sqlRepository.ExecuteAsync("insert into TEST_TB_USER(name,age,sex,addwho) values(:name,:age,:sex,:addwho)", new
                //{
                //    name = "小明",
                //    age = new Random().Next(18, 100),
                //    sex = new Random().Next(0, 2),
                //    addwho = "lzy"
                //});
                //var list = await _sqlRepository.QueryAsync<string>("select bill_no from mes_inv_item_moves_a@jq where bill_no='YK4010202203290179'");
                //list_res.Add(list.First());
                list_res.Add(i.ToString());
            }
            return list_res;
            //return Task.CompletedTask;
        }

        public async Task<List<string>> TestDB2()
        {
            var list_res = new List<string>();
            using (var conn = DBHelper.Connection)
            {
                try
                {
                    conn.Open();
                    for (int i = 0; i < 10; i++)
                    {

                        var a = conn.State;
                        var list = await conn.QueryAsync<string>("select asnno from doc_asn_header where warehouseid=:warehouseid and asnno=:asnno", new
                        {
                            warehouseid = "JM01",
                            asnno = "JM01A2303060002"
                        });
                        //var res = await conn.ExecuteAsync("insert into TEST_TB_USER(name,age,sex,addwho) values(:name,:age,:sex,:addwho)", new
                        //{
                        //    name = "小明",
                        //    age = new Random().Next(18, 100),
                        //    sex = new Random().Next(0, 2),
                        //    addwho = "lzy"
                        //});
                        //var list = await conn.QueryAsync<string>("select bill_no from mes_inv_item_moves_a@jq where bill_no='YK4010202203290179'");
                        var b = conn.State;
                        //list_res.Add(list.First());
                        list_res.Add(i.ToString());
                    }
                }
                finally
                {
                    conn.Close();
                }
            }

            return list_res;
            //return Task.CompletedTask;
        }
    }
}
