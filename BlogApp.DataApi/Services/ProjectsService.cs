using Ardalis.Result;
using BlogApp.DataApi.Entities;
using BlogApp.DataApi.Repositories.Interfaces;
using BlogApp.DataApi.Services.Interfaces;

namespace BlogApp.DataApi.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _projectsRepository;

        public ProjectsService(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        public async Task<Result<Projects>> GetAsync(int id)
        {
            return await _projectsRepository.GetAsync(id);
        }

        public async Task<Result<IEnumerable<Projects>>> GetAllAsync()
        {
            return await _projectsRepository.GetAllAsync();
        }

        public async Task<Result<Projects>> CreateAsync(Projects entity)
        {
            return await _projectsRepository.CreateAsync(entity);
        }

        public async Task<Result<Projects>> UpdateAsync(Projects entity)
        {
            return await _projectsRepository.UpdateAsync(entity);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            return await _projectsRepository.DeleteAsync(id);
        }
    }
}
