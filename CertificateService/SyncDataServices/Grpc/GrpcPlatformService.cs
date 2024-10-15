using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using CertificateService.Data;
namespace CertificateService.SyncDataServices.Grpc
{
    public class GrpcCertificateService : GrpcCertificate.GrpcCertificateBase
    {
        private readonly ICertificateRepo _repository;
        private readonly IMapper _mapper;

        public GrpcCertificateService(ICertificateRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<CertificateResponse> GetAllCertificates(GetAllRequest request, ServerCallContext context)
        {
            var response = new CertificateResponse();
            var certificates = _repository.GetAllCertificates();

            foreach(var cer in certificates)
            {
                response.Certificate.Add(_mapper.Map<GrpcCertificateModel>(cer));
            }

            return Task.FromResult(response);
        }
    }
}
