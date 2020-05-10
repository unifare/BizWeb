using FluentMigrator;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniOrm.Common;

namespace UniOrm.DataMigrationiHistrory
{
    [Migration(116)]
    public class DBMIgrate_116 : DBMIgrateBase
    {

        public override void Up()
        {
          

            var SystemDictionary2 = new
            {
                Name = "网站管理目录",
                IsSytem = true,
                Value = @"<div id=""side-nav"" style=""user-select: auto;"">
        <ul id=""nav"" style=""user-select: auto;"">
            <li style=""user-select: auto;"">
                <a href=""javascript:;"" style=""user-select: auto;"">
                    <i class=""iconfont left-nav-li"" lay-tips=""会员管理"" style=""user-select: auto;""></i>
                    <cite style=""user-select: auto;"">会员管理</cite>
                    <i class=""iconfont nav_right"" style=""user-select: auto;""></i>
                </a>
                <ul class=""sub-menu"" style=""user-select: auto;"">
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('会员列表(静态表格)','UserMng')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">会员列表</cite>
                        </a>
                    </li>
                </ul>
            </li>
            <li style=""user-select: auto;"">
                <a href=""javascript:;"" style=""user-select: auto;"">
                    <i class=""iconfont left-nav-li"" lay-tips=""订单管理"" style=""user-select: auto;""></i>
                    <cite style=""user-select: auto;"">订单管理</cite>
                    <i class=""iconfont nav_right"" style=""user-select: auto;""></i>
                </a>
                <ul class=""sub-menu"" style=""user-select: auto;"">
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('订单列表','orderlist')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">订单列表</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('订单列表1','order-list1.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">订单列表1</cite>
                        </a>
                    </li>
                </ul>
            </li>
            <li style=""user-select: auto;"">
                <a href=""javascript:;"" style=""user-select: auto;"">
                    <i class=""iconfont left-nav-li"" lay-tips=""分类管理"" style=""user-select: auto;""></i>
                    <cite style=""user-select: auto;"">分类管理</cite>
                    <i class=""iconfont nav_right"" style=""user-select: auto;""></i>
                </a>
                <ul class=""sub-menu"" style=""user-select: auto;"">
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('多级分类','cate.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">多级分类</cite>
                        </a>
                    </li>
                </ul>
            </li>
            <li style=""user-select: auto;"">
                <a href=""javascript:;"" style=""user-select: auto;"">
                    <i class=""iconfont left-nav-li"" lay-tips=""城市联动"" style=""user-select: auto;""></i>
                    <cite style=""user-select: auto;"">城市联动</cite>
                    <i class=""iconfont nav_right"" style=""user-select: auto;""></i>
                </a>
                <ul class=""sub-menu"" style=""user-select: auto;"">
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('三级地区联动','city.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">三级地区联动</cite>
                        </a>
                    </li>
                </ul>
            </li>
            <li style=""user-select: auto;"">
                <a href=""javascript:;"" style=""user-select: auto;"">
                    <i class=""iconfont left-nav-li"" lay-tips=""管理员管理"" style=""user-select: auto;""></i>
                    <cite style=""user-select: auto;"">管理员管理</cite>
                    <i class=""iconfont nav_right"" style=""user-select: auto;""></i>
                </a>
                <ul class=""sub-menu"" style=""user-select: auto;"">
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('管理员列表','admin-list.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">管理员列表</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('角色管理','admin-role.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">角色管理</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('权限分类','admin-cate.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">权限分类</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('权限管理','admin-rule.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">权限管理</cite>
                        </a>
                    </li>
                </ul>
            </li>
            <li style=""user-select: auto;"">
                <a href=""javascript:;"" style=""user-select: auto;"">
                    <i class=""iconfont left-nav-li"" lay-tips=""系统统计"" style=""user-select: auto;""></i>
                    <cite style=""user-select: auto;"">系统统计</cite>
                    <i class=""iconfont nav_right"" style=""user-select: auto;""></i>
                </a>
                <ul class=""sub-menu"" style=""user-select: auto;"">
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('拆线图','echarts1.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">拆线图</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('拆线图','echarts2.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">拆线图</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('地图','echarts3.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">地图</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('饼图','echarts4.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">饼图</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('雷达图','echarts5.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">雷达图</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('k线图','echarts6.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">k线图</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('热力图','echarts7.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">热力图</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('仪表图','echarts8.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">仪表图</cite>
                        </a>
                    </li>
                </ul>
            </li>
            <li style=""user-select: auto;"">
                <a href=""javascript:;"" style=""user-select: auto;"">
                    <i class=""iconfont left-nav-li"" lay-tips=""图标字体"" style=""user-select: auto;""></i>
                    <cite style=""user-select: auto;"">图标字体</cite>
                    <i class=""iconfont nav_right"" style=""user-select: auto;""></i>
                </a>
                <ul class=""sub-menu"" style=""user-select: auto;"">
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('图标对应字体','unicode.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">图标对应字体</cite>
                        </a>
                    </li>
                </ul>
            </li>
            <li style=""user-select: auto;"">
                <a href=""javascript:;"" style=""user-select: auto;"">
                    <i class=""iconfont left-nav-li"" lay-tips=""其它页面"" style=""user-select: auto;""></i>
                    <cite style=""user-select: auto;"">其它页面</cite>
                    <i class=""iconfont nav_right"" style=""user-select: auto;""></i>
                </a>
                <ul class=""sub-menu"" style=""user-select: auto;"">
                    <li style=""user-select: auto;"">
                        <a href=""login.html"" target=""_blank"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">登录页面</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('错误页面','error.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">错误页面</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('示例页面','demo.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">示例页面</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('更新日志','log.html')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">更新日志</cite>
                        </a>
                    </li>
                </ul>
            </li>
            <li class=""open"" style=""user-select: auto;"">
                <a href=""javascript:;"" class=""active"" style=""user-select: auto;"">
                    <i class=""iconfont left-nav-li"" lay-tips=""高级功能"" style=""user-select: auto;""></i>
                    <cite style=""user-select: auto;"">高级功能</cite>
                    <i class=""iconfont nav_right"" style=""user-select: auto;""></i>
                </a>
                <ul class=""sub-menu"" style=""display: block; user-select: auto;"">
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('底层配置','/sd23nj/admin/allcon')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">底层配置</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('多语言','/sd23nj/langs/index')"" style=""user-select: auto;"">
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">多语言</cite>
                        </a>
                    </li>
                  <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('Action管理','/sd23nj/DAction/DActionList')""  >
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">Action管理</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('字典管理','/sd23nj/Dictionary/index')"" >
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">字典管理</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('文件管理','/sd23nj/afile/FileMng')"" >
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">文件管理</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('页面代码','/sd23nj/Html/Index')""  >
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">页面代码管理</cite>
                        </a>
                    </li>
  <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('功能代码','/sd23nj/Function/FunctionList')""  >
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">功能代码管理</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('抓取网页','/sd23nj/afile/GetUrl')"" >
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">抓取网页</cite>
                        </a>
                    </li>
                    <li style=""user-select: auto;"">
                        <a onclick=""xadmin.add_tab('数据库管理','/sd23nj/DBform/datamng')""  >
                            <i class=""iconfont"" style=""user-select: auto;""></i>
                            <cite style=""user-select: auto;"">数据库管理</cite>
                        </a>
                    </li>
                </ul>
            </li>
        </ul>
    </div>",
                AddTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,

            };
            Update.Table(WholeTableName("SystemHtml")).Set(SystemDictionary2).Where(new { Name = "网站管理目录", IsSytem = true });

            IfDatabase("mysql").Create.Table(WholeTableName("AConMvcClass"))
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(300).Nullable()
                 .WithColumn("VersionNum").AsString(300).Nullable()
                .WithColumn("UsingNameSpance").AsString(2000).Nullable()
                .WithColumn("ExReferenceName").AsString(2000).Nullable()
                 .WithColumn("ActionCode").AsCustom("text").Nullable()
                 .WithColumn("ClassName").AsString(300).Nullable()
                 .WithColumn("UrlRule").AsString(300).Nullable()
                 .WithColumn("ClassAttrs").AsString(300).Nullable()
                   .WithColumn("IsEanable").AsBoolean().Nullable().WithDefaultValue(false)
               .WithColumn("AddTime").AsDateTime().Nullable();
            ;

            IfDatabase("SqlServer", "Postgres", "sqlite").Create.Table(WholeTableName("AConMvcClass"))
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(300).Nullable()
                 .WithColumn("VersionNum").AsString(300).Nullable()
                .WithColumn("UsingNameSpance").AsString(2000).Nullable()
                .WithColumn("ExReferenceName").AsString(2000).Nullable()
                 .WithColumn("ActionCode").AsCustom("ntext").Nullable()
                 .WithColumn("ClassName").AsString(300).Nullable()
                 .WithColumn("UrlRule").AsString(300).Nullable()
                 .WithColumn("ClassAttrs").AsString(300).Nullable()
                  .WithColumn("IsEanable").AsBoolean().Nullable().WithDefaultValue(false)
               .WithColumn("Addtime").AsDateTime().Nullable();
            ;
        }

        public override void Down()
        {
         


        }
    }
}
