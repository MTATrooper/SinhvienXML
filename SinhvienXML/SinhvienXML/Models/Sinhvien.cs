using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinhvienXML.Models
{
    public class Sinhvien
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string TENSV { get; set; }

        [Column(TypeName = "text")]
        public string NGAYSINH { get; set; }

        [StringLength(50)]
        [Column(TypeName = "ntext")]
        public string QUEQUAN { get; set; }

        public int? MADANTOC { get; set; }

        public int? MALOP { get; set; }

        public virtual Dantoc DANTOC { get; set; }

        public virtual Lop LOP { get; set; }
        public Sinhvien(int id, string ten, string ngaysinh, string que, int madt, int malop, Dantoc dtoc, Lop lop)
        {
            ID = id;
            TENSV = ten;
            NGAYSINH = ngaysinh;
            QUEQUAN = que;
            MADANTOC = madt;
            MALOP = malop;
            DANTOC = dtoc;
            LOP = lop;
        }
        public Sinhvien() { }
    }
}