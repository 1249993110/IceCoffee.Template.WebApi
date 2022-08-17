namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class T_RefreshToken
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JwtId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRevorked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpiryDate { get; set; }

    }
}
