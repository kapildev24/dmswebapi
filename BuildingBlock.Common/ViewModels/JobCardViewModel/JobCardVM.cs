using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Common.ViewModels.JobCardViewModel
{
    public class JobCardVM
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public string DealerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
