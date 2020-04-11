﻿
using Convience.Entity.Data;
using Convience.Jwtauthentication;
using Convience.Model.Models.AccountViewModels;
using Convience.Repository;
using Convience.Util.helpers;
using EasyCaching.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        private readonly IEasyCachingProvider _cachingProvider;

        private readonly IJwtFactory _jwtFactory;

        public AccountService(IUserRepository userRepository,
            IEasyCachingProvider cachingProvider,
            IJwtFactory jwtFactory)
        {
            _userRepository = userRepository;
            _cachingProvider = cachingProvider;
            _jwtFactory = jwtFactory;
        }

        public async Task<bool> IsStopUsing(string userName)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            if (user != null && user.IsActive)
            {
                return true;
            }
            return false;
        }

        public async Task<(bool, string, SystemUser)> ValidateCredentials(string userName, string password)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            if (user != null)
            {
                if (!user.IsActive)
                {
                    return (false, "此账号未激活！", null);
                }
                var isValid = await _userRepository.CheckPasswordAsync(user, password);
                if (isValid)
                {
                    var pairs = new List<(string, string)>
                    {
                        (CustomClaimTypes.UserName,user.UserName),
                        (CustomClaimTypes.UserRoleIds,user.RoleIds)
                    };
                    return (true, _jwtFactory.GenerateJwtToken(pairs), user);
                }
            }
            return (false, "错误的用户名或密码！", null);
        }

        public async Task<bool> ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            if (user != null)
            {
                var isValid = await _userRepository.ChangePasswordAsync(user, oldPassword, newPassword);
                if (isValid)
                {
                    return true;
                }
            }
            return false;

        }

        public async Task<CaptchaResult> GetCaptcha()
        {
            var randomValue = CaptchaHelper.GetValidateCode(5);
            var imageData = CaptchaHelper.CreateBase64Image(randomValue);
            var key = Guid.NewGuid().ToString();
            await _cachingProvider.SetAsync(key, randomValue, TimeSpan.FromMinutes(10));
            return new CaptchaResult
            {
                CaptchaKey = key,
                CaptchaData = imageData
            };
        }
    }
}
