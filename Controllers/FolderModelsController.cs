using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Text;

namespace task.Controllers
{
    //   [Route("[controller]")]
    public class FolderModelsController : Controller
    {
        private readonly FolderContext _context;

        private int id = 0;

        public FolderModelsController(FolderContext context)
        {
            _context = context;
        }


        [NonAction]
        List<FolderModel> ScanFolder(DirectoryInfo directory, List<FolderModel> list)
        {

            
            foreach (var subdirectory in directory.GetDirectories())
            {
                list.Add(new FolderModel() { Id = id++, Name = subdirectory.Name, ParentId = list.Where(x => x.Name.Equals(subdirectory.Parent.Name)).FirstOrDefault()?.Id });
                list.AddRange(ScanFolder(subdirectory, list));
            }


            return list;
        }

        // GET: FolderModels
        public async Task<IActionResult> Index(string folders, int arg = 0, string? path = "C:\\Users\\misha\\OneDrive\\Desktop\\uni\\courses\\task\\bin\\Debug\\net6.0\\runtimes\\unix")
        {
            var l = new List<FolderModel>();
            var currId = (await _context.Folders.FirstOrDefaultAsync(x => x.ParentId == null))?.Id;


            if (arg == 1)
            {
                List<FolderModel> data = new List<FolderModel>();
                data.AddRange(await _context.Folders.ToListAsync());

                string json = JsonSerializer.Serialize(data);
                System.IO.File.WriteAllText("D:\\output.json", json);
            }
            else
            if (arg == 2)
            {
                
                var directory = new DirectoryInfo(path);
                var list = new List<FolderModel>();

                _context.RemoveRange(_context.Folders);

                await _context.AddRangeAsync(ScanFolder(directory, list));

                _context.SaveChanges();

            }
            else
            if(arg == 3)
            {
                string json = System.IO.File.ReadAllText("D:\\output.json");

                var list = JsonSerializer.Deserialize<List<FolderModel>>(json);

                _context.RemoveRange(_context.Folders);

                _context.AddRange(list);

                _context.SaveChanges();

            }

            if (folders != null)
            {
                var strList = folders.Split('/').ToList();

                foreach (var item in strList)
                {
                    currId = (await _context.Folders.Where(x => x.ParentId == currId && x.Name == item).FirstOrDefaultAsync())?.Id;
                }


                l.Add(await _context.Folders.Where(x => x.Id.Equals(currId)).FirstOrDefaultAsync());
                l.AddRange(await _context.Folders.Where(x => x.ParentId.Equals(currId)).ToListAsync());

            }
            else
            {
                l.Add(new FolderModel() { Id = 0, Name = "Default", ParentId = 0 });
                l.AddRange(await _context.Folders.Where(x => x.ParentId == null).ToListAsync());
            }


            return View(l);

        }        
    }
}
