using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Domain.Models;

namespace ProEventos.Application.helpers
{
    public class ProEventoProfile : Profile
    {
        public ProEventoProfile()
        {
            CreateMap<Evento,EventoDTO>().ReverseMap();
            CreateMap<Lote,LoteDTO>().ReverseMap();
            CreateMap<Palestrante,PalestranteDTO>().ReverseMap();
            CreateMap<RedeSocial,RedeSocialDTO>().ReverseMap();
        }
    }
}