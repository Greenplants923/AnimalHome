# AnimalHome
宠屋——宠物社交网络系统（ASP.NET MVC）

本软件由中国人民大学信息学院软件工程专业徐欣祺开发。 

本系统以宠物为主体，构建宠物动态分享平台，为宠物创建社交网络账号，实现线上分享、交流、交友等功能。 

系统运行环境 

1、ASP.NET Framework 4.8 
  本系统是基于 ASP.NET Framework 4.8 开发的。 

2、IIS 
  在“控制面板>程序>启动或关闭 Windows 功能”中，开启 IIS 功能，注意要勾选图 1 中标明的几个内容。 

  在 IIS 中添加网站。网站名称任意；IP 地址不设置即为 localhost，即 127.0.0.1；选择物理路径时，应当选择项目文件夹下的，与项目文件夹同名的二级文件夹，如图 2 所示。启动该网站。注意要将 IP 地
址相同的其他网站停止。

3、SQL SERVER 2014 数据库 
  需要在 系统文件夹 中 的 “ Web.config ” 文 件 中 将<connectionStrings></connectionStrings>内 user id 和 password 修
改成 SQL SERVER 中连接服务器的登录名和密码。默认数据库连接信息：主机名："localhost"；数据库名：" myWeiboDB202005061120"；端口：1433。 

4、任意浏览器 
  配置好前面提到的环境后，在浏览器地址栏中输入“127.0.0.1”，即可运行本系统。 
