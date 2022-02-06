using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;

namespace ProEventos.Application.helpers
{
    public class ProEventoProfile : Profile
    {
        public ProEventoProfile()
        {
            CreateMap<Evento,EventoDTO>()
                .ForMember(x => x.QtdPessoas, opt => opt.MapFrom(e => e.QuantidadePessoas))
                .ReverseMap();
            CreateMap<Lote,LoteDTO>().ReverseMap();
            CreateMap<Palestrante,PalestranteDTO>().ReverseMap();
            CreateMap<RedeSocial,RedeSocialDTO>().ReverseMap();
            CreateMap<User,UserDTO>().ReverseMap();
            CreateMap<User,UserLoginDTO>().ReverseMap();
            CreateMap<User,UserUpdateDTO>().ReverseMap();

        }
    }
}