using BlogApp.DataApi.Entities;
using Ardalis.Result;
using System.Threading.Tasks;
using BlogApp.Common.GenericRepository.Interfaces;

namespace BlogApp.DataApi.Repositories.Interfaces
{
    public interface IAboutMeRepository : IGenericRepository<AboutMe>
    {
    }
}
