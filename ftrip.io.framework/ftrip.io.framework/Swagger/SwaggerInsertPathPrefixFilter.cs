using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace ftrip.io.framework.Swagger
{
    public class SwaggerInsertPathPrefixFilter : IDocumentFilter
    {
        private readonly string _pathPrefix;

        public SwaggerInsertPathPrefixFilter(string pathPrefix)
        {
            _pathPrefix = pathPrefix;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.Keys.ToList();
            foreach (var path in paths)
            {
                var pathToChange = swaggerDoc.Paths[path];
                swaggerDoc.Paths.Remove(path);
                swaggerDoc.Paths.Add((!string.IsNullOrEmpty(_pathPrefix) ? $"/{_pathPrefix}" : "") + path, pathToChange);
            }
        }
    }
}