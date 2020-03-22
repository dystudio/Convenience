﻿using Backend.Repository.backend.api.Data;

using Microsoft.AspNetCore.Identity;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Repository.backend.api
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<SystemRole> _roleManager;

        public RoleRepository(RoleManager<SystemRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> AddRole(SystemRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public IQueryable<SystemRole> GetRoles(Expression<Func<SystemRole, bool>> where)
        {
            return _roleManager.Roles.Where(where);
        }

        public IQueryable<SystemRole> GetRoles(Expression<Func<SystemRole, bool>> where, int page, int size)
        {
            var skip = size * (page - 1);
            return GetRoles(where).Take(size).Skip(skip);
        }

        public IQueryable<SystemRole> GetRoles(int page, int size)
        {
            var skip = size * (page - 1);
            return _roleManager.Roles.Skip(skip).Take(size);
        }

        public IQueryable<SystemRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        public async Task<bool> RemoveRole(string roleName)
        {
            var role = await GetRole(roleName);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                return result.Succeeded;
            }
            return true;
        }

        public async Task<SystemRole> GetRole(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<bool> UpdateRole(SystemRole role)
        {
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}