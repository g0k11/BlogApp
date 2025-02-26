using Ardalis.Result;
using BlogApp.DataApi.Entities;
using BlogApp.DataApi.Repositories.Interfaces;
using BlogApp.DataApi.Services.Interfaces;

namespace BlogApp.DataApi.Services
{
    public class EducationsService : IEducationsService
    {
        private readonly IEducationsRepository _educationsRepository;

        public EducationsService(IEducationsRepository educationsRepository)
        {
            _educationsRepository = educationsRepository;
        }

        public async Task<Result<Educations>> GetAsync(int id)
        {
            return await _educationsRepository.GetAsync(id);
        }

        public async Task<Result<IEnumerable<Educations>>> GetAllAsync()
        {
            return await _educationsRepository.GetAllAsync();
        }

        public async Task<Result<Educations>> CreateAsync(Educations entity)
        {
            return await _educationsRepository.CreateAsync(entity);
        }

        public async Task<Result<Educations>> UpdateAsync(Educations entity)
        {
            return await _educationsRepository.UpdateAsync(entity);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            return await _educationsRepository.DeleteAsync(id);
        }
    }
}
