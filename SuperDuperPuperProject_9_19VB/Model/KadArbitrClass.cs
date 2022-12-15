using System;
using System.Collections.Generic;

namespace SuperCoolProject_919vb.Models {
    public class KadArbitrClass {
        public int Page { get; set; } // страница 
        public int Count { get; set; } // количество
        public List<string> Courts { get; set; } // суды
        public DateTime? DateFrom { get; set; } // дата от
        public DateTime? DateTo { get; set; } // дата до
        public List<Side> Sides { get; set; } // стороны дела
        public List<Judge> Judges { get; set; } //судьи
        public List<string> CaseNumbers { get; set; } //номера дел
        public bool WithVKSInstances { get; set; } // служебные поручения
        public string CaseType { get; set; } // тип дела
    }

    public class Side {
        public string Name { get; set; } // название
        public int Type { get; set; } // участник дела
        public bool ExactMatch { get; set; } // ???
    }
    public class Judge {
        public string JudgeId { get; set; }
        public int Type { get; set; }
    }
}
