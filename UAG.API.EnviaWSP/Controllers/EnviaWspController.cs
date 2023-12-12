using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Xml.Linq;
using UAG.API.EnviaWSP.APIs;

namespace UAG.API.EnviaWSP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnviaWspController : Controller
    {
        private IConfiguration _config;
        public EnviaWspController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, string layout)
        {
            int count = 0;
            try
            {
                long size = files.Sum(f => f.Length);
                var filePaths = new List<string>();
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        // full path to file in temp location
                        var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
                        filePaths.Add(filePath);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        DataTable datatable = new DataTable();
                        StreamReader streamreader = new StreamReader(filePath);
                        char[] delimiter = new char[] { ',' };
                        string[] columnheaders = streamreader.ReadLine().Split(delimiter);
                        foreach (string columnheader in columnheaders)
                        {
                            datatable.Columns.Add(columnheader); // I've added the column headers here.
                        }
                        while (streamreader.Peek() > 0)
                        {
                            DataRow datarow = datatable.NewRow();
                            datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                            datatable.Rows.Add(datarow);
                        }

                        foreach (DataRow lector in datatable.Rows)
                        {
                            string phone = lector["phone"].ToString();
                            count++;
                            dynamic components = new List<dynamic>
                            {
                                    new
                                    {
                                        type = "body",
                                        parameters = new List<dynamic>
                                    {

                                    new
                                    {
                                        type="text",
                                        text= "UAG Solutions"
                                    }

                                    }
                                }
                            };
                            JObject respuesta = SuncoAPI.SendTemplate(phone, layout, components, _config).Result;
                        }
                    }
                }
            }
            catch (Exception err)
            {

            }
            return Ok("todo ok" + count);
        }
    }
}
