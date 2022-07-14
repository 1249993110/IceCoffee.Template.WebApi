namespace IceCoffee.Template.WebApi.Models
{
    public class ChangePasswordModel
    {
        /// <summary>
        /// 旧密码哈希值
        /// </summary>        
        [Required]
        public string OldPasswordHash { get; set; }

        /// <summary>
        /// 新密码哈希值
        /// </summary>
        [Required]
        public string NewPasswordHash { get; set; }

        public ChangePasswordModel(string oldPasswordHash, string newPasswordHash)
        {
            OldPasswordHash = oldPasswordHash;
            NewPasswordHash = newPasswordHash;
        }
    }
}
