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
    [Migration(111)]
    public class DBMIgrate_111 : DBMIgrateBase
    {

        public override void Up()
        {

            IfDatabase("SqlServer", "Postgres", "sqlite").Create.Table(WholeTableName("SystemHtml"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity() 
                .WithColumn("Name").AsString(300)
                .WithColumn("IsSytem").AsBoolean()
                .WithColumn("Value").AsCustom ("ntext").Nullable() 
                .WithColumn("AddTime").AsDateTime().Nullable()
            ;
            IfDatabase("mysql").Create.Table(WholeTableName("SystemHtml"))
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString(300)
             .WithColumn("IsSytem").AsBoolean() 
            .WithColumn("Value").AsCustom("text").Nullable() 
             .WithColumn("AddTime").AsDateTime().Nullable()
            ;
            var SystemDictionary1 = new
            {
                Name = "admin_Index",
                IsSytem = true,
                Value = @"
<!DOCTYPE html>
<html class=""x-admin-sm"">
<head>
    <meta charset=""UTF-8"">
    <title>欢迎页面-X-admin2.2</title>
    <meta name=""renderer"" content=""webkit"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge,chrome=1"">
    <meta name=""viewport"" content=""width=device-width,user-scalable=yes, minimum-scale=0.4, initial-scale=0.8,target-densitydpi=low-dpi"" />
    <link rel=""stylesheet"" href=""/xadmin/css/font.css"">
    <link rel=""stylesheet"" href=""/xadmin/css/xadmin.css"">
    

</head>
<body>
      
    <!-- 顶部开始 -->
    <div class=""container"">
        <div class=""logo"">
            <a href=""./index"">后台管理系统</a>
        </div>
        <div class=""left_open"">
            <a><i title=""展开左侧栏"" class=""iconfont"">&#xe699;</i></a>
        </div>
        <ul class=""layui-nav left fast-add"" lay-filter="""">
            <li class=""layui-nav-item"">
                <a href=""javascript:;"">+新增</a>
                <dl class=""layui-nav-child"">
                    <!-- 二级菜单 -->
                    <dd>
                        <a onclick=""xadmin.open('最大化','http://www.baidu.com','','',true)"">
                            <i class=""iconfont"">&#xe6a2;</i>弹出最大化
                        </a>
                    </dd>
                    <dd>
                        <a onclick=""xadmin.open('弹出自动宽高','http://www.baidu.com')"">
                            <i class=""iconfont"">&#xe6a8;</i>弹出自动宽高
                        </a>
                    </dd>
                    <dd>
                        <a onclick=""xadmin.open('弹出指定宽高','http://www.baidu.com',500,300)"">
                            <i class=""iconfont"">&#xe6a8;</i>弹出指定宽高
                        </a>
                    </dd>
                    <dd>
                        <a onclick=""xadmin.add_tab('在tab打开','member-list.html')"">
                            <i class=""iconfont"">&#xe6b8;</i>在tab打开
                        </a>
                    </dd>
                    <dd>
                        <a onclick=""xadmin.add_tab('在tab打开刷新','member-del.html',true)"">
                            <i class=""iconfont"">&#xe6b8;</i>在tab打开刷新
                        </a>
                    </dd>
                </dl>
            </li>
        </ul>
        <ul class=""layui-nav right"" lay-filter="""">
            <li class=""layui-nav-item"">
                <a href=""javascript:;"">admin</a>
                <dl class=""layui-nav-child"">
                    <!-- 二级菜单 -->
                    <dd>
                        <a onclick=""xadmin.open('个人信息','http://www.baidu.com')"">个人信息</a>
                    </dd>
                    <dd>
                        <a onclick=""xadmin.open('切换帐号','http://www.baidu.com')"">切换帐号</a>
                    </dd>
                    <dd>
                        <a href=""./SignOut"">退出</a>
                    </dd>
                </dl>
            </li>
            <li class=""layui-nav-item to-index"">
                <a href=""/"">前台首页</a>
            </li>
        </ul>
    </div>
    <!-- 顶部结束 -->
    <!-- 中部开始 -->
    <!-- 左侧菜单开始 -->
    <div class=""left-nav"">
        <div id=""side-nav"">
            <ul id=""nav"">
                <li>
                    <a href=""javascript:;"">
                        <i class=""iconfont left-nav-li"" lay-tips=""会员管理"">&#xe6b8;</i>
                        <cite>会员管理</cite>
                        <i class=""iconfont nav_right"">&#xe697;</i>
                    </a>
                    <ul class=""sub-menu"">
                        <li>
                            <a onclick=""xadmin.add_tab('会员列表(静态表格)','UserMng')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>会员列表</cite>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href=""javascript:;"">
                        <i class=""iconfont left-nav-li"" lay-tips=""订单管理"">&#xe723;</i>
                        <cite>订单管理</cite>
                        <i class=""iconfont nav_right"">&#xe697;</i>
                    </a>
                    <ul class=""sub-menu"">
                        <li>
                            <a onclick=""xadmin.add_tab('订单列表','orderlist')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>订单列表</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('订单列表1','order-list1.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>订单列表1</cite>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href=""javascript:;"">
                        <i class=""iconfont left-nav-li"" lay-tips=""分类管理"">&#xe723;</i>
                        <cite>分类管理</cite>
                        <i class=""iconfont nav_right"">&#xe697;</i>
                    </a>
                    <ul class=""sub-menu"">
                        <li>
                            <a onclick=""xadmin.add_tab('多级分类','cate.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>多级分类</cite>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href=""javascript:;"">
                        <i class=""iconfont left-nav-li"" lay-tips=""城市联动"">&#xe723;</i>
                        <cite>城市联动</cite>
                        <i class=""iconfont nav_right"">&#xe697;</i>
                    </a>
                    <ul class=""sub-menu"">
                        <li>
                            <a onclick=""xadmin.add_tab('三级地区联动','city.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>三级地区联动</cite>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href=""javascript:;"">
                        <i class=""iconfont left-nav-li"" lay-tips=""管理员管理"">&#xe726;</i>
                        <cite>管理员管理</cite>
                        <i class=""iconfont nav_right"">&#xe697;</i>
                    </a>
                    <ul class=""sub-menu"">
                        <li>
                            <a onclick=""xadmin.add_tab('管理员列表','admin-list.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>管理员列表</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('角色管理','admin-role.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>角色管理</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('权限分类','admin-cate.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>权限分类</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('权限管理','admin-rule.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>权限管理</cite>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href=""javascript:;"">
                        <i class=""iconfont left-nav-li"" lay-tips=""系统统计"">&#xe6ce;</i>
                        <cite>系统统计</cite>
                        <i class=""iconfont nav_right"">&#xe697;</i>
                    </a>
                    <ul class=""sub-menu"">
                        <li>
                            <a onclick=""xadmin.add_tab('拆线图','echarts1.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>拆线图</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('拆线图','echarts2.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>拆线图</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('地图','echarts3.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>地图</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('饼图','echarts4.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>饼图</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('雷达图','echarts5.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>雷达图</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('k线图','echarts6.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>k线图</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('热力图','echarts7.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>热力图</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('仪表图','echarts8.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>仪表图</cite>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href=""javascript:;"">
                        <i class=""iconfont left-nav-li"" lay-tips=""图标字体"">&#xe6b4;</i>
                        <cite>图标字体</cite>
                        <i class=""iconfont nav_right"">&#xe697;</i>
                    </a>
                    <ul class=""sub-menu"">
                        <li>
                            <a onclick=""xadmin.add_tab('图标对应字体','unicode.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>图标对应字体</cite>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href=""javascript:;"">
                        <i class=""iconfont left-nav-li"" lay-tips=""其它页面"">&#xe6b4;</i>
                        <cite>其它页面</cite>
                        <i class=""iconfont nav_right"">&#xe697;</i>
                    </a>
                    <ul class=""sub-menu"">
                        <li>
                            <a href=""login.html"" target=""_blank"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>登录页面</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('错误页面','error.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>错误页面</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('示例页面','demo.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>示例页面</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('更新日志','log.html')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>更新日志</cite>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href=""javascript:;"">
                        <i class=""iconfont left-nav-li"" lay-tips=""高级功能"">&#xe6b4;</i>
                        <cite>高级功能</cite>
                        <i class=""iconfont nav_right"">&#xe697;</i>
                    </a>
                    <ul class=""sub-menu"">
                        <li>
                            <a onclick=""xadmin.add_tab('底层配置','/sd23nj/admin/allcon')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>底层配置</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('多语言','/sd23nj/langs/index')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>多语言</cite>
                            </a>
                       </li>
                        <li>
                            <a onclick=""xadmin.add_tab('表单生成','/sd23nj/DBform/index')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>表单生成</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('字典管理','/sd23nj/Dictionary/index')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>字典管理</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('文件管理','/sd23nj/afile/FileMng')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>文件管理</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('抓取网页','/sd23nj/afile/GetUrl')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>抓取网页</cite>
                            </a>
                        </li>
                        <li>
                            <a onclick=""xadmin.add_tab('数据库管理','/sd23nj/DBform/datamng')"">
                                <i class=""iconfont"">&#xe6a7;</i>
                                <cite>数据库管理</cite>
                            </a>
                        </li>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
    <!-- <div class=""x-slide_left""></div> -->
    <!-- 左侧菜单结束 -->
    <!-- 右侧主体开始 -->
    <div class=""page-content"">
        <div class=""layui-tab tab"" lay-filter=""xbs_tab"" lay-allowclose=""false"">
            <ul class=""layui-tab-title"">
                <li class=""home"">
                    <i class=""layui-icon"">&#xe68e;</i>我的桌面
                </li>
            </ul>
            <div class=""layui-unselect layui-form-select layui-form-selected"" id=""tab_right"">
                <dl>
                    <dd data-type=""this"">关闭当前</dd>
                    <dd data-type=""other"">关闭其它</dd>
                    <dd data-type=""all"">关闭全部</dd>
                </dl>
            </div>
            <div class=""layui-tab-content"">
                <div class=""layui-tab-item layui-show"">
                    <iframe src='./welcome' frameborder=""0"" scrolling=""yes"" class=""x-iframe""></iframe>
                </div>
            </div>
            <div id=""tab_show""></div>
        </div>
    </div>
    <div class=""page-content-bg""></div>
    <style id=""theme_style""></style>
    <!-- 右侧主体结束 -->
    <!-- 中部结束 -->
 
    <script src=""/xadmin/lib/layui/layui.js"" charset=""utf-8""></script>
    <script type=""text/javascript"" src=""/xadmin/js/xadmin.js""></script>
    <!-- 让IE8/9支持媒体查询，从而兼容栅格 -->
    <!--[if lt IE 9]>
      <script src=""https://cdn.staticfile.org/html5shiv/r29/html5.min.js""></script>
      <script src=""https://cdn.staticfile.org/respond.js/1.4.2/respond.min.js""></script>
    <![endif]-->
    
</body>
</html>",
              
                AddTime = DateTime.Now,
            };
            Insert.IntoTable(WholeTableName("SystemHtml")).Row(SystemDictionary1);
        }

        public override void Down()
        {
           Delete.Table(WholeTableName("AconFunction"));
            
        }
    }
}
