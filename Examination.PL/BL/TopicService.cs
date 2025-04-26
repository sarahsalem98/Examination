using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL;

public class TopicService : ITopicService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<TopicService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TopicService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TopicService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public int Add(TopicMV topic)
    {
        int result = 0;
        try
        {
            var newTopic = _mapper.Map<Topic>(topic);
            _unitOfWork.TopicRepo.Insert(newTopic);
            result = _unitOfWork.Save();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error Occurred while adding new Topic ");
            return 0;
        }
    }

    public IEnumerable<TopicMV> GetAll()
    {
        try
        {
            var res = _unitOfWork.TopicRepo.GetAll().ToList();
            var topicMVs = _mapper.Map<List<TopicMV>>(res);
            return topicMVs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error Occurred while getting all Topics ");
            return null;
        }
    }

    public PaginatedData<TopicMV> GetAllPaginated(TopicMV topicSerach, int PageSize = 10, int Page = 1)
    {
        try
        {
            List<TopicMV> topicMVs = new List<TopicMV>();
            List<Topic> data = _unitOfWork.TopicRepo.GetAll(
                s =>
                    (String.IsNullOrEmpty(topicSerach.Name) ||
                    (!String.IsNullOrEmpty(s.Name) && s.Name.ToLower().Trim().Contains(topicSerach.Name)))).ToList();

            topicMVs = _mapper.Map<List<TopicMV>>(data);
            int TotalCounts = topicMVs.Count();
            if (TotalCounts > 0)
            {
                topicMVs = topicMVs.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

            }
            PaginatedData<TopicMV> paginatedData = new PaginatedData<TopicMV>
            {
                Items = topicMVs,
                TotalCount = TotalCounts,
                PageSize = PageSize,
                CurrentPage = Page
            };
            return paginatedData;


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error Occurred while getting all Topics ");
            return null;
        }
    }

    public TopicMV GetTopicByID(int id)
    {
        try
        {
            var topic = _unitOfWork.TopicRepo.FirstOrDefault(i => i.Id == id);
            if (topic == null)
                return null;
            var topicMv = _mapper.Map<TopicMV>(topic);
            return topicMv;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in fetch Topics ");
            return null;
        }
    }

    public int Update(TopicMV topic)
    {
        try
        {
            int result = 0;
            var newTopic = _mapper.Map<Topic>(topic);
            _unitOfWork.TopicRepo.Update(newTopic);
            result = _unitOfWork.Save();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error Occurred while updating Topic ");
            return 0;
        }
    }

    public List<TopicMV> GetTopicsByCourseId(int ID)
    {
        try
        {
            var topics = _unitOfWork.CourseTopicRepo.GetAll(i => i.CourseId == ID,"Topic").Select(i => i.Topic).ToList();
            if (topics == null)  return new List<TopicMV>();

            var mapperdTopics = _mapper.Map<List<TopicMV>>(topics);
            
            return mapperdTopics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error Occurred while getting all Topics ");
            return new List<TopicMV>();
        }
    }


}
