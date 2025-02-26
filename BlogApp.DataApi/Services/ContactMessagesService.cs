using Ardalis.Result;
using BlogApp.DataApi.Entities;
using BlogApp.DataApi.Repositories.Interfaces;
using BlogApp.DataApi.Services.Interfaces;

namespace BlogApp.DataApi.Services
{
    public class ContactMessagesService : IContactMessagesService
    {
        private readonly IContactMessagesRepository _contactMessagesRepository;

        public ContactMessagesService(IContactMessagesRepository contactMessagesRepository)
        {
            _contactMessagesRepository = contactMessagesRepository;
        }

        public async Task<Result<ContactMessages>> GetAsync(int id)
        {
            return await _contactMessagesRepository.GetAsync(id);
        }

        public async Task<Result<IEnumerable<ContactMessages>>> GetAllAsync()
        {
            return await _contactMessagesRepository.GetAllAsync();
        }

        public async Task<Result<ContactMessages>> CreateAsync(ContactMessages entity)
        {
            return await _contactMessagesRepository.CreateAsync(entity);
        }

        public async Task<Result<ContactMessages>> UpdateAsync(ContactMessages entity)
        {
            return await _contactMessagesRepository.UpdateAsync(entity);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            return await _contactMessagesRepository.DeleteAsync(id);
        }
    }
}
