using SinhvienXML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace SinhvienXML.Controllers
{
    public class SinhvienController : Controller
    {
        private string svpath = "/Database/Sinhvien.xml";
        private string dtocpath = "/Database/Dantoc.xml";
        private string loppath = "/Database/Lop.xml";
        // GET: Sinhvien
        public ActionResult Index()
        {
            ViewBag.MADANTOC = new SelectList(getListDanToc(), "ID", "TENDT");
            ViewBag.MALOP = new SelectList(getListLop(), "ID", "TENLOP");
            return View();
        }
        public JsonResult ListAll()
        {
            return Json(getListSV(), JsonRequestBehavior.AllowGet);
        }
        public IEnumerable<Sinhvien> getListSV()
        {
            XDocument sv = XDocument.Load(Server.MapPath(svpath));
            List<Sinhvien> listsv = new List<Sinhvien>();
            foreach (XElement x in sv.Element("Table").Descendants("Student"))
            {
                Sinhvien s = new Sinhvien();
                s.ID = Convert.ToInt32(x.Element("ID").Value);
                s.TENSV = x.Element("TENSV").Value;
                s.NGAYSINH = x.Element("NGAYSINH").Value;
                s.QUEQUAN = x.Element("QUEQUAN").Value;
                s.MADANTOC = Convert.ToInt32(x.Element("MADANTOC").Value);
                s.MALOP = Convert.ToInt32(x.Element("MALOP").Value);
                s.LOP = getLopbyID((int)s.MALOP);
                s.DANTOC = getDANTOCbyID((int)s.MADANTOC);
                listsv.Add(s);
            }
            return listsv;
        }
        public List<Lop> getListLop()
        {
            XDocument lop = XDocument.Load(Server.MapPath(loppath));
            List<Lop> listlop = new List<Lop>();
            foreach (XElement x in lop.Element("Table").Descendants("LOP"))
            {
                Lop s = new Lop();
                s.ID = Convert.ToInt32(x.Element("ID").Value);
                s.TENLOP = x.Element("TENLOP").Value;
                listlop.Add(s);
            }
            return listlop;
        }
        public List<Dantoc> getListDanToc()
        {
            XDocument dtoc = XDocument.Load(Server.MapPath(dtocpath));
            List<Dantoc> listdt = new List<Dantoc>();
            foreach (XElement x in dtoc.Element("Table").Descendants("DANTOC"))
            {
                Dantoc s = new Dantoc();
                s.ID = Convert.ToInt32(x.Element("ID").Value);
                s.TENDT = x.Element("TENDT").Value;
                listdt.Add(s);
            }
            return listdt;
        }
        public Lop getLopbyID(int id)
        {
            XDocument DBlop = XDocument.Load(Server.MapPath(loppath));
            var lopxml = DBlop.Element("Table").Descendants("LOP").SingleOrDefault(x => Convert.ToInt32(x.Element("ID").Value) == id);
            Lop lop = new Lop(id, lopxml.Element("TENLOP").Value);
            return lop;
        }
        public Dantoc getDANTOCbyID(int id)
        {
            XDocument DBdtoc = XDocument.Load(Server.MapPath(dtocpath));
            var dtocxml = DBdtoc.Element("Table").Descendants("DANTOC").SingleOrDefault(x => Convert.ToInt32(x.Element("ID").Value) == id);
            Dantoc dtoc = new Dantoc(id, dtocxml.Element("TENDT").Value);
            return dtoc;
        }

        [HttpPost]
        public JsonResult Details(int id)
        {
            XDocument DBsinhvien = XDocument.Load(Server.MapPath(svpath));
            var sinhvienXML = DBsinhvien.Element("Table").Descendants("Student").SingleOrDefault(x => Convert.ToInt32(x.Element("ID").Value) == id);
            Sinhvien student = new Sinhvien();
            student.ID = Convert.ToInt32(sinhvienXML.Element("ID").Value);
            student.TENSV = sinhvienXML.Element("TENSV").Value;
            student.NGAYSINH = sinhvienXML.Element("NGAYSINH").Value;
            student.QUEQUAN = sinhvienXML.Element("QUEQUAN").Value;
            student.MADANTOC = Convert.ToInt32(sinhvienXML.Element("MADANTOC").Value);
            student.MALOP = Convert.ToInt32(sinhvienXML.Element("MALOP").Value);
            student.DANTOC = getDANTOCbyID(Convert.ToInt32(sinhvienXML.Element("MADANTOC").Value));
            student.LOP = getLopbyID(Convert.ToInt32(sinhvienXML.Element("MALOP").Value));
            return Json(student);
        }
        [HttpPost]
        public JsonResult Create(string ten, int malop, string ngaysinh, string que, int madantoc)
        {
            XDocument DBsinhvien = XDocument.Load(Server.MapPath(svpath));
            Sinhvien student = new Sinhvien();
            if(DBsinhvien.Element("Table").IsEmpty)
            {
                XElement svXML = new XElement("Student",
                    new XElement("ID", 1),
                    new XElement("TENSV", ten),
                    new XElement("NGAYSINH", ngaysinh.Split(' ')[0]),
                    new XElement("QUEQUAN", que),
                    new XElement("MADANTOC", madantoc),
                    new XElement("MALOP", malop)
                );
                DBsinhvien.Root.Add(svXML);
                student = new Sinhvien(1, ten, ngaysinh, que, madantoc, malop, getDANTOCbyID(madantoc), getLopbyID(malop));
            }
            else
            {
                var sinhvienXML = DBsinhvien.Element("Table").Descendants("Student").Last();
                int id = sinhvienXML != null ? Convert.ToInt32(sinhvienXML.Element("ID").Value) : 0;
                XElement svXML = new XElement("Student",
                    new XElement("ID", id + 1),
                    new XElement("TENSV", ten),
                    new XElement("NGAYSINH", ngaysinh.Split(' ')[0]),
                    new XElement("QUEQUAN", que),
                    new XElement("MADANTOC", madantoc),
                    new XElement("MALOP", malop)
                );
                DBsinhvien.Root.Add(svXML);
                student = new Sinhvien(id + 1, ten, ngaysinh, que, madantoc, malop, getDANTOCbyID(madantoc), getLopbyID(malop));
            }
            DBsinhvien.Save(Server.MapPath(svpath));
            
            return Json(student);
        }
        [HttpPost]
        public JsonResult Edit(int id, string ten, int malop, string ngaysinh, string que, int madantoc)
        {
            XDocument DBsinhvien = XDocument.Load(Server.MapPath(svpath));
            var sinhvienXML = DBsinhvien.Element("Table").Descendants("Student").SingleOrDefault(x => Convert.ToInt32(x.Element("ID").Value) == id);
            if(sinhvienXML != null)
            {
                sinhvienXML.Element("TENSV").Value = ten;
                sinhvienXML.Element("NGAYSINH").Value = ngaysinh;
                sinhvienXML.Element("QUEQUAN").Value = que;
                sinhvienXML.Element("MALOP").Value = malop.ToString();
                sinhvienXML.Element("MADANTOC").Value = madantoc.ToString();
                //DBsinhvien.Root.Add(sinhvienXML);
                DBsinhvien.Save(Server.MapPath(svpath));
            }
            Sinhvien student = new Sinhvien(id, ten, ngaysinh, que, madantoc, malop, getDANTOCbyID(madantoc), getLopbyID(malop));
            return Json(student);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            XDocument DBsinhvien = XDocument.Load(Server.MapPath(svpath));
            var sinhvienXML = DBsinhvien.Element("Table").Descendants("Student").SingleOrDefault(x => Convert.ToInt32(x.Element("ID").Value) == id);
            sinhvienXML.Remove();
            DBsinhvien.Save(Server.MapPath(svpath));
            return Json(id);
        }
        [HttpPost]
        public JsonResult Search(string searchString)
        {
            XDocument DBsinhvien = XDocument.Load(Server.MapPath(svpath));
            var listSV_XML  = DBsinhvien.Element("Table").Descendants("Student").Where(x => x.Element("TENSV").Value.ToUpper().Contains(searchString.ToUpper()));
            List<Sinhvien> lstStudent = new List<Sinhvien>();
            foreach(XElement x in listSV_XML)
            {
                Sinhvien student = new Sinhvien();
                student.ID = Convert.ToInt32(x.Element("ID").Value);
                student.TENSV = x.Element("TENSV").Value;
                student.NGAYSINH = x.Element("NGAYSINH").Value;
                student.QUEQUAN = x.Element("QUEQUAN").Value;
                student.MADANTOC = Convert.ToInt32(x.Element("MADANTOC").Value);
                student.MALOP = Convert.ToInt32(x.Element("MALOP").Value);
                student.DANTOC = getDANTOCbyID((int)student.MADANTOC);
                student.LOP = getLopbyID((int)student.MALOP);
                lstStudent.Add(student);
            }
            return Json(lstStudent);
        }
    }
}