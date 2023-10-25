using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Web.Services;

namespace Web.Pages
{
public class IndexModel : PageModel
{
    private readonly IEmployerRepositoryAsync _employer;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRazorRenderService _renderService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, IEmployerRepositoryAsync employer, IUnitOfWork unitOfWork, IRazorRenderService renderService)
    {
        _logger = logger;
        _employer = employer;
        _unitOfWork = unitOfWork;
        _renderService = renderService;
    }
    public IEnumerable<Employer> Employers { get; set; }
    public void OnGet()
    {
    }
    public async Task<PartialViewResult> OnGetViewAllPartial()
    {
        Employers = await _employer.GetAllAsync();
        return new PartialViewResult
        {
            ViewName = "_ViewAll",
            ViewData = new ViewDataDictionary<IEnumerable<Employer>>(ViewData, Employers)
        };
    }
    public async Task<JsonResult> OnGetCreateOrEditAsync(int id = 0)
    {
        if (id == 0)
            return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CreateOrEdit", new Employer()) });
        else
        {
            var thisEmployer = await _employer.GetByIdAsync(id);
            return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CreateOrEdit", thisEmployer) });
        }
    }
    public async Task<JsonResult> OnPostCreateOrEditAsync(int id, Employer employer)
    {
        if (ModelState.IsValid)
        {
            if (id == 0)
            {
                await _employer.AddAsync(employer);
                await _unitOfWork.Commit();
            }
            else
            {
                await _employer.UpdateAsync(employer);
                await _unitOfWork.Commit();
            }
            Employers = await _employer.GetAllAsync();
            var html = await _renderService.ToStringAsync("_ViewAll", Employers);
            return new JsonResult(new { isValid = true, html = html });
        }
        else
        {
            var html = await _renderService.ToStringAsync("_CreateOrEdit", employer);
            return new JsonResult(new { isValid = false, html = html });
        }
    }
    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var employer = await _employer.GetByIdAsync(id);
        await _employer.DeleteAsync(employer);
        await _unitOfWork.Commit();
        Employers = await _employer.GetAllAsync();
        var html = await _renderService.ToStringAsync("_ViewAll", Employers);
        return new JsonResult(new { isValid = true, html = html });
    }
}
}
