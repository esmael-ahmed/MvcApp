using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            var mappedDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDepartment);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVm)
        {
            if(ModelState.IsValid)
            {
                var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                await _unitOfWork.DepartmentRepository.AddAsync(mappedDepartment);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    TempData["successMessage"] = "Department is Created";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVm);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if(id is null)
                return BadRequest();

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if(department is null)
                return NotFound();
            var mappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(viewName, mappedDepartment);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest();

            //var department = _departmentRepository.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel departmentVm, [FromRoute]int id)
        {
            if (id != departmentVm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                    _unitOfWork.DepartmentRepository.Update(mappedDepartment);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(departmentVm);
                }
            }
            return View(departmentVm);    
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentVm, [FromRoute]int id)
        {
            if (id != departmentVm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                    _unitOfWork.DepartmentRepository.Delete(mappedDepartment);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(departmentVm);
                }
                
            }
            return View(departmentVm);
        }

    }
}
