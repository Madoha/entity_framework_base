using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Repositories
{
    public class LessonsRepository
    {
        private readonly PlatformDbContext _context;
        public LessonsRepository(PlatformDbContext context)
        {
            _context = context;
        }

        public async Task Add(Guid courseId, LessonEntity lesson)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId) ?? throw new Exception();
            course.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task Add_2(Guid courseId, string title)
        {
            var lesson = new LessonEntity()
            {
                Title = title,
                CourseId = courseId
            };

            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();
        }
    }
}
