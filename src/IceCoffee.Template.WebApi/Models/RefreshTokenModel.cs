namespace IceCoffee.Template.WebApi.Models
{
    public class RefreshTokenModel
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        [Required]
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }

        public RefreshTokenModel(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
