using IceCoffee.Common.Security.Cryptography;

namespace IceCoffee.Template.WebApi.Utils
{
    public static class SignInHelper
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="loginName"></param>
        /// <param name="passwordHash">base64编码的密码</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<UserInfo> GetUserInfo(HttpContext httpContext, string loginName, string passwordHash)
        {
            string password = StringExtension.FormBase64(passwordHash);
            var userRepository = httpContext.RequestServices.GetRequiredService<IUserRepository>();
            var vUserRoleRepository = httpContext.RequestServices.GetRequiredService<IVUserRoleRepository>();
            var vRolePermissionRepository = httpContext.RequestServices.GetRequiredService<IVRolePermissionRepository>();

            // 判断用户是否存在
            var user = await userRepository.QueryByLoginNameAsync(loginName);
            if (user == null)
            {
                throw new Exception("用户不存在");
            }

            // 检查登录允许
            if (user.LoginEnabled == false)
            {
                throw new Exception("不允许登录, 请联系系统管理员");
            }

            DateTime now = DateTime.Now;
            // 检查登录锁定
            if (user.LockoutEndDate.HasValue && user.LockoutEndDate.Value > now)
            {
                throw new Exception("登录锁定, 请稍后重试");
            }

            // 检查密码
            if (PBKDF2.VerifyPassword(password, user.PasswordHash, user.PasswordSalt) == false)
            {
                ++user.AccessFailedCount;
                if (user.AccessFailedCount > 3)
                {
                    await userRepository.UpdateColumnByIdAsync("id", user.Id, "lockout_end_date", now.AddMinutes(10));
                    throw new Exception("您的账户已被锁定, 请稍后重试");
                }

                await userRepository.UpdateColumnByIdAsync("id", user.Id, "access_failed_count", user.AccessFailedCount);
                throw new Exception("密码错误");
            }

            user.AccessFailedCount = 0;
            user.LastLoginTime = now;
            user.LastLoginIp = httpContext.GetRemoteIpAddress();
            await userRepository.UpdateAsync(user);

            var vUserRoles = await vUserRoleRepository.QueryByIdAsync("user_id", user.Id);
            var roleIds = vUserRoles.Select(s => s.RoleId);
            var roleNames = vUserRoles.Select(s => s.RoleName);

            var vRolePermission = (await vRolePermissionRepository.QueryByIdsAsync("role_id", roleIds));
            var areas = vRolePermission.Select(s => s.Area).Distinct();

            return new UserInfo()
            {
                UserId = user.Id,
                RoleNames = roleNames,
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserName = user.Name,
                PhoneNumber = user.PhoneNumber,
                Areas = areas
            };
        }
    }
}
