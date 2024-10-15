using System;
using System.Collections.Generic;
using SkillService.Models;
using SkillService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SkillService.Data;

namespace SkillService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<ICertificateDataClient>();

                var certificates = grpcClient.ReturnAllCertificates();

                SeedData(serviceScope.ServiceProvider.GetService<ISkillRepo>(), certificates);
            }
        }
        
        private static void SeedData(ISkillRepo repo, IEnumerable<Certificate> certificates)
        {
            Console.WriteLine("Seeding new certificates...");

            foreach (var cert in certificates)
            {
                if(!repo.ExternalCertificateExists(cert.ExternalID))
                {
                    repo.CreateCertificate(cert);
                }
                repo.SaveChanges();
            }
        }
    }
}