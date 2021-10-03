using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ProEventos.Application.DTOs
{
    public class EventoDTO
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }
        public string Tema { get; set; }
        public int QuantidadePessoas { get; set; }
        public string ImagemUrl { get; set; }
        public string Telefone { get; set; }
        [Required()]
        public string Email { get; set; }
        public IEnumerable<LoteDTO> Lotes { get; set; }
        public IEnumerable<RedeSocialDTO> RedesSociais { get; set; }
        public IEnumerable<PalestranteDTO> PalestrantesEventos { get; set; }
    }
}