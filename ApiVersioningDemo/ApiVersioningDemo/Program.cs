using ApiVersioningDemo.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Use this block code

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;

    #region query string, header and media type versioning
    // example of combining techniques
    //o.ApiVersionReader = ApiVersionReader.Combine(
    //    new QueryStringApiVersionReader("api-version", "ver"),
    //    new HeaderApiVersionReader("X-Version"),
    //    new MediaTypeApiVersionReader("ver"));
    #endregion

});
builder.Services.AddVersionedApiExplorer(o =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    o.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    o.SubstituteApiVersionInUrl = true;
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

// Use this line of code
builder.Services.AddSwaggerGen();

//builder.Services.AddSwaggerGen(o =>
//{
//    // add a custom operation filter which sets default values
//    o.OperationFilter<SwaggerDefaultValues>();
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    #region Use this block code

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
        if (provider?.ApiVersionDescriptions != null)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        }
    });

    #endregion
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
