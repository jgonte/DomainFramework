using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWithPreRequisites.CourseBoundedContext
{
    public class CourseQueryRepository : EntityQueryRepository<Course, int>
    {
        public override (int, IEnumerable<Course>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Course>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Course> GetAll()
        {
            var result = Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Course>> GetAllAsync()
        {
            var result = await Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Course GetById(int courseId)
        {
            var result = Query<Course>
                .Single()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetById]")
                .Parameters(
                    p => p.Name("courseId").Value(courseId)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Course> GetByIdAsync(int courseId)
        {
            var result = await Query<Course>
                .Single()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetById]")
                .Parameters(
                    p => p.Name("courseId").Value(courseId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Course> GetAllIsRequiredByForCourse(int courseId)
        {
            var result = Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetAllIsRequiredBy]")
                .Parameters(
                    p => p.Name("courseId").Value(courseId)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Course>> GetAllIsRequiredByForCourseAsync(int courseId)
        {
            var result = await Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetAllIsRequiredBy]")
                .Parameters(
                    p => p.Name("courseId").Value(courseId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Course> GetAllRequiresForCourse(int courseId)
        {
            var result = Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetAllRequires]")
                .Parameters(
                    p => p.Name("courseId").Value(courseId)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Course>> GetAllRequiresForCourseAsync(int courseId)
        {
            var result = await Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetAllRequires]")
                .Parameters(
                    p => p.Name("courseId").Value(courseId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public (int, IEnumerable<Course>) GetIsRequiredByForCourse(int courseId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetIsRequiredBy]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("courseId").Value(courseId)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async Task<(int, IEnumerable<Course>)> GetIsRequiredByForCourseAsync(int courseId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetIsRequiredBy]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("courseId").Value(courseId)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public (int, IEnumerable<Course>) GetRequiresForCourse(int courseId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetRequires]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("courseId").Value(courseId)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async Task<(int, IEnumerable<Course>)> GetRequiresForCourseAsync(int courseId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Course>
                .Collection()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_GetRequires]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("courseId").Value(courseId)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Course>(new CourseQueryRepository());
        }

    }
}