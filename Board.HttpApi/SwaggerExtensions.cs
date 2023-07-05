using Board.ToolKits;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.HttpApi
{
    /// <summary>
    /// SwaggerExtensions
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 当前API版本，从appsettings.json获取
        /// </summary>
        private static readonly string version = $"v{AppSettings.ApiVersion}";

        /// <summary>
        /// Swagger描述信息
        /// </summary>
        private static readonly string description = @"<b>Board.Web</b>：<a target=""_blank"" href=""#"">#</a> <br/>
                                                       <b>Hangfire</b>：<a target=""_blank"" href=""/hangfire"">任务调度中心</a> <br/>";

        /// <summary>
        /// AddSwagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Board.Web - 接口",
                    Description = description//"Board.Web - 接口描述"
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Board.HttpApi.xml"));
                //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Resources/EveDC.Domain.xml"));
                //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Resources/EveDC.Application.Contracts.xml"));

                #region 身份认证

                #region 定义JwtBearer认证方式一

                //options.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme()
                //{
                //    Description = "这是方式一(直接在输入框中输入认证信息，不需要在开头添加Bearer)",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = SecuritySchemeType.Http,
                //    Scheme = "bearer"
                //});

                #endregion

                #region 定义JwtBearer认证方式二

                //options.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme()
                //{
                //    Description = "这是方式二(JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）)",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = SecuritySchemeType.ApiKey
                //}); 

                #endregion

                #region swagger 全局锁

                //声明一个Scheme，注意下面的Id要和上面AddSecurityDefinition中的参数name一致
                //var scheme = new OpenApiSecurityScheme()
                //{
                //    Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "JwtBearer" }
                //};
                ////注册全局认证（所有的接口都可以使用认证）
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{
                //    [scheme] = new string[0]
                //});

                #endregion

                //非全局锁
                //options.OperationFilter<AuthResponsesOperationFilter>();
                //添加参数
                //options.OperationFilter<AddParameterFilter>();



                //options.OperationFilter<AddResponseHeadersFilter>();
                //options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //options.OperationFilter<SecurityRequirementsOperationFilter>();

                #endregion
            });
        }

        /// <summary>
        /// UseSwaggerUI
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");

                // 模型的默认扩展深度，设置为 -1 完全隐藏模型
                options.DefaultModelsExpandDepth(-1);
                // API文档仅展开标记
                options.DocExpansion(DocExpansion.List);
                // API前缀设置为空
                options.RoutePrefix = string.Empty;
                // API页面Title
                options.DocumentTitle = "接口文档 - Board.Web";
            });
        }

        internal class SwaggerApiInfo
        {
            /// <summary>
            /// URL前缀
            /// </summary>
            public string UrlPrefix { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// <see cref="Microsoft.OpenApi.Models.OpenApiInfo"/>
            /// </summary>
            public OpenApiInfo OpenApiInfo { get; set; }
        }
    }
}
