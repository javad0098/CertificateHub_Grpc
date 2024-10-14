using System;
using System.Text.Json;
using AutoMapper;
using SkillService.Data;
using SkillService.Dtos;
using SkillService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace SkillService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.CertificatePublished:
                    addCertificate(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);
            switch (eventType.Event)
            {
                case "Certificate_Published":
                    Console.WriteLine("--> Certificate Published Event Detected");
                    return EventType.CertificatePublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void addCertificate(string certificatePublishedDtoMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ISkillRepo>();

                var CertificatePublishedDto = JsonSerializer.Deserialize<CertificatePublishedDto>(certificatePublishedDtoMessage);

                try
                {
                    var cer = _mapper.Map<Certificate>(CertificatePublishedDto);
                    if (!repo.ExternalCertificateExists(cer.ExternalID))
                    {
                        repo.CreateCertificate(cer);
                        repo.SaveChanges();
                        Console.WriteLine("--> Certificate added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Certificate already exisits...");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Certificate to DB {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        CertificatePublished,
        Undetermined
    }
}