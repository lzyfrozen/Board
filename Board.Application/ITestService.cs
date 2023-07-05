using Board.Infrastructure;

namespace Board.Application
{
    public interface ITestService : IBaseService
    {
        public string Hello();

        public List<dynamic> GetLocaltionList();

        public Task<List<string>> TestDB();

        public Task<List<string>> TestDB2();
    }
}
