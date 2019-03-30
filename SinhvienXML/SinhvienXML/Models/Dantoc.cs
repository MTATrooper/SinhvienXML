using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SinhvienXML.Models
{
    public class Dantoc
    {
        public Dantoc()
        {
            SINHVIENs = new HashSet<Sinhvien>();
        }

        public Dantoc(int id, string ten)
        {
            this.ID = id;
            this.TENDT = ten;
        }

        public int ID { get; set; }

        [StringLength(20)]
        public string TENDT { get; set; }

        public virtual ICollection<Sinhvien> SINHVIENs { get; set; }
    }
}