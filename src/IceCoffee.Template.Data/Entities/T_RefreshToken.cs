namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_refresh_token")]
    public class T_RefreshToken
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("fk_user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("jwt_id")]
        public Guid JwtId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("is_revorked")]
        public bool IsRevorked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }

    }
}
