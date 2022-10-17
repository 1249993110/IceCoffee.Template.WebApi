::以管理员身份运行
@echo off
::设置UTF-8编码
chcp 65001
set serviceName= "IceCoffee.Template.WebApi"

@title 安装windows服务
echo 正在安装服务...
@sc create %serviceName% binPath= "%~dp0%serviceName%.exe"

echo 正在启动服务...
@sc start %serviceName%

echo 正在配置服务...
::延迟启动
@sc config %serviceName% start= delayed-auto
::说明
@sc description %serviceName% "IceCoffee.Template.WebApi"

echo 已成功安装服务
pause
