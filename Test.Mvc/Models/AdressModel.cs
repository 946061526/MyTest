using Chloe.Entity;
using System.ComponentModel.DataAnnotations;

namespace Test.Mvc.Models
{
    public class AdressModel
    {
        public int id { get; set; }
        [Required, Display(Name = "收货人姓名", Description = "请输入收货人姓名"), StringLength(10, MinimumLength = 6)]
        public string userName { get; set; }
        public string tel { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string area { get; set; }
        public string desc { get; set; }

    }

    public class ProvinceCityArea
    {
        [NonAutoIncrement]
        [Column(IsPrimaryKey = true, Name = "ID")]
        public int ID { get; set; }

        [Column(Name = "Name")]
        public string Name { get; set; }

        [Column(Name = "ParentID")]
        public int? ParentID { get; set; }
    }
}
