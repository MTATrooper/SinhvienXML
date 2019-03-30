using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SinhvienXML.Models
{
    public class Lop
    {
        public Lop()
        {
            SINHVIENs = new HashSet<Sinhvien>();
        }

        public Lop(int id, string ten)
        {
            this.ID = id;
            this.TENLOP = ten;
        }
        public int ID { get; set; }

        [StringLength(20)]
        public string TENLOP { get; set; }

        public virtual ICollection<Sinhvien> SINHVIENs { get; set; }
    }
}