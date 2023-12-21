﻿using BA.HR_Project.Application.DTOs;
using BA.HR_Project.Domain.Entities;
using BA.HR_Project.Infrasturucture.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BA.HR_Project.Infrasturucture.Services.Concrate
{
    public interface IAppUserService : IService<AppUser,AppUserDto>
    {
        Task UpdateV2(AppUserDto user);
    }
}
