using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CoursesRepository
    {
        private readonly PlatformDbContext _context;
        public CoursesRepository(PlatformDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseEntity>> Get()
        {
            return await _context.Courses
                .AsNoTracking()
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<List<CourseEntity>> GetWithLesson()
        {
            return await _context.Courses
                .AsNoTracking()
                .Include(c => c.Lessons)
                //.Include(c => c.Students)
                .ToListAsync();
        }

        public async Task<CourseEntity?> GetById(Guid id)
        {
            return await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CourseEntity?>> GetByFilter(string title, decimal price)
        {
            var query = _context.Courses.AsNoTracking();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(c => c.Title == title);
            }

            if (price > 0)
            {
                query = query.Where(c => c.Price >= price);
            }

            return await query.ToListAsync();
        }

        public async Task<List<CourseEntity>> GetByPage(int page, int pageSize)
        {
            return await _context.Courses
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task Add(Guid id, Guid authorId, string title, string description, decimal price)
        {
            var courseEntity = new CourseEntity
            {
                Id = id,
                AuthorId = authorId,
                Title = title,
                Description = description,
                Price = price
            };

            await _context.AddAsync(courseEntity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid id, Guid authorId, string title, string description, decimal price)
        {
            var courseEntity = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception();

            courseEntity.AuthorId = authorId;
            courseEntity.Price = price;
            courseEntity.Title = title;
            courseEntity.Description = description;

            await _context.SaveChangesAsync();

            //since .net 8, it is best to use ExecuteUpdateAsync
            //await _context.Courses
            //    .Where(c => c.Id == id)
            //    .ExecuteUpdateAsync(s => s
            //        .SetProperty(c => c.Title, title)
            //        .SetProperty(c => c.Description, description)
            //        .SetProperty(c => c.Price, price)
            //        .SetProperty(c => c.AuthorId, authorId));

        } 

        public async Task Delete(Guid id)
        {
            var courseEntity = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            _context.Remove(courseEntity);
            await _context.SaveChangesAsync();

            //.net 8
            //await _context.Courses
            //    .Where(c => c.Id == id)
            //    .ExecuteDeleteAsync();
        }
    }
}
