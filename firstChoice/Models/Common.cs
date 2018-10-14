using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace firstChoice.Models
{
    public class Common
    {
        public static DatabaseAccess dbAccess = new DatabaseAccess();
        public static Random random = new Random();
    }
}