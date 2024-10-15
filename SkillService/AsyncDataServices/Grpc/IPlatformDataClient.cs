using System.Collections.Generic;
using SkillService.Models;

namespace SkillService.SyncDataServices.Grpc
{
    public interface ICertificateDataClient
    {
        IEnumerable<Certificate> ReturnAllCertificates();
    }
}