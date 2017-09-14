GIT
echo "# ToastmasterClub" >> README.md
git init
git add README.md
git commit -m "first commit"
git remote add origin https://github.com/sayedsaad07/ToastmasterClub.git
git push -u origin master


Angular/CLI helper
	"npm install --save-dev @angular/cli@latest"
	ng generate service MyService

install aspnet core templates
C:\ESAAD\_Trainings\ASPCoreAngular>
dotnet new --install Microsoft.AspNetCore.SpaTemplates::*

create angular
	dotnet new angular

restore dotnet dependencies
	dotnet restore
restore node depenencies
	npm install
run app
	dotnet run

U can use VS 2017 to run the app

WebPack: enables ontime compile and update page by replacing code 
	enables webpack
	powershell: $Env:ASPNETCORE_ENVIRONMENT = "Development"
	Or CMD: setx ASPNETCORE_ENVIRONMENT "Development"
	show settings: echo ASPNETCORE_ENVIRONMENT
	command: webpack --config webpack.config.vendor.js

publish and release production version
 	dotnet publish -c release

Create Azure Web App
Create GITHUB Repo

	git init
	git add README.md
	git commit -m "first commit"
	git remote add origin https://github.com/sayedsaad07/CoreAngular2017.git
	git push -u origin master



Entity Core
	Open the Package Manager Console (PMC): Tools > NuGet Package Manager > Package Manager Console
		Install-Package Microsoft.EntityFrameworkCore.SqlServer
		Install-Package Microsoft.EntityFrameworkCore.Tools in the PMC
	Database Migration
		Add-Migration firstMigration
Create your database
		Once you have a model, you can use migrations to create a database.
		Open the PMC:
		Tools �> NuGet Package Manager �> Package Manager Console
			Add-Migration InitialCreate 
			Remove-Migration
			Update-Database 
				to apply the new migration to the database. This command creates the database before applying migrations.
Once migration tools only 2 commands add migration and update database 
			Add-Migration My_migration_version_name 
			Update-Database 
			Add-Migration update_post_add_post_Url
			Update-Database 



http://www.blinkingcaret.com/2016/11/30/asp-net-identity-core-from-scratch/
ASP.NET Core Identity
	Add Nuget Packages
		 "Microsoft.AspNetCore.Identity": "2.0.0",
		"Microsoft.AspNetCore.Identity.EntityFrameworkCore": "1.1.0"
		Microsoft.EntityFrameworkCore.Sqlite
		Microsoft.EntityFrameworkCore.Sql
		Microsoft.EntityFrameworkCore.Tools

command migration tools
	Microsoft.EntityFrameworkCore.Tools.DotNet
	https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/migrations
	add migrations to csproj
	<ItemGroup>
		 <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
		 <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.0" />
	</ItemGroup>

	CMD
	dotnet ef migrations add initial
	dotnet database update 

	EXECUTION info		
					C:\ESAAD\_Trainings\Angular\BookKeeperSPAAngular>dotnet ef migrations add initial
					info: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[0]
						  User profile is available. Using 'C:\Users\ESaad\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
					info: Microsoft.EntityFrameworkCore.Infrastructure[100403]
						  Entity Framework Core 2.0.0-rtm-26452 initialized 'BookKeeperContext' using provider 'Microsoft.EntityFrameworkCore.SqlServer' with options: None
					Done. To undo this action, use 'ef migrations remove'
