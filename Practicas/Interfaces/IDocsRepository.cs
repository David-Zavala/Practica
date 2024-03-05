using Practicas.DTOs;

namespace Practicas.Interfaces
{
    public interface IDocsRepository
    {
        Task<List<Doc>> GetDocs();
        Task<Doc> GetDoc(int id);
        Task<Doc> PutDoc(Doc doc);
        Task<int> DeleteDoc(int id);
        Task<Doc> PostDoc(Doc doc);
    }
}