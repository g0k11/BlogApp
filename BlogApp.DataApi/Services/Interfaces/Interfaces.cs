using Ardalis.Result;
using BlogApp.DataApi.Entities;
using BlogApp.DataApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.DataApi.Services.Interfaces
{
    // 1. AboutMe
    public interface IAboutMeService
    {
        Task<Result<AboutMe>> GetAsync(int id);
        Task<Result<IEnumerable<AboutMe>>> GetAllAsync();
        Task<Result<AboutMe>> CreateAsync(AboutMe entity);
        Task<Result<AboutMe>> UpdateAsync(AboutMe entity);
        Task<Result> DeleteAsync(int id);
    }

    // 2. ContactMessages
    public interface IContactMessagesService
    {
        Task<Result<ContactMessages>> GetAsync(int id);
        Task<Result<IEnumerable<ContactMessages>>> GetAllAsync();
        Task<Result<ContactMessages>> CreateAsync(ContactMessages entity);
        Task<Result<ContactMessages>> UpdateAsync(ContactMessages entity);
        Task<Result> DeleteAsync(int id);
    }

    // 3. Educations
    public interface IEducationsService
    {
        Task<Result<Educations>> GetAsync(int id);
        Task<Result<IEnumerable<Educations>>> GetAllAsync();
        Task<Result<Educations>> CreateAsync(Educations entity);
        Task<Result<Educations>> UpdateAsync(Educations entity);
        Task<Result> DeleteAsync(int id);
    }

    // 4. Experiences
    public interface IExperiencesService
    {
        Task<Result<Experiences>> GetAsync(int id);
        Task<Result<IEnumerable<Experiences>>> GetAllAsync();
        Task<Result<Experiences>> CreateAsync(Experiences entity);
        Task<Result<Experiences>> UpdateAsync(Experiences entity);
        Task<Result> DeleteAsync(int id);
    }

    // 5. PersonalInfo
    public interface IPersonalInfoService
    {
        Task<Result<PersonalInfo>> GetAsync(int id);
        Task<Result<IEnumerable<PersonalInfo>>> GetAllAsync();
        Task<Result<PersonalInfo>> CreateAsync(PersonalInfo entity);
        Task<Result<PersonalInfo>> UpdateAsync(PersonalInfo entity);
        Task<Result> DeleteAsync(int id);
    }

    // 6. Projects
    public interface IProjectsService
    {
        Task<Result<Projects>> GetAsync(int id);
        Task<Result<IEnumerable<Projects>>> GetAllAsync();
        Task<Result<Projects>> CreateAsync(Projects entity);
        Task<Result<Projects>> UpdateAsync(Projects entity);
        Task<Result> DeleteAsync(int id);
    }
}
