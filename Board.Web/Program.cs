using Autofac;
using Autofac.Extensions.DependencyInjection;
using Board.HttpApi;
using Board.Infrastructure;
using Board.ToolKits;
using Board.Web.AppCode;
using Board.Web.Filters;
using Board.Web.Middleware;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

//load Serilog
LogHelper.SerilogSetting();

try
{
    Log.Information("Start web application");

    var builder = WebApplication.CreateBuilder(args);

    //log
    builder.Host.UseSerilog();

    builder.WebHost.UseUrls($"http://*:{AppSettings.ListenPort}");

    builder.Services.AddControllers(option =>
    {
        option.Filters.Add<GlobalExceptionFilter>();
    }).AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); //���л�ʱkeyΪ�շ���ʽ
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;//����ѭ������
    }).AddXmlSerializerFormatters();

    // Add services to the container.
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

    //�Զ�����ӳ��
    builder.Services.AddAutoMapper();
    //�Զ�ע��
    builder.Services.AddAutoInject();
    //MiniProfiler
    builder.Services.AddMiniProfiler();

    //Autofac�滻���õ�ServiceProviderFactory
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule<AutofacModelRegister>();
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    }
    app.UseStaticFiles();

    app.UseRouting();

    app.UseMiniProfiler();

    app.UseStateAutoMapper();

    // ����
    app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    // �쳣�����м��
    app.UseMiddleware<ExceptionHandlerMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}