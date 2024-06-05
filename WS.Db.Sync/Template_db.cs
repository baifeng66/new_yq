using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Db.Sync
{
    /// <summary>
    /// 说明：
    /// 作者：llw
    /// 时间：2023/8/1 11:23:11
    /// </summary>
    internal class Template_db
    {
        /// <summary>
        /// 重写数据层代码，会覆盖现有的内容
        /// </summary>
        public void start()
        {
            string basepath = @"D:\ws\trunck\WS";
            string iservices = "WS.Db.IBLL";
            string repository = "WS.Db.DAL";
            string services = "WS.Db.BLL";
            string model = "WS.Db.Model";
            string controller = @"WS.API\Controllers";

            string[] files = Directory.GetFiles(Path.Combine(basepath, model));
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".cs"))
                {
                    string cname = Path.GetFileNameWithoutExtension(files[i]);//文件名不含后缀

                    string iservicefile = Path.Combine(basepath, iservices, "I" + cname + "BLL.cs");
                    if (!File.Exists(iservicefile))
                    {
                        //生成IServices
                        string str = $"using System;\r\nusing System.Collections.Generic;\r\nusing System.Text;\r\nnamespace {iservices}\r\n{{\r\n\tpublic interface I{cname}BLL : IBaseBLL<Db.Model.{cname}>\r\n\t{{\r\n\t}}\r\n}}";
                        System.IO.File.WriteAllText(iservicefile, str);
                    }

                    string repositoryfile = Path.Combine(basepath, repository, cname + "DAL.cs");
                    if (!File.Exists(repositoryfile))
                    {
                        //生成Repo
                        string str = $"using System;\r\nusing System.Collections.Generic;\r\nusing System.Text;\r\nusing WS.Db.DAL.Base;\r\nnamespace {repository}\r\n{{\r\n\tpublic class {cname}DAL : BaseDAL<Model.{cname}>\r\n\t{{\r\n\t}}\r\n}}";
                        System.IO.File.WriteAllText(repositoryfile, str);
                    }

                    string servicesfile = Path.Combine(basepath, services, cname + "BLL.cs");
                    if (!File.Exists(servicesfile))
                    {
                        //生成bll
                        string str = $"using System;\r\nusing System.Collections.Generic;\r\nusing System.Text;\r\nusing {repository};\r\nnamespace {services}\r\n{{\r\n\tpublic class {cname}BLL : BaseBLL<Model.{cname}>, IBLL.I{cname}BLL\r\n\t{{\r\n\t\t readonly DAL.{cname}DAL repo = new DAL.{cname}DAL();\r\n \t\tpublic override void setBaseRepo()\r\n\t\t{{\r\n\t\t\tbase.repo = repo;\r\n\t\t}}\r\n\t}}\r\n}}";
                        System.IO.File.WriteAllText(servicesfile, str);
                    }
                    string controllersfile = Path.Combine(basepath, controller, cname + "Controller.cs");

                    if (!File.Exists(controllersfile))
                    {
                        //生成controller
                        string str = $"using System;\r\nusing System.Collections.Generic;\r\nusing System.Text;\r\nusing {iservices};\r\nusing {repository};\r\n" +
                            $"namespace WS.API.Controllers\r\n{{\r\n\tpublic class {cname}Controller : BaseController\r\n\t{{\r\n\t\tI{cname}BLL _service;\r\n \t\t" +
                            $"/// <summary>\r\n\t\t/// 构造函数\r\n\t\t/// </summary>\r\n\t\t/// <param name=\"iservice\"></param>\r\n\t\t/// <param name=\"ibaseLogBLL\"></param>\r\n \t\t" +
                            $"public {cname}Controller(I{cname}BLL iservice,IbaseLogBLL ibaseLogBLL) : base(ibaseLogBLL)\r\n\t\t{{\r\n\t\t\t_service = iservice;\r\n\t\t}}\r\n\t}}\r\n}}";
                        System.IO.File.WriteAllText(controllersfile, str);
                    }
                    Console.WriteLine($"{cname}完成");
                }
            }

        }
    }
}
