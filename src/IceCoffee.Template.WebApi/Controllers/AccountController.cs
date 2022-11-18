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
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

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

            await refreshTokenRepository.DeleteByIdAsync("FK_UserId", userId);
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
                ExpiryDate = refreshToken.ExpiryDate,
                IsRevorked = refreshToken.IsRevorked,
                JwtId = refreshToken.JwtId.ToString(),
            };

            try
            {
                var jwtToken = base.RefreshToken(model.AccessToken, storedRefreshToken);

                await refreshTokenRepository.InsertAsync(new T_RefreshToken()
                {
                    Id = jwtToken.RefreshToken,
                    ExpiryDate = jwtToken.RefreshTokenExpiryDate,
                    IsRevorked = false,
                    JwtId = jwtToken.Id.ToGuid(),
                    UserId = refreshToken.UserId
                });

                return SucceededResult(jwtToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error in AccountController.RefreshToken");
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
                _logger.LogWarning(ex, "Error in AccountController.SignInWithCookie");
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
                var jwtToken = GenerateJwtToken(userInfo.ToClaims());

                var refreshTokenRepository = HttpContext.RequestServices.GetRequiredService<IRefreshTokenRepository>();
                await refreshTokenRepository.InsertAsync(new T_RefreshToken()
                {
                    Id = jwtToken.RefreshToken,
                    ExpiryDate = jwtToken.RefreshTokenExpiryDate,
                    IsRevorked = false,
                    JwtId = jwtToken.Id.ToGuid(),
                    UserId = userInfo.UserId.ToGuid()
                });

                return SucceededResult(jwtToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error in AccountController.SignInWithJwt");
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
            var refreshTokenRepository = HttpContext.RequestServices.GetRequiredService<IRefreshTokenRepository>();
            var jwtId = base.SignOutWithJwt();
            await refreshTokenRepository.DeleteByIdAsync("JwtId", jwtId);

            return SucceededResult();
        }

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        [HttpGet]
        public async Task<Response<IEnumerable<MenuTreeModel>>> UserMenus()
        {
            var userId = base.UserInfo.UserId;
            var vUserRoleRepository = HttpContext.RequestServices.GetRequiredService<IVUserRoleRepository>();
            var vRoleMenuRepository = HttpContext.RequestServices.GetRequiredService<IVRoleMenuRepository>();

            // 第1步：根据用户找到所属角色
            var roleIds = await vUserRoleRepository.QueryEnabledRoleIdsByUserId(userId.ToGuid());

            if (roleIds.Any() == false)
            {
                return FailedResult($"用户: {base.UserInfo.UserName} 尚未分配角色");
            }

            // 第2步：根据角色Id找到菜单
            var roleMenus = await vRoleMenuRepository.QueryEnabledByRoleIds(roleIds);
            if (roleMenus.Any() == false)
            {
                return FailedResult($"用户: {base.UserInfo.UserName} 所关联的角色尚未分配菜单");
            }

            return SucceededResult(roleMenus.ToTreeModel());
        }
    }
}