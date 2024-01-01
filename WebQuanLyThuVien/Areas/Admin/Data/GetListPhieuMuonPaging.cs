using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQuanLyThuVien.Areas.Admin.Data
{
    public class GetListPhieuMuonPaging
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
    }
}