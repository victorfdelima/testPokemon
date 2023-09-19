using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SQLitePCL;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PokeAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // Este método é usado para adicionar serviços ao contêiner de injeção de dependência.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JwtSettings:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey
                        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Key"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
        });
        services.AddControllers();
        Batteries.Init();
        // Configurar a documentação do Swagger/OpenAPI
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PokeAPI", Version = "v1" });

            // Adicione a configuração de segurança JWT aqui
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Por favor, insira o token JWT no formato 'Bearer {seu_token}'.",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] { }
                }
            });
        });
        var connectionString = Configuration.GetConnectionString("SQLiteConnection");
        services.AddSingleton(Configuration.GetConnectionString("SQLiteConnection"));
        services.AddScoped<MestrePokemonRepository>(provider => new MestrePokemonRepository(connectionString));
        services.AddScoped<PokemonRepository>(provider => new PokemonRepository(Configuration, connectionString));
        services.AddScoped<AuthService>();
        services.AddScoped<MestrePokemonService>();
        services.AddScoped<PokemonService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokeAPI V1");

                // Configura a página de autorização do Swagger para usar o token JWT
                c.OAuthClientId("seu_client_id_aqui");
                c.OAuthClientSecret("seu_client_secret_aqui");
                c.OAuthRealm("realm");
                c.OAuthAppName("Swagger");

                // Define o token JWT prefixado como 'Bearer' por padrão
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
