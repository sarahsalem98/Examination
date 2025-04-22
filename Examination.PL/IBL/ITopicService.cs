using Examination.PL.ModelViews;

namespace Examination.PL.IBL;

public interface ITopicService
{

    public IEnumerable<TopicMV> GetAll();
    public int Add(TopicMV topic);
    public PaginatedData<TopicMV> GetAllPaginated(TopicMV topicSerach, int PageSize = 10, int Page = 1);
    public int Update(TopicMV topic);
    public TopicMV GetTopicByID(int id);


}
