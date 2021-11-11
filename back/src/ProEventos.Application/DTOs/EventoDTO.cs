using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ProEventos.Application.DTOs
{
    public class EventoDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O local é obrigatória")]
        public string Local { get; set; }
        [Required(ErrorMessage = "A data é obrigatória")]
        public string DataEvento { get; set; }
        [Required(ErrorMessage = "O tema é obrigatório"),
        StringLength(50, MinimumLength = 3, ErrorMessage = "É preciso haver entre 3 a 50 caracteres")]
        public string Tema { get; set; }
        [Required(ErrorMessage = "Quantidade de pessoas é obrigatório"),
        Range(1, 2000, ErrorMessage = "Quantidade entre 1 e 2000"),
        Display(Name = "Quantidade de pessoas")]
        public int QuantidadePessoas { get; set; }
        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "Url da imagem Inválido") ]
        public string ImagemUrl { get; set; }
        [Required(ErrorMessage = "{0} é obrigatório"),
        Phone(ErrorMessage = "{0} invalido"),
        Display(Name = "Telefone")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "E-mail é obrigatório"),
        EmailAddress(ErrorMessage = "{0} inválido"),
        Display(Name = "E-mail")]
        public string Email { get; set; }
        public IEnumerable<LoteDTO> Lotes { get; set; }
        public IEnumerable<RedeSocialDTO> RedesSociais { get; set; }
        public IEnumerable<PalestranteDTO> PalestrantesEventos { get; set; }
    }
}