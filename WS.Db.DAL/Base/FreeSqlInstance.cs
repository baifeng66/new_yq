using FreeSql;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Db.DAL.Base
{
    public class FreeSqlInstance
    {

        static IFreeSql? _freesql;
        //请务必定义成 Singleton 单例模式
        public static IFreeSql fresql()
        {
            if (_freesql == null)
            {
                IConfiguration _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
                string _connectstring = _config["ConnectionStrings:connStr"];
                _freesql = new FreeSqlBuilder()
                  //指定数据库类型以及数据库连接
                  .UseConnectionString(FreeSql.DataType.PostgreSQL, _connectstring)
                  //aop监听sql
                  .UseMonitorCommand(cmd =>//执行前
                  {
                      //Console.WriteLine("--------------------------------------------------执行前begin--------------------------------------------------");
                      //Console.WriteLine(cmd.CommandText);
                      //Console.WriteLine("--------------------------------------------------执行前end--------------------------------------------------");
                  }, (cmd, valueString) =>//执行后
                  {
                      //Console.WriteLine("--------------------------------------------------执行后begin--------------------------------------------------");
                      //Console.WriteLine(cmd.CommandText);
                      ////Console.WriteLine(valueString);
                      //Console.WriteLine("--------------------------------------------------执行后end--------------------------------------------------");
                  })
                  .UseAutoSyncStructure(false)//CodeFirst自动同步将实体同步到数据库结构（开发阶段必备），默认是true，正式环境请改为false
                  .Build();//创建实例（官方建议使用单例模式）

            }
            return _freesql;
        }

    }
}
