using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CreateAndEditImageServer.Logics;
using CreateAndEditImageServer.Services;

var builder = WebApplication.CreateBuilder(args);

// コンテナにコントローラーを追加する
builder.Services.AddControllersWithViews();

// コンテナに必要なサービスを登録する
builder.Services.AddScoped<IOpenaiImageService, OpenaiImageService>();

// コンテナにロジックを登録する
builder.Services.AddScoped<IOpenaiImageLogic, OpenaiImageLogic>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ルーティングを使用する
app.UseRouting();

// コントローラーをマッピングする
app.MapControllers();

// 將 "/" 對應到 Views/Home/Index.cshtml
app.UseEndpoints(endpoints =>
{
	_ = endpoints.MapGet("/", async context =>
	{
		var viewResult = new ViewResult { ViewName = "Home/Index" }; // 對應 Views/Home/Index.cshtml

		var actionContext = new ActionContext
		{
			HttpContext = context,
			RouteData = context.GetRouteData(),
			ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
		};

		var viewEngine = context.RequestServices.GetRequiredService<ICompositeViewEngine>();
		var tempDataProvider = context.RequestServices.GetRequiredService<ITempDataProvider>();
		var view = viewEngine.FindView(actionContext, viewResult.ViewName, false).View;

		if (view == null)
		{
			context.Response.StatusCode = 404;
			await context.Response.WriteAsync("View not found");
			return;
		}

		using var writer = new StringWriter();
		var viewContext = new ViewContext(
			actionContext,
			view,
			new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()),
			new TempDataDictionary(context, tempDataProvider),
			writer,
			new HtmlHelperOptions()
		);

		await view.RenderAsync(viewContext);
		var html = writer.ToString();
		context.Response.ContentType = "text/html; charset=utf-8";
		await context.Response.WriteAsync(html);
	});
});

app.Run();
