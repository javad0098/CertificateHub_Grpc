using System;
using System.Collections.Generic;
using AutoMapper;
using SkillService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using SkillService;
using SkillService.SyncDataServices.Grpc;
using CertificateService;

namespace SkillService.SyncDataServices.Grpc
{
    public class CertificateDataClient : ICertificateDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CertificateDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Certificate> ReturnAllCertificates()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcCertificate"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcCertificate"]);
            var client = new GrpcCertificate.GrpcCertificateClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllCertificates(request);
                return _mapper.Map<IEnumerable<Certificate>>(reply.Certificate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }
        }

    }
}