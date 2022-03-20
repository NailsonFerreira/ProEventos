using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUserRepository userRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpdateDTO, string password)
        {
            try
            {
                var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == userUpdateDTO.UserName.ToLower());

                return await signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao tentar verificar conta. Erro:{e.Message}");
            }
        }

        public async Task<UserUpdateDTO> CreateAccountAsync(UserDTO userDTO)
        {
            try
            {
                var user = mapper.Map<User>(userDTO);
                var result = await userManager.CreateAsync(user, userDTO.Password);

                if (result.Succeeded)
                {
                    return mapper.Map<UserUpdateDTO>(user);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao tentar criar conta. Erro:{e.Message}");
            }
        }

        public async Task<UserUpdateDTO> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await userRepository.GetUserByUserNameAsync(userName);
                if (user is null) return null;

                return mapper.Map<UserUpdateDTO>(user);

            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao tentar verificar conta. Erro:{e.Message}");
            }
        }

        public async Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var user = await userRepository.GetUserByUserNameAsync(userUpdateDTO.UserName);
                if (user is null) return null;

                userUpdateDTO.Id = user.Id;
                mapper.Map(userUpdateDTO, user);

                if (!(userUpdateDTO.Password is null)){
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    await userManager.ResetPasswordAsync(user, token, userUpdateDTO.Password);
                }

                userRepository.Update<User>(user);

                if (await userRepository.SaveChangesAsync())
                {
                    var userReturn = await userRepository.GetUserByUserNameAsync(user.UserName);

                    return mapper.Map<UserUpdateDTO>(userReturn);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao tentar atualizar conta. Erro:{e.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await userManager.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao tentar verificar conta existe. Erro:{e.Message}");
            }
        }
    }
}
