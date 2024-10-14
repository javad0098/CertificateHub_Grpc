using AutoMapper;
using CertificateService.Models;
using CertificateService.Dtos;

namespace CertificateService.Profiles
{
    public class CertificatesProfiles : Profile
    {
        public CertificatesProfiles() // Removed the unnecessary parameter declaration
        {
            // // CreateMap statement to map Platform to PlatformReadDto
            CreateMap<Certificate,CertificateReadDto >();
            CreateMap<CertificateCreateDtos, Certificate>();
            CreateMap<CertificateReadDto, CertificatePublishedDto>();

        }
    }
}
