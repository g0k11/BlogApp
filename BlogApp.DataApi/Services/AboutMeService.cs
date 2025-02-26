using Ardalis.Result;
using BlogApp.DataApi.Entities;
using BlogApp.DataApi.Repositories.Interfaces;
using BlogApp.DataApi.Services.Interfaces;

namespace BlogApp.DataApi.Services
{
    public class AboutMeService : IAboutMeService
    {
        private readonly IAboutMeRepository _aboutMeRepository;

        public AboutMeService(IAboutMeRepository aboutMeRepository)
        {
            _aboutMeRepository = aboutMeRepository;
        }

        public async Task<Result<AboutMe>> GetAsync(int id)
        {
            return await _aboutMeRepository.GetAsync(id);
        }

        public async Task<Result<IEnumerable<AboutMe>>> GetAllAsync()
        {
            return await _aboutMeRepository.GetAllAsync();
        }

        public async Task<Result<AboutMe>> CreateAsync(AboutMe entity)
        {
            return await _aboutMeRepository.CreateAsync(entity);
        }

        public async Task<Result<AboutMe>> UpdateAsync(AboutMe entity)
        {
            return await _aboutMeRepository.UpdateAsync(entity);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            return await _aboutMeRepository.DeleteAsync(id);
        }
    }
}
