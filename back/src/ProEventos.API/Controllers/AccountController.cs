using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ITokenService tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            this.accountService = accountService;
            this.tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao recuperar usuários. Erro: {e.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            try
            {
                if(await accountService.UserExists(userDTO.UserName))
                {
                    return BadRequest("Usuário já existe");
                }

                var user = await accountService.CreateAccountAsync(userDTO);
                if(!(user is null))
                {
                    user.Token = tokenService.CreateToken(user).Result;
                    return Ok(user);
                }

                return BadRequest("Error ao cadastrar usuario");
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao cadastrar. Erro: {e.Message}");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserUpdateDTO userDTO)
        {
            try
            {
                var user = await accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user is null) return Unauthorized("Usuario invalido");

                var userReturn = await accountService.UpdateAccount(userDTO);
                if (!(userReturn is null))
                {
                    return Ok(userReturn);
                }

                return BadRequest("Error ao atualizar usuario");
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao cadastrar. Erro: {e.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userDTO)
        {
            try
            {
                var user = await accountService.GetUserByUserNameAsync(userDTO.UserName);
                if (user is null) return Unauthorized("Usuario ou senha incorretos");

                var result = await accountService.CheckUserPasswordAsync(user, userDTO.Password);
                if(!result.Succeeded) return Unauthorized("Usuario ou senha incorretos");

                return Ok(new { userName = user.UserName, primeiroNome =user.PrimeiroNome, token = tokenService.CreateToken(user).Result});
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao logar. Erro: {e.Message}");
            }
        }

    }
}
