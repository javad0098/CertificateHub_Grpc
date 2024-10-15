using AutoMapper;
using CertificateService.Models;
using CertificateService.Dtos;

namespace CertificateService.Profiles
{
    public class CertificatesProfiles : Profile
    {
        public CertificatesProfiles() // Removed the unnecessary parameter declaration
        {
            CreateMap<Certificate, CertificateReadDto>();
            CreateMap<CertificateCreateDtos, Certificate>();
            CreateMap<CertificateReadDto, CertificatePublishedDto>();
            CreateMap<Certificate, GrpcCertificateModel>()
            .ForMember(dest => dest.CertificateId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}

