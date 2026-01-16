using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Common.EntityModels.JobCardEntity
{
    public class JobCard
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public Guid? DealerId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
