﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_CCNPMM.Models
{
    public partial class MatHangAnh
    {
        public int MaAnh { get; set; }
        public int MaMatHang { get; set; }
        public string Anh { get; set; }

        public virtual MatHang MaMatHangNavigation { get; set; }
    }
}
