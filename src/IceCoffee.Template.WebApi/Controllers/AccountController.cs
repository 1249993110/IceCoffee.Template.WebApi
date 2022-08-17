using IceCoffee.Common.Security.Cryptography;
using IceCoffee.Template.WebApi.Extensions;
using IceCoffee.Template.WebApi.Utils;

namespace IceCoffee.Template.WebApi.Controllers
{
    /// <summary>
    /// 账户
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class AccountController : AccountControllerBase
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public new Response<UserInfo> UserInfo()
        {
            return SucceededResult(base.UserInfo);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Response> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var userId = base.UserInfo.UserId;
            var userRepository = HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var refreshTokenRepository = HttpContext.RequestServices.GetRequiredService<IRefreshTokenRepository>();

            var user = (await userRepository.QueryByIdAsync("Id", userId)).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("修改密码异常, 用户不存在");
            }

            string oldPassword = StringExtension.FormBase64(model.OldPasswordHash);
            string newPassword = StringExtension.FormBase64(model.NewPasswordHash);

            // 检查密码
            if (PBKDF2.VerifyPassword(oldPassword, user.PasswordHash, user.PasswordSalt))
            {
                return FailedResult("修改密码失败: 原密码错误");
            }

            PBKDF2.HashPassword(newPassword, out string newPasswordHash, out string newPasswordSalt);

            user.PasswordHash = newPasswordHash;
            user.PasswordSalt = newPasswordSalt;
            await userRepository.UpdateAsync(user);

            await refreshTokenRepository.DeleteByIdAsync("Fk_UserId", userId);
            return SucceededResult();
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Response<JwtToken>> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var refreshTokenRepository = HttpContext.RequestServices.GetRequiredService<IRefreshTokenRepository>();
            var refreshToken = (await refreshTokenRepository.QueryByIdAsync("Id", model.RefreshToken)).First();
            var storedRefreshToken = new StoredRefreshToken()
            {
                Id = refreshToken.Id,
                CreatedDate = refreshToken.CreatedDate,
                ExpiryDate = refreshToken.ExpiryDate,
                IsRevorked = refreshToken.IsRevorked,
                JwtId = refreshToken.JwtId,
                UserId = refreshToken.UserId
            };

            try
            {
                var jwtToken = await base.RefreshToken(model.AccessToken, storedRefreshToken);
                return SucceededResult(jwtToken);
            }
            catch (Exception ex)
            {
                return FailedResult(ex.Message);
            }
        }

        /// <summary>
        /// 通过 Cookie 登录
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Response<UserInfo>> SignInWithCookie([FromBody] SignInModel model)
        {
            try
            {
                var userInfo = await SignInHelper.GetUserInfo(HttpContext, model.LoginName, model.PasswordHash);

                await base.SignInWithCookie(userInfo);
                return SucceededResult(userInfo);
            }
            catch (Exception ex)
            {
                return FailedResult("登录失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 通过 JWT 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Response<JwtToken>> SignInWithJwt([FromBody] SignInModel model)
        {
            try
            {
                var userInfo = await SignInHelper.GetUserInfo(HttpContext, model.LoginName, model.PasswordHash);
                var jwtToken = await GenerateJwtToken(userInfo);

                return SucceededResult(jwtToken);
            }
            catch (Exception ex)
            {
                return FailedResult("登录失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 通过 Cookie 注销
        /// </summary>
        [HttpDelete]
        public new async Task<Response> SignOutWithCookie()
        {
            await base.SignOutWithCookie();
            return SucceededResult();
        }
        /// <summary>
        /// 通过 JWT 注销
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public new async Task<Response> SignOutWithJwt()
        {
            await base.SignOutWithJwt();
            return SucceededResult();
        }
        /// <summary>
        /// 获取用户菜单
        /// </summary>
        [HttpGet]
        public async Task<Response<IEnumerable<MenuTreeModel>>> UserMenus()
        {
            var userId = HttpContext.RequestServices.GetRequiredService<UserInfo>().UserId;
            var menuRepository = HttpContext.RequestServices.GetRequiredService<IMenuRepository>();
            var vUserRoleRepository = HttpContext.RequestServices.GetRequiredService<IVUserRoleRepository>();
            var roleMenuRepository = HttpContext.RequestServices.GetRequiredService<IRoleMenuRepository>();

            // 第1步：根据用户找到所属角色
            var userRoles = await vUserRoleRepository.QueryByIdAsync("UserId", userId);

            if (userRoles.Any() == false)
            {
                return FailedResult("获取失败");
            }

            var hashSet = new HashSet<string>();

            foreach (var role in userRoles)
            {
                hashSet.Add(role.RoleId);
            }

            // 第2步：根据角色Id找到菜单Id
            var roleMenus = await roleMenuRepository.QueryByIdsAsync("Fk_RoleId", hashSet);
            if (roleMenus.Any() == false)
            {
                return FailedResult("获取失败");
            }

            hashSet.Clear();

            foreach (var item in roleMenus)
            {
                hashSet.Add(item.MenuId);
            }

            // 第3步：获取菜单
            var menus = await menuRepository.QueryByIdsAsync("Id", hashSet);
            // 过滤禁用的菜单
            menus = menus.Where(p => p.IsEnabled);

            if (menus.Any() == false)
            {
                return FailedResult("获取失败");
            }

            return SucceededResult(menus.ToTreeModel());
        }
    }
}