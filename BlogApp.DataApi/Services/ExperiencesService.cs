using Ardalis.Result;
using BlogApp.DataApi.Entities;
using BlogApp.DataApi.Repositories.Interfaces;
using BlogApp.DataApi.Services.Interfaces;

namespace BlogApp.DataApi.Services
{
    public class ExperiencesService : IExperiencesService
    {
        private readonly IExperiencesRepository _experiencesRepository;

        public ExperiencesService(IExperiencesRepository experiencesRepository)
        {
            _experiencesRepository = experiencesRepository;
        }

        public async Task<Result<Experiences>> GetAsync(int id)
        {
            return await _experiencesRepository.GetAsync(id);
        }

        public async Task<Result<IEnumerable<Experiences>>> GetAllAsync()
        {
            return await _experiencesRepository.GetAllAsync();
        }

        public async Task<Result<Experiences>> CreateAsync(Experiences entity)
        {
            return await _experiencesRepository.CreateAsync(entity);
        }

        public async Task<Result<Experiences>> UpdateAsync(Experiences entity)
        {
            return await _experiencesRepository.UpdateAsync(entity);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            return await _experiencesRepository.DeleteAsync(id);
        }
    }
}
