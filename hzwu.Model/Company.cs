using System;
using hzwu.Framework.SqlDataValidate;
using hzwu.Framework.SqlMapping;

namespace hzwu.Model
{
    /// <summary>
    /// 数据库是Company  但是程序是CompanyModel
    /// </summary>
    //[Table("Company;Company")]
    [Table("Company")]
    public class CompanyModel : BaseModel
    {
        [Column("Name")]
        [Required,Length(4, 14)]
        public string CompanyName { get; set; }

        public DateTime CreateTime { get; set; }

        [Int(1,999999999)]
        public int CreatorId { get; set; }

        public Nullable<int> LastModifierId { get; set; }

        public DateTime? LastModifyTime { get; set; }
    }
}