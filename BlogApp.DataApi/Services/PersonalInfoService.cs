using Ardalis.Result;
using BlogApp.DataApi.Entities;
using BlogApp.DataApi.Repositories.Interfaces;
using BlogApp.DataApi.Services.Interfaces;

namespace BlogApp.DataApi.Services
{
    public class PersonalInfoService : IPersonalInfoService
    {
        private readonly IPersonalInfoRepository _personalInfoRepository;

        public PersonalInfoService(IPersonalInfoRepository personalInfoRepository)
        {
            _personalInfoRepository = personalInfoRepository;
        }

        public async Task<Result<PersonalInfo>> GetAsync(int id)
        {
            return await _personalInfoRepository.GetAsync(id);
        }

        public async Task<Result<IEnumerable<PersonalInfo>>> GetAllAsync()
        {
            return await _personalInfoRepository.GetAllAsync();
        }

        public async Task<Result<PersonalInfo>> CreateAsync(PersonalInfo entity)
        {
            return await _personalInfoRepository.CreateAsync(entity);
        }

        public async Task<Result<PersonalInfo>> UpdateAsync(PersonalInfo entity)
        {
            return await _personalInfoRepository.UpdateAsync(entity);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            return await _personalInfoRepository.DeleteAsync(id);
        }
    }
}
