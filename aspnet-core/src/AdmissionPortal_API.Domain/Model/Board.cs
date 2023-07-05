using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class Board
    {
        public int Id { get; set; }
        public string SchoolBoard { get; set; }
        public string ShortName { get; set; }
    }
    public class BankMaster
    {

        public string BANK_Code { get; set; }
        public string BANK_Name_En { get; set; }
        //public string BANK_Short_Name { get; set; }
        //public string IFSC_Init { get; set; }
        //public string Acc_Digit { get; set; }
        //public string IsActive { get; set; }
        //public int Id { get; set; }
    }
    public class islockDetails
    {
        public List<UGIsLock> UGIsLock { get; set; }
        public List<PgIsLock> PGIsLock { get; set; }
    }
    public class UGIsLock
    {
        public string name { get; set; }
        public Boolean IsLock { get; set; }
        public Boolean status { get; set; }
    }
    public class PgIsLock
    {
        public string name { get; set; }
        public Boolean IsLock { get; set; }
    }
    public class BoardSubject
    {
        public string Subject { get; set; }
        public string Type { get; set; }
    }
}
