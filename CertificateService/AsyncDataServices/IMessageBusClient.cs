using CertificateService.Dtos;

namespace CertificateService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewCertificate(CertificatePublishedDto certificatePublishedDto);
    }
}
