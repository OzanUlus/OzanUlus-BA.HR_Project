﻿using AutoMapper;
using BA.HR_Project.Application.DTOs;
using BA.HR_Project.Infrasturucture.Managers.Concrate;
using BA.HR_Project.Infrasturucture.Services.Concrate;
using BA.HR_Project.WEB.Models;
using BA.HR_Project.WEB.ModelValidators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BA.HR_Project.WEB.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyManager;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService companyManager, IMapper mapper)
        {
            _companyManager = companyManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult AddCompany()
        {
            ViewBag.Photopath = "/mexant/assets/images/defaultCompanyPhoto.png";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyViewModel vm)
        {
            var LogoPath = await HelperMethods.ImageHelper.SaveImageFile(vm.Photo);
            vm.LogoPath = LogoPath;

            if (vm.LogoPath == null)
            {
                vm.LogoPath = "/mexant/assets/images/defaultCompanyPhoto.png";
            }



            var validator = new CompanyViewModelValidator();
            var validationResult = await validator.ValidateAsync(vm);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(vm);
            }
            vm.Id = Guid.NewGuid().ToString();
            var CompanyDto = _mapper.Map<CompanyDto>(vm);
            //var createCompanyAction = await _companyManager.Insert(CompanyDto);
            var createCompanyAction = await _companyManager.AddCompany(CompanyDto);
            if (createCompanyAction.IsSuccess)
            {
                return RedirectToAction("ListCompany");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCompany(string Id)
        {


            var GetRelatedCompany = await _companyManager.GetByIdAsync(Id);
            if (GetRelatedCompany.IsSuccess)
            {
                var vm = _mapper.Map<CompanyViewModel>(GetRelatedCompany.Context);
                return View(vm);
            }
            return RedirectToAction("ListCompany");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompany(CompanyViewModel vm)
        {
            var LogoPath = await HelperMethods.ImageHelper.SaveImageFile(vm.Photo);
            vm.LogoPath = LogoPath;

            var validator = new CompanyViewModelValidator();
            var validationResult = await validator.ValidateAsync(vm);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(vm);
            }

            var CompanyDto = _mapper.Map<CompanyDto>(vm);
            var updateAction = await _companyManager.Update(CompanyDto);

            if (updateAction.IsSuccess)
            {
                return RedirectToAction("ListCompany");
            }
            return View(vm);

        }

        [HttpGet]
        public async Task<IActionResult> ListCompany()
        {
            var GetcompanysDto = await _companyManager.GetAll();
            if (GetcompanysDto.IsSuccess)
            {
                var companysVM = _mapper.Map<List<CompanyViewModel>>(GetcompanysDto.Context);
                return View(companysVM);
            }
            return View();
        }
    }
}
