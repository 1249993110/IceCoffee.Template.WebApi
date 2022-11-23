::以管理员身份运行
@echo off
::设置UTF-8编码
chcp 65001
set serviceName=IceCoffee.Template.WebApi

@title 卸载windows服务
echo 正在停止服务...
@sc stop %serviceName%

echo 正在卸载服务...
@sc delete %serviceName%

echo 已成功卸载服务
pause
