//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyWeiboProject202005061120.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public partial class SpecialFollower
    {
        public int Id { get; set; }
        [DisplayName("关注时间")]
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public int UsersId { get; set; }
        public Nullable<int> SPFollowersId { get; set; }
    
        public virtual Users Users { get; set; }
        public virtual Users UsersSet { get; set; }
    }
}
