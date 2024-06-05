using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.IBLL;
using WS.Db.DAL;
using Microsoft.AspNetCore.Mvc;
using WS.Db.Model;
using System.Linq.Expressions;

namespace WS.API.Controllers
{
    /// <summary>
    /// �˵�����
    /// </summary>
	public class baseMenuController : BaseController
    {
        IHttpContextAccessor _accessor;
        IbaseMenuBLL _service;
        IuserInfoBLL _userinfoBll;
        IroleMenuBLL _roleMenuBll;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="iservice"></param>
        /// <param name="ibaseLogBLL"></param>
        /// <param name="iuserInfoBLL"></param>
        /// <param name="accessor"></param>
        public baseMenuController(IbaseMenuBLL iservice, IbaseLogBLL ibaseLogBLL, IuserInfoBLL iuserInfoBLL, IroleMenuBLL iroleMenuBLL, IHttpContextAccessor accessor) : base(ibaseLogBLL, accessor)
        {
            _service = iservice;
            _accessor = accessor;
            _userinfoBll = iuserInfoBLL;
            _roleMenuBll = iroleMenuBLL;
        }

        /// <summary>
        /// ��ȡ���в˵���Ϣ
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "�����б�", Type = typeof(baseMenu))]
        public IActionResult listall()
        {
            var list = _service.All().OrderByDescending(c => c.menuId).ToList();
            return success(list.Count, list);
        }

        /// <summary>
        /// ɾ���˵�
        /// </summary>
        /// <param name="id">�˵���id</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpDelete]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "�������", Type = typeof(Core.ApiResponse))]
        public IActionResult delete([FromQuery] int id)
        {
            var b = _service.Delete(id);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Delete, $"�˵�ɾ���ɹ���ID��{id}");
                return success("ɾ���ɹ�");
            }
            else
            {
                return fail("ɾ��ʧ��");
            }
        }

        /// <summary>
        /// ��Ӳ˵�
        /// </summary>
        /// <param name="mode">�˵���ʵ�弯��</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "�������", Type = typeof(Core.ApiResponse))]
        public IActionResult add([FromBody] baseMenu mode)
        {
            if (string.IsNullOrEmpty(mode.menuName))
            {
                return fail("�˵����Ʋ���Ϊ��");
            }
            var b = _service.Insert(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Add, $"�˵���ӳɹ����˵�ID��{mode.menuPid}");
                return success("��ӳɹ�");
            }
            else
            {
                return fail("���ʧ��");
            }
        }

        /// <summary>
        /// �޸Ĳ˵�
        /// </summary>
        /// <param name="mode">�˵���ʵ�弯��</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "�������", Type = typeof(Core.ApiResponse))]
        public IActionResult edit([FromBody] baseMenu mode)
        {
            var b = _service.Update(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Update, $"�˵��޸ĳɹ����˵�ID��{mode.menuPid}");
                return success("�޸ĳɹ�");
            }
            else
            {
                return fail("�޸�ʧ��");
            }
        }
        /// <summary>
        /// ��ҳ��ѯ����
        /// </summary>
        /// <param name="menuName">�˵�����</param>
        /// <param name="state">״̬��0-���ã�1-����</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "�����б�", Type = typeof(baseMenu))]
        public IActionResult page(string? menuName = "", string? state = "", int page = 1, int size = 20)
        {
            Expression<Func<baseMenu, bool>> exp = c => c.menuId > 0;
            if (!string.IsNullOrEmpty(menuName))
            {
                exp = exp.And(c => c.menuName.Contains(menuName));
            }
            if (!string.IsNullOrEmpty(state))
            {
                exp = exp.And(c => c.state.Equals(state));
            }
            var list = _service.PageOrderByDesc(exp, c => c.menuId, (page - 1) * size, size);
            return success(list.Count, list);
        }

        /// <summary>
        ///��ȡ���в˵����������Щ���Ѿ�����ɫ��Ȩ�Ĳ˵�
        /// </summary>
        /// <param name="rid">ϵͳ��ɫid</param>
        /// <returns></returns>
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "�����б�", Type = typeof(Db.Model.ext.layui_tree))]
        [HttpGet]
        public JsonResult treeall(int rid = 0)
        {
            if (rid == 0)
            {
                return fail("��������");
            }
            IList<baseMenu> listmenu = _service.All().OrderBy(c => c.menuPid).ThenBy(c => c.orderBy).ToList();
            IList<roleMenu> listusermenu = _roleMenuBll.expAll(c => c.roleId == rid);
            List<Db.Model.ext.layui_tree> roots = new List<Db.Model.ext.layui_tree>();
            foreach (var menu in listmenu.Where(c => c.menuPid == 0))
            {
                Db.Model.ext.layui_tree rootnode = new Db.Model.ext.layui_tree();
                rootnode.field = "name";
                rootnode.id = "menu_" + menu.menuId;
                rootnode.spread = true;
                rootnode.title = menu.menuName;
                rootnode.children = findNode(menu.menuId, listmenu, listusermenu);
                if (rootnode.children.Count == 0)
                    rootnode.@checked = listusermenu.Count(c => c.menuId == menu.menuId) > 0;
                roots.Add(rootnode);
            }
            return success(roots.Count, roots);
        }

        private List<Db.Model.ext.layui_tree> findNode(int pid, IList<baseMenu> listmenu, IList<roleMenu> listusermenu)
        {
            List<Db.Model.ext.layui_tree> list = new List<Db.Model.ext.layui_tree>();
            foreach (var menu in listmenu.Where(c => c.menuPid == pid))
            {
                Db.Model.ext.layui_tree node = new Db.Model.ext.layui_tree();
                node.field = "name";
                node.title = menu.menuName;
                node.spread = false;
                node.id = "menu_" + menu.menuId;
                node.children = findNode(menu.menuId, listmenu, listusermenu);
                if (node.children.Count == 0)
                {
                    node.@checked = listusermenu.Count(c => c.menuId == menu.menuId) > 0;
                }
                list.Add(node);
            }
            return list;
        }

        /// <summary>
        /// ��ȡ�û�������Ȩ�Ĳ˵�Ȩ��
        /// </summary>
        /// <returns></returns>
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "�����б�", Type = typeof(Db.Model.ext.baseMenuExt))]
        [HttpGet]
        public JsonResult usermenu()
        {
            Db.Model.userInfo uinfo = _userinfoBll.GetModel(base.uid);
            if (base.uid==0)//��������Ա�ĳ���Ȩ��
            {
                List<baseMenu> listmunu = _service.All();
                if (listmunu != null)
                {
                    List<baseMenu> rootmunu = listmunu.Where(c => c.menuPid == 0).OrderBy(c => c.orderBy).ToList();
                    List<Db.Model.ext.baseMenuExt> roots = new List<Db.Model.ext.baseMenuExt>();
                    for (int i = 0; i < rootmunu.Count; i++)
                    {
                        Db.Model.ext.baseMenuExt rootnode = Core.MyConvert.Mapper<Db.Model.ext.baseMenuExt, baseMenu>(rootmunu[i]);
                        List<baseMenu> childmenu = listmunu.Where(c => c.menuPid == rootnode.menuId).OrderBy(c => c.orderBy).ToList();
                        rootnode.children.AddRange(childmenu);
                        roots.Add(rootnode);
                    }
                    return success(roots.Count, roots);
                }
            }
            List<Db.Model.roleMenu> rolemenus = _roleMenuBll.expAll(c => c.roleId == uinfo.rid);
            if (rolemenus != null)
            {
                int[] mids = rolemenus.Select(c => c.menuId.GetValueOrDefault()).ToArray();
                List<baseMenu> listmunu = _service.expAll(c => mids.Contains(c.menuId) && c.state == "1");//($"id in({string.Join(',', mids)}) and state=1 and type{(base.LoginType > 0 ? "=1" : "=0")}");

                if (listmunu != null)
                {
                    List<baseMenu> rootmunu = listmunu.Where(c => c.menuPid == 0).OrderBy(c => c.orderBy).ToList();
                    List<Db.Model.ext.baseMenuExt> roots = new List<Db.Model.ext.baseMenuExt>();
                    for (int i = 0; i < rootmunu.Count; i++)
                    {
                        Db.Model.ext.baseMenuExt rootnode = Core.MyConvert.Mapper<Db.Model.ext.baseMenuExt, baseMenu>(rootmunu[i]);
                        List<baseMenu> childmenu = listmunu.Where(c => c.menuPid == rootnode.menuId).OrderBy(c => c.orderBy).ToList();
                        //List<Db.Model.ext.baseMenuExt> chids = new List<Db.Model.ext.baseMenuExt>();
                        //for (int j = 0; j < childmenu.Count; j++)
                        //{
                        //    Db.Model.ext.baseMenuExt chidnode = Core.MyConvert.Mapper<Db.Model.ext.baseMenuExt, baseMenu>(childmenu[j]);
                        //    //chidnode.role = rolemenus.SingleOrDefault(c => c.menuId == childmenu[j].menuId);
                        //    chids.Add(chidnode);
                        //}
                        rootnode.children.AddRange(childmenu);
                        roots.Add(rootnode);
                    }
                    return success(roots.Count, roots);
                }
                else
                {
                    return fail("��Ȩ��");
                }
            }
            else
            {
                return fail("��Ȩ��");
            }
        }


        /// <summary>
        /// ���ý�ɫ�Ĳ˵�Ȩ��
        /// </summary>
        /// <param name="rid">��ɫid</param>
        /// <param name="ids">�˵�Ȩ���ַ���</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult set([FromForm] int rid, [FromForm] string ids)
        {
            int b = _roleMenuBll.Delete(new { roleId = rid });
            int addcount = 0;
            if (!string.IsNullOrEmpty(ids))
            {
                string[] id = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < id.Length; i++)
                {
                    var item = id[i];
                    roleMenu rm = new roleMenu();
                    rm.menuId = int.Parse(item);
                    rm.roleId = rid;
                    if (_roleMenuBll.Insert(rm) > 0)
                    {
                        addcount += 1;
                    }
                }
            }
            else
            {
                return fail("��ѡ��Ȩ��");
            }
            base.addlog(baseLog.ApiLogType.Update, $"��ɫȨ�����óɹ�����ɫID��{rid}");
            return success("Ȩ�����óɹ�");
        }


    }
}