using BlogApp.Common.GenericRepository;
using BlogApp.DataApi.Data;
using BlogApp.DataApi.Entities;
using BlogApp.DataApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.DataApi.Repositories
{
    public class AboutMeRepository : GenericRepository<AboutMe> , IAboutMeRepository
    {
        public AboutMeRepository(DbContext context) : base(context) { }
    }

    public class ContactMessagesRepository : GenericRepository<ContactMessages> , IContactMessagesRepository
    {
        public ContactMessagesRepository(DbContext context) : base(context) { }
    }

    public class EducationsRepository : GenericRepository<Educations> , IEducationsRepository
    {
        public EducationsRepository(DbContext context) : base(context) { }
    }

    public class ExperiencesRepository : GenericRepository<Experiences> , IExperiencesRepository
    {
        public ExperiencesRepository(DbContext context) : base(context) { }
    }

    public class PersonalInfoRepository : GenericRepository<PersonalInfo> , IPersonalInfoRepository
    {
        public PersonalInfoRepository(DbContext context) : base(context) { }
    }

    public class ProjectsRepository : GenericRepository<Projects> , IProjectsRepository
    {
        public ProjectsRepository(DbContext context) : base(context) { }
    }
}
