using System;
using hzwu.DAL;
using hzwu.Model;

namespace hzwu.ORMExplore
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                SqlHelper helper = new SqlHelper();

                int id = 10;

                //按条件查询
                var companys = helper.FindCondition<CompanyModel>(c => c.Id > 10
                && c.CompanyName.StartsWith("cscc")
                && c.CompanyName.EndsWith("123")
                && c.CompanyName.Contains("hzwu"));

                //按条件更新
                //var companys = helper.UpdateCondition<CompanyModel>(c => c.Id > id && c.Id > 20);

                //按条件删除
                //helper.DeleteCondition<User>(c => c.Id > id && c.Name == "admin");

            }
            Console.ReadLine();
        }
    }
}
