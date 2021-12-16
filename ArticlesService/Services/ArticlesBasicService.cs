using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace ArticlesService.Services
{
    public class ArticlesBasicService :ArticlesService.ArticlesBasicService.ArticlesBasicServiceBase
    {
        public override Task<ArticleId> CreateArticle(Article request, ServerCallContext context)
        {
            return Task.FromResult(new ArticleId()
            {
                Id = new Guid().ToString()
            });
        }
    }
}
