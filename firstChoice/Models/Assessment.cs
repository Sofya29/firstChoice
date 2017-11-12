using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace firstChoice.Models
{
    public class Assessment
    {
        public int assessmentID { get; set; }
        public string title { get; set; }
        public string moduleID { get; set; }
        public int percentageWorth { get; set; }
        public int duration { get; set; }
        public string lecturerID { get; set; }
    }
}