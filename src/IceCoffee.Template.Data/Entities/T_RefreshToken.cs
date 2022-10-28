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
        [IgnoreInsert, IgnoreUpdate]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid JwtId { get; set; }

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
