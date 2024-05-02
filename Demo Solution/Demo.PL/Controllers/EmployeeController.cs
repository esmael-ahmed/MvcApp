using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            dynamic employees;
            if (string.IsNullOrEmpty(SearchValue))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                //ViewData["message"] = "hello from viewData";
                //ViewBag.message = "hello from view bag";
                
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
                
            }
            var mappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployee);

        }

        public async Task<IActionResult> Create()
        {
            ViewBag.departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVm)
        {
            if (ModelState.IsValid)
            {
                employeeVm.ImageName = DocumentSettings.UploadFile(employeeVm.Image, "Images");

                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                await _unitOfWork.EmployeeRepository.AddAsync(mappedEmployee);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    TempData["successMessage"] = "Employee is Added";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVm);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
                return NotFound();
            var mappedEmployee = _mapper.Map<Employee,  EmployeeViewModel>(employee);
            return View(viewName, mappedEmployee);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVm, [FromRoute] int? id)
        {
            if (id != employeeVm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVm.Image is not null)
                    {
                        employeeVm.ImageName = DocumentSettings.UploadFile(employeeVm.Image, "Images");
                    }
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                    _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(employeeVm);
                }
            }
            return View(employeeVm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVm, [FromRoute] int id)
        {
            if (id != employeeVm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                    _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                    var result = await _unitOfWork.CompleteAsync();
                    if (result > 0 && employeeVm.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(employeeVm.ImageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(employeeVm);
                }

            }
            return View(employeeVm);
        }
    }
}
