using Examination.PL.Attributes;
using Examination.PL.BL;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers;



[Area("Admin")]
[UserTypeAuthorize(Constants.UserTypes.Admin)]
public class TopicController : Controller
{

    private readonly ITopicService _topicService;

    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Topics";
        return View();
    }


    public IActionResult List(TopicMV topicSearch, int Page = 1, int PageSize = 10)
    {
        ViewData["Title"] = "Students List";

        var topics = _topicService.GetAllPaginated(topicSearch, PageSize: 10, Page);
        return View(topics);
    }



    [HttpGet]
    public IActionResult AddUpdate(int id)
    {
        ViewData["Title"] = "Add Update Topics";
        var topic = new TopicMV();

        if (id > 0)
        {
            topic = _topicService.GetTopicByID(id);

            if (topic == null)
            {
                return NotFound();
            }
        }

        return View(topic);

    }




    [HttpPost]
    public IActionResult AddUpdate(TopicMV model)
    {

        ResponseMV response = new ResponseMV();
        if (ModelState.IsValid)
        {

            if (model.Id > 0)
            {
                var result = _topicService.Update(model);
                if (result > 0)
                {
                    response.Success = true;
                    response.Message = "Topic updated successfully";
                    response.RedirectUrl = null;
                }
                else if (result == -1)
                {
                    response.Success = false;
                    response.Message = "Topic Not Found";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error occurred while updating Topic";
                }
            }
            else
            {
                var result = _topicService.Add(model);
                if (result > 0)
                {
                    response.Success = true;
                    response.Message = "Topic added successfully";
                    response.RedirectUrl = null;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error occurred while adding Topic";
                }

            }
        }
        else
        {
            response.Success = false;
            response.Message = "Invalid data";
        }
        return Json(response);

    }



}
