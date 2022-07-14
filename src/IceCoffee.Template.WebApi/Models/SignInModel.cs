namespace IceCoffee.Template.WebApi.Models
{
    public class SignInModel
    {
        /// <summary>
        /// 登录名, 可以是用户名或者手机号
        /// </summary>
        [Required]
        public string LoginName { get; set; }

        /// <summary>
        /// 密码base64编码值
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        public SignInModel(string loginName, string passwordHash)
        {
            LoginName = loginName;
            PasswordHash = passwordHash;
        }
    }
}
