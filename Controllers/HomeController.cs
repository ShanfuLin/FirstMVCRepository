using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ColorGridSSE.Models;
using System.Text.Json;


namespace ColorGridSSE.Controllers;

public class HomeController : Controller
{
    private ColorChangeTracker tracker;
    
    public HomeController(ColorChangeTracker tracker)
    {
        this.tracker = tracker;
    }

    public IActionResult Index()
    {
        return View();
    }

    public void SetNodeColor(string nodeId, string color)
    {
        tracker.SetColor(nodeId, color);
    }

    public IActionResult GetNodeColors()
    {        
        return Json(tracker.GetColors());
    }

    public IActionResult Message()
    {
        Response.ContentType = "text/event-stream";

        while (true) {
            long now = DateTimeOffset.Now.ToUnixTimeSeconds();

            // if last changed was within 3 secs, inform all clients
            if (now - tracker.ChgTimestamp < 3) {
                string s = JsonSerializer.Serialize(tracker.GetColors());

                // sending new changes to attached clients
                Response.WriteAsync(string.Format("data: {0}\n\n", s)); 
            }            

            // sleep for 2 secs
            System.Threading.Thread.Sleep(2000);            
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

