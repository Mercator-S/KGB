using KGB_Dev_.Data;
using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;

namespace KGB_Dev_.Data_Retrieving
{
    public class IndexTable : IDataRetrivingServices
    {
        private readonly ApplicationDbContext? _context;
        public IndexTable(ApplicationDbContext? context)
        {
            _context = context;
        }
        public async Task<List<KGB_Knowledge>> GetListOfKnowledge()
        {
            var result = _context.KGB_Knowledge.ToList();
            return await Task.FromResult(result);
        }
    }
}
