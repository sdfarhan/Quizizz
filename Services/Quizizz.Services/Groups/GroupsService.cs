namespace Quizizz.Services.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Quizizz.Data.Common.Repositories;
    using Quizizz.Data.Models;
    using Quizizz.Services.Common;
    using Quizizz.Services.EventsGroups;
    using Quizizz.Services.Mapping;
    using Quizizz.Services.StudentsGroups;
    using Quizizz.Services.Tools.Expressions;

    public class GroupsService : IGroupsService
    {
        private readonly IDeletableEntityRepository<Group> groupRepository;
        private readonly IStudentsGroupsService studentsGroupsService;
        private readonly IEventsGroupsService eventsGroupsService;
        private readonly IExpressionBuilder expressionBuilder;

        public GroupsService(
            IDeletableEntityRepository<Group> groupRepository,
            IStudentsGroupsService studentsGroupsService,
            IEventsGroupsService eventsGroupsService,
            IExpressionBuilder expressionBuilder)
        {
            this.groupRepository = groupRepository;
            this.studentsGroupsService = studentsGroupsService;
            this.eventsGroupsService = eventsGroupsService;
            this.expressionBuilder = expressionBuilder;
        }

        public async Task AssignStudentsToGroupAsync(string groupId, IList<string> studentIds)
        {
            foreach (var studentId in studentIds)
            {
                await this.studentsGroupsService.CreateStudentGroupAsync(groupId, studentId);
            }
        }

        public async Task<string> CreateGroupAsync(string name, string creatorId)
        {
            var newGroup = new Group
            {
                Name = name,
                CreatorId = creatorId,
            };

            await this.groupRepository.AddAsync(newGroup);
            await this.groupRepository.SaveChangesAsync();

            return newGroup.Id;
        }

        public async Task<IList<T>> GetAllAsync<T>(string creatorId = null, string eventId = null)
        {
            var query = this.groupRepository.AllAsNoTracking();

            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            if (eventId != null)
            {
                query = query.Where(x => x.EventsGroups.Any(x => x.EventId == eventId));
            }

            return await query.OrderByDescending(x => x.CreatedOn).To<T>().ToListAsync();
        }

        public async Task<IList<T>> GetGroupModelAsync<T>(string groupId)
        => await this.groupRepository
            .AllAsNoTracking()
            .Where(x => x.Id == groupId)
            .To<T>()
            .ToListAsync();

        public async Task DeleteAsync(string groupId)
        {
            var groupToBeDeleted = await this.groupRepository
                .AllAsNoTracking()
                .Where(x => x.Id == groupId)
                .FirstOrDefaultAsync();

            this.groupRepository.Delete(groupToBeDeleted);
            await this.groupRepository.SaveChangesAsync();
        }

        public async Task DeleteEventFromGroupAsync(string groupId, string eventId)
        {
            await this.eventsGroupsService.DeleteAsync(eventId, groupId);
        }

        public async Task DeleteStudentFromGroupAsync(string groupId, string studentId)
        {
            await this.studentsGroupsService.DeleteAsync(groupId, studentId);
        }

        public async Task UpdateNameAsync(string groupId, string name)
        {
            var groupToBeUpdated = await this.groupRepository
                .AllAsNoTracking()
                .Where(x => x.Id == groupId)
                .FirstOrDefaultAsync();

            groupToBeUpdated.Name = name;

            this.groupRepository.Update(groupToBeUpdated);
            await this.groupRepository.SaveChangesAsync();
        }

        public async Task AssignEventsToGroupAsync(string groupId, IList<string> evenstIds)
        {
            foreach (var eventId in evenstIds)
            {
                await this.eventsGroupsService.CreateEventGroupAsync(eventId, groupId);
            }
        }

        public async Task<IEnumerable<T>> GetAllByEventIdAsync<T>(string eventId)
        => await this.groupRepository
                .AllAsNoTracking()
                .Where(x => x.EventsGroups.Any(ev => ev.EventId == eventId))
                .To<T>()
                .ToListAsync();

        public async Task<IList<T>> GetAllPerPageAsync<T>(
            int page,
            int countPerPage,
            string creatorId = null,
            string searchCriteria = null,
            string searchText = null)
        {
            var query = this.groupRepository.AllAsNoTracking();

            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            var emptyNameInput = searchText == null && searchCriteria == ServicesConstants.Name;
            if (searchCriteria != null && !emptyNameInput)
            {
                var filter = this.expressionBuilder.GetExpression<Group>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query
                .OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetAllGroupsCountAsync(
            string creatorId = null,
            string searchCriteria = null,
            string searchText = null)
        {
            var query = this.groupRepository.AllAsNoTracking();

            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            var emptyNameInput = searchText == null && searchCriteria == ServicesConstants.Name;
            if (searchCriteria != null && !emptyNameInput)
            {
                var filter = this.expressionBuilder.GetExpression<Group>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public async Task<T> GetEventsFirstGroupAsync<T>(string eventId)
        => await this.groupRepository
                .AllAsNoTracking()
                .Where(x => x.EventsGroups.Any(ev => ev.EventId == eventId))
                .To<T>()
                .FirstOrDefaultAsync();
    }
}
