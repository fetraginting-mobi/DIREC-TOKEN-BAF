using System.Data;

namespace Token_Ifinsys.Models
{
    public class DataList
    {
        public int status { get; set; }
        public string token { get; set; }
        public string message { get; set; }
        public DataTable datalist { get; set; }
    }
}
