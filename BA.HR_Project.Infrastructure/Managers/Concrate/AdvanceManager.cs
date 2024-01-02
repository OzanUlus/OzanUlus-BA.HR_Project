﻿using AutoMapper;
using BA.HR_Project.Application.DTOs;
using BA.HR_Project.Application.Interfaces;
using BA.HR_Project.Domain.Entities;
using BA.HR_Project.Domain.Enums;
using BA.HR_Project.Infrastructure.Services.Abstract;
using BA.HR_Project.Infrasturucture.Managers.Abstract;
using BA.HR_Project.Infrasturucture.RequestResponse;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BA.HR_Project.Infrastructure.Managers.Concrate
{
    public class AdvanceManager : BaseManager<Advance, AdvanceDto>, IAdvanceService
    {
        private readonly UserManager<AppUser> _userManager;
        public AdvanceManager(IMapper mapper, IUow uow, UserManager<AppUser> userManager) : base(mapper, uow)
        {
            _userManager = userManager;
        }

        public async Task<Response> CreateAvance(AdvanceDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.AppUserId);

            var advances = await GetAll();
            var individualAdvances =  advances.Context.Where(x => x.AppUserId == dto.AppUserId && x.AdvanceType == AdvanceType.Individual && (x.ConfirmStatus == ConfirmStatus.Waiting || x.ConfirmStatus == ConfirmStatus.Approved));
            var institutionalAdvances = advances.Context.Where(x => x.AppUserId == dto.AppUserId && x.AdvanceType == AdvanceType.Institutional && (x.ConfirmStatus == ConfirmStatus.Waiting || x.ConfirmStatus == ConfirmStatus.Approved));
            
            if(dto.Amount>0)
            {
                if(dto.Currency == Currency.TRY)
                {
                    if ((dto.AdvanceType == AdvanceType.Individual && (individualAdvances.Sum(x => x.Amount) + dto.Amount) < user.Salary * 3) || (dto.AdvanceType == AdvanceType.Institutional && (institutionalAdvances.Sum(x => x.Amount) + dto.Amount) < 200000) )
                    {
                        dto.ConfirmStatus = ConfirmStatus.Waiting;
                        dto.RequestDate = DateTime.Now;
                        await Insert(dto);
                        return Response.Success();
                    }
                    else
                    {
                        return Response.Failure("The individual advance amount has exceeded the upper limit. Please note that the individual advance amount cannot exceed 3 salaries and the corporate advance amount has an upper limit.");
                    }
                }
                else if(dto.Currency == Currency.USD)
                {
                    if ((dto.AdvanceType == AdvanceType.Individual && (individualAdvances.Sum(x => x.Amount) + dto.Amount) < (user.Salary/20) * 3) || (dto.AdvanceType == AdvanceType.Institutional && (institutionalAdvances.Sum(x => x.Amount) + dto.Amount) < 10000))
                    {
                        dto.ConfirmStatus = ConfirmStatus.Waiting;
                        dto.RequestDate = DateTime.Now;
                        await Insert(dto);
                        return Response.Success();
                    }
                    else
                    {
                        return Response.Failure("The individual advance amount has exceeded the upper limit. Please note that the amount of an individual advance cannot exceed the amount of 3 salaries and the corporate advance amount has an upper limit.");
                    }
                }
                else if (dto.Currency == Currency.EUR)
                {
                    if ((dto.AdvanceType == AdvanceType.Individual && (individualAdvances.Sum(x => x.Amount) + dto.Amount) < (user.Salary / 30) * 3) || (dto.AdvanceType == AdvanceType.Institutional && (institutionalAdvances.Sum(x => x.Amount) + dto.Amount) < 6670))
                    {
                        dto.ConfirmStatus = ConfirmStatus.Waiting;
                        dto.RequestDate = DateTime.Now;
                        await Insert(dto);
                        return Response.Success();
                    }
                    else
                    {
                        return Response.Failure("The individual advance amount has exceeded the upper limit. Please note that the amount of an individual advance cannot exceed the amount of 3 salaries and the corporate advance amount has an upper limit.");
                    }
                }
            }
            return Response.Failure("The advance amount cannot be less than 0");
        }

        public async Task<List<AdvanceDto>> GetAllAvance(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var advancesAction = await GetAll();
            var userAdvances = advancesAction.Context.Where(x=>x.AppUserId == userId).ToList();
            return userAdvances;
        }
    }
}
