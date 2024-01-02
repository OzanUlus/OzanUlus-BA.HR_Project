﻿using BA.HR_Project.Application.DTOs;
using BA.HR_Project.Domain.Entities;
using BA.HR_Project.Infrasturucture.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BA.HR_Project.Infrastructure.Services.Concrate
{
    public interface IExpenseTypeService : IService<ExpenseType, ExpenseTypeDto>
    {
        Task<float> CalculateMaxPrice(float mainPrice, float maxFactor);
        Task<float> CalculateMinPrice(float mainPrice, float minFactor);

    }
}
